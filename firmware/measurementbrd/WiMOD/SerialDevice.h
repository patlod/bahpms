//------------------------------------------------------------------------------ 
// 
//  File: SerialDevice.h 
// 
//  Abstract: Serial Device Interface
// 
//  Version: 0.1 
// 
//  Date: --.--.--
// 
//  Disclaimer: Author of this code is Patrick Lodes
//
// ------------------------------------------------------------------------------ 


#ifndef SERIALDEVICE_H
#define SERIALDEVICE_H

// ------------------------------------------------------------------------------
// 
// Include Files
//
// ------------------------------------------------------------------------------

#include "WiMOD_HCI_Layer.h"
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <fcntl.h>
#include <unistd.h>
#include <termios.h>


// ------------------------------------------------------------------------------
// 
// General Declarations & Definitions
// 
// ------------------------------------------------------------------------------


#define BAUD    115200          // or 9600, ... 

int fd;                         // File handle for opened serial port


// ------------------------------------------------------------------------------
// 
// Function Prototypes
//
// ------------------------------------------------------------------------------

/**
 * Helper function to print buffer content in HEX.
 * @param buf Buffer to read into
 * @param size Size of the handed buffer
 */
void printHexBuffer(UINT8 *buf, ssize_t size);
					
/**
 * 
 *  Open serial port. (Similar as in python library 'PySerial')
 *  @param comPort The name of the COM Port
 *  @param baudrate Baudrate for IMST should be 115200
 *  @param databits Default should be 8 Bits
 *  @param parity Default is no parity. false.
 *  @return true on success. Else false.  
 */
bool open_serial(const char *comPort);

/**
 * Close serial port.
 */ 
void close_serial(void);

/**
 * Read data (Similar as in python library 'PySerial')
 * @param rxBuf Pointer to the buffer of transmission data.
 * @param rxBufSize Length of the transmission data.
 * @return Length of the data read.
 */
ssize_t read_serial(UINT8 *rxBuf, size_t rxBufSize);

/**
 * 
 * Write data (Similar as in python library 'PySerial')
 * @param txBuf Pointer to the buffer of transmission data.
 * @param txLength Length of the transmission data.
 * @return Success: Length of transmitted data. Failure: 0.
 */
int write_serial(UINT8 *txBuf, int txLength);



#endif // SERIALDEVICE_H
