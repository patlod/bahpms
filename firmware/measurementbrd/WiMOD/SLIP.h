//------------------------------------------------------------------------------ 
// 
//  File: SLIP.h 
// 
//  Abstract: SLIP Encoder / Decoder 
// 
//  Version: 0.2 
// 
//  Date: 18.05.2016 
// 
//  Disclaimer: This example code is provided by IMST GmbH on an "AS IS" 
//             basis without any warranties. 
// 
//------------------------------------------------------------------------------ 

#ifndef SLIP_H
#define SLIP_H
//------------------------------------------------------------------------------
//
//	Include Files
//
//------------------------------------------------------------------------------

#include <stdint.h>
#include <stdbool.h>

//------------------------------------------------------------------------------
//
//	General Definitions
//
//------------------------------------------------------------------------------

typedef uint8_t		UINT8;

//------------------------------------------------------------------------------
//
//	Function Prototypes
//
//------------------------------------------------------------------------------

// SLIP message receiver callback
typedef UINT8*  (*TSLIP_CbRxMessage)(UINT8* message, int length);

// Init SLIP layer
void
SLIP_Init(TSLIP_CbRxMessage cbRxMessage);

// Init first receiver buffer
bool
SLIP_SetRxBuffer(UINT8* rxBuffer, int rxBufferSize);

// Decode incoming data
void
SLIP_DecodeData(UINT8* srcData, int srcLength);

void
SLIP_ProcessRxByte(unsigned char rxData);

// Encode outgoing Data
int
SLIP_EncodeData(UINT8* dstBuffer, int txBufferSize, UINT8* srcData, int srcLength);

#endif // SLIP_H

//------------------------------------------------------------------------------
// end of file
//------------------------------------------------------------------------------