/*
 * UART0.h
 *
 * Created: 03.01.2020 16:55:27
 *  Author: ar
 */ 


#ifndef UART0_H_
#define UART0_H_

#include <string.h>
#include <avr/io.h>
#define F_CPU 7372800UL

/************************************************************************/
/* Communicating with the UART usb module						        */
/************************************************************************/

void uart0_init(int ubrr);

void uart0_tx_byte(unsigned char *data);
// Has to be called with a pointer to the data packet which shall be casted as e.g. (int *)&data
void uart0_tx_packet(unsigned char *data, size_t bytes);

void uart0_flush(void);
void uart0_rx_byte(unsigned char * data);
void uart0_rx_packet(unsigned char *data);


#endif /* UART0_H_ */