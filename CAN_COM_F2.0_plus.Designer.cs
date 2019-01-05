namespace CAN_COM_F2._0
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox_FrameFormat = new System.Windows.Forms.ComboBox();
            this.setBaudcomboBox = new System.Windows.Forms.ComboBox();
            this.comboBox_FrameType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.button_StopCAN = new System.Windows.Forms.Button();
            this.button_StartCAN = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_SendType = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox_Info = new System.Windows.Forms.ListBox();
            this.button_Send = new System.Windows.Forms.Button();
            this.textBox_Data = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_ID = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.timer_rec = new System.Windows.Forms.Timer(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.SdataGridView = new System.Windows.Forms.DataGridView();
            this.set_01_comboBox = new System.Windows.Forms.ComboBox();
            this.setDestination_01_textBox = new System.Windows.Forms.TextBox();
            this.setPLimite_01_textBox = new System.Windows.Forms.TextBox();
            this.setNlimit_01_settextBox = new System.Windows.Forms.TextBox();
            this.setExecutivetime_01_textBox = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.readDatabutton = new System.Windows.Forms.Button();
            this.sendDatabutton_01 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.IDSelectcomboBox = new System.Windows.Forms.ComboBox();
            this.gridID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDestination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridNowPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridInitial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridPLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridNLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viewSDataTypebutton = new System.Windows.Forms.Button();
            this.viewMDataTypebutton = new System.Windows.Forms.Button();
            this.MdataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SdataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.comboBox_FrameFormat);
            this.groupBox1.Controls.Add(this.setBaudcomboBox);
            this.groupBox1.Controls.Add(this.comboBox_FrameType);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.buttonConnect);
            this.groupBox1.Controls.Add(this.button_StopCAN);
            this.groupBox1.Controls.Add(this.button_StartCAN);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.comboBox_SendType);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 57);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备参数";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(399, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 7;
            this.label15.Text = "速率：";
            // 
            // comboBox_FrameFormat
            // 
            this.comboBox_FrameFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_FrameFormat.Enabled = false;
            this.comboBox_FrameFormat.FormattingEnabled = true;
            this.comboBox_FrameFormat.Items.AddRange(new object[] {
            "数据帧",
            "远程帧"});
            this.comboBox_FrameFormat.Location = new System.Drawing.Point(323, 18);
            this.comboBox_FrameFormat.Name = "comboBox_FrameFormat";
            this.comboBox_FrameFormat.Size = new System.Drawing.Size(70, 20);
            this.comboBox_FrameFormat.TabIndex = 1;
            // 
            // setBaudcomboBox
            // 
            this.setBaudcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setBaudcomboBox.FormattingEnabled = true;
            this.setBaudcomboBox.Items.AddRange(new object[] {
            "双滤波",
            "单滤波"});
            this.setBaudcomboBox.Location = new System.Drawing.Point(440, 18);
            this.setBaudcomboBox.Name = "setBaudcomboBox";
            this.setBaudcomboBox.Size = new System.Drawing.Size(70, 20);
            this.setBaudcomboBox.TabIndex = 6;
            // 
            // comboBox_FrameType
            // 
            this.comboBox_FrameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_FrameType.Enabled = false;
            this.comboBox_FrameType.FormattingEnabled = true;
            this.comboBox_FrameType.Items.AddRange(new object[] {
            "标准帧",
            "扩展帧"});
            this.comboBox_FrameType.Location = new System.Drawing.Point(193, 18);
            this.comboBox_FrameType.Name = "comboBox_FrameType";
            this.comboBox_FrameType.Size = new System.Drawing.Size(70, 20);
            this.comboBox_FrameType.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(274, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "帧格式:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(516, 17);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 6;
            this.buttonConnect.Text = "连接";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // button_StopCAN
            // 
            this.button_StopCAN.Location = new System.Drawing.Point(678, 17);
            this.button_StopCAN.Name = "button_StopCAN";
            this.button_StopCAN.Size = new System.Drawing.Size(75, 23);
            this.button_StopCAN.TabIndex = 7;
            this.button_StopCAN.Text = "复位CAN";
            this.button_StopCAN.UseVisualStyleBackColor = true;
            this.button_StopCAN.Click += new System.EventHandler(this.button_StopCAN_Click);
            // 
            // button_StartCAN
            // 
            this.button_StartCAN.Location = new System.Drawing.Point(597, 17);
            this.button_StartCAN.Name = "button_StartCAN";
            this.button_StartCAN.Size = new System.Drawing.Size(75, 23);
            this.button_StartCAN.TabIndex = 8;
            this.button_StartCAN.Text = "启动CAN";
            this.button_StartCAN.UseVisualStyleBackColor = true;
            this.button_StartCAN.Click += new System.EventHandler(this.button_StartCAN_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(144, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "帧类型:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "发送格式:";
            // 
            // comboBox_SendType
            // 
            this.comboBox_SendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SendType.FormattingEnabled = true;
            this.comboBox_SendType.Items.AddRange(new object[] {
            "正常发送",
            "单次正常发送",
            "自发自收",
            "单次自发自收"});
            this.comboBox_SendType.Location = new System.Drawing.Point(67, 18);
            this.comboBox_SendType.Name = "comboBox_SendType";
            this.comboBox_SendType.Size = new System.Drawing.Size(70, 20);
            this.comboBox_SendType.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.listBox_Info);
            this.groupBox3.Controls.Add(this.button_Send);
            this.groupBox3.Controls.Add(this.textBox_Data);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.textBox_ID);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(12, 479);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(759, 137);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "手动发送数据帧";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(204, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "D0-D1-D2-D3-D4-D5-D6-D7";
            // 
            // listBox_Info
            // 
            this.listBox_Info.FormattingEnabled = true;
            this.listBox_Info.ItemHeight = 12;
            this.listBox_Info.Location = new System.Drawing.Point(12, 55);
            this.listBox_Info.Name = "listBox_Info";
            this.listBox_Info.Size = new System.Drawing.Size(738, 76);
            this.listBox_Info.TabIndex = 0;
            // 
            // button_Send
            // 
            this.button_Send.Location = new System.Drawing.Point(476, 26);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(75, 23);
            this.button_Send.TabIndex = 5;
            this.button_Send.Text = "发送";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // textBox_Data
            // 
            this.textBox_Data.Location = new System.Drawing.Point(207, 27);
            this.textBox_Data.Name = "textBox_Data";
            this.textBox_Data.Size = new System.Drawing.Size(251, 21);
            this.textBox_Data.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(170, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "数据:";
            // 
            // textBox_ID
            // 
            this.textBox_ID.Location = new System.Drawing.Point(60, 27);
            this.textBox_ID.Name = "textBox_ID";
            this.textBox_ID.Size = new System.Drawing.Size(70, 21);
            this.textBox_ID.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "帧ID:0x";
            // 
            // timer_rec
            // 
            this.timer_rec.Tick += new System.EventHandler(this.timer_rec_Tick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.MdataGridView);
            this.groupBox5.Controls.Add(this.viewMDataTypebutton);
            this.groupBox5.Controls.Add(this.viewSDataTypebutton);
            this.groupBox5.Controls.Add(this.SdataGridView);
            this.groupBox5.Location = new System.Drawing.Point(12, 151);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(755, 322);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "信息控制";
            // 
            // SdataGridView
            // 
            this.SdataGridView.AllowUserToOrderColumns = true;
            this.SdataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridID,
            this.gridV,
            this.gridC,
            this.gridT,
            this.gridDestination,
            this.gridNowPosition,
            this.gridInitial,
            this.gridPLimit,
            this.gridNLimit});
            this.SdataGridView.Location = new System.Drawing.Point(37, 16);
            this.SdataGridView.Name = "SdataGridView";
            this.SdataGridView.RowHeadersWidth = 25;
            this.SdataGridView.RowTemplate.Height = 23;
            this.SdataGridView.Size = new System.Drawing.Size(710, 300);
            this.SdataGridView.TabIndex = 8;
            // 
            // set_01_comboBox
            // 
            this.set_01_comboBox.DropDownHeight = 90;
            this.set_01_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.set_01_comboBox.FormattingEnabled = true;
            this.set_01_comboBox.IntegralHeight = false;
            this.set_01_comboBox.ItemHeight = 12;
            this.set_01_comboBox.Items.AddRange(new object[] {
            "数据帧",
            "远程帧"});
            this.set_01_comboBox.Location = new System.Drawing.Point(582, 28);
            this.set_01_comboBox.MaxDropDownItems = 3;
            this.set_01_comboBox.Name = "set_01_comboBox";
            this.set_01_comboBox.Size = new System.Drawing.Size(90, 20);
            this.set_01_comboBox.TabIndex = 35;
            // 
            // setDestination_01_textBox
            // 
            this.setDestination_01_textBox.Location = new System.Drawing.Point(418, 41);
            this.setDestination_01_textBox.MaxLength = 4;
            this.setDestination_01_textBox.Name = "setDestination_01_textBox";
            this.setDestination_01_textBox.Size = new System.Drawing.Size(54, 21);
            this.setDestination_01_textBox.TabIndex = 19;
            this.setDestination_01_textBox.Text = "0";
            // 
            // setPLimite_01_textBox
            // 
            this.setPLimite_01_textBox.Location = new System.Drawing.Point(358, 41);
            this.setPLimite_01_textBox.MaxLength = 4;
            this.setPLimite_01_textBox.Name = "setPLimite_01_textBox";
            this.setPLimite_01_textBox.Size = new System.Drawing.Size(54, 21);
            this.setPLimite_01_textBox.TabIndex = 18;
            this.setPLimite_01_textBox.Text = "0";
            // 
            // setNlimit_01_settextBox
            // 
            this.setNlimit_01_settextBox.Location = new System.Drawing.Point(298, 41);
            this.setNlimit_01_settextBox.MaxLength = 4;
            this.setNlimit_01_settextBox.Name = "setNlimit_01_settextBox";
            this.setNlimit_01_settextBox.Size = new System.Drawing.Size(54, 21);
            this.setNlimit_01_settextBox.TabIndex = 17;
            this.setNlimit_01_settextBox.Text = "0";
            // 
            // setExecutivetime_01_textBox
            // 
            this.setExecutivetime_01_textBox.Location = new System.Drawing.Point(238, 41);
            this.setExecutivetime_01_textBox.MaxLength = 3;
            this.setExecutivetime_01_textBox.Name = "setExecutivetime_01_textBox";
            this.setExecutivetime_01_textBox.Size = new System.Drawing.Size(54, 21);
            this.setExecutivetime_01_textBox.TabIndex = 16;
            this.setExecutivetime_01_textBox.Text = "0";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(418, 20);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(54, 21);
            this.textBox4.TabIndex = 15;
            this.textBox4.Text = "目标位置";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(358, 20);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(54, 21);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "顺最大值";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(298, 20);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(54, 21);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "逆最大值";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(238, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(54, 21);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "执行时间";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // readDatabutton
            // 
            this.readDatabutton.Location = new System.Drawing.Point(157, 20);
            this.readDatabutton.Name = "readDatabutton";
            this.readDatabutton.Size = new System.Drawing.Size(75, 35);
            this.readDatabutton.TabIndex = 11;
            this.readDatabutton.Text = "读取数据";
            this.readDatabutton.UseVisualStyleBackColor = true;
            this.readDatabutton.Click += new System.EventHandler(this.readDatabutton_Click);
            // 
            // sendDatabutton_01
            // 
            this.sendDatabutton_01.Location = new System.Drawing.Point(678, 20);
            this.sendDatabutton_01.Name = "sendDatabutton_01";
            this.sendDatabutton_01.Size = new System.Drawing.Size(75, 35);
            this.sendDatabutton_01.TabIndex = 10;
            this.sendDatabutton_01.Text = "发送命令";
            this.sendDatabutton_01.UseVisualStyleBackColor = true;
            this.sendDatabutton_01.Click += new System.EventHandler(this.sendDatabutton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.IDSelectcomboBox);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.set_01_comboBox);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.setDestination_01_textBox);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.readDatabutton);
            this.groupBox2.Controls.Add(this.setPLimite_01_textBox);
            this.groupBox2.Controls.Add(this.setExecutivetime_01_textBox);
            this.groupBox2.Controls.Add(this.sendDatabutton_01);
            this.groupBox2.Controls.Add(this.setNlimit_01_settextBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 69);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息配置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(478, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "配置信息类型：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "选择ID：";
            // 
            // IDSelectcomboBox
            // 
            this.IDSelectcomboBox.FormattingEnabled = true;
            this.IDSelectcomboBox.ItemHeight = 12;
            this.IDSelectcomboBox.Location = new System.Drawing.Point(72, 28);
            this.IDSelectcomboBox.MaxDropDownItems = 12;
            this.IDSelectcomboBox.Name = "IDSelectcomboBox";
            this.IDSelectcomboBox.Size = new System.Drawing.Size(65, 20);
            this.IDSelectcomboBox.TabIndex = 36;
            // 
            // gridID
            // 
            this.gridID.HeaderText = "ID";
            this.gridID.Name = "gridID";
            this.gridID.ReadOnly = true;
            this.gridID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridV
            // 
            this.gridV.HeaderText = "电压/V";
            this.gridV.Name = "gridV";
            this.gridV.ReadOnly = true;
            // 
            // gridC
            // 
            this.gridC.HeaderText = "电流/A";
            this.gridC.Name = "gridC";
            this.gridC.ReadOnly = true;
            // 
            // gridT
            // 
            this.gridT.HeaderText = "温度/度";
            this.gridT.Name = "gridT";
            this.gridT.ReadOnly = true;
            // 
            // gridDestination
            // 
            this.gridDestination.HeaderText = "目的/度";
            this.gridDestination.Name = "gridDestination";
            this.gridDestination.ReadOnly = true;
            // 
            // gridNowPosition
            // 
            this.gridNowPosition.HeaderText = "当前/度";
            this.gridNowPosition.Name = "gridNowPosition";
            this.gridNowPosition.ReadOnly = true;
            // 
            // gridInitial
            // 
            this.gridInitial.HeaderText = "零点度";
            this.gridInitial.Name = "gridInitial";
            this.gridInitial.ReadOnly = true;
            // 
            // gridPLimit
            // 
            this.gridPLimit.HeaderText = "顺最大值/度";
            this.gridPLimit.Name = "gridPLimit";
            this.gridPLimit.ReadOnly = true;
            // 
            // gridNLimit
            // 
            this.gridNLimit.HeaderText = "逆最大值/度";
            this.gridNLimit.Name = "gridNLimit";
            this.gridNLimit.ReadOnly = true;
            // 
            // viewSDataTypebutton
            // 
            this.viewSDataTypebutton.Location = new System.Drawing.Point(6, 65);
            this.viewSDataTypebutton.Name = "viewSDataTypebutton";
            this.viewSDataTypebutton.Size = new System.Drawing.Size(25, 70);
            this.viewSDataTypebutton.TabIndex = 9;
            this.viewSDataTypebutton.Text = "舵机";
            this.viewSDataTypebutton.UseVisualStyleBackColor = true;
            this.viewSDataTypebutton.Click += new System.EventHandler(this.viewSDataTypebutton_Click);
            // 
            // viewMDataTypebutton
            // 
            this.viewMDataTypebutton.Location = new System.Drawing.Point(6, 163);
            this.viewMDataTypebutton.Name = "viewMDataTypebutton";
            this.viewMDataTypebutton.Size = new System.Drawing.Size(25, 70);
            this.viewMDataTypebutton.TabIndex = 10;
            this.viewMDataTypebutton.Text = "电缸";
            this.viewMDataTypebutton.UseVisualStyleBackColor = true;
            this.viewMDataTypebutton.Click += new System.EventHandler(this.viewMDataTypebutton_Click);
            // 
            // MdataGridView
            // 
            this.MdataGridView.AllowUserToOrderColumns = true;
            this.MdataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.MdataGridView.Location = new System.Drawing.Point(37, 16);
            this.MdataGridView.Name = "MdataGridView";
            this.MdataGridView.RowHeadersWidth = 25;
            this.MdataGridView.RowTemplate.Height = 23;
            this.MdataGridView.Size = new System.Drawing.Size(710, 300);
            this.MdataGridView.TabIndex = 11;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "电压/V";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "电流/A";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "温度/度";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "目的/度";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "当前/度";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(782, 617);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "CAN_COM_F2.0_plus_VCI_USBCAN2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SdataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MdataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button button_StopCAN;
        private System.Windows.Forms.Button button_StartCAN;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox_FrameFormat;
        private System.Windows.Forms.ComboBox comboBox_FrameType;
        private System.Windows.Forms.Button button_Send;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox_SendType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_Data;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_ID;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListBox listBox_Info;
        private System.Windows.Forms.Timer timer_rec;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView SdataGridView;
        private System.Windows.Forms.Button sendDatabutton_01;
        private System.Windows.Forms.Button readDatabutton;
        private System.Windows.Forms.TextBox setDestination_01_textBox;
        private System.Windows.Forms.TextBox setPLimite_01_textBox;
        private System.Windows.Forms.TextBox setNlimit_01_settextBox;
        private System.Windows.Forms.TextBox setExecutivetime_01_textBox;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox set_01_comboBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox setBaudcomboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox IDSelectcomboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridID;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridV;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridC;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridT;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDestination;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridNowPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridInitial;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridPLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridNLimit;
        private System.Windows.Forms.Button viewSDataTypebutton;
        private System.Windows.Forms.Button viewMDataTypebutton;
        private System.Windows.Forms.DataGridView MdataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}

