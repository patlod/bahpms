/*
 * UART1.c
 *
 * Created: 03.01.2020 17:32:32
 *  Author: ar
 */ 

#include "UART1.h"

//RF Radio Baud 115200 8N1



void uart1_init(int ubrr)
{
	/* Set baud rate */
	UBRR1H = (unsigned char)(ubrr>>8);
	UBRR1L = (unsigned char)ubrr;/* Enable receiver and transmitter */
	UCSR1A = (1<<U2X1);
	UCSR1B = (1<<RXEN1)|(1<<TXEN1)|(1<<RXCIE1);/* Set frame format: 8data, 1stop bit */
	UCSR1C = (1<<UCSZ11)|(1<<UCSZ10);
}


void uart1_tx_byte(unsigned char * byte)
{
	/* Wait for empty transmit buffer */
	while ( !( UCSR1A & (1<<UDRE1)) );/* Put data into buffer, sends the data */
	UDR1 = *byte;
}


int uart1_tx_packet(unsigned char *data, size_t bytes)
{
	int i;
	for(i=0; i < bytes; i++){
		uart1_tx_byte(&data[i]);
	}
	return i;
}

void uart1_flush(void)
{
	unsigned char depp;
	while ( UCSR1A & (1<<RXC1) ) depp = UDR1;
}

void uart1_rx_byte(unsigned char *byte)
{
	/* Wait for data to be received */
	while ( !(UCSR1A & (1<<RXC1)) )
	;
	/* Get and return received data from buffer */
	*byte = UDR1;
}

int uart1_rx_packet(unsigned char *data, size_t bytes)
{
	int i;
	for(i=0; i < bytes; i++){
		uart0_rx_byte(&data);
		data++;
	}
	/*int i = 0;
	while ( UCSR1A & (1<<RXC1) ){
		 *data++ = UDR0;
		 i++;
	}*/
	return i;
}