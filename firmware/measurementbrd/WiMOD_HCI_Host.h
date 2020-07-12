/*
 * IncFile1.h
 *
 * Created: 22.05.2020 11:07:21
 *  Author: patrickl
 */ 


#ifndef INCFILE1_H_
#define INCFILE1_H_

#include "WiMOD/WiMOD_HCI_Layer.h"
#include "WiMOD/SLIP.h"
#include "UART/UART0.h"
#include "UART/UART1.h"
#include "ringbuffer.h"

/* ----------------------------------------------- WiMOD HCI ---------------------------------------------------------*/

/* --------------- Endpoint Identifiers --------------- */
#define DEVMGMT_ID										0x01
#define RLT_ID											0x02
#define RADIOLINK_ID									0x03
#define REMOTE_CTRL_ID									0x04
#define HWTEST_ID										0xA1

/* ------------------------------ Device Management Identifier  ------------------------------ */
/* -------- Device Management Message Identifier -------- */
#define DEVMGMT_MSG_PING_REQ							0x01
#define DEVMGMT_MSG_PING_RSP							0x02

#define DEVMGMT_MSG_GET_DEVICE_INFO_REQ					0x03
#define DEVMGMT_MSG_GET_DEVICE_INFO_RSP					0x04

#define DEVMGMT_MSG_GET_FW_INFO_REQ						0x05
#define DEVMGMT_MSG_GET_FW_INFO_RSP						0x06

#define DEVMGMT_MSG_RESET_REQ							0x07
#define DEVMGMT_MSG_RESET_RSP							0x08

#define DEVMGMT_MSG_SET_OPMODE_REQ						0x09
#define DEVMGMT_MSG_SET_OPMODE_RSP						0x0A
#define DEVMGMT_MSG_GET_OPMODE_REQ						0x0B
#define DEVMGMT_MSG_GET_OPMODE_RSP						0x0C

#define DEVMGMT_MSG_SET_RTC_REQ							0x0D
#define DEVMGMT_MSG_SET_RTC_RSP							0x0E
#define DEVMGMT_MSG_GET_RTC_REQ							0x0F
#define DEVMGMT_MSG_GET_RTC_RSP							0x10

#define DEVMGMT_MSG_SET_RADIO_CONFIG_REQ				0x11
#define DEVMGMT_MSG_SET_RADIO_CONFIG_RSP				0x12
#define DEVMGMT_MSG_GET_RADIO_CONFIG_REQ				0x13
#define DEVMGMT_MSG_GET_RADIO_CONFIG_RSP				0x14
#define DEVMGMT_MSG_RESET_RADIO_CONFIG_REQ				0x15
#define DEVMGMT_MSG_RESET_RADIO_CONFIG_RSP				0x16

#define DEVMGMT_MSG_GET_SYSTEM_STATUS_REQ				0x17
#define DEVMGMT_MSG_GET_SYSTEM_STATUS_RSP				0x18

#define DEVMGMT_MSG_SET_RADIO_MODE_REQ					0x19
#define DEVMGMT_MSG_SET_RADIO_MODE_RSP					0x1A

#define DEVMGMT_MSG_ENTER_LPM_REQ						0x1B	// Firmware v1.6
#define DEVMGMT_MSG_ENTER_LPM_RSP						0x1C	// Firmware v1.6
#define DEVMGMT_MSG_POWER_UP_IND						0x20	// Firmware v1.6

#define DEVMGMT_MSG_SET_AES_KEY_REQ						0x21	// Firmware v1.10
#define DEVMGMT_MSG_SET_AES_KEY_RSP						0x22	// Firmware v1.10
#define DEVMGMT_MSG_GET_AES_KEY_REQ						0x23	// Firmware v1.10
#define DEVMGMT_MSG_GET_AES_KEY_RSP						0x24	// Firmware v1.10

/* -------- Device Management Status Byte -------- */
#define DEVMGMT_STATUS_OK								0x00
#define DEVMGMT_STATUS_ERROR							0x01
#define DEVMGMT_STATUS_CMD_NOT_SUPPORTED				0x02
#define DEVMGMT_STATUS_WRONG_PARAMETER					0x03


/* ------------------------------ Radio Link Identifier -------------------------------------- */

/* -------- Radio Link Message Identifier -------- */
#define RADIOLINK_MSG_SEND_U_DATA_REQ					0x01
#define RADIOLINK_MSG_SEND_U_DATA_RSP					0x02
#define RADIOLINK_MSG_U_DATA_RX_IND						0x04
#define RADIOLINK_MSG_U_DATA_TX_IND						0x06	// Firmware v1.6
#define RADIOLINK_MSG_RAW_DATA_RX_IND					0x08
#define RADIOLINK_MSG_SEND_C_DATA_REQ					0x09
#define RADIOLINK_MSG_SEND_C_DATA_RSP					0x0A
#define RADIOLINK_MSG_C_DATA_RX_IND						0x0C
#define RADIOLINK_MSG_C_DATA_TX_IND						0x0E
#define RADIOLINK_MSG_ACK_RX_IND						0x10
#define RADIOLINK_MSG_ACK_TIMEOUT_IND					0x12
#define RADIOLINK_MSG_ACK_TX_IND						0x14
#define RADIOLINK_MSG_SET_ACK_DATA_REQ					0x15
#define RADIOLINK_MSG_SET_ACK_DATA_RSP					0x16

/* -------- Radio Link Status Byte -------- */
#define RADIOLINK_STATUS_OK								0x00
#define RADIOLINK_STATUS_ERROR							0x01
#define RADIOLINK_STATUS_CMD_NOT_SUPPORTED				0x02
#define RADIOLINK_STATUS_WRONG_PARAMETER				0x03
#define RADIOLINK_STATUS_WRONG_RADIO_MODE				0x04
#define RADIOLINK_STATUS_MEDIA_BUSY						0x05
#define RADIOLINK_STATUS_BUFFER_FULL					0x07
#define RADIOLINK_STATUS_LENGTH_ERROR					0x08

/* ------------------------------ Radio Link Test Identifier ---------------------------------- */
/* -------- Radio Link Test Message Identifier -------- */
#define RLT_MSG_START_REQ								0x01
#define RLT_MSG_START_RSP								0x02
#define RLT_MSG_STOP_REQ								0x03
#define RLT_MSG_STOP_RSP								0x04
#define RLT_MSG_STATUS_IND								0x06

/* -------- Radio Link Test Status Byte -------- */
#define RLT_STATUS_OK									0x00
#define RLT_STATUS_ERROR								0x01
#define RLT_STATUS_CMD_NOT_SUPPORTED					0x02
#define RLT_STATUS_WRONG_PARAMETER						0x03
#define RLT_STATUS_WRONG_RADIO_MODE						0x04

/* ------------------------------ Hardware Test Identifier ------------------------------------ */
/* -------- Hardware Test Message Identifier -------- */
#define HWTEST_MSG_RADIO_TEST_REQ						0x01
#define HWTEST_MSG_RADIO_TEST_RSP						0x02

/* -------- Hardware Test Status Byte -------- */
#define HWTEST_STATUS_OK								0x00
#define HWTEST_STATUS_ERROR								0x01
#define HWTEST_STATUS_CMD_NOT_SUPPORTED					0x02
#define HWTEST_STATUS_WRONG_PARAMETER					0x03	


/* ------------------------------ Remote Control Identifier ------------------------------------ */
/* -------- Remote Control Message Identifier -------- */
#define REMOTE_CTRL_MSG_BUTTON_PRESSED_IND				0x02



/* --------------- Addressing --------------- */
#define GROUP_ADDR 0x25
#define HARDWARE_ADDR 0x4711
//#define USB1_ADDR 1201
#define USB2_ADDR 1234

//#define SERIAL_BUFFER_SIZE 40
//UINT8	serialRXBuffer[SERIAL_BUFFER_SIZE];
TWiMOD_HCI_Message rxMessage;


/* --------------- Methods --------------- */
TWiMOD_HCI_Message*
wimod_rx_callback(TWiMOD_HCI_Message* rxMessage);



#endif /* INCFILE1_H_ */