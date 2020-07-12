/*
 * playground.h
 *
 * Created: 11.11.2019 12:41:43
 *  Author: patrickl
 */ 


#ifndef PLAYGROUND_H_
#define PLAYGROUND_H_

#define F_CPU 1000000UL

#include <avr/io.h>
#include <avr/wdt.h>
#include <util/delay.h>
#include "SPI.h"

void init_all(void);


#endif /* PLAYGROUND_H_ */