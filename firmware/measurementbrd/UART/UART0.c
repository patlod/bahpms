/*
 * UART0.c
 *
 * Created: 03.01.2020 16:55:16
 *  Author: ar
 */ 

#include "UART0.h"

//RF Radio Baud 115200 8N1

void uart0_init(int ubrr){
	UBRR0H = (unsigned char)(ubrr>>8);				/* Set baud rate */
	UBRR0L = (unsigned char)ubrr;					/* Enable receiver and transmitter */
	UCSR0A = (1<<U2X0);	
	UCSR0B = (1<<RXEN0)|(1<<TXEN0)|(1<<RXCIE0);		/* Set frame format: 8data, 1stop bit */
	UCSR0C = (1<<UCSZ01)|(1<<UCSZ00);
}


void uart0_tx_byte(unsigned char *byte){
	/* Wait for empty transmit buffer */
	while ( !( UCSR0A & (1<<UDRE0)) );/* Put data into buffer, sends the data */
	UDR0 = *byte;
}

void uart0_tx_packet(unsigned char *data, size_t bytes)
{
	int i;
	// Reading that is big endian (most significant byte is sent first)
	for(i=0; i < bytes; i++){
		uart0_tx_byte(&data[i]);
	}
}

void uart0_flush(void)
{
	unsigned char depp;
	while ( UCSR0A & (1<<RXC0) ) depp = UDR0;
}

void uart0_rx_byte(unsigned char *byte)
{
	/* Wait for data to be received */
	while ( !(UCSR0A & (1<<RXC0)) )
	;
	/* Get and return received data from buffer */
	*byte = UDR0;
}

void uart0_rx_packet(unsigned char *data)
{
	/*int i;
	for(i=0; i < bytes; i++){
		uart0_tx_byte(&data);
		data++;
	}*/
	while ( UCSR0A & (1<<RXC0) ) *data++ = UDR0;
}
