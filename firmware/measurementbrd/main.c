/*
 * main.c
 *
 * Created: 23.01.2020 12:35:42
 * Author : Patrick Lodes
 */ 

#include "main.h"

/* ----------------------- Ringbuffer for serial communication -----------------------------*/
#include "ringbuffer.h"

/* ----------------------- UART communication -----------------------------*/
#include "UART/UART0.h"
#include "UART/UART1.h"

/* ----------------------- SPI communication -----------------------------*/
#include "SPI.h"

/* ----------------------- LoRa Wireless Interface -----------------------------*/
#include "WiMOD/WiMOD_HCI_Layer.h"
#include "WiMOD_HCI_Host.h"

/* ----------------------- MODBUS libraries -----------------------------*/
#include "modbus/include/mb.h"
#include "modbus/include/mbport.h"

/* ----------------------- AVR libraries -----------------------------*/
#include <avr/interrupt.h>
#include <avr/io.h>

/* ----------------------- Standard C -----------------------------*/
#include <util/delay.h>
#include <stdio.h>



/* ----------------------- Defines MODBUS------------------------------------------*/
// Input Registers allocation:
// 	[0] - Volts Bat0
// 	[1] - Volts Bat1
// 	[2] - Volts Bat2
// 	[3] - Volts Bat3
// 	[4] - Volts Solar
// 	[5] - Current Solar
// 	[6] - Current Charge
// 	[7] - Current Load
// 	[8] - ActiveFlag
// 	[9] - ChargeFlag
#define REG_INPUT_START 30000
#define REG_INPUT_NREGS 10

// Holding Registers allocation:
// 	[0] - Operation Mode
// 	[1] - # Bats
// 	[2] - # Cells per Bat (fixed for now)
// 	[3] - Active Bat (Only if MANUAL_MODE)
//  [4] - Chrgng Bat (Only if MANUAL_MODE)
#define REG_HOLDING_START 40000
#define REG_HOLDING_NREGS 9


/* ----------------------- Static variables MODBUS ---------------------------------*/
// Input Registers
static USHORT   usRegInputStart = REG_INPUT_START;
static USHORT   usRegInputBuf[REG_INPUT_NREGS];
// Holding Registers 
static USHORT   usRegHoldingStart = REG_HOLDING_START;
static USHORT   usRegHoldingBuf[REG_HOLDING_NREGS];

/* ---------- MODBUS dynamic variables used for MODBUS initialization ---------- */
// Slave characteristics
static eMBMode MB_MODE = MB_RTU;
static UCHAR SLAVE_ADDRESS = 0x0A; // Decimal: 10
static UCHAR PORT = 0;
static ULONG BAUDRATRE = 115200;  
static eMBParity PARITY = MB_PAR_NONE;

const UCHAR     ucSlaveID[] = { 0xAA, 0xBB, 0xCC };
eMBErrorCode    eStatus;


/* ----------------------- Variables Timer 0: For Fetching ADC data ---------------------------------*/
// Prescaler of 1024:	t_interval = ( 2^8 * 1024 ) / 7372800 = 0.036s
// So 28 timer overflows in the second
// For an interval of 1s we react only every 28th interrupt
#define ADC_TIMER_OVF_FACTOR				28	 
volatile uint8_t adc_timer_ovf_cnt;
// For an interval of 4s we react only every 56th interrupt
#define WIMOD_TIMER_OVF_FACTOR				224	//	112
volatile uint16_t wimod_timer_ovf_cnt;


/* ----------- Control pins for battery management ------------ */
// ATTENTION: The batteries indices used in firmware are not directly matching 
// 			  the indices of the controller pins.
// Index-translations:
// Ctrl:   FW   |   HW 		  ||	Chrg: 	FW	 |   HW
//          0	|	PA3       ||			 0   |   PA7
// 			1   |   PA0       ||			 1   |   PA4
// 			2   |   PA2       || 			 2   |   PA6
// 			3   |	PA1       ||			 3   |   PA5

#define BAT_0_CTRL 					PA3		// Default: Conducting
#define BAT_1_CTRL					PA0		// Default: Non-Conducting
#define BAT_2_CTRL  				PA2		// Default: Non-Conducting		
#define BAT_3_CTRL  				PA1		// Default: Non-Conducting

#define BAT_0_CHRG  				PA7		// Default: Non-Conducting
#define BAT_1_CHRG  				PA4		// Default: Non-Conducting
#define BAT_2_CHRG  				PA6		// Default: Non-Conducting
#define BAT_3_CHRG  				PA5		// Default: Non-Conducting

/* ------- Management mode -------- */
#define NUM_2_BAT						2
#define NUM_3_BAT						3		// 15277
#define NUM_4_BAT						4		// 19373

/* ------------------------ Number of battery cells ------------------------------- */
#define BAT_CELLS					2

//* ----------------------- Voltage limits of charging ---------------------------- */
// Values from here: https://de.wikipedia.org/wiki/Lithium-Polymer-Akkumulator
// Per cell:
// 		Min: 3.3v			(Cutoff voltage - Entladeschlussspannung)
//		Max: 4.0v to 4.2v	(End-of-Charge voltage - Ladeschlussspannung)
//
// NOTE: voltage values represented as scaled value between 0 and 65536 from ADC
#if BAT_CELLS == 2
#define BAT_MIN_VOLTS				48907 		// Margin of 0.05: U_adc_scaled = (6.6 + 0.05) / C_volt_divider * C_adc_scale
#define BAT_MAX_VOLTS				61410		// Margin of 0.05: U_adc_scaled = (8.4 - 0.05) / C_volt_divider * C_adc_scale
#endif

/* 
 T.b.d. Different voltage dividers required i.e. the constant for the divider differs.
#elif BAT_CELLS == 3
#define BAT_MIN_VOLTS				9.9 + 0.05				
#define BAT_MAX_VOLTS				12.6 - 0.05

#elif BAT_CELLS == 4
#define BAT_MIN_VOLTS				13.2 + 0.05				
#define BAT_MAX_VOLTS				16.8 - 0.05
#endif

/* ------- Constants for "Rest" times in 3-Bat system ------- */
// Simple strategy:
// As the voltage of lithium-ion batteries has a plateau somewhere between
// 80% (and 20%) we just use exactly these limits to start charging.
// https://www.quora.com/How-does-a-smartphone-measure-battery-capacity-when-the-voltage-is-constant-from-20-to-80

// This FACTOR reduces the "rest time" automatically (when using a smaller then premise)
// as the SOC sinks because batteries might not have been fully charged.

// To find the right factor some calculations or fine-tuning will be useful.
//#if NUM_BATS == _3_BAT
#define SOC_INIT_CHRG				0.9
//#endif


/* ----------------------- Flags for status in battery management ---------------------------------*/
/*typedef USHORT ActiveFlag, 
			   ChargeFlag; */

const uint16_t STAT_ACT_B0 = 		0x0000;
const uint16_t STAT_ACT_B1 =		0x0001;
const uint16_t STAT_ACT_B2 =		0x0002;
const uint16_t STAT_ACT_B3 =		0x0003;
const uint16_t STAT_ACT_NONE =		0xbabe;

const uint16_t STAT_CHRG_B0 =		0x0004;
const uint16_t STAT_CHRG_B1 =		0x0005;
const uint16_t STAT_CHRG_B2 =		0x0006;
const uint16_t STAT_CHRG_B3	=		0x0007;
const uint16_t STAT_CHRG_NONE =		0xbabe;

uint16_t *bat_active = &usRegInputBuf[8];
uint16_t *bat_chrgng = &usRegInputBuf[9];


/************************************************************************/
/* MODE SETTINGS                                                        */
/************************************************************************/
bool *MANUAL_MODE			=	&usRegHoldingBuf[0];
uint16_t *NUM_BATS			=	&usRegHoldingBuf[1];
// uint16_t BAT_CELLS = 2; see above
uint16_t *next_bat_active	=   &usRegHoldingBuf[3];
uint16_t *next_bat_chrgng   =   &usRegHoldingBuf[4];
bool *WIRELESS_MODE			=   &usRegHoldingBuf[5];	// 0 - Communication via MODBUS; 1 - Communication via WiMOD
bool *NEW_SETTINGS			=   &usRegHoldingBuf[6];
bool *CHRG_CYCLE			=   &usRegHoldingBuf[7];
bool *EMERGENCY_STOP		=   &usRegHoldingBuf[8];



/* ---------- Fetches ADC values per SPI ---------- */
// ATTENTION: The batteries indices used in firmware are not directly matching 
// 			  the indices of the controller pins.
// Index-translations:
// Ctrl:   FW   |   HW 
//          0	|	SS3
// 			1   |   SS0
// 			2   |   SS2
// 			3   |	SS1
void fetch_adc_values(void){
	usRegInputBuf[0] = spi_rx16_adc_portc(DD_SS3);	// Volts Bat0
	usRegInputBuf[1] = spi_rx16_adc_portb(DD_SS0);	// Volts Bat1
	usRegInputBuf[2] = spi_rx16_adc_portc(DD_SS2);	// Volts Bat2
	usRegInputBuf[3] = spi_rx16_adc_portc(DD_SS1);  // Volts Bat3
	usRegInputBuf[4] = spi_rx16_adc_portc(DD_SS4);  // Volts Solar

	usRegInputBuf[5] = spi_rx16_adc_portc(DD_SS5);  // Current Solar
	usRegInputBuf[6] = spi_rx16_adc_portc(DD_SS6);  // Current Charge
	usRegInputBuf[7] = spi_rx16_adc_portc(DD_SS7);  // Current Load
}

USHORT get_volts_bat(USHORT index)
{
	return usRegInputBuf[(index % 4)];
}

USHORT get_volts_next_bat(USHORT index)
{
	switch(*NUM_BATS)
	{
		case NUM_2_BAT:
		return usRegInputBuf[((index + 1) % NUM_2_BAT)];
		//break;
		case NUM_3_BAT:
		return usRegInputBuf[((index + 1) % NUM_3_BAT)];
		//break;
		case NUM_4_BAT:
		return usRegInputBuf[((index + 1) % NUM_4_BAT)];
		//break;
		default:
		// Kaesekuchen
		break;
	}
}
	

USHORT get_volts_active_bat(void)
{
	return usRegInputBuf[usRegInputBuf[8]];
}

USHORT get_volts_charge_bat(void)
{
	return usRegInputBuf[usRegInputBuf[9] % 4];
}

/**
 * Checks whether batteries are {connected|!damaged}. 
 */ 
bool assert_bats_ok(void){
	switch(*NUM_BATS){
		case NUM_2_BAT:
			return (usRegInputBuf[0] >= BAT_MIN_VOLTS && usRegInputBuf[0] <= BAT_MAX_VOLTS)
				&& (usRegInputBuf[1] >= BAT_MIN_VOLTS && usRegInputBuf[1] <= BAT_MAX_VOLTS);
		case NUM_3_BAT:
			return (usRegInputBuf[0] >= BAT_MIN_VOLTS && usRegInputBuf[0] <= BAT_MAX_VOLTS)
				&& (usRegInputBuf[1] >= BAT_MIN_VOLTS && usRegInputBuf[1] <= BAT_MAX_VOLTS)
				&& (usRegInputBuf[2] >= BAT_MIN_VOLTS && usRegInputBuf[2] <= BAT_MAX_VOLTS);
		case NUM_4_BAT:
			return (usRegInputBuf[0] >= BAT_MIN_VOLTS && usRegInputBuf[0] <= BAT_MAX_VOLTS)
				&& (usRegInputBuf[1] >= BAT_MIN_VOLTS && usRegInputBuf[1] <= BAT_MAX_VOLTS)
				&& (usRegInputBuf[2] >= BAT_MIN_VOLTS && usRegInputBuf[2] <= BAT_MAX_VOLTS)
				&& (usRegInputBuf[3] >= BAT_MIN_VOLTS && usRegInputBuf[3] <= BAT_MAX_VOLTS);
		default:
			return false;
	}

}

bool assert_charge_source_connected(void){
	// Check for a sensible value to garantuee that energy source for charging is connected.
}

void stop_chargng_bat(void){
	switch(*bat_chrgng){
		case (uint16_t) STAT_CHRG_B0:
			PORTA &= ~(1 << BAT_0_CHRG);
		break;
		case (uint16_t) STAT_CHRG_B1:
			PORTA &= ~(1 << BAT_1_CHRG);
		break;
		case (uint16_t) STAT_CHRG_B2:
			PORTA &= ~(1 << BAT_2_CHRG);
		break;
		case (uint16_t) STAT_CHRG_B3:
			PORTA &= ~(1 << BAT_3_CHRG);
		break;
		default:
		break;

		*bat_chrgng = STAT_CHRG_NONE;
	}
}

/**
 * Routine for energy management with 2 batteries
 *     | Init  | Cycle  |  1   |  2   |  3   |  4   | ..
 *	B0 | Fly   | Chrg   | Fly  | Chrg | Fly  | Chrg | .. 
 *  B1 | ----  | Fly    | Chrg | Fly  | Chrg | Fly  | ..
 */
//#if NUM_BATS == _2_BAT
void manage_2_BAT(void)
{
	if(*CHRG_CYCLE == false){
		if((float) get_volts_bat(*bat_active) <= BAT_MIN_VOLTS){
			// Check whether next battery is above its minimum State of charge
			if((float) get_volts_next_bat(*bat_active) <= BAT_MIN_VOLTS)
			{
				*EMERGENCY_STOP = true;
				return;
			}
			
			switch(usRegInputBuf[8]){
				case (uint16_t) STAT_ACT_B0:
					PORTA |= (1 << BAT_0_CTRL);		// PULL-UP!!  
					PORTA |= (1 << BAT_1_CTRL);
					// Charge Bat 0
					PORTA |= (1 << BAT_0_CHRG);

					// Update the indices here.
					usRegInputBuf[8] = STAT_ACT_B1;
					usRegInputBuf[9] = STAT_CHRG_B0;

					*CHRG_CYCLE = true;			/* Enable charge cycle */
				break;
				case (uint16_t) STAT_ACT_B1:
					// Should not happen..
				break;
				default:
				break;
			}

		}
	}else{
		if((float) get_volts_bat(*bat_active) <= BAT_MIN_VOLTS){
			// Check whether next battery is above its minimum State of charge
			if((float) get_volts_next_bat(*bat_active) <= BAT_MIN_VOLTS)
			{
				*EMERGENCY_STOP = true;
				return;
			}
			
			switch(usRegInputBuf[8]){
				case (uint16_t) STAT_ACT_B0:
					// (Dis)Active charging Bat 1
					PORTA &= ~(1 << BAT_1_CHRG);
					// (Dis)Activate Bat-(0)1
					PORTA |= (1 << BAT_0_CTRL);		// PULL-UP!! 
					PORTA |= (1 << BAT_1_CTRL);
					// Charge Bat 0
					PORTA |= (1 << BAT_0_CHRG);

					// Update the indices here.
					usRegInputBuf[8] = STAT_ACT_B1;
					usRegInputBuf[9] = STAT_CHRG_B0;
				break;
				case (uint16_t) STAT_ACT_B1:
					// (Dis)Active charging Bat 0
					PORTA &= ~(1 << BAT_0_CHRG);
					// (Dis)Activate Bat-(1)0
					PORTA &= ~(1 << BAT_1_CTRL); 
					PORTA &= ~(1 << BAT_0_CTRL);  	// PULL-UP!! 
					// Charge Bat 1
					PORTA |= (1 << BAT_1_CHRG);

					// Update the indices here.
					usRegInputBuf[8] = STAT_ACT_B0;
					usRegInputBuf[9] = STAT_CHRG_B1;
				break;

				default:
					// Should not happen..
				break;
			}	
		}else{		/* Check that charging battery is ok. */

			if((float) get_volts_bat(*bat_chrgng) >= BAT_MAX_VOLTS){
				stop_chargng_bat( );
			}
		}
	}
}
//#endif

/**
 *  Routine for energy management with 3 batteries
 *     | Init |   |   1  |  2   |  3   |  4   |  5   |  5   |  6   | ..
 *	B0 | Fly  | R | Chrg | R| Fly  | R | Chrg | R | Fly  |R | Ch..
 *  B1 | ---- | Fly  | R | Chrg | R | Fly  |R | Chrg | R | Fly  | .. 
 *  B2 | ---- | ---- | Fly  | R | Chrg | R | Fly | R | Chrg | R | .. 
 */

// Charging process here is activated according to the SOC of active battery.
//#if NUM_BATS == _3_BAT
void manage_3_BAT(void)
{
	if(*CHRG_CYCLE == false){
		if((float) get_volts_bat(*bat_active) <= BAT_MIN_VOLTS){
			
			if((float) get_volts_next_bat(*bat_active) <= BAT_MIN_VOLTS)
			{
				*EMERGENCY_STOP = true;
				return;
			}
			
			switch(*bat_active){

				case (uint16_t) STAT_ACT_B0:
					// (Dis)Activate Bat-(0)1
					PORTA |= (1 << BAT_0_CTRL);		// PULL-UP!!  
					PORTA |= (1 << BAT_1_CTRL);
					// Update the indices here.
					*bat_active = STAT_ACT_B1;

					*CHRG_CYCLE = true;			/* Enable charge cycle */
				break;

				case (uint16_t) STAT_ACT_B1:
				case (uint16_t) STAT_ACT_B2:
				default:
					// Should not happen..
				break;
			}
		}
	}else{
		if((float) get_volts_bat(*bat_active) <= BAT_MIN_VOLTS){
			
			if((float) get_volts_next_bat(*bat_active) <= BAT_MIN_VOLTS)
			{
				*EMERGENCY_STOP = true;
				return;
			}

			switch(*bat_active){

				case (uint16_t) STAT_ACT_B0:
					// (Dis)Activate Bat-(0)1
					PORTA |= (1 << BAT_0_CTRL);		// PULL-UP!!  	
					PORTA |= (1 << BAT_1_CTRL);	
					// Update the indices here.
					*bat_active = STAT_ACT_B1;
				break;

				case (uint16_t) STAT_ACT_B1:
					// (Dis)Activate Bat-(1)2
					PORTA &= ~(1 << BAT_1_CTRL);	// PULL-UP!!  
					PORTA |= (1 << BAT_2_CTRL);
					// Update the indices here.
					*bat_active = STAT_ACT_B2;
				break;

				case (uint16_t) STAT_ACT_B2:
					// (Dis)Activate Bat-(2)0
					PORTA &= ~(1 << BAT_2_CTRL);		 
					PORTA &= ~(1 << BAT_0_CTRL);	// PULL-UP!! 
					// Update the indices here.
					*bat_active = STAT_ACT_B0;
				break;

				default:
					// Should not happen..
				break;
			}

			
		}else if( (float) get_volts_bat(*bat_active) <= SOC_INIT_CHRG * BAT_MAX_VOLTS ){
			
			/* --- Charge after a certain rest time --- */
			switch(*bat_active){

				case (uint16_t) STAT_ACT_B0:
					stop_chargng_bat( );
					// Charge Bat 0
					PORTA |= (1 << BAT_2_CHRG);
					*bat_chrgng = STAT_CHRG_B2;
				break;

				case (uint16_t) STAT_ACT_B1:
					stop_chargng_bat( );
					// Charge Bat 0
					PORTA |= (1 << BAT_0_CHRG);
					*bat_chrgng = STAT_CHRG_B0;
				break;

				case (uint16_t) STAT_ACT_B2:
					stop_chargng_bat( );
					// Charge Bat 0
					PORTA |= (1 << BAT_1_CHRG);
					*bat_chrgng = STAT_CHRG_B1;
				break;

				default:
					// Should not happen..
				break;
			}
		
		}else{		/* Check that charging battery is o.k. */

			if((float) get_volts_bat(*bat_chrgng) >= BAT_MAX_VOLTS){
				stop_chargng_bat( );
			}
		}
	}
}
//#endif


/**
 * Routine for energy management with 4 batteries
 *     | Init |Cycle |  1   |  2   |  3   |  4   |   5  |   6  |  7   |  8   | ...
 *	B0 | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | ...
 *  B1 | ---- | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | ...
 *  B2 | ---- | ---- | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | ...
 *  B3 | ---- | ---- | ---- | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | Rest | Fly  | Rest | Chrg | ...
 */
//#if NUM_BATS == _4_BAT
void manage_4_BAT(void)
{
	
	if(*CHRG_CYCLE == false){
		if((float) get_volts_bat(*bat_active) <= BAT_MIN_VOLTS){
			
			if((float) get_volts_next_bat(*bat_active) <= BAT_MIN_VOLTS)
			{
				*EMERGENCY_STOP = true;
				return;
			}

			switch(*bat_active){

				case (uint16_t) STAT_ACT_B0:
					// (Dis)Activate Bat-(0)1
					PORTA |= (1 << BAT_0_CTRL);		// PULL-UP!! 
					PORTA |= (1 << BAT_1_CTRL);

					// Update the indices here.
					*bat_active = STAT_ACT_B1;
				break;

				case (uint16_t) STAT_ACT_B1:
					// (Dis)Activate Bat-(1)2
					PORTA &= ~(1 << BAT_1_CTRL); 
					PORTA |= (1 << BAT_2_CTRL);  
					// Charge Bat 0
					PORTA |= (1 << BAT_0_CHRG);

					// Update the indices here.
					*bat_active = STAT_ACT_B2;
					*bat_chrgng = STAT_CHRG_B0;

					*CHRG_CYCLE = true;			/* Enable charge cycle */
				break;

				case (uint16_t) STAT_ACT_B2:
				case (uint16_t) STAT_ACT_B3:
				default:
					// Should not happen..
				break;
			}
		}
	}else{
		if((float) get_volts_bat(*bat_active) <= BAT_MIN_VOLTS){
			
			if((float) get_volts_next_bat(*bat_active) <= BAT_MIN_VOLTS)
			{
				*EMERGENCY_STOP = true;
				return;
			}

			switch(*bat_active){

				case (uint16_t) STAT_ACT_B0:
					// (Dis)Activate Bat-(0)1
					PORTA |= (1 << BAT_0_CTRL); 	// PULL-UP!! 
					PORTA |= (1 << BAT_1_CTRL);  
					// Charge Bat 3
					PORTA |= (1 << BAT_3_CHRG);

					// Update the indices here.
					*bat_active = STAT_ACT_B1;
					*bat_chrgng = STAT_CHRG_B3;
				break;

				case (uint16_t) STAT_ACT_B1:
					// (Dis)Activate Bat-(1)2
					PORTA &= ~(1 << BAT_1_CTRL); 
					PORTA |= (1 << BAT_2_CTRL);  
					// Charge Bat 2
					PORTA |= (1 << BAT_0_CHRG);

					// Update the indices here.
					*bat_active = STAT_ACT_B2;
					*bat_chrgng = STAT_CHRG_B0;
				break;

				case (uint16_t) STAT_ACT_B2:
					// (Dis)Activate Bat-(2)3
					PORTA &= ~(1 << BAT_2_CTRL); 
					PORTA |= (1 << BAT_3_CTRL);  
					// Charge Bat 1
					PORTA |= (1 << BAT_1_CHRG);

					// Update the indices here.
					*bat_active = STAT_ACT_B3;
					*bat_chrgng = STAT_CHRG_B1;
				break;

				case (uint16_t) STAT_ACT_B3:
					// (Dis)Activate Bat-(3)0
					PORTA &= ~(1 << BAT_3_CTRL); 
					PORTA &= ~(1 << BAT_0_CTRL);  	// PULL-UP!! 
					// Charge Bat 2
					PORTA |= (1 << BAT_2_CHRG);

					// Update the indices here.
					*bat_active = STAT_ACT_B0;
					*bat_chrgng = STAT_CHRG_B1;
				break;

				default:
					// Should not happen..
				break;
			}

			
		}else{		/* Check that charging battery is ok. */

			if((float) get_volts_bat(*bat_chrgng) >= BAT_MAX_VOLTS){
				stop_chargng_bat( );
			}
		}
	}
}
//#endif

void energy_management(void){
	switch(*NUM_BATS){
		case NUM_2_BAT:
			manage_2_BAT( );
		break;
		case NUM_3_BAT:
			manage_3_BAT( );	// NOT TESTED
		break;
		case NUM_4_BAT:
			manage_4_BAT( );	// NOT TESTED
		break;
		default:
			// Won't happen..
		break;
	}
}

static void some_testing_stuff(void)
{
		// Testing
	//PORTA |= (1 << BAT_1_CHRG);
	//PORTA |= (1 << BAT_2_CHRG);
	// PORTA |= (1 << BAT_3_CHRG);
	
	// Test switching 
	// Version 1 -- Save
// 	PORTA |= (1 << BAT_1_CTRL);
// 	PORTA |= (1 << BAT_0_CTRL);
	// Version 2 -- Not as save
// 	PORTA |= (1 << BAT_1_CTRL);
// 	PORTA |= (1 << BAT_0_CTRL); 
	
	
	/* --- Test initialization holding registers --- */
// 	usRegHoldingBuf[0] = (uint16_t) 0x000a;
// 	usRegHoldingBuf[1] = (uint16_t) 0x000b;
// 	usRegHoldingBuf[2] = (uint16_t) 0x000c;
// 	usRegHoldingBuf[3] = (uint16_t) 0x000d;
// 	usRegHoldingBuf[4] = (uint16_t) 0x000e;
}

/**
 * MAIN-LOOP
 */
int main(void)
{

    /* Initialize routine.. */
	init_all();
	
	//some_testing_stuff( ); 
	//send_system_info_radio();
	

	config_lora_module();

	while(1)
	{
		/* ---- Check whether batteries are ok. ---- */
		if(assert_bats_ok( )){		// if not break the control loop.
				
			/************************************************************************************************/
			/*	Something is not okay with batteries so best would be to completely cut the					*/
			/*	connection!!! But This doesn't work as the MOSFET for Bat0 would be automatically switched  */
			/*	back on on power loss!!!																	*/
			/*	Responsibility lays in the hands of the experiment conductor.								*/
			/************************************************************************************************/
				
			*EMERGENCY_STOP = false;
		}else{
			*EMERGENCY_STOP = true;
		}

		cli();
		// Every 1s -- Fetch system data and do energy management
		if(adc_timer_ovf_cnt == ADC_TIMER_OVF_FACTOR)
		{		
			// Reset overflow counter
			adc_timer_ovf_cnt = 0;

			// Update ADC values
			fetch_adc_values( );
				
			/* ---------- Execute battery management routine ---------- */
// 				if(*MANUAL_MODE == false)
// 				{
// 					energy_management( );
// 				}

					
		}
		sei();

			
		cli();
		if(wimod_timer_ovf_cnt == WIMOD_TIMER_OVF_FACTOR)
		{
			wimod_timer_ovf_cnt = 0;
				
			send_system_info_radio();	
		}
		sei();
			
		/* ---------- Cyclical polling.. ---------- */
		/*
		if(*WIRELESS_MODE)
		{
			// .. the LoRa interface
			cli();
			if(BufferCount() > 0)
				WiMOD_HCI_ATmega64_Process( );
			sei();
		}
		else
		{
			// .. or the MODBUS 
			( void )eMBPoll( );
		}
		*/
			
		// .. or the MODBUS 
		( void )eMBPoll( );

 		if(*EMERGENCY_STOP == false && *MANUAL_MODE == false)
 		{
 			energy_management( );
		}
		else if(*MANUAL_MODE && *NEW_SETTINGS)
		{
			// Activate new settings fetched from holding registers...
			manual_set_active();
			manual_set_chrgng();
				
			*NEW_SETTINGS = false;
		}
			
	}
}

void manual_set_active(void){
	// Set all batteries to null (spares a switch statement, considering I don't want to refactor my 
	// data structures..)
	PORTA &= ~( (1 << BAT_3_CTRL) | (1 << BAT_2_CTRL) | (1 << BAT_1_CTRL) | (1 << BAT_0_CTRL));
	switch(*next_bat_active){
		case (uint16_t) STAT_ACT_B0:
			// Activate B0
			PORTA &= ~(1 << BAT_0_CTRL);		// PULL-UP
			*bat_active = STAT_ACT_B0;
		break;
		case (uint16_t) STAT_ACT_B1:
		// Activate B1
			PORTA |= (1 << BAT_1_CTRL);
			*bat_active = STAT_ACT_B1;
		break;
		case (uint16_t) STAT_ACT_B2:
			// Activate B2
			PORTA |= (1 << BAT_2_CTRL);
			*bat_active = STAT_ACT_B2;
		break;
		case (uint16_t) STAT_ACT_B3:
			// Activate B3
			PORTA |= (1 << BAT_3_CTRL);
			*bat_active = STAT_ACT_B3;
		break;
		case (uint16_t) STAT_ACT_NONE:
			// No battery charging..
			*bat_active = STAT_ACT_NONE;
		default:
		break;
	}	
}

void manual_set_chrgng(void){
	// Set all batteries to null (spares a switch statement, considering I don't want to refactor my 
	// data structures..)
	PORTA &= ~( (1 << BAT_3_CHRG) | (1 << BAT_2_CHRG) | (1 << BAT_1_CHRG) | (1 << BAT_0_CHRG));
	switch(*next_bat_chrgng){
		case (uint16_t) STAT_CHRG_B0:
			// Activate charging B0
			PORTA |= (1 << BAT_0_CTRL);
			*bat_chrgng = STAT_CHRG_B0;
		break;
		case (uint16_t) STAT_CHRG_B1:
		// Activate charging B1
			PORTA |= (1 << BAT_1_CHRG);
			*bat_chrgng = STAT_CHRG_B1;
		break;
		case (uint16_t) STAT_CHRG_B2:
			// Activate charging B2
			PORTA |= (1 << BAT_2_CHRG);
			*bat_chrgng = STAT_CHRG_B2;
		break;
		case (uint16_t) STAT_CHRG_B3:
			// Activate charging B3
			PORTA |= (1 << BAT_3_CHRG);
			*bat_chrgng = STAT_CHRG_B3;
		break;
		case (uint16_t) STAT_CHRG_NONE:
			// No battery charging..
			*bat_chrgng = STAT_CHRG_NONE;
		default:
		break;
	}
}

void settings_to_holding(void)
{
	/* --- Write settings to holding registers --- */
	usRegHoldingBuf[0] = (int16_t) MANUAL_MODE;
	usRegHoldingBuf[1] = (int16_t) NUM_BATS;
	usRegHoldingBuf[2] = (int16_t) BAT_CELLS;
	usRegHoldingBuf[3] = (int16_t) *bat_active;
	usRegHoldingBuf[4] = (int16_t) *bat_chrgng;
}

void mode_settings_init(void)
{
	/* ---------- Set status flags for the battery management ---------- */
	*bat_active = STAT_ACT_B0;				// Bat 3 is active on default
	*bat_chrgng = STAT_CHRG_NONE;			// No battery charging
	
	/* --- Set the holding registers for settings --- */
	*MANUAL_MODE	   = false;				// usRegHoldingBuf[0] = false;				// MANUAL_MODE
	usRegHoldingBuf[1] = NUM_2_BAT;			// NUM_BATs
	usRegHoldingBuf[2] = BAT_CELLS;			// BAT_CELLS
	usRegHoldingBuf[3] = STAT_ACT_B0;		// ACTIVE_BAT
	usRegHoldingBuf[4] = STAT_CHRG_NONE;	// CHRGNG_BAT
	*WIRELESS_MODE	   = false;					// usRegHoldingBuf[5] = false;				// COMM_MODE
	*NEW_SETTINGS	   = false;					// usRegHoldingBuf[6] = false;				// NEW_SETTINGS
	*CHRG_CYCLE		   = false;					// usRegHoldingBuf[7] = false;				// CHRG_CYCLE active?		
	*EMERGENCY_STOP	   = false;				// usRegHoldingBuf[8] = false;				// EMERGENCY_STOP active?
}

/*
 * Initializes timer registers (TIMER 0 8-Bit). 
 */
void timer_init(void){
	TIMSK = (1 << TOIE0);
	// Prescaler of 1024: 	t_interval = ( 2^8 * 1024 ) / 7372800 = 0.036s
	TCCR0 = ( (1 << CS01) | (1 << CS01) | (1 << CS00) );
	TCNT0 = 0;
	adc_timer_ovf_cnt = 0;
	wimod_timer_ovf_cnt = 0;
}

/*
 * Initializes output pins used in the management routines. 
 */
void ctrl_pin_init(void) {
	// Set pins PA0...PA7 as OUTPUT in GPIO Register A
	DDRA |=  ( (1 << PA7) | (1 << PA6) | (1 << PA5) | (1 << PA4)  | (1 << PA3) | (1 << PA2) | (1 << PA1) | (1 << PA0) );
	// Set all pins to zero in PORTA
	PORTA &= ~ ( (1 << PA7) | (1 << PA6) | (1 << PA5) | (1 << PA4)  | (1 << PA3) | (1 << PA2) | (1 << PA1) | (1 << PA0) );
}


/*
 * Configure the LoRa wireless module. 
 * 
 * Only settings that are changed are the Group Addr. and Device Addr.
 */
void config_lora_module(void){
	TWiMOD_HCI_Message configMsg;
	configMsg.SapID = DEVMGMT_ID;								// 0x01
	configMsg.MsgID = DEVMGMT_MSG_SET_RADIO_CONFIG_REQ;			// 0x11
	
	/* --------------- Payload -------------- */
	configMsg.Payload[0] = 0x00;		// Store NVM Flag
	/* ---------- Radio Configuration Field ---------- */
	configMsg.Payload[1] = 0x00;						// Radio Mode
	configMsg.Payload[2] = 0x25;						// Group Address
	configMsg.Payload[3] = 0x25;						// Tx Group Address
	configMsg.Payload[4] = 0x11;						// Device Address [low-byte]
	configMsg.Payload[5] = 0x47;						// Device Address [high-byte]
	configMsg.Payload[6] = 0x11;						// Tx Device Address [low-byte]
	configMsg.Payload[7] = 0x47;						// Tx Device Address [high-byte]
	configMsg.Payload[8] = 0x00;						// Modulation: 0x00 == LoRa
	configMsg.Payload[9] = 0x99;						// RF Carrier Frequency [least significant bits]
	configMsg.Payload[10] = 0x61;						// RF Carrier Frequency [intermediate bits]
	configMsg.Payload[11] = 0xd9;						// RF Carrier Frequency [most significant bits]
	configMsg.Payload[12] = 0x00;						// LoRa Signal Bandwidth
	configMsg.Payload[13] = 0x0b;						// LoRa Spreading Factor
	configMsg.Payload[14] = 0x02;						// Error Coding
	configMsg.Payload[15] = 0x11;						// Power level
	configMsg.Payload[16] = 0x00;						// Tx control
	configMsg.Payload[17] = 0x01;						// Rx control
	configMsg.Payload[18] = 0xb8;						// Rx window time [low byte]
	configMsg.Payload[19] = 0x0b;						// Rx window time [high byte]
	configMsg.Payload[20] = 0x07;						// LED control
	configMsg.Payload[21] = 0x03;						// Misc. Options
	configMsg.Payload[22] = 0x02;						// FSK Datarate
	configMsg.Payload[23] = 0x00;						// Automatic Power Saving
	configMsg.Payload[24] = 0x00;						// LBT Threshold [low-byte]
	configMsg.Payload[25] = 0x00;						// LBT Threshold [high-byte]
	/* ------------ Length ------------ */
	configMsg.Length = 1 + 25;

	WiMOD_HCI_SendMessage(&configMsg);
	
	/*memset(&configMsg, 0, sizeof(TWiMOD_HCI_Message));
	configMsg.SapID = 0x01;			// DEVMGMT_ID
	configMsg.MsgID = 0x13;			// DEVMGMT_MSG_GET_RADIO_CONFIG_REQ
	configMsg.Length = 0;
	WiMOD_HCI_SendMessage(&configMsg);*/
}

/*
 * Central initialization method
 */
void init_all(){
	cli();
	
	ctrl_pin_init();
	
	// ONLY FOR TESTING PURPOSES (wimod interface) -- Normally claimed by MODBUS
	//uart0_init(7); //115200 8N1 -- USB
	
	uart1_init(7); //115200 8N1 -- WiMOD
	
	spi_init();
	
	/* ---------- Initialize Timer ---------- */
	timer_init();
	
	mode_settings_init();
		
	/* ---------- Make an initial fetch of ADC values ---------- */
	fetch_adc_values( );

	
	const char _BLANK = "BLANK";
	WiMOD_HCI_Init(_BLANK, wimod_rx_callback, &rxMessage);


	/* ---------- MODBUS initialization as AVR/port/demo.c ---------- */
    eStatus = eMBInit( MB_MODE, SLAVE_ADDRESS, PORT, BAUDRATRE, PARITY );

    eStatus = eMBSetSlaveID( 0x34, TRUE, ucSlaveID, 3 );
    //sei(  );

    /* Enable the MODBUS Protocol Stack. */
    eStatus = eMBEnable(  );
	/* ---------- AVR/port/demo.c definitions ---------- */ 
	sei();
	
	//config_lora_module( );
	
}


/*
 * Callback for the Input Registers in the MODBUS.
 */
eMBErrorCode
eMBRegInputCB( UCHAR * pucRegBuffer, USHORT usAddress, USHORT usNRegs )
{
    eMBErrorCode    eStatus = MB_ENOERR;
    int             iRegIndex;

    if( ( usAddress >= REG_INPUT_START )
        && ( usAddress + usNRegs <= REG_INPUT_START + REG_INPUT_NREGS ) )
    {
        iRegIndex = ( int )( usAddress - usRegInputStart );
        while( usNRegs > 0 )
        {
            *pucRegBuffer++ =
                ( unsigned char )( usRegInputBuf[iRegIndex] >> 8 );
            *pucRegBuffer++ =
                ( unsigned char )( usRegInputBuf[iRegIndex] & 0xFF );
            iRegIndex++;
            usNRegs--;
        }
    }
    else
    {
        eStatus = MB_ENOREG;
    }


	/* Trigger the WiMOD transmission here too. */

    return eStatus;
}


eMBErrorCode
eMBRegHoldingCB( UCHAR * pucRegBuffer, USHORT usAddress, USHORT usNRegs,
                 eMBRegisterMode eMode )
{
	eMBErrorCode    eStatus = MB_ENOERR;
    int             iRegIndex;
	
	switch(eMode){
		case MB_REG_READ:
		// On read just return the holding registers same procedure as for Input registers (copy-paste).
			if( ( usAddress >= REG_HOLDING_START )
				&& ( usAddress + usNRegs <= REG_HOLDING_START + REG_HOLDING_NREGS ) )
			{
				iRegIndex = ( int )( usAddress - usRegHoldingStart );
				while( usNRegs > 0 )
				{
					*pucRegBuffer++ =
						( unsigned char )( usRegHoldingBuf[iRegIndex] >> 8 );
					*pucRegBuffer++ =
						( unsigned char )( usRegHoldingBuf[iRegIndex] & 0xFF );
					iRegIndex++;
					usNRegs--;
				}
			}
			else
			{
				eStatus = MB_ENOREG;
			}
		
		break;
		case MB_REG_WRITE:
		// On Write set the holding registers to new values and in case of manual mode set
		// active and charging bat accordingly
			if( ( usAddress >= REG_HOLDING_START )
				&& ( usAddress + usNRegs <= REG_HOLDING_START + REG_HOLDING_NREGS ) )
			{
				iRegIndex = ( int )( usAddress - usRegHoldingStart );
				while( usNRegs > 0 )
				{
					usRegHoldingBuf[iRegIndex] = 
						( unsigned short ) ( *pucRegBuffer++ << 8 );
					usRegHoldingBuf[iRegIndex] |= ( unsigned short ) *pucRegBuffer++;
					iRegIndex++;
					usNRegs--;
				}
				
				/* ---
 
					Indicate new settings..
					
				--- */
				*NEW_SETTINGS = true;
			}
			else
			{
				eStatus = MB_ENOREG;
			}
		
		break;
		default:
		break;
	}
	
	// Holding registers have to be updated anytime the management routines do alterations.
    return eStatus;
}


eMBErrorCode
eMBRegDiscreteCB( UCHAR * pucRegBuffer, USHORT usAddress, USHORT usNDiscrete )
{
    return MB_ENOREG;
}


eMBErrorCode
eMBRegCoilsCB( UCHAR * pucRegBuffer, USHORT usAddress, USHORT usNCoils,
               eMBRegisterMode eMode )
{
    return MB_ENOREG;
}


/************************************************************************/
/* Interrupt Service Routines                                           */
/************************************************************************/

ISR(TIMER0_OVF_vect){
	adc_timer_ovf_cnt++;
	wimod_timer_ovf_cnt++;
}

ISR(USART1_RX_vect)
{
	/*******************************************************************************************/
	/* I assume no buffering is need if Wimod interface lays in between						   */
	/* This may not be needed when using the wimod process function in main loop			   */
	/* Optionally call the process function here.											   */
	/*******************************************************************************************/
	unsigned char irxdata;
	irxdata = UDR1;
	//uart0_tx_byte(&irxdata);
	BufferIn(irxdata);
	
	//WiMOD_HCI_ATmega64_Process(serialRXBuffer, sizeof(serialRXBuffer));
	//WiMOD_HCI_ATmega64_Process();
	//SLIP_ProcessRxByte(&UDR1);

}
