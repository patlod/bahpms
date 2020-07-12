//------------------------------------------------------------------------------ 
// 
//  File: WiMOD_HCI_Layer.cpp 
// 
//  Abstract: WiMOD HCI Message Layer 
// 
//  Version: 0.1 
// 
//  Date: 18.05.2016 
// 
//  Disclaimer: This example code is provided by IMST GmbH on an "AS IS" 
//             basis without any warranties. 
// 
//------------------------------------------------------------------------------ 



//------------------------------------------------------------------------------
//
// Include Files
//
//------------------------------------------------------------------------------

#include "WiMOD_HCI_Layer.h"
#include "CRC16.h"
#include "SLIP.h"
#include "../UART/UART1.h"
#include "../UART/UART0.h"
#include <string.h>

//------------------------------------------------------------------------------
//
// Forward Declaration
//
//------------------------------------------------------------------------------

// SLIP Message Receiver Callback
static UINT8*	WiMOD_HCI_ProcessRxMessage(UINT8* rxData, int rxLength);

//------------------------------------------------------------------------------
// 
// Declare Layer Instance
//
//------------------------------------------------------------------------------

typedef struct
{
	// CRC Error counter
	uint32_t					CRCErrors;
	
	// RxMessage
	TWiMOD_HCI_Message* 	RxMessage;
	
	// Receiver callback
	TWiMOD_HCI_CbRxMessage	CbRxMessage;
}TWiMOD_HCI_MsgLayer;

//------------------------------------------------------------------------------
//
// Section RAM
//
//------------------------------------------------------------------------------

// reserve HCI Instance
static TWiMOD_HCI_MsgLayer	HCI;

// reserve one TxBuffer
static UINT8				TxBuffer[sizeof( TWiMOD_HCI_Message ) * 2 + 2];

//------------------------------------------------------------------------------
//
// Init
//
// @brief: Init HCI Message layer
//
//------------------------------------------------------------------------------

void
WiMOD_HCI_Init(const char*				comPort, 		// comPort
			   TWiMOD_HCI_CbRxMessage	cbRxMessage,	// HCI msg receiver callback
			   TWiMOD_HCI_Message*	    rxMessage)		// initial rxMessage
			   {
				   // init error counter
				   HCI.CRCErrors  =    0;


				   /**
					* It is better to make an if-statement and let the Init fail instead of 
					* checking this indirectly and hidden in the callback method WiMOD_HCI_ProcessRxMessage.
					* To justify this correction I assume the premise is true that there is no need to even receive the data
					* if there is no callback for it. 
					*/ 
				   // save receiver callback
				   HCI.CbRxMessage = 	cbRxMessage;
				   
				   //save RxMessage
				   HCI.RxMessage   =     rxMessage;
				   
				   // Init SLIP
				   SLIP_Init(WiMOD_HCI_ProcessRxMessage);
				   
				   // Init first RxBuffer to SAP_ID of HCI message, size without 16-Bit Length Field
				   SLIP_SetRxBuffer(&rxMessage->SapID, sizeof(TWiMOD_HCI_Message) - sizeof(UINT16));
				   
// 				   // init serial device ===> Optionally init UART1 here
// 				   //return open_serial(comPort);
				   
			   }

//------------------------------------------------------------------------------
// 
//	Process
//
//	@brief: read incomming serial data
//
//------------------------------------------------------------------------------

/************************************************************************/
/* TODO: Adjust so that it is conform with firmware environment.        */
/* i.e. being called in interrupt service routine and reading byte-wise */
/************************************************************************/
void
WiMOD_HCI_ATmega64_Process(void)
{
	uint8_t buffered_byte;
	BufferOut(&buffered_byte);
	SLIP_ProcessRxByte(buffered_byte);
}

void
WiMOD_HCI_Process(UINT8 *rxBuf, size_t size)
{

		// read small chunk of data
		int rxLength = read_serial(rxBuf, size);
		
		// data available ?
		if(rxLength > 0)
		{
			// yes, forward to SLIP decoder, decoded SLIP message will be passed to
			// function "WiMOD_HCI_ProcessRxMessage"
			SLIP_DecodeData(rxBuf, rxLength);

			//  reset the serial rx buffer with memset or similar..
			//  Because this function will most likely be called in a while loop
			memset(rxBuf, 0, sizeof(*rxBuf));
		}
}

//------------------------------------------------------------------------------
//
//	WiMOD_HCI_ProcessRxMessage
//
//	@brief: process received SLIP message and return new rxBuffer
//
//------------------------------------------------------------------------------
static UINT8*
WiMOD_HCI_ProcessRxMessage(UINT8* rxData, int rxLength)
{
	// check min length
	if(rxLength >= (WIMOD_HCI_MSG_HEADER_SIZE + WIMOD_HCI_MSG_FCS_SIZE))
	{
		//uart0_tx_packet(rxData, rxLength);
		if(CRC16_Check(rxData, rxLength, CRC16_INIT_VALUE))
		{	

			// receiver registered ?
			if(HCI.CbRxMessage)
			{
				// yes, complete message info
				HCI.RxMessage->Length = rxLength - (WIMOD_HCI_MSG_HEADER_SIZE + WIMOD_HCI_MSG_FCS_SIZE);
				//call upper layer receiver and save new RxMessage
				// pack the central switch-statment in this call back.
				// The reason to wrap it in a call back is, because the actual decoded
				// data will be used one layer above WiMOD_HCI_Layer, so it has to become
				// available there. 
				// That is also the reason why from the perspective of the upmost layer, 
				// there is an indirection over 2 callbacks to the actual data.
				HCI.RxMessage = (*HCI.CbRxMessage)(HCI.RxMessage);
			}
		}
		else
		{
			HCI.CRCErrors++;
		}
	}
	
	// free HCI message available ? 
	// ==> Checks for pointer, so the CbRxMessage should
	// use the data from buffer, reset the buffer, and return a pointer.
	// I will do this directly in the switch-case within this method.
	if(HCI.RxMessage)
	{
		// yes, return pointer to first byte
		return &HCI.RxMessage->SapID;
	}
	
	// error, disable SLIP decoder
	return 0;
}


//------------------------------------------------------------------------------
//
//	SendMessage
//
//	@brief: Send a HCI message (with or without payload)
//
//------------------------------------------------------------------------------

int
WiMOD_HCI_SendMessage(TWiMOD_HCI_Message* txMessage)
{
	// 1. check parameter
	//
	// 1.1 check ptr
	//
	if(!txMessage)
	{
		// error
		return -1;
	}
	
	// 2. Calculate CRC16 over header and optional payload
	UINT16 crc16 = CRC16_Calc(&txMessage->SapID,
	txMessage->Length + WIMOD_HCI_MSG_HEADER_SIZE,
	CRC16_INIT_VALUE);
	
	// 2.1 get 1's complement !!!
	crc16 = ~crc16;
	
	// 2.2 attach CRC16 and correct length, LSB first
	//
	txMessage->Payload[txMessage->Length]		= LOBYTE(crc16);
	txMessage->Payload[txMessage->Length + 1]	= HIBYTE(crc16);
	
	// 3. perform SLIP encoding
	// 	- start transmission with SAP ID
	// 	- correct length by header size
	
	int txLength = SLIP_EncodeData(TxBuffer,
									sizeof(TxBuffer),
									&txMessage->SapID,
									txMessage->Length + WIMOD_HCI_MSG_HEADER_SIZE + WIMOD_HCI_MSG_FCS_SIZE);
	
	// message ok?
	if(txLength > 0)
	{
		// 4. send octet sequence over serial device
		/* Send via UART to radio module */
		if( uart1_tx_packet(TxBuffer, txLength) > 0 )
		{
			// return ok
			
			return 1;
		}
		
		// error - SLIP layer couldn't encode message - buffer to small?
		return -1;
	}
}
