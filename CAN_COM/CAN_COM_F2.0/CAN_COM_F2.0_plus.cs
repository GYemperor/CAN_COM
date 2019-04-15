using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


//1.ZLGCAN系列接口卡信息的数据类型。
public struct VCI_BOARD_INFO
{
    public UInt16 hw_Version;
    public UInt16 fw_Version;
    public UInt16 dr_Version;
    public UInt16 in_Version;
    public UInt16 irq_Num;
    public byte can_Num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] public byte[] str_Serial_Num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
    public byte[] str_hw_Type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] Reserved;
}
//2.定义CAN信息帧的数据类型。
unsafe public struct VCI_CAN_OBJ  //使用不安全代码
{
    public uint ID;
    public uint TimeStamp;
    public byte TimeFlag;
    public byte SendType;
    public byte RemoteFlag;//是否是远程帧
    public byte ExternFlag;//是否是扩展帧
    public byte DataLen;

    public fixed byte Data[8];

    public fixed byte Reserved[3];

}
//3.定义CAN控制器状态的数据类型。
public struct VCI_CAN_STATUS
{
    public byte ErrInterrupt;
    public byte regMode;
    public byte regStatus;
    public byte regALCapture;
    public byte regECCapture;
    public byte regEWLimit;
    public byte regRECounter;
    public byte regTECounter;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Reserved;
}
//4.定义错误信息的数据类型。
public struct VCI_ERR_INFO
{
    public UInt32 ErrCode;
    public byte Passive_ErrData1;
    public byte Passive_ErrData2;
    public byte Passive_ErrData3;
    public byte ArLost_ErrData;
}

//5.定义初始化CAN的数据类型
public struct VCI_INIT_CONFIG
{
    public UInt32 AccCode;
    public UInt32 AccMask;
    public UInt32 Reserved;
    public byte Filter;
    public byte Timing0;
    public byte Timing1;
    public byte Mode;
}
public struct CHGDESIPANDPORT
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public byte[] szpwd;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] szdesip;
    public Int32 desport;

    public void Init()
    {
        szpwd = new byte[10];
        szdesip = new byte[20];
    }
}


namespace CAN_COM_F2._0
{
    public partial class Form1 : Form
    {

        const int VCI_USBCAN2 = 4;

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);

        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);

        static UInt32 m_devtype = 4;//USBCAN2

        UInt32 m_bOpen = 0;
        UInt32 m_devind = 0;
        UInt32 m_canind = 0;

        VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];

        UInt32[] m_arrdevtype = new UInt32[20];

         /*发送速率时钟参数选择数组*/    
        string[] timing = { "031c","011c","001c","0014"};

        //命令解析
        string sendFrameID, sendStep_1_Str, sendAction_1_DataHStr, sendAction_1_DataLStr, sendTime_1_Str,
                            sendStep_2_Str, sendAction_2_DataHStr, sendAction_2_DataLStr, sendTime_2_Str;
        string[] getAction_1_DataArrayStr = new string[15];
        string[] getTime_1_DataArrayStr = new string[15];
        string[] getAction_2_DataArrayStr = new string[15];
        string[] getTime_2_DataArrayStr = new string[15];

        String[] actionIDArray = {  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O"};
        String[] deviceIDArray = { "YE01","YE02","YE03","YE04","YE05","YE06","YE07","YE08",
                                    "YE09","YE10","YE11","YE12","YE13","YE14","YE15","YE16",
                                     "YE17","YE18","YE19","YE20","YE21","YE22","YE23","YE24",
                                    "YS01","YS02","YS03","YS04","YS05","YS06","YS07",
                                    "YS08","YS09","YS20","YS21","YS22","YS23","YS24"};


        /*定义舵机逆时针最大值，顺时针最大值，目标位置*/
        string SExecutiveTimeStr, SNMaxStr, SPMaxStr, SDestinationStr;
        /*定义电缸零点位置、电缸执行时间、电缸长度高字节、电缸长度低字节、目标位置高字节、目标位置低字节、位置PID-KP高字节、位置PID-KP低字节*/
        string MZeroStr,MExecutiveTimeStr, MLengthHStr, MLengthLStr, MDestinationHStr, MDestinationLStr, MPositionHStr, MPositionLStr;
        String MDestinationStr;
        /*定义电缸数据使能*/
        string MDataEnabledStr;
        

        /*定义设备id号地址段数组0-255*/
        /*定义设备id号地址段数组0-255*/
        /*目前仅定义1-20*/
        String[] IDSTRAdresss = { "01   0x01", "02   0x02", "03   0x03", "04   0x04", "05   0x05", "06   0x06", "07   0x07", "08   0x08",
            "09   0x09", "10   0x0A","11   0x0B","12   0x0C","13   0x0D","14   0x0E","15   0x0F","16   0x10","17   0x11","18   0x12","19   0x13","20   0x14","21   0x15",
            "22   0x16","23   0x17","24   0x18","25   0x19","26   0x1A","27   0x1B","28   0x1C","29   0x1D","30   0x1E" };
        String[] IDAdresss = { "01", "02", "03", "04", "05", "06", "07", "08",
            "09", "0A","0B","0C","0D","0E","0F","10","11","12","13","14","15",
            "16","17","18","19","1A","1B","1C","1D","1E" };
        /*电缸零点位置数组*/
        String[] MZeroPosition = { "底部","顶部"};
        /*电缸故障状态数组*/
        String[] MFaultState = {"正常", "过流", "过压" ,"" ,"编码器故障" ,"","","",
                                "位置偏差大","","","","","","","",
                                "欠压","","","","","","","",
                                   "","","","","","","","","过载"};
        /*语音模块播放模式数组*/
        String[] BPlayType = { "01", "FE", "FF" };
        /*定义读取文件出错行号*/
        int count_lineError = 0;
        /*定义读取文件出错标志*/
        Boolean txtErrorFlag = false;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            /*初始化电缸故障状态数组*/
            //MFaultState[0] = "正常";
            //MFaultState[1] = "过流";
            //MFaultState[2] = "过压";
            //MFaultState[4] = "编码器故障";
            //MFaultState[8] = "位置偏差大";
            //MFaultState[16] = "欠压";
            //MFaultState[32] = "过载";
            /*初始化舵机/电缸显示界面*/
            SdataGridView.Visible = true;
            MdataGridView.Visible = false;
            SDatagroupBox.Visible = true;
            MDatagroupBox.Visible = false;
            BgroupBox.Visible = false;
            viewSDataTypebutton.Enabled = false;
            viewMDataTypebutton.Enabled = true;
            SetBbutton.Enabled = true;

            /*初始化电缸datagridview*/
            /*共计30行*/
            for (int i=0;i<30;i++)
            {
                this.MdataGridView.Rows.Add();
                this.MdataGridView.Rows[i].Cells[0].Value = IDSTRAdresss[i];
            }
            /*初始化电缸ID下拉选择框*/
            MIDSelectcomboBox.Items.Clear();
            for (int i = 0; i < 30; i++)
            {
                MIDSelectcomboBox.Items.Add((i + 1).ToString());
            }
            MIDSelectcomboBox.SelectedIndex = 0;

            /*初始化电缸零点位置下拉选则框*/
            MZeroPositioncomboBox.Items.Clear();
            MZeroPositioncomboBox.Items.Add("底部");
            MZeroPositioncomboBox.Items.Add("顶部");
            MZeroPositioncomboBox.SelectedIndex = 0;
            /*初始化电缸配置信息区域下拉选择框*/
            MSetcomboBox.Items.Clear();
            MSetcomboBox.Items.Add("调试信息");
            MSetcomboBox.Items.Add("设定参数");
            MSetcomboBox.Items.Add("置当前为零点");
            MSetcomboBox.Items.Add("装配模式");
            MSetcomboBox.Items.Add("检测模式");
            MSetcomboBox.SelectedIndex = 0;
            /*初始化舵机dataGridView*/
            /*共计16行*/
            for (int i = 0;i<16;i++)
            {
                this.SdataGridView.Rows.Add();
                this.SdataGridView.Rows[i].Cells[0].Value = IDSTRAdresss[i];
            }

            /*初始化舵机ID选择下拉选择框*/
            SIDSelectcomboBox.Items.Clear();
            for (int i=0;i<16;i++)
            {
                SIDSelectcomboBox.Items.Add((i+1).ToString());
            }
            SIDSelectcomboBox.SelectedIndex = 0;

            /*初始化语音模块下拉选择框*/
            BPlayTypecomboBox.Items.Clear();
            BPlayTypecomboBox.Items.Add("播放一遍");
            BPlayTypecomboBox.Items.Add("单曲循环");
            BPlayTypecomboBox.Items.Add("全部循环");
            BPlayTypecomboBox.SelectedIndex = 1;
            /*初始化舵机配置信息区域下拉选择框*/
            SSet_01_comboBox.Items.Clear();
            SSet_01_comboBox.Items.Add("发送舵机信息");
            SSet_01_comboBox.Items.Add("置当前目标为零点");
            SSet_01_comboBox.Items.Add("舵机装配模式");
            SSet_01_comboBox.Items.Add("舵机检测模式");
            SSet_01_comboBox.SelectedIndex = 0;

            /*波特率选择*/
            setBaudcomboBox.Items.Clear();
            setBaudcomboBox.Items.Add("125kb/s");
            setBaudcomboBox.Items.Add("250kb/s");
            setBaudcomboBox.Items.Add("500kb/s");
            setBaudcomboBox.Items.Add("1000kb/s");
            setBaudcomboBox.SelectedIndex = 1;//推荐选择250kb/s

    
            /*帧发送种类、帧格式、帧类型下拉选择框初始化*/
            comboBox_SendType.SelectedIndex = 0;
            comboBox_FrameFormat.SelectedIndex = 0;
            comboBox_FrameType.SelectedIndex = 1;
            /*手动发送区ID及数据初始化*/
            textBox_ID.Text = "00150124";
            textBox_Data.Text = "00 00 00 00 00 00 00 00 ";


        }

        private void openTxtbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "命令txt文件(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string txt_FilePath = openFileDialog.FileName;
                StreamReader streamReader = new StreamReader(txt_FilePath, Encoding.Default);

                sendData("1E01005F", "00", "00", "00", "00", "00", "00", "00", "00");
                System.Threading.Thread.Sleep(30);
                
                while (!streamReader.EndOfStream)
                {
                    count_lineError++;
                    String txtStr = streamReader.ReadLine();
                    //analyse and send data here
                    codeOderAndSendData(txtStr);
                }
                count_lineError = 0;
                streamReader.Close();
                sendData("1F01005F", "00", "00", "00", "00", "00", "00", "00", "00");
                MessageBox.Show("发送完毕！");
            }
            else
            {
                MessageBox.Show("请打开文本文件");
            }

        }


        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (m_bOpen == 1)
            {
                VCI_CloseDevice(m_devtype, m_devind);
                m_bOpen = 0;
            }
            else
            {
                m_devtype = VCI_USBCAN2;

                
                if (VCI_OpenDevice(m_devtype, m_devind, 0) == 0)
                {
                    
                    MessageBox.Show("打开设备失败,请检查设备类型和设备索引号是否正确", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                m_bOpen = 1;
                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                config.AccCode = System.Convert.ToUInt32("0x00000000", 16);//验收码
                config.AccMask = System.Convert.ToUInt32("0xFFFFFFFF", 16);//屏蔽码

                
                config.Timing0 = System.Convert.ToByte("0x" + timing[setBaudcomboBox.SelectedIndex].Substring(0,2), 16);//定时器0
                config.Timing1 = System.Convert.ToByte("0x" + timing[setBaudcomboBox.SelectedIndex].Substring(2, 2), 16);//定时器1
                
                config.Filter = 1;
                //config.Filter = (Byte)comboBox_Filter.SelectedIndex;
                config.Mode = 0;
                //config.Mode = (Byte)comboBox_Mode.SelectedIndex;
                VCI_InitCAN(m_devtype, 0, 0, ref config);
               // VCI_InitCAN(m_devtype, m_devind, m_canind, ref config);
            }
            buttonConnect.Text = m_bOpen == 1 ? "断开" : "连接";
            timer_rec.Enabled = m_bOpen == 1 ? true : false;
        }

        unsafe private void readAllMDatabutton_Click(object sender, EventArgs e)
        {

            MdataGridView.Rows.Clear();
            for (int i = 0; i < 30; i++)
            {
                this.MdataGridView.Rows.Add();
                this.MdataGridView.Rows[i].Cells[0].Value = IDSTRAdresss[i];
            }


            if (m_bOpen == 0)
                return;

            sendData("0003fe53",
                "00", "00", "00", "00", "00", "00", "00", "00");
        }

        unsafe private void readAllSDatabutton_Click(object sender, EventArgs e)
        {

            SdataGridView.Rows.Clear();
            for (int i = 0; i < 16; i++)
            {
                this.SdataGridView.Rows.Add();
                this.SdataGridView.Rows[i].Cells[0].Value = IDSTRAdresss[i];
            }
            if (m_bOpen == 0)
                return;

            sendData("0002fe53",
                "00", "00", "00", "00", "00", "00", "00", "00");
        }

        private void button_StartCAN_Click(object sender, EventArgs e)
        {
            if (m_bOpen == 0)
                return;
            VCI_StartCAN(m_devtype, m_devind, m_canind);
        }

        private void button_StopCAN_Click(object sender, EventArgs e)
        {
            if (m_bOpen == 0)
                return;
            
            VCI_ResetCAN(m_devtype, m_devind, m_canind);

        }

        unsafe private void button_Send_Click(object sender, EventArgs e)
        {
            if (m_bOpen == 0)
                return;

            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();
            //sendobj.Init();
            sendobj.SendType = (byte)comboBox_SendType.SelectedIndex;
            sendobj.RemoteFlag = (byte)comboBox_FrameFormat.SelectedIndex;
            sendobj.ExternFlag = (byte)comboBox_FrameType.SelectedIndex;
            sendobj.ID = System.Convert.ToUInt32("0x" + textBox_ID.Text, 16);
            int len = (textBox_Data.Text.Length + 1) / 3;
            sendobj.DataLen = System.Convert.ToByte(len);
            String strdata = textBox_Data.Text;
            int i = -1;
            if (i++ < len - 1)
                sendobj.Data[0] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[1] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[2] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[3] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[4] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[5] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[6] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
            if (i++ < len - 1)
                sendobj.Data[7] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);

            if (VCI_Transmit(m_devtype, m_devind, m_canind, ref sendobj, 1) == 0)
            {
                MessageBox.Show("发送失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BPlaybutton_Click(object sender, EventArgs e)
        {
            String BPlayOrderStr, BPlayVolumeStr;
            BPlayOrderStr = Convert.ToString(Convert.ToInt16(BOrdertextBox.Text, 10), 16);
            BPlayVolumeStr = Convert.ToString(Convert.ToInt16(BVolumetextBox.Text, 10), 16);
            if (BPlayOrderStr.Length == 1)
            {
                BPlayOrderStr = "0" + BPlayOrderStr;
            }
            if (BPlayVolumeStr.Length == 1)
            {
                BPlayVolumeStr = "0" + BPlayVolumeStr;
            }
            //播放
            sendData("00040152","00", "00", "00", "00", "01", BPlayVolumeStr, BPlayType[BPlayTypecomboBox.SelectedIndex%3], BPlayOrderStr);

        }

        private void SetBbutton_Click(object sender, EventArgs e)
        {
            SdataGridView.Visible = false;
            MdataGridView.Visible = false;
            SDatagroupBox.Visible = false;
            MDatagroupBox.Visible = false;
            BgroupBox.Visible = true;
            viewMDataTypebutton.Enabled = true;
            viewSDataTypebutton.Enabled = true;
            SetBbutton.Enabled = false;
        }

        private void BStopbutton_Click(object sender, EventArgs e)
        {
            String BPlayOrderStr, BPlayVolumeStr;
            BPlayOrderStr = Convert.ToString(Convert.ToInt16(BOrdertextBox.Text, 10), 16);
            BPlayVolumeStr = Convert.ToString(Convert.ToInt16(BVolumetextBox.Text, 10), 16);
            if (BPlayOrderStr.Length == 1)
            {
                BPlayOrderStr = "0" + BPlayOrderStr;
            }
            if (BPlayVolumeStr.Length == 1)
            {
                BPlayVolumeStr = "0" + BPlayVolumeStr;
            }
            //停止播放
            sendData("00040152","00", "00", "00", "00", "FE", BPlayVolumeStr, BPlayType[BPlayTypecomboBox.SelectedIndex % 3], BPlayOrderStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string txtStr = orderAnalysetextBox.Text;
            if(txtStr.Length<4)
            {
                MessageBox.Show("请输入命令", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                codeOderAndSendData(txtStr);
            }
            

        }

        unsafe private void timer_rec_Tick(object sender, EventArgs e)
        {

            //设置电机datagridview的光标
            for (int i = 0; i < 30; i++)
            {
                if (MIDSelectcomboBox.SelectedIndex == i)
                {
                    MdataGridView.CurrentCell = this.MdataGridView.Rows[i].Cells[0];
                    this.MdataGridView.Rows[i].DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
                else
                {
                    this.MdataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }

            //设置舵机datagridview的光标
            for (int i=0;i<16;i++)
            {
                if(SIDSelectcomboBox.SelectedIndex == i)
                {
                    SdataGridView.CurrentCell = this.SdataGridView.Rows[i].Cells[0];
                    this.SdataGridView.Rows[i].DefaultCellStyle.BackColor = Color.SpringGreen;
                }
                else
                {
                    this.SdataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
            

            UInt32 res = new UInt32();


            res = VCI_GetReceiveNum(m_devtype, m_devind, m_canind);
            if (res == 0)
                return;

            /////////////////////////////////////
            UInt32 con_maxlen = 50;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);

            res = VCI_Receive(m_devtype, m_devind, m_canind, pt, con_maxlen, 100);
            ////////////////////////////////////////////////////////

            String str = "";
            for (UInt32 i = 0; i < res; i++)
            {
                VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                str += DateTime.Now.ToString();
                str += "  帧ID:0x" + "00"+System.Convert.ToString((Int32)obj.ID, 16);

                /*解析发给上位机的frame*/
                /*预留高两位*/
                /*发送给上位机的数据帧*/
                /*判断ID有效位数，转化为字符串应该为六位*/
                if (System.Convert.ToString((Int32)obj.ID, 16).Length == 6)//不等于6位则无效
                {
                    str += " ID有效";
                    if (System.Convert.ToString((Int32)obj.ID, 16).Substring(1, 1) == "5")
                    {
                        /*返回的数据*/
                        if (System.Convert.ToString((Int32)obj.ID, 16).Substring(5, 1) == "4")
                        {
                            /*收到的是数据帧而非远程帧*/
                            if (obj.RemoteFlag == 0)
                            {
                                //舵机和电缸的数组索引值，最大索引值不超过30
                                Int32 ID_Str_Value_index = ((((Int32)obj.ID) / 16 / 16 % 16) - 1 + ((Int32)obj.ID) / 16 / 16 / 16 % 16 * 16) %30;

                                //////////////////
                                /*来自舵机的数据*/
                                //////////////////
                                if (System.Convert.ToString((Int32)obj.ID, 16).Substring(4, 1) == "2")//来自舵机返回的数据
                                {
                                    str += "  舵机数据";
                                    //舵机数据1
                                    if (System.Convert.ToString((Int32)obj.ID, 16).Substring(0, 1) == "1")
                                    {
                                        str += "  页一";
                                        //舵机电压
                                        this.SdataGridView.Rows[ID_Str_Value_index].Cells[1].Value =
                                        System.Int32.Parse(System.Convert.ToString(obj.Data[0] / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                        + "." + System.Int32.Parse(System.Convert.ToString(obj.Data[0] % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                        //舵机电流
                                        this.SdataGridView.Rows[ID_Str_Value_index].Cells[2].Value =
                                        System.Int32.Parse(System.Convert.ToString(obj.Data[1] / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                        + "." + System.Int32.Parse(System.Convert.ToString(obj.Data[1] % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                        //舵机温度
                                        this.SdataGridView.Rows[ID_Str_Value_index].Cells[3].Value =
                                        System.Int32.Parse(System.Convert.ToString(obj.Data[2], 16), System.Globalization.NumberStyles.HexNumber);
                                    }
                                    //舵机数据2
                                    if (System.Convert.ToString((Int32)obj.ID, 16).Substring(0, 1) == "2")
                                    {
                                        str += "  页二";
                                        //舵机目标位置
                                        if (obj.Data[7] % 2 == 0)
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[4].Value =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[0], 16), System.Globalization.NumberStyles.HexNumber);
                                            DestinationPositionlabel.Text =
                                                System.Int32.Parse(System.Convert.ToString(obj.Data[0], 16), System.Globalization.NumberStyles.HexNumber).ToString();

                                        }
                                        else
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[4].Value =
                                            "-"+System.Int32.Parse(System.Convert.ToString(obj.Data[0], 16), System.Globalization.NumberStyles.HexNumber);
                                            DestinationPositionlabel.Text =
                                                "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[0], 16), System.Globalization.NumberStyles.HexNumber).ToString();

                                        }
                                        //舵机当前位置
                                        if (obj.Data[7] / 2 % 2 == 0)
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[5].Value =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[1], 16), System.Globalization.NumberStyles.HexNumber);

                                        }
                                        else
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[5].Value =
                                            "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[1], 16), System.Globalization.NumberStyles.HexNumber);

                                        }
                                        //舵机0点位置
                                        if (obj.Data[7] / 4 % 2 == 0)
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[6].Value =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[2], 16), System.Globalization.NumberStyles.HexNumber);

                                        }
                                        else
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[6].Value =
                                           "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[2], 16), System.Globalization.NumberStyles.HexNumber);

                                        }

                                        //舵机顺时针最大位置
                                        if (obj.Data[7] / 8 % 2 == 0)
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[7].Value =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[3], 16), System.Globalization.NumberStyles.HexNumber);
                                            PLimitlabel.Text =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[3], 16), System.Globalization.NumberStyles.HexNumber).ToString();
                                        }
                                        else
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[7].Value =
                                            "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[3], 16), System.Globalization.NumberStyles.HexNumber);
                                            PLimitlabel.Text = "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[3], 16), System.Globalization.NumberStyles.HexNumber).ToString();
                                        }
                                        //舵机逆时针最大位置
                                        if (obj.Data[7] / 16 % 2 == 0)
                                        {

                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[8].Value =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[4], 16), System.Globalization.NumberStyles.HexNumber);
                                            NLimitlabel.Text =
                                            System.Int32.Parse(System.Convert.ToString(obj.Data[4], 16), System.Globalization.NumberStyles.HexNumber).ToString();
                                        }
                                        else
                                        {
                                            this.SdataGridView.Rows[ID_Str_Value_index].Cells[8].Value =
                                            "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[4], 16), System.Globalization.NumberStyles.HexNumber);
                                            NLimitlabel.Text = "-" + System.Int32.Parse(System.Convert.ToString(obj.Data[4], 16), System.Globalization.NumberStyles.HexNumber).ToString();
                                        }
                                    }


                                }
                                /////////////////
                                /*来自电缸的数据*/
                                /*电缸的数据有两页*/
                                /*需进行判别处理*/
                                if (System.Convert.ToString((Int32)obj.ID, 16).Substring(4, 1) == "3")
                                {
                                    str += "  电缸数据";
                                    /*电缸数据页一*/
                                    //母线电压
                                    //电流
                                    //目标位置
                                    //当前位置
                                    //故障状态
                                    //零点位置
                                    if (System.Convert.ToString((Int32)obj.ID, 16).Substring(0, 1) == "1")
                                    {
                                        //电缸电压
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[1].Value =
                                        System.Int32.Parse(System.Convert.ToString(obj.Data[0], 16), System.Globalization.NumberStyles.HexNumber);
                                        //电缸电流
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[2].Value =
                                        System.Int32.Parse(System.Convert.ToString(obj.Data[1] / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                        + "." + System.Int32.Parse(System.Convert.ToString(obj.Data[1] % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                        //电缸目标位置
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[3].Value =
                                        System.Int16.Parse(System.Convert.ToString((obj.Data[2] + obj.Data[3] * 16 * 16), 16), System.Globalization.NumberStyles.HexNumber);
                                        

                                        //电缸当前位置
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[4].Value =
                                        System.Int16.Parse(System.Convert.ToString((obj.Data[4] + obj.Data[5] * 16 * 16), 16), System.Globalization.NumberStyles.HexNumber);

                                        //电缸故障状态
                                        /*故障状态采用字符数组索引*/
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[5].Value = MFaultState[obj.Data[6] % 33];
                                        //电缸零点位置
                                        /*零点位置采用字符数组索引*/
                                        if (obj.Data[7]>0)
                                        {
                                            this.MdataGridView.Rows[ID_Str_Value_index].Cells[6].Value = MZeroPosition[(obj.Data[7] - 1) % 2];
                                        }
                                        else
                                        {
                                            MessageBox.Show("电缸零点位置不应为： 0x00 ！", "Error");
                                        }
                                        

                                    }
                                    /*电缸数据页二*/
                                    //电缸长度
                                    //传感器行程
                                    //位置PID-KP
                                    //相对零点位置
                                    if (System.Convert.ToString((Int32)obj.ID, 16).Substring(0, 1) == "2")
                                    {

                                        //电缸长度
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[7].Value =
                                        System.Int32.Parse(System.Convert.ToString((obj.Data[0] + obj.Data[1] * 16 * 16) / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                        + "." + System.Int32.Parse(System.Convert.ToString((obj.Data[0] + obj.Data[1] * 16 * 16) % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                        //传感器行程
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[8].Value =
                                        System.Int32.Parse(System.Convert.ToString((obj.Data[2] + obj.Data[3] * 16 * 16) / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                        + "." + System.Int32.Parse(System.Convert.ToString((obj.Data[2] + obj.Data[3] * 16 * 16) % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                        //位置PID-KP
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[9].Value =
                                        System.Int32.Parse(System.Convert.ToString((obj.Data[4] + obj.Data[5] * 16 * 16), 16), System.Globalization.NumberStyles.HexNumber);
                                        //相对零点位置
                                        this.MdataGridView.Rows[ID_Str_Value_index].Cells[10].Value =
                                         System.Int32.Parse(System.Convert.ToString((obj.Data[6] + obj.Data[7] * 16 * 16), 16), System.Globalization.NumberStyles.HexNumber);
                                    }




                                }

                            }
                        }
                        else
                        //if (System.Convert.ToString((Int32)obj.ID, 16).Substring(7, 1).ToUpper() == "E")
                        {
                            /*收到ACK*/
                        }
                    }
                }
                

                
                if (obj.RemoteFlag == 0)
                {
                    byte len = (byte)(obj.DataLen % 9);
                    byte j = 0;

                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[0], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[1], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[2], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[3], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[4], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[5], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[6], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[7], 16);
                    str += "  帧格式:";
                    str += "数据帧 ";
                }
                    
                else
                    str += "远程帧 ";
                if (obj.ExternFlag == 0)
                    str += "标准帧 ";
                else
                    str += "扩展帧 ";

                listBox_Info.Items.Add(str);
                listBox_Info.SelectedIndex = listBox_Info.Items.Count - 1;
            }
            Marshal.FreeHGlobal(pt);


        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_bOpen == 1)
            {
                VCI_CloseDevice(m_devtype, m_devind);
            }
        }

        private void viewSDataTypebutton_Click(object sender, EventArgs e)
        {
            SdataGridView.Visible = true;
            MdataGridView.Visible = false;
            SDatagroupBox.Visible = true;
            MDatagroupBox.Visible = false;
            BgroupBox.Visible = false;
            viewMDataTypebutton.Enabled = true;
            viewSDataTypebutton.Enabled = false;
            SetBbutton.Enabled = true;

        }

        private void viewMDataTypebutton_Click(object sender, EventArgs e)
        {
            SdataGridView.Visible = false;
            MdataGridView.Visible = true;

            SDatagroupBox.Visible = false;
            MDatagroupBox.Visible = true;
            BgroupBox.Visible = false;
            viewSDataTypebutton.Enabled = true;
            viewMDataTypebutton.Enabled = false;
            SetBbutton.Enabled = true;
        }

        private unsafe  void readMDatabutton_Click(object sender, EventArgs e)
        {
            if (m_bOpen == 0)
                return;
            //Convert.ToString((Convert.ToInt16("11", 10)), 16);
            sendData("0003" + IDAdresss[MIDSelectcomboBox.SelectedIndex] + "53",
                "00", "00", "00", "00", "00", "00", "00", "00");
        }

        private void clearDatabutton_Click(object sender, EventArgs e)
        {
            listBox_Info.Items.Clear();
        }

        private void MSendDatabutton_Click(object sender, EventArgs e)
        {
            //电缸使能位
            MDataEnabledStr = "0" + ((MSetcomboBox.SelectedIndex + 1) % 5).ToString();
            //发送调试或者设定参数信息
            if (MSetcomboBox.SelectedIndex <= 1)
            {
                /*发送调试信息*/
                MZeroStr = "0" + ((MZeroPositioncomboBox.SelectedIndex % 2)+1).ToString();
                MExecutiveTimeStr = Convert.ToString(Convert.ToInt16(MExecuteTimetextBox.Text, 10), 16);
                //电缸长度
                MLengthHStr = Convert.ToString(Convert.ToInt32(MLengthtextBox.Text, 10) / 256, 16);
                MLengthLStr = Convert.ToString(Convert.ToInt32(MLengthtextBox.Text, 10) % 256, 16);
                while (MLengthHStr.Length < 2)
                {
                    MLengthHStr = "0" + MLengthHStr;
                }
                while (MLengthLStr.Length < 2)
                {
                    MLengthLStr = "0" + MLengthLStr;
                }

                //电缸目标位置
                MDestinationStr = Convert.ToString(Convert.ToInt16(MDestinationPositiontextBox.Text, 10), 16);
                while (MDestinationStr.Length < 4)
                {
                    MDestinationStr = "0" + MDestinationStr;
                }
                MDestinationHStr = MDestinationStr.Substring(0, 2);
                MDestinationLStr = MDestinationStr.Substring(2, 2);
                
                //电缸位置PID
                MPositionHStr = Convert.ToString(Convert.ToInt32(MPositionPIDtextBox.Text, 10)/256, 16);
                MPositionLStr = Convert.ToString(Convert.ToInt32(MPositionPIDtextBox.Text, 10)%256, 16);
                
                //处理执行时间输入值
                if (MExecutiveTimeStr.Length == 1)
                {
                    MExecutiveTimeStr = "0" + MExecutiveTimeStr;
                }



                //从左至右为ID和byte7到byte0
                //发送执行时间，0点位置，电缸长度，目标位置
                sendData("0013" + IDAdresss[MIDSelectcomboBox.SelectedIndex] + "52",
                    MDataEnabledStr, MExecutiveTimeStr, "00", MZeroStr, MLengthHStr, MLengthLStr, MDestinationHStr, MDestinationLStr);
                //System.Threading.Thread.Sleep(500);

                //处理电缸位置PID
                if (MPositionHStr.Length == 1)
                {
                    MPositionHStr = "0" + MPositionHStr;
                }
                if (MPositionLStr.Length == 1)
                {
                    MPositionLStr = "0" + MPositionLStr;
                }

                //发送位置PID
                sendData("0023" + IDAdresss[MIDSelectcomboBox.SelectedIndex] + "52", 
                    MDataEnabledStr, "00", "00", "00", "00", "00", MPositionHStr, MPositionLStr);

            }
            if (MSetcomboBox.SelectedIndex == 2)
            {
                /*设置当前位置为零点*/
                //
                
                sendData("0023" + IDAdresss[MIDSelectcomboBox.SelectedIndex] + "52",
                    MDataEnabledStr, "00", "00", "00", "00", "00", "00", "00");
            }

            if (MSetcomboBox.SelectedIndex == 3)
            {
                /*装配模式*/
                //
                //预留功能
                //
                sendData("0023" + IDAdresss[MIDSelectcomboBox.SelectedIndex] + "52",
                    MDataEnabledStr, "00", "00", "00", "00", "00", "00", "00");
            }
            if (MSetcomboBox.SelectedIndex == 4)
            {
                /*设置行为动态监测模式*/
                sendData("0023" + IDAdresss[MIDSelectcomboBox.SelectedIndex] + "52",
                    MDataEnabledStr, "00", "00", "00", "00", "00", "00", "00");
            }
        }

        private void sendDatabutton_Click(object sender, EventArgs e)
        {

            if (SSet_01_comboBox.SelectedIndex == 0)
            {
                UInt16 SFlag = 0x00;
                string SFlagStr;
                /*定义逆时针最大值，顺时针最大值，目标位置*/
                //SExecutiveTimeStr = setExecutivetime_01_textBox.Text;
                SNMaxStr = setNlimit_01_settextBox.Text;
                SPMaxStr = setPLimite_01_textBox.Text;
                SDestinationStr = setDestination_01_textBox.Text;
                SExecutiveTimeStr = Convert.ToString(Convert.ToInt16(setExecutivetime_01_textBox.Text, 10), 16);

                //处理执行时间输入值
                if (SExecutiveTimeStr.Length == 1)
                {
                    SExecutiveTimeStr = "0" + SExecutiveTimeStr;
                }
                //处理逆时针最大值输入值
                if (setNlimit_01_settextBox.Text.Substring(0, 1) == "-")
                {
                    SNMaxStr = Convert.ToString(Convert.ToInt16(setNlimit_01_settextBox.Text.Substring(1, setDestination_01_textBox.TextLength - 1), 10), 16);
                    SFlag |= 0x04;
                }
                else
                {
                    SNMaxStr = Convert.ToString(Convert.ToInt16(setNlimit_01_settextBox.Text, 10), 16);
                    SFlag |= 0x00;
                }
                if (SNMaxStr.Length == 1)
                {
                    SNMaxStr = "0" + SNMaxStr;
                }
                //处理顺时针最大值输入值
                if (setPLimite_01_textBox.Text.Substring(0, 1) == "-")
                {
                    SPMaxStr = Convert.ToString(Convert.ToInt16(setPLimite_01_textBox.Text.Substring(1, setDestination_01_textBox.TextLength - 1), 10), 16);
                    SFlag |= 0x02;
                }
                else
                {
                    SPMaxStr = Convert.ToString(Convert.ToInt16(setPLimite_01_textBox.Text, 10), 16);
                    SFlag |= 0x00;
                }
                if (SPMaxStr.Length == 1)
                {
                    SPMaxStr = "0" + SPMaxStr;
                }
                //处理目标角度输入数据
                if (setDestination_01_textBox.Text.Substring(0, 1) == "-")
                {
                    SDestinationStr = Convert.ToString(Convert.ToInt16(setDestination_01_textBox.Text.Substring(1, setDestination_01_textBox.TextLength - 1), 10), 16);
                    SFlag |= 0x01;
                }
                else
                {
                    SDestinationStr = Convert.ToString(Convert.ToInt16(setDestination_01_textBox.Text, 10), 16);
                    SFlag |= 0x00;
                }
                if (SDestinationStr.Length == 1)
                {
                    SDestinationStr = "0" + SDestinationStr;
                }
                SFlagStr = SFlag.ToString();
                if (SFlagStr.Length == 1)
                {
                    SFlagStr = "0" + SFlagStr;
                }
                if(SFlagStr.Length >2)
                {
                    MessageBox.Show("error SDataFlag!");
                }

                //从左至右为ID和byte7到byte0
                //发送配置信息，执行时间，逆时针最大位置，顺时针最大位置，目标位置
                sendData("0002"+ IDAdresss[SIDSelectcomboBox.SelectedIndex] + "52", "01", SExecutiveTimeStr, SFlagStr, "00", "00", SNMaxStr, SPMaxStr, SDestinationStr);
            }
            if (SSet_01_comboBox.SelectedIndex == 1)
            {
                UInt16 SFlag = 0x00;
                string SFlagStr;
                //发送配置信息，设置当前目标位置为零点位置
                /*弹出对话框，点击确认后，继续操作*/
                DialogResult setDesAsZerDialogResult = MessageBox.Show("确认设置当前目标位置作为零点位置？",
                    "提示", 
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2);
                /*进一步确认是否发送*/
                if(setDesAsZerDialogResult == DialogResult.Yes)
                {
                    /*目标位置*/
                    SDestinationStr = setDestination_01_textBox.Text;
   
                    //处理目标角度输入数据
                    if (setDestination_01_textBox.Text.Substring(0, 1) == "-")
                    {
                        SDestinationStr = Convert.ToString(Convert.ToInt16(setDestination_01_textBox.Text.Substring(1, setDestination_01_textBox.TextLength - 1), 10), 16);
                        SFlag |= 0x01;
                    }
                    else
                    {
                        SDestinationStr = Convert.ToString(Convert.ToInt16(setDestination_01_textBox.Text, 10), 16);
                        SFlag |= 0x00;
                    }
                    if (SDestinationStr.Length == 1)
                    {
                        SDestinationStr = "0" + SDestinationStr;
                    }
                    SFlagStr = SFlag.ToString();
                    if (SFlagStr.Length == 1)
                    {
                        SFlagStr = "0" + SFlagStr;
                    }
                    if (SFlagStr.Length > 2)
                    {
                        MessageBox.Show("error SDataFlag!");
                    }
                    sendData("0002"+ IDAdresss[SIDSelectcomboBox.SelectedIndex] + "52", "02", "00", SFlagStr, "00", "00", "00", "00", SDestinationStr);
                }
            }
            if (SSet_01_comboBox.SelectedIndex == 2)
            {
                //设置  选  中  的舵机进入装配模式
                sendData("0002"+ IDAdresss[SIDSelectcomboBox.SelectedIndex] + "52", "03", "00", "00","00","00","00", "00", "00" );
            }
            if (SSet_01_comboBox.SelectedIndex == 3)
            {
                //设置  选  中  的舵机进入检测模式
                sendData("0002" + IDAdresss[SIDSelectcomboBox.SelectedIndex] + "52", "04", "00", "00", "00", "00", "00", "00", "00");
            }


        }

        private unsafe void readDatabutton_Click(object sender, EventArgs e)
        {
            if (m_bOpen == 0)
                return;
            //Convert.ToString((Convert.ToInt16("11", 10)), 16);
            sendData("0002"+ IDAdresss[SIDSelectcomboBox.SelectedIndex] + "53",
                "00", "00", "00", "00", "00", "00", "00", "00");

        }
        /*CAN发送函数
         从左至右为ID和byte7到byte0
         String ID_str:发送信息ID，不需要包含0x
         String data0_str:发送信息最高位，不需要包含0x
         String data1_str：不需要包含0x 
         String data2_str：不需要包含0x 
         String data3_str：不需要包含0x 
         String data4_str：不需要包含0x 
         String data5_str：不需要包含0x 
         String data6_str：不需要包含0x 
         String data7_str ：最低位不需要包含0x    
        */
        private unsafe void sendData(String ID_str,String data0_str, String data1_str, String data2_str, String data3_str,
            String data4_str, String data5_str, String data6_str, String data7_str)
        {

            if (m_bOpen == 0)
                return;
            
            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();
            //sendobj.Init();
            sendobj.SendType = (byte)comboBox_SendType.SelectedIndex;
            sendobj.RemoteFlag = (byte)comboBox_FrameFormat.SelectedIndex;
            sendobj.ExternFlag = (byte)comboBox_FrameType.SelectedIndex;
            sendobj.ID = System.Convert.ToUInt32("0x" + ID_str, 16);
            //sendobj.ID = System.Convert.ToUInt32("0x" + textBox_ID.Text, 16);

            sendobj.DataLen = 8;
            sendobj.Data[0] = System.Convert.ToByte("0x" + data7_str, 16);
            sendobj.Data[1] = System.Convert.ToByte("0x" + data6_str, 16);
            sendobj.Data[2] = System.Convert.ToByte("0x" + data5_str, 16);
            sendobj.Data[3] = System.Convert.ToByte("0x" + data4_str, 16);
            sendobj.Data[4] = System.Convert.ToByte("0x" + data3_str, 16);
            sendobj.Data[5] = System.Convert.ToByte("0x" + data2_str, 16);
            sendobj.Data[6] = System.Convert.ToByte("0x" + data1_str, 16);
            sendobj.Data[7] = System.Convert.ToByte("0x" + data0_str, 16);

            if (VCI_Transmit(m_devtype, m_devind, m_canind, ref sendobj, 1) == 0)
            {
                
                MessageBox.Show("发送失败,请检查线路连接或重新“连接”设备并点击“启动Can”按钮！", "错误",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
             
                
                
            }

        }

        //analysis txt order and senddata
        private void codeOderAndSendData(string str)
        {
            string[] getActionIDArrayStr = Regex.Split(str, "\\>");
            string actionIDStr = getActionIDArrayStr[0];
            var actionID = actionIDArray.ToList().IndexOf(actionIDStr.Substring(1, 1)) + 1;
            if(actionID == 0)
            {
                MessageBox.Show("ID出错line:"+count_lineError);
                return;
            }
            //get the real action data action num
            string sendActionIDStr = Convert.ToString(actionID % 16, 16) + Convert.ToString(Convert.ToUInt16(actionIDStr.Substring(2, 2), 10) % 16, 16);

            if (getActionIDArrayStr.Length == 2)
            {
                string[] getDevice_1_IDArrayTmpStr = Regex.Split(getActionIDArrayStr[1], "\\)");
                string getDevice_1_IDStr = getDevice_1_IDArrayTmpStr[0];
                string[] getDevice_1_IDArrayStr = Regex.Split(getDevice_1_IDStr, "\\(");
                string device_1_IDStr = getDevice_1_IDArrayStr[0];
                //get the data page
                var Device_1_ID = getDevice_1_IDArrayStr.ToList().IndexOf(device_1_IDStr) / 2 + 1;
                if (Device_1_ID == 0)
                {
                    MessageBox.Show("ID出错line:" + count_lineError);
                    return;
                }
                string[] getActionAndTime_1_DataArrayStr = Regex.Split(getDevice_1_IDArrayStr[1], "\\,");
                string getAction_1_DataStr = getActionAndTime_1_DataArrayStr[0];
                string getTime_1_DataStr = getActionAndTime_1_DataArrayStr[1];
                //data
                getAction_1_DataArrayStr = Regex.Split(getAction_1_DataStr, "\\/");
                getTime_1_DataArrayStr = Regex.Split(getTime_1_DataStr, "\\/");

                string sendDevice_1_IDStr = Convert.ToString(Device_1_ID, 16);
                while (sendDevice_1_IDStr.Length < 2)
                {
                    sendDevice_1_IDStr = "0" + sendDevice_1_IDStr;
                }
                sendFrameID = sendDevice_1_IDStr + "21" + sendActionIDStr + "5F";
                for (int i = 0; i < getAction_1_DataArrayStr.Length; i++)
                {
                    sendStep_1_Str = Convert.ToString(getAction_1_DataArrayStr.Length % 16, 16) + Convert.ToString((i + 1) % 16, 16);
                    if (getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "K" && getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "k")
                    {
                        string action_1_Str = Convert.ToString(Convert.ToInt16(getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length], 10), 16);
                        while (action_1_Str.Length < 4)
                        {
                            action_1_Str = "0" + action_1_Str;
                        }
                        sendAction_1_DataHStr = action_1_Str.Substring(0, 2);
                        sendAction_1_DataLStr = action_1_Str.Substring(2, 2);

                    }
                    else
                    {
                        sendAction_1_DataHStr = "5A";
                        sendAction_1_DataLStr = "A5";
                    }

                    sendTime_1_Str = Convert.ToString(Convert.ToInt16(((UInt16)(Convert.ToDouble(getTime_1_DataArrayStr[i % getAction_1_DataArrayStr.Length]) * 10)).ToString(), 10), 16);
                    while (sendTime_1_Str.Length < 2)
                    {
                        sendTime_1_Str = "0" + sendTime_1_Str;
                    }
                    //sendData here
                    sendData(sendFrameID, "00", "00", "00", "00", sendStep_1_Str, sendTime_1_Str, sendAction_1_DataHStr, sendAction_1_DataLStr);
                }
            }
            else
            {

                int actionCount = 1;
                while (actionCount < getActionIDArrayStr.Length - 1)
                {
                    string[] getDevice_1_IDArrayTmpStr = Regex.Split(getActionIDArrayStr[actionCount], "\\)");
                    string getDevice_1_IDStr = getDevice_1_IDArrayTmpStr[0];
                    string[] getDevice_1_IDArrayStr = Regex.Split(getDevice_1_IDStr, "\\(");
                    string device_1_IDStr = getDevice_1_IDArrayStr[0];
                    //get the data page
                    var Device_1_ID = deviceIDArray.ToList().IndexOf(device_1_IDStr) / 2 + 1;
                    if (Device_1_ID == 0)
                    {
                        MessageBox.Show("ID出错line:" + count_lineError);
                        return;
                    }
                    string[] getActionAndTime_1_DataArrayStr = Regex.Split(getDevice_1_IDArrayStr[1], "\\,");
                    string getAction_1_DataStr = getActionAndTime_1_DataArrayStr[0];
                    string getTime_1_DataStr = getActionAndTime_1_DataArrayStr[1];
                    //data
                    getAction_1_DataArrayStr = Regex.Split(getAction_1_DataStr, "\\/");
                    getTime_1_DataArrayStr = Regex.Split(getTime_1_DataStr, "\\/");

                    string[] getDevice_2_IDArrayTmpStr = Regex.Split(getActionIDArrayStr[actionCount+1], "\\)");
                    string getDevice_2_IDStr = getDevice_2_IDArrayTmpStr[0];
                    string[] getDevice_2_IDArrayStr = Regex.Split(getDevice_2_IDStr, "\\(");
                    string device_2_IDStr = getDevice_2_IDArrayStr[0];
                    //get the data page
                    var Device_2_ID = deviceIDArray.ToList().IndexOf(device_2_IDStr) / 2 + 1;
                    if (Device_2_ID == 0)
                    {
                        MessageBox.Show("ID出错line:" + count_lineError);
                        return;
                    }
                    string sendDevice_2_IDStr = Convert.ToString(Device_2_ID, 16);
                    while (sendDevice_2_IDStr.Length < 2)
                    {
                        sendDevice_2_IDStr = "0" + sendDevice_2_IDStr;
                    }
                    string[] getActionAndTime_2_DataArrayStr = Regex.Split(getDevice_2_IDArrayStr[1], "\\,");
                    string getAction_2_DataStr = getActionAndTime_2_DataArrayStr[0];
                    string getTime_2_DataStr = getActionAndTime_2_DataArrayStr[1];
                    //data
                    getAction_2_DataArrayStr = Regex.Split(getAction_2_DataStr, "\\/");
                    getTime_2_DataArrayStr = Regex.Split(getTime_2_DataStr, "\\/");

                    //get the action step and senddata frame
                    //total step equals the array`s length
                    //the current step equals the inx of the array
                    //but if the two length was different 
                    //then the short array filled till two have the same lenth with zeros
                    //the action data k equals the 0x5aa5


                    string action_1_Str, action_2_Str;
                    if (Device_1_ID == Device_2_ID)
                    {
                        sendFrameID = sendDevice_2_IDStr + "21" + sendActionIDStr + "5F";
                        for (int i = 0; i < getAction_1_DataArrayStr.Length || i < getAction_2_DataArrayStr.Length; i++)
                        {
                            //*get the send data part 1*//
                            if (i < getAction_1_DataArrayStr.Length)
                            {
                                sendStep_1_Str = Convert.ToString(getAction_1_DataArrayStr.Length % 16, 16) + Convert.ToString((i + 1) % 16, 16);

                                if (getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "K" && getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "k")
                                {
                                    action_1_Str = Convert.ToString(Convert.ToInt16(getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length], 10), 16);
                                    while (action_1_Str.Length < 4)
                                    {
                                        action_1_Str = "0" + action_1_Str;
                                    }
                                    sendAction_1_DataHStr = action_1_Str.Substring(0, 2);
                                    sendAction_1_DataLStr = action_1_Str.Substring(2, 2);

                                }
                                else
                                {
                                    sendAction_1_DataHStr = "5A";
                                    sendAction_1_DataLStr = "A5";
                                }

                                sendTime_1_Str = Convert.ToString(Convert.ToInt16(((UInt16)(Convert.ToDouble(getTime_1_DataArrayStr[i % getAction_1_DataArrayStr.Length]) * 10)).ToString(), 10), 16);
                                while (sendTime_1_Str.Length < 2)
                                {
                                    sendTime_1_Str = "0" + sendTime_1_Str;
                                }
                            }
                            else
                            {
                                sendStep_1_Str = "00";
                                sendAction_1_DataHStr = "00";
                                sendAction_1_DataLStr = "00";
                                sendTime_1_Str = "00";
                            }
                            //*get the send data part 2*//
                            if (i < getAction_2_DataArrayStr.Length)
                            {
                                sendStep_2_Str = Convert.ToString(getAction_2_DataArrayStr.Length % 16, 16) + Convert.ToString((i + 1) % 16, 16);

                                if (getAction_2_DataArrayStr[i % getAction_2_DataArrayStr.Length] != "K" && getAction_2_DataArrayStr[i % getAction_2_DataArrayStr.Length] != "k")
                                {
                                    action_2_Str = Convert.ToString(Convert.ToInt16(getAction_2_DataArrayStr[i % getAction_2_DataArrayStr.Length], 10), 16);
                                    while (action_2_Str.Length < 4)
                                    {
                                        action_2_Str = "0" + action_2_Str;
                                    }
                                    sendAction_2_DataHStr = action_2_Str.Substring(0, 2);
                                    sendAction_2_DataLStr = action_2_Str.Substring(2, 2);

                                }
                                else
                                {
                                    sendAction_2_DataHStr = "5A";
                                    sendAction_2_DataLStr = "A5";
                                }
                                sendTime_2_Str = Convert.ToString(Convert.ToInt16(((UInt16)(Convert.ToDouble(getTime_2_DataArrayStr[i % getAction_2_DataArrayStr.Length]) * 10)).ToString(), 10), 16);
                                while (sendTime_2_Str.Length < 2)
                                {
                                    sendTime_2_Str = "0" + sendTime_2_Str;
                                }
                            }
                            else
                            {
                                sendStep_2_Str = "00";
                                sendAction_2_DataHStr = "00";
                                sendAction_2_DataLStr = "00";
                                sendTime_2_Str = "00";
                            }


                            //sendData here
                            sendData(sendFrameID, sendStep_2_Str, sendTime_2_Str, sendAction_2_DataHStr, sendAction_2_DataLStr, sendStep_1_Str, sendTime_1_Str, sendAction_1_DataHStr, sendAction_1_DataLStr);
                        }
                    }
                    else
                    {
                        //send page first
                        string sendDevice_1_IDStr = Convert.ToString(Device_1_ID, 16);
                        while (sendDevice_1_IDStr.Length < 2)
                        {
                            sendDevice_1_IDStr = "0" + sendDevice_1_IDStr;
                        }
                        sendFrameID = sendDevice_1_IDStr + "21" + sendActionIDStr + "5F";
                        for (int i = 0; i < getAction_1_DataArrayStr.Length; i++)
                        {
                            sendStep_1_Str = Convert.ToString(getAction_1_DataArrayStr.Length % 16, 16) + Convert.ToString((i + 1) % 16, 16);
                            if (getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "K" && getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "k")
                            {
                                action_1_Str = Convert.ToString(Convert.ToInt16(getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length], 10), 16);
                                while (action_1_Str.Length < 4)
                                {
                                    action_1_Str = "0" + action_1_Str;
                                }
                                sendAction_1_DataHStr = action_1_Str.Substring(0, 2);
                                sendAction_1_DataLStr = action_1_Str.Substring(2, 2);

                            }
                            else
                            {
                                sendAction_1_DataHStr = "5A";
                                sendAction_1_DataLStr = "A5";
                            }

                            sendTime_1_Str = Convert.ToString(Convert.ToInt16(((UInt16)(Convert.ToDouble(getTime_1_DataArrayStr[i % getAction_1_DataArrayStr.Length]) * 10)).ToString(), 10), 16);
                            while (sendTime_1_Str.Length < 2)
                            {
                                sendTime_1_Str = "0" + sendTime_1_Str;
                            }
                            //sendData here
                            sendData(sendFrameID, "00", "00", "00", "00", sendStep_1_Str, sendTime_1_Str, sendAction_1_DataHStr, sendAction_1_DataLStr);
                        }
                        //send page second
                        sendFrameID = sendDevice_2_IDStr + "21" + sendActionIDStr + "5F";
                        for (int i = 0; i < getAction_2_DataArrayStr.Length; i++)
                        {
                            sendStep_2_Str = Convert.ToString(getAction_2_DataArrayStr.Length % 16, 16) + Convert.ToString((i + 1) % 16, 16);
                            if (getAction_2_DataArrayStr[i % getAction_2_DataArrayStr.Length] != "K" && getAction_2_DataArrayStr[i % getAction_2_DataArrayStr.Length] != "k")
                            {
                                action_2_Str = Convert.ToString(Convert.ToInt16(getAction_2_DataArrayStr[i % getAction_2_DataArrayStr.Length], 10), 16);
                                while (action_2_Str.Length < 4)
                                {
                                    action_2_Str = "0" + action_2_Str;
                                }
                                sendAction_2_DataHStr = action_2_Str.Substring(0, 2);
                                sendAction_2_DataLStr = action_2_Str.Substring(2, 2);

                            }
                            else
                            {
                                sendAction_2_DataHStr = "5A";
                                sendAction_2_DataLStr = "A5";
                            }
                            sendTime_2_Str = Convert.ToString(Convert.ToInt16(((UInt16)(Convert.ToDouble(getTime_2_DataArrayStr[i % getAction_2_DataArrayStr.Length]) * 10)).ToString(), 10), 16);
                            while (sendTime_2_Str.Length < 2)
                            {
                                sendTime_2_Str = "0" + sendTime_2_Str;
                            }
                            //send data here
                            sendData(sendFrameID, sendStep_2_Str, sendTime_2_Str, sendAction_2_DataHStr, sendAction_2_DataLStr, "00", "00", "00", "00");

                        }

                    }
                    actionCount += 2;
                }
                if (getActionIDArrayStr.Length % 2 == 0)
                {
                    string[] getDevice_1_IDArrayTmpStr = Regex.Split(getActionIDArrayStr[getActionIDArrayStr.Length-1], "\\)");
                    string getDevice_1_IDStr = getDevice_1_IDArrayTmpStr[0];
                    string[] getDevice_1_IDArrayStr = Regex.Split(getDevice_1_IDStr, "\\(");
                    string device_1_IDStr = getDevice_1_IDArrayStr[0];
                    //get the data page
                    var Device_1_ID = getDevice_1_IDArrayStr.ToList().IndexOf(device_1_IDStr) / 2 + 1;
                    if (Device_1_ID == 0)
                    {
                        MessageBox.Show("ID出错line:" + count_lineError);
                        return;
                    }
                    string[] getActionAndTime_1_DataArrayStr = Regex.Split(getDevice_1_IDArrayStr[1], "\\,");
                    string getAction_1_DataStr = getActionAndTime_1_DataArrayStr[0];
                    string getTime_1_DataStr = getActionAndTime_1_DataArrayStr[1];
                    //data
                    getAction_1_DataArrayStr = Regex.Split(getAction_1_DataStr, "\\/");
                    getTime_1_DataArrayStr = Regex.Split(getTime_1_DataStr, "\\/");

                    string sendDevice_1_IDStr = Convert.ToString(Device_1_ID, 16);
                    while (sendDevice_1_IDStr.Length < 2)
                    {
                        sendDevice_1_IDStr = "0" + sendDevice_1_IDStr;
                    }
                    sendFrameID = sendDevice_1_IDStr + "21" + sendActionIDStr + "5F";
                    for (int i = 0; i < getAction_1_DataArrayStr.Length; i++)
                    {
                        sendStep_1_Str = Convert.ToString(getAction_1_DataArrayStr.Length % 16, 16) + Convert.ToString((i + 1) % 16, 16);
                        if (getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "K" && getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length] != "k")
                        {
                            string action_1_Str = Convert.ToString(Convert.ToInt16(getAction_1_DataArrayStr[i % getAction_1_DataArrayStr.Length], 10), 16);
                            while (action_1_Str.Length < 4)
                            {
                                action_1_Str = "0" + action_1_Str;
                            }
                            sendAction_1_DataHStr = action_1_Str.Substring(0, 2);
                            sendAction_1_DataLStr = action_1_Str.Substring(2, 2);

                        }
                        else
                        {
                            sendAction_1_DataHStr = "5A";
                            sendAction_1_DataLStr = "A5";
                        }

                        sendTime_1_Str = Convert.ToString(Convert.ToInt16(((UInt16)(Convert.ToDouble(getTime_1_DataArrayStr[i % getAction_1_DataArrayStr.Length]) * 10)).ToString(), 10), 16);
                        while (sendTime_1_Str.Length < 2)
                        {
                            sendTime_1_Str = "0" + sendTime_1_Str;
                        }
                        //sendData here
                        sendData(sendFrameID, "00", "00", "00", "00", sendStep_1_Str, sendTime_1_Str, sendAction_1_DataHStr, sendAction_1_DataLStr);
                    }
                }

            }

    


        }

    }
}
