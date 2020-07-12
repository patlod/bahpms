//------------------------------------------------------------------------------ 
// 
//  File: SerialDevice.cpp
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

//------------------------------------------------------------------------------
//
// Include Files
//
//------------------------------------------------------------------------------

#include "SerialDevice.h"


void printHexBuffer(UINT8 *buf, ssize_t size){
    int i;
    for(int i=0; i < size; i++){
        // fprintf(stdout, "%x ", buf[i]);
        // Besseres print
        fprintf(stdout, "%02x ", buf[i]);
    }
    fprintf(stdout, "\n");
}


/**
 * 
 *  Open serial port. (Similar as in python library 'PySerial')
 *  @param comPort The name of the COM Port
 *  @param baudrate Baudrate for IMST should be 115200
 *  @param databits Default should be 8 Bits
 *  @param parity Default is no parity. false.
 *  @return true on success. Else false.  
 */
bool open_serial(const char *comPort)
{
    speed_t baud;
    // Switch to match local defined BAUD rate to constants in termios.h
    switch(BAUD){
        case 115200:
            baud =  B115200;  /*  baud rate */
        break;
        default:
            return false;
        break;
    }
    
    // Open the device
    fd = open(comPort, O_RDWR | O_NOCTTY); // | O_NONBLOCK

    if(fd < 0){
        fprintf(stderr, "Error happened opening serial port.. Make sure your device is connected.\n");
        exit(EXIT_FAILURE);  // handle error
    }

    printf("Device opened..\n");

    /* set the other settings (in this case, 115200 8N1) */
    printf("Configure settings of device..\n");
    struct termios settings;
    tcgetattr(fd, &settings);

    cfsetspeed(&settings, baud); /* baud rate */

    // Turn off any options that might interfere with our ability to send and
    // receive raw binary bytes.

    // c_iflag
    settings.c_iflag &= ~(INLCR | IGNCR | ICRNL | IXON | IXOFF);
    // c_oflag
    settings.c_oflag &= ~(ONLCR | OCRNL);
    // c_lflag
    settings.c_lflag &= ~(ECHO | ECHONL | ICANON | ISIG | IEXTEN);

    // c_cflag
    settings.c_cflag &= ~PARENB; /* no parity */
    settings.c_cflag &= ~CSTOPB; /* 1 stop bit */
    settings.c_cflag &= ~CSIZE;
    settings.c_cflag |= CS8 | CLOCAL; /* 8 bits */

    // settings.c_cflag &= ~ICANON;    /* set non-canonical mode -- THIS WAS BUG. */
    // settings.c_oflag &= ~OPOST; /* raw output */

    /* See Canonical and noncanonical mode here: https://linux.die.net/man/3/tcgetattr */
    settings.c_cc[VTIME] = 2; 
    settings.c_cc[VMIN] = (cc_t) 400;
  
    tcsetattr(fd, TCSANOW, &settings); 
    tcflush(fd, TCOFLUSH);
    
    printf("Ready for I/O..\n");
    return true;
};


/**
 * Close serial port.
 */ 
void close_serial(void){
    close(fd);
}


/**
 * Read data (Similar as in python library 'PySerial')
 * @param rxBuf Pointer to the buffer of transmission data.
 * @param rxBufSize Length of the transmission data.
 * @return Length of the data read.
 */
ssize_t read_serial(UINT8 *rxBuf, size_t rxBufSize)
{
    fprintf(stdout, "Reading up to %lu bytes from serial...\n", rxBufSize);
    // Read..
    ssize_t rdB = read(fd, rxBuf, rxBufSize);

    /**
     * NOTE: 
     * I'm aware that this is bad practice to end the program here.
     * Instead values should be returned which than can be evaluated by if-statement.
     * The error handling including program exit should only be done from the main loop.
     */ 
    if(rdB > 0){
        fprintf(stdout, "Read Bytes: %zd\n", rdB);
        printHexBuffer(rxBuf, rdB);
    }else{
        fprintf(stderr, "Error happened reading from device..\n");
        /*close(fd);
        exit(EXIT_FAILURE);*/
    }
    return rdB;
}


/**
 * 
 * Write data (Similar as in python library 'PySerial')
 * @param txBuf Pointer to the buffer of transmission data.
 * @param txLength Length of the transmission data.
 * @return Success: Length of transmitted data. Failure: 0.
 */
/*int write_serial(UINT8 *txBuf, int txLength)
{
    // TODO: 
} */