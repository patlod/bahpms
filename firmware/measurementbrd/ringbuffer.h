/*
 * ringbuffer.h
 *
 * Created: 08.01.2020 16:00:05
 *  Author: ar
 */ 


/*
 * --------------------------------------------
 * Praktikum: Eingebettete Mikrocontroller-Systeme
 * (PEMSY)
 *
 * Ringpuffer
 *
 */

#if !defined __RINGBUFFER_H
#define __RINGBUFFER_H
#include <inttypes.h>

#define BUFFER_SIZE 128             //64 // muss 2^n betragen (8, 16, 32, 64 ...)
#define BUFFER_MASK (BUFFER_SIZE-1) // Klammern auf keinen Fall vergessen

#define SUCCESS 1
#define FAIL 0



uint8_t BufferOut(uint8_t *pByte);
uint8_t BufferIn(uint8_t byte);
uint8_t BufferCount(void);

#endif


