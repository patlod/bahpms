#ifndef UART1_H_
#define UART1_H_

#include <avr/io.h>
#include <string.h>

#define F_CPU 7372800UL

/************************************************************************/
/* Communicating with the radio module type:	 WiMOD iM881A-M         */
/************************************************************************/

typedef unsigned char				UINT8;

void uart1_init(int ubrr);

void uart1_tx_byte(unsigned char* data);
// Has to be called with a pointer to the data packet which shall be casted as e.g. (int *)&data
int uart1_tx_packet(unsigned char *data, size_t bytes);

void uart1_flush(void);
void uart1_rx_byte(unsigned char* data);
int uart1_rx_packet(unsigned char *data, size_t bytes);

#endif /* UART1_H_ */                                                                                                                                                                                                                                                                                     