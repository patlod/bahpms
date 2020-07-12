using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWComClient
{
    public static class WiMOD_HCI_Layer
    {

    }

    static class SerialDevice
    {
        private static int BAUD = 115200;

        public static void printHexBuffer(byte[] buf)
        {

        }

        public static Boolean openSerial(string comPort)
        {
            return false;
        }

        public static void closeSerial()
        {

        }

        public static byte[] readSerial()
        {
            return null;
        }

        public static Boolean writeSerial(byte[] buf)
        {
            return false;
        }
    }

    static class SLIP
    {
        private static int SLIP_END         = 0xC0;     // Dez. 192
        private static int SLIP_ESC         = 0xDB;     // Dez. 219
        private static int SLIP_ESC_END     = 0xDC;     // Dez. 220
        private static int SLIP_ESC_ESC     = 0xDD;     // Dez. 221

        private static int SLIPDEC_IDLE_STATE       = 0;
        private static int SLIPDEC_START_STATE      = 1;
        private static int SLIPDEC_IN_FRAME_STATE   = 2;
        private static int SLIPDEC_ESC_TABLE        = 3;
    }

    static class CRC16
    {
         
    }
}
