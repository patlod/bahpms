/*
 * playground.c
 *
 * Created: 11.11.2019 12:41:14
 * Author : patrickl
 */ 


#include "playground.h"

int main(void)
{
	init_all();
	wdt_enable(WDTO_1S);
    /* Replace with your application code */
    while (1) 
    {	
		// wdt_reset();

		DDRB = (1 << PB0) | (1 << PB1) | (1 << PB2) | (1 << PB3) | (1 << PB4);
		
		PORTB |= (1 << PB0);
		_delay_ms(260);

		PORTB |= (1 << PB1);
		_delay_ms(260);

		PORTB |= (1 << PB2);
		_delay_ms(260);

		PORTB |= (1 << PB3);
		_delay_ms(260);

		PORTB |= (1 << PB4);
		_delay_ms(260);
		
		PORTB &= ~( (1 << PB0) | (1 << PB1) | (1 << PB2) | (1 << PB3) | (1 << PB4) );
		
    }
	
	
}

void init_all(void){
	// spi_init();
}
