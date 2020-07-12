/*
 * SPI.h
 *
 * Created: 11.11.2019 12:42:03
 *  Author: patrickl
 */ 


#ifndef SPI_H_
#define SPI_H_
#include <avr/io.h>

/************************************************************************/
/* Mapping SPI pins ADC MCP33131-05 onto pins ATmega64                  */
/* ATmega64         ADC													*/
/* --------		   ------											*/
/* PB0 (~SS0)      CNVST0												*/
/* PB1 (SCK)        SCLK												*/
/* PB2 (MOSI)		none												*/
/* PB3 (MISO)     SDO_{0..3}											*/
/* PC7 (~SS1)      CNVST1												*/
/*   ...												*/
/* PC1 (~SS7)      CNVST7												*/
/************************************************************************/


/* Slave select lines for the external ADCs */

#define DD_SCK		PB1
#define DD_MOSI		PB2
#define DD_MISO		PB3
#define DD_SS0		PB0		// V_BAT_0+

#define DD_SS1		PC7		// V_BAT_1+
#define DD_SS2		PC6		// V_BAT_2+
#define DD_SS3		PC5		// V_BAT_3+
#define DD_SS4		PC4		// V_SOLAR+
#define DD_SS5		PC3		// I_SOLAR
#define DD_SS6		PC2		// I_CHARGE
#define DD_SS7		PC1		// I_LOAD



void spi_init();
void spi_tx(uint8_t slave);



#endif /* SPI_H_ */