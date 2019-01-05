using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
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


        /*定义逆时针最大值，顺时针最大值，目标位置*/
        string ExecutiveTimeStr, NMaxStr, PMaxStr, DestinationStr;

        /*定义设备id号地址段数组0-255*/
        /*目前仅定义1-16*/
        String[] IDAdresss = { "01", "02", "03", "04", "05", "06", "07", "08",
            "09", "0A","0B","0C","0D","0E","0F","10" };
 
                
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            /*初始化舵机/电缸显示界面*/
            SdataGridView.Visible = true;
            MdataGridView.Visible = false;
            viewSDataTypebutton.Enabled = false;
            viewMDataTypebutton.Enabled = true;

            /*初始化dataGridView*/
            /*共计16行*/
            for (int i = 0;i<16;i++)
            {
                this.SdataGridView.Rows.Add();
                this.SdataGridView.Rows[i].Cells[0].Value = "0x" + IDAdresss[i];
            }

            /*初始化ID选择下拉选择框*/
            IDSelectcomboBox.Items.Clear();
            for (int i=0;i<16;i++)
            {
                IDSelectcomboBox.Items.Add((i+1).ToString());
            }
            IDSelectcomboBox.SelectedIndex = 0;


            /*初始化配置信息区域下拉选择框*/
            set_01_comboBox.Items.Clear();
            set_01_comboBox.Items.Add("发送信息");
            set_01_comboBox.Items.Add("置当前目标为零点");
            set_01_comboBox.Items.Add("装配模式");
            set_01_comboBox.Items.Add("检测模式");
            set_01_comboBox.SelectedIndex = 0;

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

        unsafe private void timer_rec_Tick(object sender, EventArgs e)
        {

            //设置datagridview的光标
            for(int i=0;i<16;i++)
            {
                if(IDSelectcomboBox.SelectedIndex == i)
                {
                    //dataGridView.Rows[i].Selected = true;
                    this.SdataGridView.Rows[i].DefaultCellStyle.BackColor = Color.SpringGreen;
                }
                else
                {
                    //dataGridView.Rows[i].Selected = false;
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

                str = "接收到数据: ";
                str += "  帧ID:0x" + System.Convert.ToString((Int32)obj.ID, 16);
                /*收到信息并解析ID*/

                /*解析发给上位机的frame*/
                /*显示到gridview里面*/
                /*预留高两位*/
                if (System.Convert.ToString((Int32)obj.ID, 16).Substring(1, 1) == "5")
                //if (System.Convert.ToString((Int32)obj.ID, 16).Substring(3, 1) == "5")
                {
                    if (System.Convert.ToString((Int32)obj.ID, 16).Substring(5, 1) == "4")
                    {
                        if(System.Convert.ToString((Int32)obj.ID, 16).Substring(0, 1)=="1")
                        {
                            //收到数据帧
                            if (obj.RemoteFlag == 0)
                                {
                                Int32 ID_Str_Value_index = ((((Int32)obj.ID) / 16 / 16 % 16)-1+ ((Int32)obj.ID)/16/16/16%16 * 16)%16;
                                    str += "数据页1: ";
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

                                //电压
                                this.SdataGridView.Rows[ID_Str_Value_index].Cells[1].Value =
                                    System.Int32.Parse(System.Convert.ToString(obj.Data[0] / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                    + "." + System.Int32.Parse(System.Convert.ToString(obj.Data[0] % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                //电流
                                this.SdataGridView.Rows[ID_Str_Value_index].Cells[2].Value =
                                System.Int32.Parse(System.Convert.ToString(obj.Data[1] / 10, 16), System.Globalization.NumberStyles.HexNumber)
                                + "." + System.Int32.Parse(System.Convert.ToString(obj.Data[1] % 10, 16), System.Globalization.NumberStyles.HexNumber);
                                //温度
                                this.SdataGridView.Rows[ID_Str_Value_index].Cells[3].Value =
                                System.Int32.Parse(System.Convert.ToString(obj.Data[2], 16), System.Globalization.NumberStyles.HexNumber);
                                //目标位置
                                if (obj.Data[3] >= 128)
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[4].Value =
                                    System.Int32.Parse(System.Convert.ToString(128 - obj.Data[3], 16), System.Globalization.NumberStyles.HexNumber);

                                }
                                else
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[4].Value =
                                    System.Int32.Parse(System.Convert.ToString(obj.Data[3], 16), System.Globalization.NumberStyles.HexNumber);

                                }
                                //当前位置
                                if (obj.Data[4] >= 128)
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[5].Value =
                                    System.Int32.Parse(System.Convert.ToString(128 - obj.Data[4], 16), System.Globalization.NumberStyles.HexNumber);

                                }
                                else
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[5].Value =
                                    System.Int32.Parse(System.Convert.ToString(obj.Data[4], 16), System.Globalization.NumberStyles.HexNumber);

                                }
                                //0点位置
                                if (obj.Data[5] >= 128)
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[6].Value =
                                    System.Int32.Parse(System.Convert.ToString(128 - obj.Data[5], 16), System.Globalization.NumberStyles.HexNumber);

                                }
                                else
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[6].Value =
                                    System.Int32.Parse(System.Convert.ToString(obj.Data[5], 16), System.Globalization.NumberStyles.HexNumber);

                                }

                                //顺时针最大位置
                                if (obj.Data[6] >= 128)
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[7].Value =
                                    System.Int32.Parse(System.Convert.ToString(128 - obj.Data[6], 16), System.Globalization.NumberStyles.HexNumber);
                                }
                                else
                                {
                                    this.SdataGridView.Rows[ID_Str_Value_index].Cells[7].Value =
                                    System.Int32.Parse(System.Convert.ToString(obj.Data[6], 16), System.Globalization.NumberStyles.HexNumber);
                                }
                                //逆时针最大位置
                                if (obj.Data[7]>=128)
                                    {
                                    
                                        this.SdataGridView.Rows[ID_Str_Value_index].Cells[8].Value =
                                        System.Int32.Parse(System.Convert.ToString(128-obj.Data[7], 16), System.Globalization.NumberStyles.HexNumber);
                                    }
                                    else
                                    {
                                        this.SdataGridView.Rows[ID_Str_Value_index].Cells[8].Value = 
                                        System.Int32.Parse(System.Convert.ToString(obj.Data[7], 16), System.Globalization.NumberStyles.HexNumber);
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
                  

                str += "  帧格式:";
                if (obj.RemoteFlag == 0)
                    str += "数据帧 ";
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
            
            viewMDataTypebutton.Enabled = true;
            viewSDataTypebutton.Enabled = false;

        }

        private void viewMDataTypebutton_Click(object sender, EventArgs e)
        {
            SdataGridView.Visible = false;
            MdataGridView.Visible = true;
            viewSDataTypebutton.Enabled = true;
            viewMDataTypebutton.Enabled = false;
        }

        private void sendDatabutton_Click(object sender, EventArgs e)
        {

            if (set_01_comboBox.SelectedIndex == 0)
            {

                /*定义逆时针最大值，顺时针最大值，目标位置*/
                ExecutiveTimeStr = setExecutivetime_01_textBox.Text;
                NMaxStr = setNlimit_01_settextBox.Text;
                PMaxStr = setPLimite_01_textBox.Text;
                DestinationStr = setDestination_01_textBox.Text;
                ExecutiveTimeStr = Convert.ToString(Convert.ToInt16(setExecutivetime_01_textBox.Text, 10), 16);
                //处理执行时间输入值
                if (ExecutiveTimeStr.Length == 1)
                {
                    ExecutiveTimeStr = "0" + ExecutiveTimeStr;
                }
                //处理逆时针最大值输入值
                if (setNlimit_01_settextBox.Text.Substring(0, 1) == "-")
                {
                    NMaxStr = Convert.ToString((128 - Convert.ToInt16(setNlimit_01_settextBox.Text, 10)), 16);
                }
                else
                {
                    NMaxStr = Convert.ToString(Convert.ToInt16(setNlimit_01_settextBox.Text, 10), 16);
                }
                if (NMaxStr.Length == 1)
                {
                    NMaxStr = "0" + NMaxStr;
                }
                //处理顺时针最大值输入值
                if (setPLimite_01_textBox.Text.Substring(0, 1) == "-")
                {
                    PMaxStr = Convert.ToString((128 - Convert.ToInt16(setPLimite_01_textBox.Text, 10)), 16);
                }
                else
                {
                    PMaxStr = Convert.ToString(Convert.ToInt16(setPLimite_01_textBox.Text, 10), 16);
                }
                if (PMaxStr.Length == 1)
                {
                    PMaxStr = "0" + PMaxStr;
                }
                //处理目标角度输入数据
                if (setDestination_01_textBox.Text.Substring(0, 1) == "-")
                {
                    DestinationStr = Convert.ToString((128 - Convert.ToInt16(setDestination_01_textBox.Text, 10)), 16);
                }
                else
                {
                    DestinationStr = Convert.ToString(Convert.ToInt16(setDestination_01_textBox.Text, 10), 16);
                }
                if (DestinationStr.Length == 1)
                {
                    DestinationStr = "0" + DestinationStr;
                }

                //从左至右为ID和byte7到byte0
                //发送配置信息，执行时间，逆时针最大位置，顺时针最大位置，目标位置

                sendData("0002"+ IDAdresss[IDSelectcomboBox.SelectedIndex] + "52", "01", ExecutiveTimeStr,  "00", "00", "00", NMaxStr, PMaxStr, DestinationStr);
            }
            if (set_01_comboBox.SelectedIndex == 1)
            {
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
                    DestinationStr = setDestination_01_textBox.Text;
   
                    //处理目标角度输入数据
                    if (setDestination_01_textBox.Text.Substring(0, 1) == "-")
                    {
                        DestinationStr = Convert.ToString((128 - Convert.ToInt16(setDestination_01_textBox.Text, 10)), 16);
                    }
                    else
                    {
                        DestinationStr = Convert.ToString(Convert.ToInt16(setDestination_01_textBox.Text, 10), 16);
                    }
                    if (DestinationStr.Length == 1)
                    {
                        DestinationStr = "0" + DestinationStr;
                    }
                    sendData("0002"+ IDAdresss[IDSelectcomboBox.SelectedIndex] + "52", "02", "00", "00", "00", "00", "00", "00", DestinationStr);
                }
            }
            if (set_01_comboBox.SelectedIndex == 2)
            {
                //设置  选  中  的舵机进入装配模式
                sendData("0002"+ IDAdresss[IDSelectcomboBox.SelectedIndex] + "52", "03", "00", "00","00","00","00", "00", "00" );
            }
            if (set_01_comboBox.SelectedIndex == 3)
            {
                //设置  选  中  的舵机进入检测模式
                sendData("0002" + IDAdresss[IDSelectcomboBox.SelectedIndex] + "52", "04", "00", "00", "00", "00", "00", "00", "00");
            }


        }

        private unsafe void readDatabutton_Click(object sender, EventArgs e)
        {
            Convert.ToString((Convert.ToInt16("11", 10)), 16);
            sendData("0002"+ IDAdresss[IDSelectcomboBox.SelectedIndex] + "53",
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
                MessageBox.Show("发送失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


    }
}
