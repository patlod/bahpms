using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HWComClient
{
    public class WiMOD_HCI_Host
    {
        /* ----------------------------------------------- WiMOD HCI ---------------------------------------------------------*/

        /* --------------- Endpoint Identifiers --------------- */
        public const byte DEVMGMT_ID                            = 0x01;
        public const byte RLT_ID                                = 0x02;
        public const byte RADIOLINK_ID                          = 0x03;
        public const byte REMOTE_CTRL_ID                        = 0x04;
        public const byte HWTEST_ID                             = 0xA1;

        /* ------------------------------ Device Management Identifier  ------------------------------ */
        /* -------- Device Management Message Identifier -------- */
        public const byte DEVMGMT_MSG_PING_REQ                  = 0x01;
        public const byte DEVMGMT_MSG_PING_RSP                  = 0x02;

        public const byte DEVMGMT_MSG_GET_DEVICE_INFO_REQ       = 0x03;
        public const byte DEVMGMT_MSG_GET_DEVICE_INFO_RSP       = 0x04;

        public const byte DEVMGMT_MSG_GET_FW_INFO_REQ           = 0x05;
        public const byte DEVMGMT_MSG_GET_FW_INFO_RSP           = 0x06;

        public const byte DEVMGMT_MSG_RESET_REQ                 = 0x07;
        public const byte DEVMGMT_MSG_RESET_RSP                 = 0x08;

        public const byte DEVMGMT_MSG_SET_OPMODE_REQ            = 0x09;
        public const byte DEVMGMT_MSG_SET_OPMODE_RSP            = 0x0A;
        public const byte DEVMGMT_MSG_GET_OPMODE_REQ            = 0x0B;
        public const byte DEVMGMT_MSG_GET_OPMODE_RSP            = 0x0C;

        public const byte DEVMGMT_MSG_SET_RTC_REQ               = 0x0D;
        public const byte DEVMGMT_MSG_SET_RTC_RSP               = 0x0E;
        public const byte DEVMGMT_MSG_GET_RTC_REQ               = 0x0F;
        public const byte DEVMGMT_MSG_GET_RTC_RSP               = 0x10;

        public const byte DEVMGMT_MSG_SET_RADIO_CONFIG_REQ      = 0x11;
        public const byte DEVMGMT_MSG_SET_RADIO_CONFIG_RSP      = 0x12;
        public const byte DEVMGMT_MSG_GET_RADIO_CONFIG_REQ      = 0x13;
        public const byte DEVMGMT_MSG_GET_RADIO_CONFIG_RSP      = 0x14;
        public const byte DEVMGMT_MSG_RESET_RADIO_CONFIG_REQ    = 0x15;
        public const byte DEVMGMT_MSG_RESET_RADIO_CONFIG_RSP    = 0x16;

        public const byte DEVMGMT_MSG_GET_SYSTEM_STATUS_REQ     = 0x17;
        public const byte DEVMGMT_MSG_GET_SYSTEM_STATUS_RSP     = 0x18;

        public const byte DEVMGMT_MSG_SET_RADIO_MODE_REQ        = 0x19;
        public const byte DEVMGMT_MSG_SET_RADIO_MODE_RSP        = 0x1A;

        public const byte DEVMGMT_MSG_ENTER_LPM_REQ             = 0x1B;	// Firmware v1.6
        public const byte DEVMGMT_MSG_ENTER_LPM_RSP             = 0x1C;	// Firmware v1.6
        public const byte DEVMGMT_MSG_POWER_UP_IND              = 0x20;	// Firmware v1.6

        public const byte DEVMGMT_MSG_SET_AES_KEY_REQ           = 0x21;	// Firmware v1.10
        public const byte DEVMGMT_MSG_SET_AES_KEY_RSP           = 0x22;   // Firmware v1.10
        public const byte DEVMGMT_MSG_GET_AES_KEY_REQ           = 0x23;	// Firmware v1.10
        public const byte DEVMGMT_MSG_GET_AES_KEY_RSP           = 0x24;	// Firmware v1.10

        /* -------- Device Management Status Byte -------- */
        public const byte DEVMGMT_STATUS_OK                     = 0x00;
        public const byte DEVMGMT_STATUS_ERROR                  = 0x01;
        public const byte DEVMGMT_STATUS_CMD_NOT_SUPPORTED      = 0x02;
        public const byte DEVMGMT_STATUS_WRONG_PARAMETER        = 0x03;


        /* ------------------------------ Radio Link Identifier -------------------------------------- */

        /* -------- Radio Link Message Identifier -------- */
        public const byte RADIOLINK_MSG_SEND_U_DATA_REQ         = 0x01;
        public const byte RADIOLINK_MSG_SEND_U_DATA_RSP         = 0x02;
        public const byte RADIOLINK_MSG_U_DATA_RX_IND           = 0x04;
        public const byte RADIOLINK_MSG_U_DATA_TX_IND           = 0x06;	// Firmware v1.6   
        public const byte RADIOLINK_MSG_RAW_DATA_RX_IND         = 0x08;
        public const byte RADIOLINK_MSG_SEND_C_DATA_REQ         = 0x09;
        public const byte RADIOLINK_MSG_SEND_C_DATA_RSP         = 0x0A;
        public const byte RADIOLINK_MSG_C_DATA_RX_IND           = 0x0C;
        public const byte RADIOLINK_MSG_C_DATA_TX_IND           = 0x0E;
        public const byte RADIOLINK_MSG_ACK_RX_IND              = 0x10;
        public const byte RADIOLINK_MSG_ACK_TIMEOUT_IND         = 0x12;
        public const byte RADIOLINK_MSG_ACK_TX_IND              = 0x14;
        public const byte RADIOLINK_MSG_SET_ACK_DATA_REQ        = 0x15;
        public const byte RADIOLINK_MSG_SET_ACK_DATA_RSP        = 0x16;

        /* -------- Radio Link Status Byte -------- */
        public const byte RADIOLINK_STATUS_OK                   = 0x00;
        public const byte RADIOLINK_STATUS_ERROR                = 0x01;
        public const byte RADIOLINK_STATUS_CMD_NOT_SUPPORTED    = 0x02; 
        public const byte RADIOLINK_STATUS_WRONG_PARAMETER      = 0x03;
        public const byte RADIOLINK_STATUS_WRONG_RADIO_MODE     = 0x04;
        public const byte RADIOLINK_STATUS_MEDIA_BUSY           = 0x05;
        public const byte RADIOLINK_STATUS_BUFFER_FULL          = 0x07;
        public const byte RADIOLINK_STATUS_LENGTH_ERROR	        = 0x08;

        /* ------------------------------ Radio Link Test Identifier ---------------------------------- */
        /* -------- Radio Link Test Message Identifier -------- */
        public const byte RLT_MSG_START_REQ	                    = 0x01;
        public const byte RLT_MSG_START_RSP                     = 0x02;
        public const byte RLT_MSG_STOP_REQ                      = 0x03;
        public const byte RLT_MSG_STOP_RSP                      = 0x04;
        public const byte RLT_MSG_STATUS_IND                    = 0x06;

        /* -------- Radio Link Test Status Byte -------- */
        public const byte RLT_STATUS_OK                         = 0x00;
        public const byte RLT_STATUS_ERROR                      = 0x01;
        public const byte RLT_STATUS_CMD_NOT_SUPPORTED          = 0x02;
        public const byte RLT_STATUS_WRONG_PARAMETER            = 0x03;
        public const byte RLT_STATUS_WRONG_RADIO_MODE           = 0x04;

        /* ------------------------------ Hardware Test Identifier ------------------------------------ */
        /* -------- Hardware Test Message Identifier -------- */
        public const byte HWTEST_MSG_RADIO_TEST_REQ             = 0x01;
        public const byte HWTEST_MSG_RADIO_TEST_RSP             = 0x02;

        /* -------- Hardware Test Status Byte -------- */
        public const byte HWTEST_STATUS_OK                      = 0x00;
        public const byte HWTEST_STATUS_ERROR                   = 0x01;
        public const byte HWTEST_STATUS_CMD_NOT_SUPPORTED       = 0x02;
        public const byte HWTEST_STATUS_WRONG_PARAMETER         = 0x03;


        /* ------------------------------ Remote Control Identifier ------------------------------------ */
        /* -------- Remote Control Message Identifier -------- */
        public const byte REMOTE_CTRL_MSG_BUTTON_PRESSED_IND    = 0x02;



        /* --------------- Addressing --------------- */
        public const byte GROUP_ADDR                            = 0x25;
        public const UInt16 HARDWARE_ADDR                       = 0x4711;
        //#define USB1_ADDR 1201
        public const UInt16 USB2_ADDR                           = 0x1234;

        //#define SERIAL_BUFFER_SIZE 40
        //UINT8	serialRXBuffer[SERIAL_BUFFER_SIZE];

        //TWiMOD_HCI_Message rxMessage;


        /* --------------- Methods --------------- */
        // TWiMOD_HCI_Message*
        //wimod_rx_callback(TWiMOD_HCI_Message* rxMessage);


        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool fibonacci_next();
        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void fibonacci_init(int a, int b);
        // Get the current value in the sequence.
        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 fibonacci_current();
        // Get the position of the current value in the sequence.
        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 fibonacci_index();

        // Just print some test string
        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void printYourName(string name);
        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int add(int a, int b);
        [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int subtract(int a, int b);
    }
}
