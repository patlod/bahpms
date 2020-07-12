/*
 * SPI.c
 *
 * Created: 11.11.2019 12:41:52
 *  Author: patrickl
 */ 
#include "SPI.h"

void spi_init(){
	// Assign output pins. MISO stays input pin.
	DDRB = (1 << DD_MOSI) 
		| (1 << DD_SCK)
		| (1 << DD_SS0);
	
	DDRC = (1 << DD_SS1)
		| (1 << DD_SS2)
		| (1 << DD_SS3)
		| (1 << DD_SS4)
		| (1 << DD_SS5)
		| (1 << DD_SS6)
		| (1 << DD_SS7);
		
	SPCR = (1 << SPE) // enable SPI 
		| (1 << MSTR) // set master
		| (1 << CPHA); // CPHA - Clock Phase = 1; CPOL - Clock Polarity = 0   ==> Sampling on falling edge
		
	// SPI clock rate: Neither SPR0, SPR1 nor SPI2X are set in SPCR => f_sck = f_osc/4
	
	PORTB |= (1 << DD_SS0);
	// Set all slave selection pins to 0
	PORTC |= ( (1 << DD_SS1) | (1 << DD_SS2) | (1 << DD_SS3) | (1 << DD_SS4)  | (1 << DD_SS5) | (1 << DD_SS6) | (1 << DD_SS7) );
}



/************************************************************************/
/* ACHTUNG BUG!!! Die Methode ist nur für die PORTC pins ausgelegt.     */
/* FIX: Alle SS pins auf PCx legen.										*/
/* Wrote separate method as the ss flags cant really be compared        */
/************************************************************************/
uint16_t spi_rx16_adc_portc(uint8_t ss)
{
	uint16_t adc_data;
	
	// Set slave selection bit
	PORTC &= ~(1 << ss);
	
	// Write dummy byte to request 
	SPDR = 0x00;
	
	// Receive MSB
	while(!(SPSR & (1 << SPIF)));
	adc_data = SPDR;
	adc_data <<= 8;
	
	SPDR = 0x00;
	
	// Receive LSB
	while(!(SPSR & (1 << SPIF)));
	// Combine [MSB,xxx] + [LSB] = [MSB,LSB]
	adc_data |= SPDR;
	
	// Reset the slave selection bit. End of packet.
	PORTC |= (1 << ss);
	
	return adc_data;
}

uint16_t spi_rx16_adc_portb(uint8_t ss)
{	
	uint16_t adc_data;
	
	// Set slave selection bit
	PORTB &= ~(1 << ss);
	
	// Write dummy byte to request
	SPDR = 0x00;
	
	// Receive MSB
	while(!(SPSR & (1 << SPIF)));
	adc_data = SPDR;
	adc_data <<= 8;
	
	SPDR = 0x00;
	
	// Receive LSB
	while(!(SPSR & (1 << SPIF)));
	// Combine [MSB,xxx] + [LSB] = [MSB,LSB]
	adc_data |= SPDR;
	
	// Reset the slave selection bit. End of packet.
	PORTB |= (1 << ss);
	
	return adc_data;
}