//------------------------------------------------------------------------------ 
// 
//  File: WiMOD_HCI_Layer.h 
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
// ------------------------------------------------------------------------------ 


#ifndef WIMOD_HCI_LAYER_2_H
#define WIMOD_HCI_LAYER_2_H

// ------------------------------------------------------------------------------
// 
// Include Files
//
// ------------------------------------------------------------------------------

#include <stdio.h>
#include <stdint.h>
#include <stdbool.h>
#include <util/delay.h>


// ------------------------------------------------------------------------------
// 
// General Declarations & Definitions
// 
// ------------------------------------------------------------------------------

typedef unsigned char					UINT8;
typedef uint16_t						UINT16;
typedef int								ssize_t;

#define WIMOD_HCI_MSG_HEADER_SIZE		2
#define WIMOD_HCI_MSG_PAYLOAD_SIZE		300
#define WIMOD_HCI_MSG_FCS_SIZE			2

#define LOBYTE(x)						(x)
#define HIBYTE(x)						( (UINT16) (x) >> 8 )

// ------------------------------------------------------------------------------
// 
// HCI Message Structure (internal software usage)
//
// ------------------------------------------------------------------------------

typedef struct
{
	// Payload Length Information,
	// this field not transmitted over UART interface !!!
	UINT16 Length;
	
	// Service Access Point Identifier
	UINT8 SapID;
	
	// Message Identifier
	UINT8 MsgID;
	
	// Payload Field
	UINT8 Payload[WIMOD_HCI_MSG_PAYLOAD_SIZE];
	
	// Frame Check Sequence Field 
	UINT8 CRC16[WIMOD_HCI_MSG_FCS_SIZE];
}TWiMOD_HCI_Message;

// ------------------------------------------------------------------------------
// 
// Function Prototypes
//
// ------------------------------------------------------------------------------

// Message receiver callback
typedef TWiMOD_HCI_Message* (*TWiMOD_HCI_CbRxMessage)(TWiMOD_HCI_Message* rxMessage);

// Init HCI Layer
void WiMOD_HCI_Init(const char* 			comPort,
					TWiMOD_HCI_CbRxMessage	cbRxMessage,
					TWiMOD_HCI_Message*		rxMessage);

// Receiver Process for ATmega64
void 
WiMOD_HCI_ATmega64_Process(void);

// Default Receiver Process
void
WiMOD_HCI_Process(UINT8 *rxBuf, size_t size);

// Send HCI Message
int 
WiMOD_HCI_SendMessage(TWiMOD_HCI_Message* txMessage);

void
send_system_info_radio(void);

#endif // WIMOD_HCI_LAYER_2_H


