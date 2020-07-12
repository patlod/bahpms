/*
 * ringbuffer.c
 *
 * Created: 08.01.2020 15:59:25
 *  Author: ar
 */ 

#include "ringbuffer.h"

struct Buffer {
	uint8_t data[BUFFER_SIZE];
	uint8_t read; // zeigt auf das Feld mit dem ältesten Inhalt
	uint8_t write; // zeigt immer auf leeres Feld
	uint8_t count;
	} buffer = {{}, 0, 0, 0};

	uint8_t BufferIn(uint8_t byte)
	{
		uint8_t next = ((buffer.write + 1) & BUFFER_MASK);
		if (buffer.read == next)
			return FAIL;
		buffer.data[buffer.write] = byte;
		// buffer.data[buffer.write & BUFFER_MASK] = byte; // absolut Sicher
		buffer.write = next;
		buffer.count++;
		return SUCCESS;
	}
	
	uint8_t BufferOut(uint8_t *pByte)
	{
		if (buffer.read == buffer.write)
			return FAIL;
		*pByte = buffer.data[buffer.read];
		buffer.read = (buffer.read+1) & BUFFER_MASK;
		buffer.count--;
		return SUCCESS;
	}
	
	uint8_t BufferCount(void)
	{
		return buffer.count;
	}