using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using EasyModbus;
using System.Threading;
using System.Runtime.InteropServices;

namespace HWComClient
{
    public partial class Form1 : Form
    {

        public static class WimodHCIWrapper
        {
            /**
             * General variables for WiMOD HCI Layer
             */
            public const UInt16 WIMOD_HCI_MSG_PAYLOAD_SIZE = 300;
            public const UInt16 WIMOD_HCI_MSG_FCS_SIZE = 2;


            /* ---------------------- WiMOD_HCI_Init -------------------------- */
            static byte[] Payload = new byte[WIMOD_HCI_MSG_PAYLOAD_SIZE];
            static byte[] CRC16 = new byte[WIMOD_HCI_MSG_FCS_SIZE];

            [StructLayout(LayoutKind.Sequential)]
            public struct TWiMOD_HCI_Message
            {
                public UInt16 Length;
                public byte SapID;
                public byte MsgID;
                public IntPtr Payload;
                public IntPtr CRC16;
            }

            public delegate IntPtr CallbackFunc(IntPtr rxMessage);


            [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool WiMOD_HCI_Init(string comPort,
                                                     CallbackFunc  cbRxMessage,
					                                 ref TWiMOD_HCI_Message rxMessage);

            public static IntPtr wimod_rx_callback(IntPtr rxMessage)
            {
                //TWiMOD_HCI_Message msgObject = new TWiMOD_HCI_Message();
                // Marshal.PtrToStructure(rxMessage, msgObject);

                //Debug.WriteLine(msgObject.SapID);
                //Debug.WriteLine(msgObject.MsgID);

                /*
                unsafe { 
                    byte *ptr = (byte *)rxMessage.ToPointer();

                }
                */
                return IntPtr.Zero;
            }

            public static bool wrap_WiMOD_HCI_Init()
            {
                GCHandle payloadHandle = GCHandle.Alloc(Payload, GCHandleType.Pinned);
                try
                {
                    GCHandle crc16Handle = GCHandle.Alloc(CRC16, GCHandleType.Pinned);
                    try
                    {
                        TWiMOD_HCI_Message rxMessage = new TWiMOD_HCI_Message();
                        rxMessage.Payload = payloadHandle.AddrOfPinnedObject();
                        rxMessage.CRC16 = crc16Handle.AddrOfPinnedObject();

                        bool ret = WiMOD_HCI_Init("\\\\.\\COM13",
                                          wimod_rx_callback,
                                          ref rxMessage);
                        return ret;
                    }
                    finally
                    {
                       crc16Handle.Free();
                    }
                }
                finally
                {
                    payloadHandle.Free();
                }
            }

            /* ------------------------------ WiMOD_HCI_Process ----------------------------- */
            public static byte[] rxBuf = new byte[100];


            [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void WiMOD_HCI_Process(ref byte[] rxBuf, int size);

            public static void wrap_WiMOD_HCI_Process()
            {
                WiMOD_HCI_Process(ref rxBuf, rxBuf.Length);
            }

            [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt16 printFromHCIDLL();

            [DllImport("Wimod_HCI_Layer.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt16 getWiMODPayloadSize();
        }

        public static class MathLibraryWrapper
        {
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool fibonacci_next();
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void fibonacci_init(int a, int b);
            // Get the current value in the sequence.
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt64 fibonacci_current();
            // Get the position of the current value in the sequence.
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt32 fibonacci_index();

            // Just print some test string
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void printYourName(string name);
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int add(int a, int b);
            [DllImport("MathLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int subtract(int a, int b);
        }
        

        // Packet size in bytes 
        //private int PACKET_SIZE = 20;
        // Request byte
        //private byte[] REQ_SERIAL = new byte[] {0x01};
        private Boolean CYCLIC_READ = false;


        /* ------- Variables MODBUS - Fixed values determined set for slave on FW side ------- */
        private EasyModbus.ModbusClient modbusClient;
        private int BAUDRATE = 115200;
        private byte SLAVE_ADDR = 0x0A;

        // Allocation Input Registers:
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
        private int INPUT_REG_START = 30000;
        private int INPUT_REG_NUM_REGS = 10;
        // Allocation I/O Holding Registers
        // 	[0] - Mode (Auto/Manual)
        // 	[1] - # Batteries
        // 	[2] - Active Battery
        // 	[3] - Charging Battery
        private int HOLDING_REG_START = 40000;
        private int HOLDING_REG_NUM_REGS = 9;

        public Form1()
        {
            InitializeComponent();

            initCbbPorts();

            initModeSelection();

            mountEventHandlers();

            /* initialise modbus client here.. */
            mountModbusMaster();

            /* Initialise the log file */
            DataLogging.setCurrentLogFile();
            Debug.WriteLine("Created a new log file at: " + DataLogging.CURRENT_LOG_FILE);

            //printYourName("David");

            Debug.WriteLine(MathLibraryWrapper.add(1, 2).ToString());

            Debug.WriteLine(MathLibraryWrapper.subtract(100, 1).ToString());

            Debug.WriteLine(WimodHCIWrapper.printFromHCIDLL());

            Debug.WriteLine(WimodHCIWrapper.wrap_WiMOD_HCI_Init());

            Debug.WriteLine(WimodHCIWrapper.getWiMODPayloadSize());


            /*fibonacci_init(1,1);

            do
            {
                Debug.WriteLine(fibonacci_index() + ": " + fibonacci_current());
                fibonacci_next();
            } while (fibonacci_index() <= 20);*/


        }


        /*private void BtnRxLora_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Triggered the WiMOD event polling loop..");
            while (true)
            {
                string[] r = lora_modem.testEventPolling();
                if (r != null)
                {
                    for(int i = 0; i < r.Length; i++)
                    {
                        Debug.Write(r[i] + " ");
                    }
                    Debug.Write("\n");

                }
                    
            }
        }*/

        private void ComboBoxPorts_Click(object sender, EventArgs e)
        {
            comboBoxPorts.Items.Clear();
            comboBoxPorts.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

            private void initCbbPorts()
        {
            comboBoxPorts.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        private void initModeSelection()
        {
            // Default is: Auto Mode + 2 batteries + 2 cell batteries
            // Init cbb select mode
            cbbModeSelect.Items.AddRange(new string[] { "Auto", "Manual" });
            cbbModeSelect.SelectedIndex = 0;
            // Init cbb num bats
            cbbNumBats.Items.AddRange(new string[] { "2", "3", "4" });
            cbbNumBats.SelectedIndex = 0;
            // Cells will be not adjustable for now.
            cbbNumBatCells.Enabled = false;
            string[] num_cells = new string[] { "2", "3", "4" };
            cbbNumBatCells.Items.AddRange(num_cells);
            cbbNumBatCells.SelectedIndex = 0;

            cbbActiveBat.Items.AddRange(new string[] { "0", "1" });
            cbbActiveBat.Visible = false;
            cbbActiveBatLabel.Visible = false;
            cbbChrgBat.Items.AddRange(new string[] { "none", "0", "1" });
            cbbChrgBat.Visible = false;
            cbbChrgBatLabel.Visible = false;

            btnApplySettings.Location = new Point(106, 231);
        }

        private void mountEventHandlers()
        {
            comboBoxPorts.Click += new EventHandler(ComboBoxPorts_Click);
            btnPortConnect.Click += new EventHandler(btnPortConnect_Click);
            btnRead1.Click += new EventHandler(btnRead1_Click);
            btnCyclicRead.Click += new EventHandler(btnCyclicRead_Click);

            cbbModeSelect.SelectedIndexChanged += new EventHandler(CbbModeSelect_SelectedIndexChanged);
            cbbNumBats.SelectedIndexChanged += new EventHandler(CbbNumBats_SelectedIndexChanged);
            cbbActiveBat.SelectedIndexChanged += new EventHandler(CbbActiveBat_SelectedIndexChanged);
            cbbChrgBat.SelectedIndexChanged += new EventHandler(CbbChrgBat_SelectedIndexChanged);
            btnApplySettings.Click += new EventHandler(btnApplySettings_Click);
        }

        private void CbbModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbModeSelect.SelectedIndex == 1) {

                btnApplySettings.Location = new Point(106, 350);

                cbbActiveBat.Visible = true;
                cbbActiveBatLabel.Visible = true;

                cbbChrgBat.Visible = true;
                cbbChrgBatLabel.Visible = true;
            }
            else if (cbbModeSelect.SelectedIndex == 0)
            {
                cbbActiveBat.Visible = false;
                cbbActiveBatLabel.Visible = false;
                cbbActiveBat.SelectedIndex = -1;

                cbbChrgBat.Visible = false;
                cbbChrgBatLabel.Visible = false;
                cbbChrgBat.SelectedIndex = -1;

                btnApplySettings.Location = new Point(106, 231);
            }
            else
            {
                // None..
            }
        }

        private void CbbNumBats_SelectedIndexChanged(object sender, EventArgs e)
        {
                switch (cbbNumBats.SelectedIndex)
                {
                    case 0:  // 2 batteries
                        cbbActiveBat.SelectedIndex = -1;
                        cbbActiveBat.Items.Clear();
                        cbbActiveBat.Items.AddRange(new string[] { "0", "1" });

                        cbbChrgBat.SelectedIndex = -1;
                        cbbChrgBat.Items.Clear();
                        cbbChrgBat.Items.AddRange(new string[] { "none", "0", "1" });                        
                        break;

                    case 1:  // 3 batteries
                        cbbActiveBat.SelectedIndex = -1;
                        cbbActiveBat.Items.Clear();
                        cbbActiveBat.Items.AddRange(new string[] { "0", "1", "2" });

                        cbbChrgBat.SelectedIndex = -1;
                        cbbChrgBat.Items.Clear();
                        cbbChrgBat.Items.AddRange(new string[] { "none", "0", "1", "2" });
                        break;

                    case 2:  // 4 batteries
                        cbbActiveBat.SelectedIndex = -1;
                        cbbActiveBat.Items.Clear();
                        cbbActiveBat.Items.AddRange(new string[] { "0", "1", "2", "3" });

                        cbbChrgBat.SelectedIndex = -1;
                        cbbChrgBat.Items.Clear();
                        cbbChrgBat.Items.AddRange(new string[] { "none", "0", "1", "2", "3" });
                        break;

                    default:
                        break;
                }
            
        }

        private void CbbActiveBat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(cbbActiveBat.SelectedItem);
            if(cbbActiveBat.SelectedItem == cbbChrgBat.SelectedItem)
            {
                cbbActiveBat.SelectedIndex = -1;

                // Error charging battery can't be active
            }

            // O.k.
        }

        private void CbbChrgBat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(cbbChrgBat.SelectedItem);
            if (cbbChrgBat.SelectedItem == cbbActiveBat.SelectedItem)
            {
                cbbChrgBat.SelectedIndex = -1;
                // Error charging battery can't be active
            }

            // O.k.
        }

        private void clearCurrentSettings()
        {
            txtStateModeSettings.Items.Clear();
        }

        private void alertEmergencyStop()
        {
            string message = "Management board stopped automatic algorithms.\n\nMake sure to use the right setup and all batteries are connected.";
            string title = "Emergency Stop";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        }

        /**
         * Helper method for updating the display of current settings..
         */
        private void updateCurrentSettings(int[] serverResponse)
        {
            Debug.WriteLine("Updating Current Settings display");

            clearCurrentSettings();

            string comm_mode = "Comm Mode: \t" + (((ushort)serverResponse[5] == 0) ? "MODBUS" : "WiMOD");
            txtStateModeSettings.Items.Add(comm_mode);
            string op_mode = "Op Mode: \t" + ((((ushort)serverResponse[0]) == 0) ? "Auto" : "Manual");
            txtStateModeSettings.Items.Add(op_mode);
            string num_bats = "# Batteries: \t" + ((ushort)serverResponse[1]).ToString();
            txtStateModeSettings.Items.Add(num_bats);
            string num_cells = "# Battery cells: \t" + ((ushort)serverResponse[2]).ToString();
            txtStateModeSettings.Items.Add(num_cells);
            string bat_active = "Active Bat: \t" + ((((ushort)serverResponse[3]) > 3) ? "none" /*((ushort)serverResponse[3]).ToString("X")*/ : ((ushort)serverResponse[3]).ToString());
            txtStateModeSettings.Items.Add(bat_active);
            string bat_chrgng = "Charging Bat: \t" + ((((ushort)serverResponse[4]) > 7) ? "none" /*((ushort)serverResponse[4]).ToString("X")*/ : (((ushort)serverResponse[4]) % 4).ToString());
            txtStateModeSettings.Items.Add(bat_chrgng);
            string charge_cycle = "Charge cycle: \t" + (((ushort)serverResponse[7] == 0) ? "false" : "true");
            txtStateModeSettings.Items.Add(charge_cycle);
            string emergency_stop = "EMERGENCY: \t" + (((ushort)serverResponse[8] == 0) ? "false" : "true");
            txtStateModeSettings.Items.Add(emergency_stop);

        }        

        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            // Check for serial port connection first.
            if (!modbusClient.Connected)
            {
                // Show error message
                Debug.WriteLine("Submit of settings not possible. No connection to MODBUS.");
                txtFetchError.Text = "You have to connect to serial port before you can fetch data!";
                txtConnectState.Text = "MODBUS connected";
                txtConnectState.BackColor = Color.Red;
                return;
            }

            // TODO:
            //      - Check settings are valid:
            if ( false )
            {
                // Can't happen..
            }

            //      - Stop all requests e.g. CYCLIC_READ
            if (CYCLIC_READ)
            {
                CYCLIC_READ = false;
                btnCyclicRead.Text = "Start Cyclic Read";
                btnCyclicRead.BackColor = Color.Plum;

                btnRead1.Enabled = true;
            }

            //      - Clear displayed data 
            // clearData();

            //      - Read input fields and write settings to MODBUS holding registers 
            try
            {
                int[] values = new int[5];      // { 1, 2, 3, 4, 5 };

                values[0] = cbbModeSelect.SelectedIndex;
                int.TryParse(cbbNumBats.Text, out values[1]);
                int.TryParse(cbbNumBatCells.Text, out values[2]);

                if (cbbActiveBat.SelectedIndex >= 0)
                    int.TryParse(cbbActiveBat.Text, out values[3]);
                else
                    values[3] = 0;

                if (cbbChrgBat.SelectedIndex >= 1)
                {
                    int.TryParse(cbbChrgBat.Text, out values[4]);
                    values[4] += 4;
                }
                else if(cbbChrgBat.SelectedIndex == 0)
                {
                    values[4] = 0xBABE;
                }
                else
                {
                    values[4] = 0xBABE;
                }

                Debug.WriteLine(values[0]);
                Debug.WriteLine(values[1]);
                Debug.WriteLine(values[2]);
                Debug.WriteLine(values[3]);
                Debug.WriteLine(values[4]);

                // Fetch server response
                modbusClient.WriteMultipleRegisters(HOLDING_REG_START - 1, values);
                Debug.WriteLine("Wrote HOLDING REGISTER values to Server");

            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception Writing HOLDING to Server.");

            }

            // Update "Current Settings" on successful response.
            inputRegToGUI();
            /*try
            {
                // Fetch server response
                int[] serverResponse = modbusClient.ReadHoldingRegisters(HOLDING_REG_START - 1, HOLDING_REG_NUM_REGS);

                Debug.WriteLine("Read DEFAULT-settings from holding registers");
                for(int i=0; i<serverResponse.Length; i++)
                {
                    Debug.WriteLine(serverResponse[i]);
                }
                
                updateCurrentSettings(serverResponse);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception Reading HOLDING from Server.");

            }*/
        }

        private void mountModbusMaster()
        {
            modbusClient = new ModbusClient();
            modbusClient.ReceiveDataChanged += new EasyModbus.ModbusClient.ReceiveDataChangedHandler(UpdateReceiveData);
            modbusClient.SendDataChanged += new EasyModbus.ModbusClient.SendDataChangedHandler(UpdateSendData);
            modbusClient.ConnectedChanged += new EasyModbus.ModbusClient.ConnectedChangedHandler(UpdateConnectedChanged);
        }

        /* --------------------- Event handlers for easyModbus library --------------------- */
       string receiveData = null;

       void UpdateReceiveData(object sender)
       {
           receiveData = "Rx: " + BitConverter.ToString(modbusClient.receiveData).Replace("-", " ") + System.Environment.NewLine;
           Thread thread = new Thread(updateReceiveTextBox);
           thread.Start();
       }
       delegate void UpdateReceiveDataCallback();
       void updateReceiveTextBox()
       {
           if (textBox11.InvokeRequired)
           {
               UpdateReceiveDataCallback d = new UpdateReceiveDataCallback(updateReceiveTextBox);
               this.Invoke(d, new object[] { });
           }
           else
           {
                textBox11.Text = receiveData;
            }
        }

        string sendData = null;
        void UpdateSendData(object sender)
        {
            sendData = "Tx: " + BitConverter.ToString(modbusClient.sendData).Replace("-", " ") + System.Environment.NewLine;
            Thread thread = new Thread(updateSendTextBox);
            thread.Start();

        }

        void updateSendTextBox()
        {
            if (textBox10.InvokeRequired)
            {
                UpdateReceiveDataCallback d = new UpdateReceiveDataCallback(updateSendTextBox);
                this.Invoke(d, new object[] { });
            }
            else
            {
                textBox10.Text = sendData;
            }
        }


        private void btnPortConnect_Click(object sender, EventArgs e)
        {
            if (btnPortConnect.Text == "Connect")
            {
                
                if (comboBoxPorts.SelectedItem == null)
                {
                    Debug.WriteLine("Connection not possible. No Port selected.");
                    txtFetchError.Text = "Choose a serial port first!";
                    return;
                }

                try
                {
                    // Configure the modbusClient (Master) 
                    modbusClient.SerialPort = comboBoxPorts.SelectedItem.ToString();
                    modbusClient.UnitIdentifier = SLAVE_ADDR;
                    modbusClient.Baudrate = BAUDRATE;
                    // The MODBUS implementation on FW side doesn't work with any parities except "NONE"
                    modbusClient.Parity = System.IO.Ports.Parity.None;
                    modbusClient.StopBits = System.IO.Ports.StopBits.One;

                    modbusClient.Connect();
                    if (modbusClient.Connected)
                    {
                        txtConnectState.Text = "Connected!";
                        txtConnectState.BackColor = Color.Lime;

                        txtComErrors.Text = "";
                    }

                    /**
                     * Read 'default' settings from the firmware via modbus.
                     * (Which mode, how many batteries, how many cells per battery, etc.)
                     * 
                     * Show info in the status textbox (green or red)
                     */
                    try
                    {
                        /* --- Fetch server response --- */
                        int[] serverResponse = modbusClient.ReadHoldingRegisters(HOLDING_REG_START - 1, HOLDING_REG_NUM_REGS);

                        Debug.WriteLine("Read DEFAULT-settings from holding registers:");

                        updateCurrentSettings(serverResponse);

                        if ((ushort)serverResponse[8] == 1)
                            alertEmergencyStop();
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine("Exception Reading HOLDING from Server.");
                        txtConnectState.Text = "MODBUS not connected";
                        txtConnectState.BackColor = Color.Red;

                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception occured. Could not connect to Port/Server.");
                }
            }
            else
            {
                modbusClient.Disconnect();
                txtConnectState.Text = "MODBUS not connected";
                txtConnectState.BackColor = Color.Red;
                clearData();
                clearCurrentSettings();
            }

        }

        private void UpdateConnectedChanged(object sender)
        {
            if (modbusClient.Connected)
            {
                Debug.WriteLine("Connected to Port: " + comboBoxPorts.SelectedItem.ToString());
                btnPortConnect.Text = "Disconnect";
                btnPortConnect.BackColor = Color.LightPink;
                // Clear the error message 
                txtFetchError.Text = "";
            }
            else
            {
                btnPortConnect.Text = "Connect";
                btnPortConnect.BackColor = Color.LightGreen;

                if (comboBoxPorts.SelectedItem != null) { 
                    Debug.WriteLine("Disconnected from MODBUS on port: " + comboBoxPorts.SelectedItem.ToString());
                    comboBoxPorts.SelectedItem = null;
                }

                CYCLIC_READ = false;
                btnCyclicRead.Text = "Start Cyclic Read";
                btnCyclicRead.BackColor = Color.Plum;

                btnRead1.Enabled = true;
            }
        }

        private void clearData()
        {
            /* --- Clear data fields --- */
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            /* --- Clear packet display --- */
            textBox10.Text = "";
            textBox11.Text = "";
            /* --- Clear checkboxes --- */
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
        }

        private void setActiveChkbx(int index)
        {
            // Set checkboxes 1 to 4
            switch (index)
            {
                case 0:
                    checkBox1.Checked = true;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    break;
                case 1:
                    checkBox1.Checked = false;
                    checkBox2.Checked = true;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    break;
                case 2:
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = true;
                    checkBox4.Checked = false;
                    break;
                case 3:
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = true;
                    break;
                default:
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    break;
            }
        }

        private void setChargeChkbx(int index)
        {
            // Set checkboxes 5 to 8
            switch (index - 4)
            {
                case 0:
                    checkBox5.Checked = true;
                    checkBox6.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    break;
                case 1:
                    checkBox5.Checked = false;
                    checkBox6.Checked = true;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    break;
                case 2:
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    checkBox7.Checked = true;
                    checkBox8.Checked = false;
                    break;
                case 3:
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = true;
                    break;
                default:
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    break;
            }
        }

        private class EmergencyStopException : Exception
        {
            public EmergencyStopException()
            {
            }

            public EmergencyStopException(string message)
            : base(message)
                {
                }
        }

        delegate void inputRegToGUIDelegate();
        void inputRegToGUI()
        {
            if (textBox1.InvokeRequired)
            {
                inputRegToGUIDelegate d = new inputRegToGUIDelegate(inputRegToGUI);
                this.Invoke(d, new object[] { });
            }
            else
            {

                try
                {
                    /* --- Fetch server response --- */
                    int[] serverResponse = modbusClient.ReadInputRegisters(INPUT_REG_START - 1, INPUT_REG_NUM_REGS);

                    /* --- Voltages battery 0 to 3 --- */
                    string[] log_data = new string[10];

                    log_data[0] = ConversionUtils.trueBatVoltsFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[0])).ToString();
                    textBox1.Text = log_data[0];
                    log_data[1] = ConversionUtils.trueBatVoltsFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[1])).ToString();
                    textBox2.Text = log_data[1];
                    log_data[2] = ConversionUtils.trueBatVoltsFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[2])).ToString();
                    textBox3.Text = log_data[2];
                    log_data[3] = ConversionUtils.trueBatVoltsFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[3])).ToString();
                    textBox4.Text = log_data[3];
                    /* --- Voltage solar --- */
                    log_data[4] = ConversionUtils.trueSolarVoltsFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[4])).ToString();
                    textBox5.Text = log_data[4];
                    /* --- Current solar --- */
                    log_data[5] = ConversionUtils.trueCurrentFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[5]),
                                                                       ConversionUtils.RS_PV_CHARGE, ConversionUtils.RL_PV_CHARGE).ToString();
                    textBox6.Text = log_data[5];
                    /* --- Current charging --- */
                    log_data[6] = ConversionUtils.trueCurrentFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[6]),
                                                                       ConversionUtils.RS_PV_CHARGE, ConversionUtils.RL_PV_CHARGE).ToString();
                    textBox7.Text = log_data[6];
                    /* --- Current load --- */
                    log_data[7] = ConversionUtils.trueCurrentFromADC(ConversionUtils.adcValueFromScaled((ushort)serverResponse[7]),
                                                                       ConversionUtils.RS_LOAD, ConversionUtils.RL_LOAD).ToString();
                    textBox8.Text = log_data[7];
                    /* --- Active and charging states --- */
                    setActiveChkbx((int)serverResponse[8]);
                    log_data[8] = ((int)serverResponse[8]).ToString();
                    Debug.WriteLine("Active battery: " + ((int)serverResponse[8]).ToString());
                    setChargeChkbx((int)serverResponse[9]);
                    Debug.WriteLine("Charging battery: " + ((int)serverResponse[9]).ToString());
                    log_data[9] = ((int)serverResponse[9]).ToString();

                    /* Maybe also calculate the power consume of the system. */

                    /* Write to system log */
                    DataLogging.WriteDataToLog(log_data);

                    Debug.Write("Wrote data to log file\n");

                    int[] holdingServerRsp= modbusClient.ReadHoldingRegisters(HOLDING_REG_START - 1, HOLDING_REG_NUM_REGS);
                    updateCurrentSettings(holdingServerRsp);

                    if ((ushort)holdingServerRsp[8] == 1)
                        throw new EmergencyStopException();
                }
                catch(EmergencyStopException esexc)
                {
                    if (CYCLIC_READ)
                    {
                        CYCLIC_READ = false;
                        btnCyclicRead.Text = "Start Cyclic Read";
                        btnCyclicRead.BackColor = Color.Plum;
                        btnRead1.Enabled = true;
                    }

                    alertEmergencyStop();
                }
                catch (Exception exc)
                {
                    Debug.WriteLine("Exception Reading values from Server.");

                    // Print error message
                    txtComErrors.Text = "No response from server. Sure you're connected?";

                    txtConnectState.Text = "MODBUS not connected";
                    txtConnectState.BackColor = Color.Red;

                    if (CYCLIC_READ)
                    {
                        CYCLIC_READ = false;
                        btnCyclicRead.Text = "Start Cyclic Read";
                        btnCyclicRead.BackColor = Color.Plum;
                        btnRead1.Enabled = true;
                    }

                }
            }
        }

        private void btnRead1_Click(object sender, EventArgs e)
        {
            // Clear error field..
            txtComErrors.Text = "";

            if (!modbusClient.Connected)
            {
                Debug.WriteLine("Connection not possible. No Port selected.");
                txtFetchError.Text = "You have to connect to serial port before you can fetch data!";
                txtConnectState.Text = "MODBUS not connected";
                txtConnectState.BackColor = Color.Red;
                return;
            }

            inputRegToGUI( );
        }

        private void cyclic_read()
        {
            while (CYCLIC_READ)
            {
                inputRegToGUI();

                Thread.Sleep(2000);
            }
        }

        private void btnCyclicRead_Click(object sender, EventArgs e)
        {
            // Clear error field..
            txtComErrors.Text = "";

            if (modbusClient.Connected == false)
            {
                Debug.WriteLine("Connection not possible. No Port selected.");
                txtFetchError.Text = "You have to connect to serial port before you can fetch data!";
                txtConnectState.Text = "MODBUS not connected";
                txtConnectState.BackColor = Color.Red;
                return;
            }

            if ( btnCyclicRead.Text == "Start Cyclic Read")
            {
                // Disable the 'Read Once' button
                btnRead1.Enabled = false;

                CYCLIC_READ = true;
                new Thread(delegate () {
                    cyclic_read();
                }).Start();

                btnCyclicRead.Text = "Stop Cyclic Read";
                btnCyclicRead.BackColor = Color.HotPink;
            }
            else
            {
                CYCLIC_READ = false;
                btnCyclicRead.Text = "Start Cyclic Read";
                btnCyclicRead.BackColor = Color.Plum;

                // Enable the 'Read Once' button
                btnRead1.Enabled = true;
            }
        }

        private void BtnTestReadHold_Click(object sender, EventArgs e)
        {
            try
            {
                /* --- Fetch server response --- */
                int[] serverResponse = modbusClient.ReadHoldingRegisters(HOLDING_REG_START - 1, HOLDING_REG_NUM_REGS);

                Debug.WriteLine("Read from HOLDING registers:");
                for (int i = 0; i < serverResponse.Length; i++)
                {
                    Debug.WriteLine((ushort)serverResponse[i]);
                }
                Debug.WriteLine("");

                Debug.WriteLine("Updating Current Settings display");

                txtStateModeSettings.Items.Clear();

                //string comm_mode = "Communication Mode: \t" + (((ushort)serverResponse[5] == 0) ? "MODBUS" : "WiMOD");
                //txtStateModeSettings.Items.Add(comm_mode);
                string opMode = "Operation Mode: \t" + ((((ushort)serverResponse[0]) == 0) ? "Auto" : "Manual");
                txtStateModeSettings.Items.Add(opMode);
                string num_bats = "# Batteries: \t" + ((ushort)serverResponse[1]).ToString();
                txtStateModeSettings.Items.Add(num_bats);
                string num_cells = "# Battery cells: \t" + ((ushort)serverResponse[2]).ToString();
                txtStateModeSettings.Items.Add(num_cells);
                string bat_active = "Active battery: \t" + ((((ushort)serverResponse[3]) > 3) ? ((ushort)serverResponse[3]).ToString("X") : ((ushort)serverResponse[3]).ToString());
                txtStateModeSettings.Items.Add(bat_active);
                string bat_chrgng = "Charging battery: \t" + ((((ushort)serverResponse[4]) > 3) ? ((ushort)serverResponse[4]).ToString("X") : ((ushort)serverResponse[4]).ToString());
                txtStateModeSettings.Items.Add(bat_chrgng);
                
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception Reading HOLDING from Server.");

            }
        }

        private void BtnTestWriteHold_Click(object sender, EventArgs e)
        {
            try
            {
                int[] values = new int[5] { 1, 2, 3, 4, 5 };

                /* --- Fetch server response --- */
                modbusClient.WriteMultipleRegisters(HOLDING_REG_START - 1, values);
                Debug.WriteLine("Wrote HOLDING REGISTER values to Server");
                Debug.WriteLine("");


                /*Debug.WriteLine("Read from HOLDING registers:");
                for (int i = 0; i < serverResponse.Length; i++)
                {
                    Debug.WriteLine(serverResponse[i]);
                }
                Debug.WriteLine("");*/
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception Reading HOLDING from Server.");

            }
        }

        private void BtnResetDefault_Click(object sender, EventArgs e)
        {
            try
            {
                int[] values = new int[5] {
                    0,          // Auto-Mode
                    2,          // 2 Batteries
                    2,          // 2 Battery cells
                    0,          // Bat0 active
                    0xbabe      // No battery charging.
                };

                /* --- Write settings to MODBUS --- */
                modbusClient.WriteMultipleRegisters(HOLDING_REG_START - 1, values);
                Debug.WriteLine("Wrote RESET DEFAULT SETTINGS to Server");

                /* --- Read new settings from MODBUS --- */
                int[] serverResponse = modbusClient.ReadHoldingRegisters(HOLDING_REG_START - 1, HOLDING_REG_NUM_REGS);

                updateCurrentSettings(serverResponse);

                if ((ushort)serverResponse[8] == 1)
                    alertEmergencyStop();
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception writing RESET DEFAULT SETTINGS to Server.");

            }
        }

        /**
         * 
         * Helper class for data conversion
         *
         */
        public static class ConversionUtils
        {
            // Formula: C_scale = (U_ref/2^#Bits) = 2.5 / 65536 = 0.00003815
            public static double C_ADC_SCALE = 0.00003815;
            // voltage divider (R1+R2)/R2 = 139/39 = 3.564103
            public static double C_DIVIDER_U_BATS = 3.564103;
            // Set R2 to 14.8k
            // voltage divider (50k + R1) + R2 / R2 = 150k / 14.8k = 10.13513514
            public static double C_DIVIDER_U_SOLAR = 10.13513514;

            // Formula: V_out = G_m * (I_s) * (R_s) * (R_L)
            public static double G_m = 0.000200;
            // PV and Charge current
            public static double RS_PV_CHARGE = 0.003;
            public static double RL_PV_CHARGE = 560000;
            // Load current
            public static double RS_LOAD = 0.000500;
            public static double RL_LOAD = 820000;


            /**
             * Converts the scaled value between 0 and 2^16 (65536) received from ADC into voltage
             */
            public static double adcValueFromScaled(uint scaled_volts_adc)
            {
                return C_ADC_SCALE * scaled_volts_adc;
            }

            /**
             * Calculates the true voltage from measured scaled voltage of the batteries. 
             * Voltage divider is R1 = 10k and R2 = 3.9k
             * U_out = U_in * R2/(R1+R2)  ==> U_in = U_out * (R1+R2)/R2
             * (R1+R2)/R2 = 139/39 = 3.564103
             */
            public static double trueBatVoltsFromADC(double volts_adc)
            {
                return C_DIVIDER_U_BATS * volts_adc;
            }

            /**
             * Calculates the true voltage from measured scaled voltage of solar input.
             * Voltage divider dependent on TRIMPOT.
             */
            public static double trueSolarVoltsFromADC(double volts_adc){
                return C_DIVIDER_U_SOLAR * volts_adc;
            }


            // Formula 1: V_out = G_m * (I_s) * (R_s) * (R_L)
            // Formula 2: I_s = V_out * ( 1 / (G_m) * (R_s) * (R_L) ) 
            public static double trueCurrentFromADC(double volts_adc,
                                    double shunt_resistance, double load_resistance)
            {
                Debug.WriteLine("true_volts " + volts_adc.ToString());
                return volts_adc * ( 1 / ( G_m * shunt_resistance * load_resistance ) );
            }

            public static double calcPower(double volts, double amps)
            {
                return volts * amps;
            }

        }

        /**
         * Helper class for data logging
         * 
         */
        class DataLogging
        {
            /**
             * The data logs are saved in CSV format.
             */
            public static string CURRENT_LOG_FILE;

            public static void setCurrentLogFile()
            {
                Debug.Write(Directory.GetCurrentDirectory() + "\n");

                string ancestorPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
                ancestorPath = Directory.GetParent(ancestorPath).ToString();
                ancestorPath = Directory.GetParent(ancestorPath).ToString();

                Debug.Write(ancestorPath + "\n");
                string[] paths = { ancestorPath, "logs" };
                string logsPath = Path.Combine(paths);
                Debug.Write(logsPath + "\n");
                if (!Directory.Exists(logsPath))
                    Directory.CreateDirectory(logsPath);


                string filename = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                filename += "_log.csv";
                paths[0] = logsPath;
                paths[1] = filename;
                CURRENT_LOG_FILE = Path.Combine(paths);
                Debug.Write("current log file path: " + CURRENT_LOG_FILE + "\n");
                
                // Create new log in .csv format
                using (StreamWriter w = File.AppendText(CURRENT_LOG_FILE))
                {
                    w.WriteLine("datetime;U_bat0;U_bat1;U_bat2;U_bat3;U_solar;I_solar;I_charge;I_load;active_bat;chargng_bat");
                    Debug.Write("Wrote to new log file \n");
                }

            }

            public static void WriteDataToLog(string[] data)
            {

                string logMessage = "";
                logMessage += DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + ";";
                for (int i = 0; i < data.Length; i++)
                {
                    logMessage += data[i];

                    if (i < data.Length - 1)
                    {
                        logMessage += ";";
                    }
                }

                using (StreamWriter w = File.AppendText(CURRENT_LOG_FILE))
                {
                    w.WriteLine(logMessage);
                }
            }

            public static void DumpLog(StreamReader r)
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }



        /**
         * Helper code for debugging the Wimod DLL
         */

        private void cyclic_lora_rx()
        {
            Debug.WriteLine("Thread Cyclic Lora Rx started.");
            while (LORA_RX_ACTIVE)
            {
                WimodHCIWrapper.wrap_WiMOD_HCI_Process();
                Thread.Sleep(1000);
            }
            Debug.WriteLine("THREAD Cyclic Lora Rx stopped.");
        }

        bool LORA_RX_ACTIVE = false;
        private void BtnRxLora_Click(object sender, EventArgs e)
        {
            
            /*if (!LORA_RX_ACTIVE) { 
                LORA_RX_ACTIVE = true;
                new Thread(delegate () {
                    cyclic_lora_rx();
                }).Start();
                btnRxLora.Text = "Stop WiMOD Rx";
            }
            else
            {
                LORA_RX_ACTIVE = false;
                btnRxLora.Text = "Start WiMOD Rx";
            }*/

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
