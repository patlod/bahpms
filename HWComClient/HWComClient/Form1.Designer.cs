namespace HWComClient
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPortBlimp = new System.IO.Ports.SerialPort(this.components);
            this.btnPortConnect = new System.Windows.Forms.Button();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.btnRead1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtFetchError = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.btnCyclicRead = new System.Windows.Forms.Button();
            this.txtBlank6 = new System.Windows.Forms.TextBox();
            this.txtBlank5 = new System.Windows.Forms.TextBox();
            this.txtBlank4 = new System.Windows.Forms.TextBox();
            this.txtBlank3 = new System.Windows.Forms.TextBox();
            this.txtBlank2 = new System.Windows.Forms.TextBox();
            this.txtBlank1 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbbModeSelect = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.cbbNumBats = new System.Windows.Forms.ComboBox();
            this.cbbActiveBat = new System.Windows.Forms.ComboBox();
            this.cbbActiveBatLabel = new System.Windows.Forms.Label();
            this.cbbChrgBatLabel = new System.Windows.Forms.Label();
            this.cbbChrgBat = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cbbNumBatCells = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtComErrors = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.btnTestReadHold = new System.Windows.Forms.Button();
            this.btnTestWriteHold = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.txtStateModeSettings = new System.Windows.Forms.ListBox();
            this.btnResetDefault = new System.Windows.Forms.Button();
            this.btnRxLora = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.txtConnectState = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPortBlimp
            // 
            this.serialPortBlimp.BaudRate = 115200;
            // 
            // btnPortConnect
            // 
            this.btnPortConnect.BackColor = System.Drawing.Color.LightGreen;
            this.btnPortConnect.Location = new System.Drawing.Point(348, 14);
            this.btnPortConnect.Name = "btnPortConnect";
            this.btnPortConnect.Size = new System.Drawing.Size(99, 21);
            this.btnPortConnect.TabIndex = 0;
            this.btnPortConnect.Text = "Connect";
            this.btnPortConnect.UseVisualStyleBackColor = false;
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(198, 14);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(117, 21);
            this.comboBoxPorts.Sorted = true;
            this.comboBoxPorts.TabIndex = 1;
            // 
            // btnRead1
            // 
            this.btnRead1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnRead1.Location = new System.Drawing.Point(442, 295);
            this.btnRead1.Name = "btnRead1";
            this.btnRead1.Size = new System.Drawing.Size(150, 23);
            this.btnRead1.TabIndex = 2;
            this.btnRead1.Text = "Read Once";
            this.btnRead1.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(63, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Battery 0";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(307, 220);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(121, 20);
            this.textBox7.TabIndex = 4;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(307, 256);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(121, 20);
            this.textBox8.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(63, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Battery 1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(148, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 8;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(148, 81);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(121, 20);
            this.textBox2.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(63, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Battery 2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(63, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Battery 3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(178, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Voltage [V]";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoCheck = false;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(480, 48);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoCheck = false;
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(480, 81);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoCheck = false;
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(480, 116);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(15, 14);
            this.checkBox3.TabIndex = 15;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoCheck = false;
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(480, 150);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(15, 14);
            this.checkBox4.TabIndex = 16;
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoCheck = false;
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(553, 48);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(15, 14);
            this.checkBox5.TabIndex = 17;
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoCheck = false;
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(553, 81);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(15, 14);
            this.checkBox6.TabIndex = 18;
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoCheck = false;
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(553, 116);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(15, 14);
            this.checkBox7.TabIndex = 19;
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoCheck = false;
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(553, 150);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(15, 14);
            this.checkBox8.TabIndex = 20;
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(453, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Supplying";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(535, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Charging";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(148, 115);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(121, 20);
            this.textBox3.TabIndex = 23;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(148, 150);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(121, 20);
            this.textBox4.TabIndex = 24;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(148, 185);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(121, 20);
            this.textBox5.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(63, 259);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Load";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(63, 223);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Charge";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(307, 185);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(121, 20);
            this.textBox6.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(63, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Solar";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(336, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Current [A]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(39, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Choose Serial Port";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(231, 26);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtFetchError);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxPorts);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnPortConnect);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label20);
            this.splitContainer1.Panel2.Controls.Add(this.btnCyclicRead);
            this.splitContainer1.Panel2.Controls.Add(this.txtBlank6);
            this.splitContainer1.Panel2.Controls.Add(this.txtBlank5);
            this.splitContainer1.Panel2.Controls.Add(this.txtBlank4);
            this.splitContainer1.Panel2.Controls.Add(this.txtBlank3);
            this.splitContainer1.Panel2.Controls.Add(this.txtBlank2);
            this.splitContainer1.Panel2.Controls.Add(this.txtBlank1);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.btnRead1);
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.textBox6);
            this.splitContainer1.Panel2.Controls.Add(this.textBox7);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.textBox8);
            this.splitContainer1.Panel2.Controls.Add(this.label12);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.textBox5);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.textBox4);
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox3);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox8);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox7);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox2);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox6);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox3);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox5);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox4);
            this.splitContainer1.Size = new System.Drawing.Size(629, 406);
            this.splitContainer1.SplitterDistance = 66;
            this.splitContainer1.TabIndex = 35;
            // 
            // txtFetchError
            // 
            this.txtFetchError.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFetchError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFetchError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFetchError.ForeColor = System.Drawing.Color.Firebrick;
            this.txtFetchError.Location = new System.Drawing.Point(198, 41);
            this.txtFetchError.Name = "txtFetchError";
            this.txtFetchError.Size = new System.Drawing.Size(414, 13);
            this.txtFetchError.TabIndex = 35;
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label20.Location = new System.Drawing.Point(8, 1);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(621, 2);
            this.label20.TabIndex = 56;
            // 
            // btnCyclicRead
            // 
            this.btnCyclicRead.BackColor = System.Drawing.Color.Plum;
            this.btnCyclicRead.Location = new System.Drawing.Point(269, 295);
            this.btnCyclicRead.Name = "btnCyclicRead";
            this.btnCyclicRead.Size = new System.Drawing.Size(150, 23);
            this.btnCyclicRead.TabIndex = 39;
            this.btnCyclicRead.Text = "Start Cyclic Read";
            this.btnCyclicRead.UseVisualStyleBackColor = false;
            // 
            // txtBlank6
            // 
            this.txtBlank6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtBlank6.Enabled = false;
            this.txtBlank6.Location = new System.Drawing.Point(148, 256);
            this.txtBlank6.Name = "txtBlank6";
            this.txtBlank6.ReadOnly = true;
            this.txtBlank6.Size = new System.Drawing.Size(121, 20);
            this.txtBlank6.TabIndex = 38;
            // 
            // txtBlank5
            // 
            this.txtBlank5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtBlank5.Enabled = false;
            this.txtBlank5.Location = new System.Drawing.Point(148, 220);
            this.txtBlank5.Name = "txtBlank5";
            this.txtBlank5.ReadOnly = true;
            this.txtBlank5.Size = new System.Drawing.Size(121, 20);
            this.txtBlank5.TabIndex = 37;
            // 
            // txtBlank4
            // 
            this.txtBlank4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtBlank4.Enabled = false;
            this.txtBlank4.Location = new System.Drawing.Point(307, 150);
            this.txtBlank4.Name = "txtBlank4";
            this.txtBlank4.ReadOnly = true;
            this.txtBlank4.Size = new System.Drawing.Size(121, 20);
            this.txtBlank4.TabIndex = 36;
            // 
            // txtBlank3
            // 
            this.txtBlank3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtBlank3.Enabled = false;
            this.txtBlank3.Location = new System.Drawing.Point(307, 118);
            this.txtBlank3.Name = "txtBlank3";
            this.txtBlank3.ReadOnly = true;
            this.txtBlank3.Size = new System.Drawing.Size(121, 20);
            this.txtBlank3.TabIndex = 35;
            // 
            // txtBlank2
            // 
            this.txtBlank2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtBlank2.Enabled = false;
            this.txtBlank2.Location = new System.Drawing.Point(307, 84);
            this.txtBlank2.Name = "txtBlank2";
            this.txtBlank2.ReadOnly = true;
            this.txtBlank2.Size = new System.Drawing.Size(121, 20);
            this.txtBlank2.TabIndex = 34;
            // 
            // txtBlank1
            // 
            this.txtBlank1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.txtBlank1.Enabled = false;
            this.txtBlank1.Location = new System.Drawing.Point(307, 51);
            this.txtBlank1.Name = "txtBlank1";
            this.txtBlank1.ReadOnly = true;
            this.txtBlank1.Size = new System.Drawing.Size(121, 20);
            this.txtBlank1.TabIndex = 33;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(273, 483);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(419, 20);
            this.textBox10.TabIndex = 38;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(273, 509);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(419, 20);
            this.textBox11.TabIndex = 39;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(246, 512);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(22, 13);
            this.label13.TabIndex = 40;
            this.label13.Text = "Rx";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(246, 486);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Tx";
            // 
            // cbbModeSelect
            // 
            this.cbbModeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbModeSelect.FormattingEnabled = true;
            this.cbbModeSelect.Location = new System.Drawing.Point(35, 139);
            this.cbbModeSelect.Name = "cbbModeSelect";
            this.cbbModeSelect.Size = new System.Drawing.Size(75, 21);
            this.cbbModeSelect.TabIndex = 42;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(32, 123);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 13);
            this.label15.TabIndex = 43;
            this.label15.Text = "Select Mode";
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Location = new System.Drawing.Point(106, 350);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(75, 23);
            this.btnApplySettings.TabIndex = 44;
            this.btnApplySettings.Text = "Apply";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(32, 169);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 13);
            this.label16.TabIndex = 45;
            this.label16.Text = "# Batteries";
            // 
            // cbbNumBats
            // 
            this.cbbNumBats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbNumBats.FormattingEnabled = true;
            this.cbbNumBats.Location = new System.Drawing.Point(35, 185);
            this.cbbNumBats.Name = "cbbNumBats";
            this.cbbNumBats.Size = new System.Drawing.Size(64, 21);
            this.cbbNumBats.TabIndex = 46;
            // 
            // cbbActiveBat
            // 
            this.cbbActiveBat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbActiveBat.FormattingEnabled = true;
            this.cbbActiveBat.Location = new System.Drawing.Point(33, 252);
            this.cbbActiveBat.Name = "cbbActiveBat";
            this.cbbActiveBat.Size = new System.Drawing.Size(68, 21);
            this.cbbActiveBat.TabIndex = 47;
            this.cbbActiveBat.SelectedIndexChanged += new System.EventHandler(this.CbbActiveBat_SelectedIndexChanged);
            // 
            // cbbActiveBatLabel
            // 
            this.cbbActiveBatLabel.AutoSize = true;
            this.cbbActiveBatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbActiveBatLabel.Location = new System.Drawing.Point(32, 236);
            this.cbbActiveBatLabel.Name = "cbbActiveBatLabel";
            this.cbbActiveBatLabel.Size = new System.Drawing.Size(87, 13);
            this.cbbActiveBatLabel.TabIndex = 48;
            this.cbbActiveBatLabel.Text = "Active Battery";
            // 
            // cbbChrgBatLabel
            // 
            this.cbbChrgBatLabel.AutoSize = true;
            this.cbbChrgBatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbChrgBatLabel.Location = new System.Drawing.Point(32, 287);
            this.cbbChrgBatLabel.Name = "cbbChrgBatLabel";
            this.cbbChrgBatLabel.Size = new System.Drawing.Size(101, 13);
            this.cbbChrgBatLabel.TabIndex = 49;
            this.cbbChrgBatLabel.Text = "Charging Battery";
            // 
            // cbbChrgBat
            // 
            this.cbbChrgBat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbChrgBat.FormattingEnabled = true;
            this.cbbChrgBat.Location = new System.Drawing.Point(35, 308);
            this.cbbChrgBat.Name = "cbbChrgBat";
            this.cbbChrgBat.Size = new System.Drawing.Size(66, 21);
            this.cbbChrgBat.TabIndex = 50;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(112, 169);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 13);
            this.label19.TabIndex = 51;
            this.label19.Text = "# Cells";
            // 
            // cbbNumBatCells
            // 
            this.cbbNumBatCells.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbNumBatCells.Enabled = false;
            this.cbbNumBatCells.FormattingEnabled = true;
            this.cbbNumBatCells.Location = new System.Drawing.Point(113, 185);
            this.cbbNumBatCells.Name = "cbbNumBatCells";
            this.cbbNumBatCells.Size = new System.Drawing.Size(68, 21);
            this.cbbNumBatCells.TabIndex = 52;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(32, 82);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(111, 17);
            this.label17.TabIndex = 53;
            this.label17.Text = "Mode Settings";
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Location = new System.Drawing.Point(223, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(2, 515);
            this.label18.TabIndex = 54;
            // 
            // txtComErrors
            // 
            this.txtComErrors.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtComErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComErrors.ForeColor = System.Drawing.Color.Firebrick;
            this.txtComErrors.Location = new System.Drawing.Point(276, 535);
            this.txtComErrors.Name = "txtComErrors";
            this.txtComErrors.Size = new System.Drawing.Size(414, 13);
            this.txtComErrors.TabIndex = 36;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(12, 401);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(98, 13);
            this.label21.TabIndex = 56;
            this.label21.Text = "Current Settings";
            // 
            // btnTestReadHold
            // 
            this.btnTestReadHold.Location = new System.Drawing.Point(729, 480);
            this.btnTestReadHold.Name = "btnTestReadHold";
            this.btnTestReadHold.Size = new System.Drawing.Size(131, 23);
            this.btnTestReadHold.TabIndex = 57;
            this.btnTestReadHold.Text = "Test Read Holding";
            this.btnTestReadHold.UseVisualStyleBackColor = true;
            this.btnTestReadHold.Click += new System.EventHandler(this.BtnTestReadHold_Click);
            // 
            // btnTestWriteHold
            // 
            this.btnTestWriteHold.Enabled = false;
            this.btnTestWriteHold.Location = new System.Drawing.Point(729, 509);
            this.btnTestWriteHold.Name = "btnTestWriteHold";
            this.btnTestWriteHold.Size = new System.Drawing.Size(131, 23);
            this.btnTestWriteHold.TabIndex = 58;
            this.btnTestWriteHold.Text = "Test Write Holding";
            this.btnTestWriteHold.UseVisualStyleBackColor = true;
            this.btnTestWriteHold.Click += new System.EventHandler(this.BtnTestWriteHold_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(236, 463);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(142, 13);
            this.label22.TabIndex = 59;
            this.label22.Text = "Last request / response";
            // 
            // txtStateModeSettings
            // 
            this.txtStateModeSettings.FormattingEnabled = true;
            this.txtStateModeSettings.Location = new System.Drawing.Point(15, 422);
            this.txtStateModeSettings.Name = "txtStateModeSettings";
            this.txtStateModeSettings.Size = new System.Drawing.Size(190, 108);
            this.txtStateModeSettings.TabIndex = 60;
            // 
            // btnResetDefault
            // 
            this.btnResetDefault.BackColor = System.Drawing.Color.IndianRed;
            this.btnResetDefault.ForeColor = System.Drawing.SystemColors.Control;
            this.btnResetDefault.Location = new System.Drawing.Point(54, 536);
            this.btnResetDefault.Name = "btnResetDefault";
            this.btnResetDefault.Size = new System.Drawing.Size(151, 23);
            this.btnResetDefault.TabIndex = 61;
            this.btnResetDefault.Text = "Reset to Default Settings";
            this.btnResetDefault.UseVisualStyleBackColor = false;
            this.btnResetDefault.Click += new System.EventHandler(this.BtnResetDefault_Click);
            // 
            // btnRxLora
            // 
            this.btnRxLora.Location = new System.Drawing.Point(52, 34);
            this.btnRxLora.Name = "btnRxLora";
            this.btnRxLora.Size = new System.Drawing.Size(129, 23);
            this.btnRxLora.TabIndex = 62;
            this.btnRxLora.Text = "Start WiMOD Rx";
            this.btnRxLora.UseVisualStyleBackColor = true;
            this.btnRxLora.Click += new System.EventHandler(this.BtnRxLora_Click);
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label23.Location = new System.Drawing.Point(245, 451);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(624, 2);
            this.label23.TabIndex = 63;
            // 
            // txtConnectState
            // 
            this.txtConnectState.BackColor = System.Drawing.Color.Red;
            this.txtConnectState.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.txtConnectState.Location = new System.Drawing.Point(515, 554);
            this.txtConnectState.Name = "txtConnectState";
            this.txtConnectState.Size = new System.Drawing.Size(345, 32);
            this.txtConnectState.TabIndex = 64;
            this.txtConnectState.Text = "MODBUS not connected";
            this.txtConnectState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(881, 612);
            this.Controls.Add(this.txtConnectState);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.btnRxLora);
            this.Controls.Add(this.btnResetDefault);
            this.Controls.Add(this.txtStateModeSettings);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.btnTestWriteHold);
            this.Controls.Add(this.btnTestReadHold);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.txtComErrors);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.cbbNumBatCells);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.cbbChrgBat);
            this.Controls.Add(this.cbbChrgBatLabel);
            this.Controls.Add(this.cbbActiveBatLabel);
            this.Controls.Add(this.cbbActiveBat);
            this.Controls.Add(this.cbbNumBats);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnApplySettings);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cbbModeSelect);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "HW Communication Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPortBlimp;
        private System.Windows.Forms.Button btnPortConnect;
        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.Button btnRead1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtBlank6;
        private System.Windows.Forms.TextBox txtBlank5;
        private System.Windows.Forms.TextBox txtBlank4;
        private System.Windows.Forms.TextBox txtBlank3;
        private System.Windows.Forms.TextBox txtBlank2;
        private System.Windows.Forms.TextBox txtBlank1;
        private System.Windows.Forms.TextBox txtFetchError;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Button btnCyclicRead;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbbModeSelect;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbbNumBats;
        private System.Windows.Forms.ComboBox cbbActiveBat;
        private System.Windows.Forms.Label cbbActiveBatLabel;
        private System.Windows.Forms.Label cbbChrgBatLabel;
        private System.Windows.Forms.ComboBox cbbChrgBat;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cbbNumBatCells;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtComErrors;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnTestReadHold;
        private System.Windows.Forms.Button btnTestWriteHold;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ListBox txtStateModeSettings;
        private System.Windows.Forms.Button btnResetDefault;
        private System.Windows.Forms.Button btnRxLora;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtConnectState;
    }
}

