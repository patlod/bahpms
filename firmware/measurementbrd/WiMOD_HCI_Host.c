/*
 * WiMOD_HCI_Host.c
 *
 * Created: 22.05.2020 11:07:02
 *  Author: patrickl
 */ 

#include "WiMOD_HCI_Host.h"

/* ------------- Test variables ------------- */
unsigned char SLIP_END = 0xC0;
// Test packet to test wireless connection (TODO: define struct for this)
UINT8 test_packet[25] = {0xc0, 0x03, 0x01, 0x25, 0x34, 0x12, 0, 0, 0, 0, 0xcb, 0x00, 0xc0};


/*!
 * Callback and protocol specific functions that are called when a WiMOD frame is received by Wimod HCI Layer. 
 * The example below explains a typical WiMOD serial frame.
 * NOTES: 
 *     - For serial transmission a WiMOD contains additional data called SLIP-Bytes.
 *     - The user payload of the message field usually contains a customized protocol extension
 *       to include use-case specific commands in WiMOD message. This is called myWiMOD-Host-Extension.
 *       This would be necessary e.g. when different actions should be triggered via a communication method.
 * 
 * PDU stands for Protocol Data Unit
 *
 * <code>
 *  |<-------------------------------- WiMOD Serial UART PDU (1) ------------------------------------------------->|
 *             |<-------------------------- WiMOD HCI Message Format (1') ------------------->|       
 *  +----------+-------+-------+--------------------------------------------------------------+---------+----------+
 *  | SLIP-END | SapID | MsgId |              Payload									      | CRC/LRC | SLIP-END |
 *  +----------+-------+-------+--------------------------------------------------------------+---------+----------+
 *  |          |       |       |															  |		    |
 * (2)       (3/2')  (4/3')  (5/4')															(6/5')	   (7)
 *							   |															  |
 *							   |															  |
 *							   |<-------- Payload: (Rx/Tx) Radio Message Field (1'') -------->|
 *							   +-----------+-----------+-----------+-----------+--------------+
 *							   | Dst. Grp. | Dst. Dev. | Src. Grp. | Src. Dev. | User Payload |
 *							   | Addr.     | Addr.	   | Addr.     | Addr.     | 			  |
 *							   +-----------+-----------+-----------+-----------+--------------+
 *							   |		   |		   |		   |		   |			  |
 *							  (2'')		 (3'')		 (4'')		 (5'')		 (6'')
 *																			/
 * Detailed user payload for monitoring board use case. (With myWiMOD-Host-Extension):
 *																		  / 
 *	    +----------------------------------------------------------------+
 *     /<---------------------------------------------------------------------------- User Payload (1''') -------------------- ... 
 *    +-------------------+--------+--------+--------+--------+---------+---------+----------+--------+------------+------------
 *    | myWiMOD-Host-Func | U_BAT0 | U_BAT1 | U_BAT2 | U_BAT3 | U_Solar | I_Solar | I_Charge | I_Load | Active Bat | Chrgng Bat ..
 *    +-------------------+--------+--------+--------+--------+---------+---------+----------+--------+------------+------------
 *    |					  |        |        |        |        |         |         |          |        |            |		
 *	 (1''')			   (2''')	 (3''')  (4''')   (5''')   (6''')    (7''')    (8''')	  (9''')   (10''')      (11''')
 *
 *          .. -------------------------------------------------------------------->|
 *            +---------+--------+--------------+----------+------------+-----------+ 			
 *			..| Op_mode | # Bats | # Bat. Cells | Act. Bat | Chrgng Bat | Comm Mode |
 *			  +---------+--------+--------------+----------+------------+-----------+
 *			  |         |        |              |          |            |           |
 *         (13''')   (14''')  (15''')        (16''')    (17''')      (18''')      
 *
 * </code>
 */

static void process_devmgmgt(TWiMOD_HCI_Message * rxMessage)
{
	switch(rxMessage->MsgID){
		case DEVMGMT_MSG_GET_RADIO_CONFIG_REQ:
		uart0_tx_byte(&SLIP_END);
		case DEVMGMT_MSG_SET_RADIO_CONFIG_RSP:
		uart0_tx_byte(&SLIP_END);
		default:
		break;
	}
}

static void process_rlt(TWiMOD_HCI_Message * rxMessage)
{
	switch(rxMessage->MsgID){
		default:
		break;
	}
}

static void process_radiolink(TWiMOD_HCI_Message * rxMessage)
{
	//uart0_tx_packet(&rxMessage->Payload, rxMessage->Length);
	switch(rxMessage->MsgID){
		case RADIOLINK_MSG_SEND_U_DATA_RSP:
		uart0_tx_packet(&rxMessage->Payload, rxMessage->Length);
		break;
		case RADIOLINK_MSG_U_DATA_RX_IND:
		//uart0_tx_packet(&rxMessage->Payload, rxMessage->Length);
		send_system_info_radio();
		break;
		case RADIOLINK_MSG_U_DATA_TX_IND:
		uart0_tx_packet(&rxMessage->Payload, rxMessage->Length);
		break;
		case RADIOLINK_MSG_C_DATA_RX_IND:
		// Check whether data request or system settings.
		// When data request send data accordingly 
		// For system settings the ACK is sufficient.
		break;
		case RADIOLINK_MSG_ACK_RX_IND:
		break;
		default:
		break;
	}
}

static void process_remote_ctrl(TWiMOD_HCI_Message * rxMessage)
{
	switch(rxMessage->MsgID){
		default:
		break;
	}
}

static void process_hwtest(TWiMOD_HCI_Message * rxMessage)
{
	switch(rxMessage->MsgID){
		default:
		break;
	}
}

static void process_myWiMOD_extension(UINT8 *payload)
{
	// Fetch the first byte in the payload which is function code.
	
	// Exec function or work on data
}


TWiMOD_HCI_Message*
wimod_rx_callback(TWiMOD_HCI_Message* rxMessage)
{

	unsigned char brk[24] = { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
	
	// 	uart0_tx_byte(&rxMessage->SapID);
	// 	uart0_tx_byte(&rxMessage->MsgID);
	// 	uart0_tx_packet(&rxMessage->Payload, rxMessage->Length);
	//uart0_tx_byte(&brk);
	
	switch(rxMessage->SapID){
		case DEVMGMT_ID:
		process_devmgmgt(rxMessage);
		break;
		case RLT_ID:
		process_rlt(rxMessage);
		break;
		case RADIOLINK_ID:
		process_radiolink(rxMessage);
		break;
		case REMOTE_CTRL_ID:
		process_remote_ctrl(rxMessage);
		break;
		case HWTEST_ID:
		process_hwtest(rxMessage);
		break;
		default:
		// Should not happen..
		break;
	}

	memset(rxMessage, 0, sizeof(TWiMOD_HCI_Message));
	return rxMessage;
}




void send_system_info_radio(void)
{
	/* Send data per WiMOD interface */
	// Test message
	TWiMOD_HCI_Message testMsg;
	testMsg.SapID = RADIOLINK_ID;		
	testMsg.MsgID = RADIOLINK_MSG_SEND_U_DATA_REQ; 
	testMsg.Payload[0] = GROUP_ADDR;			// Dst. Group Addr
	testMsg.Payload[1] = 0x34;				// Dst. Device Addr low-byte
	testMsg.Payload[2] = 0x12;				// Dst. Device Addr high-byte
	testMsg.Payload[3] = 0;
	testMsg.Payload[4] = 0;
	testMsg.Payload[5] = 0;
	testMsg.Payload[6] = 0;
	testMsg.CRC16[0] = 0;
	testMsg.CRC16[1] = 0;
	testMsg.Length = 7;		// is size of payload field so: (SapID, MsgID | Payload | CRC ) without (SapID, MsgID) and (CRC)
	
	//_delay_ms(1);

	WiMOD_HCI_SendMessage(&testMsg);
}

