using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace Athena.Unitop.Sure.Lib
{
    public class ElecWeightTool
    {
        private SerialPort spReceive = new SerialPort();
      
        public ElecWeightTool() { 

        }

        public ElecWeightTool(int ReceivedBytesThreshold, int DataBits, string PortName, int ReadTimeout, int BaudRate)//,ref TextBox rtb)
        {
            this._ReceivedBytesThreshold = ReceivedBytesThreshold;
            this._DataBits = DataBits;
            this._PortName = PortName;
            this._ReadTimeout = ReadTimeout;
            this._BaudRate = BaudRate;
            //rtb.Text = _strValues;

            //InitialSerialPortParameter();
            //ReadData();
            
        }

       private int _ReceivedBytesThreshold;
       private int _DataBits;
       private string _PortName;
       private int _ReadTimeout;
       private int _BaudRate;
       public string strWeight;
       private string _strValues;

      
       public int ReceivedBytesThreshold {
           set { _ReceivedBytesThreshold = value; }
           get { return _ReceivedBytesThreshold; }
       }

       public int DataBits
       {
           set { _DataBits = value; }
           get { return _DataBits; }
       }
       /// <summary>
       /// 
       /// </summary>
       public string PortName
       {
           set { _PortName = value; }
           get { return _PortName; }
       }

        /// <summary>
        /// 
        /// </summary>
       public int ReadTimeout
       {
           set { _ReadTimeout = value; }
           get { return _ReadTimeout; }
       }

        /// <summary>
        /// 
        /// </summary>
       public int BaudRate {
           set { _BaudRate = value; }
           get { return _BaudRate; }
       }

        /// <summary>
        /// 
        /// </summary>
       public string strValues
       {
           set { _strValues = value; }
           get { return _strValues; }
       }

       private delegate void HandleInterfaceUpdateDelegate(string text);//委托，此为重点 
       private HandleInterfaceUpdateDelegate interfaceUpdateHandle; //用于更新label上的重量数据

       public delegate void HandleWeightDelegate(string weight);
       public HandleWeightDelegate HandleWeightEvent;
       public delegate void TextBoxHandler();  //委托 TEXTBOX，传值
       public TextBoxHandler TextBoxHandlerEvent;
       public void InitialSerialPortParameter()
       {
           spReceive.ReceivedBytesThreshold = this.ReceivedBytesThreshold; //缓冲区为8位
           spReceive.DataBits = this.DataBits;  //数据位
           spReceive.PortName = this.PortName; //串口名
           spReceive.ReadTimeout = this.ReadTimeout; //超时时间
           spReceive.BaudRate = this.BaudRate; //波特率
           spReceive.Parity = Parity.None;  //校验位
           spReceive.StopBits = System.IO.Ports.StopBits.One; //停止位 1位
           spReceive.DataReceived += new SerialDataReceivedEventHandler(spReceive_DataReceived); //当接收到串口数据时触发DataRecieve事件
           //interfaceUpdateHandle = new HandleInterfaceUpdateDelegate(UpdateLabel);   //委托方法UpdateLabel显示更新过重数据
           //TextBoxHandlerEvent +=new
       }

       protected void spReceive_DataReceived(object sender, SerialDataReceivedEventArgs e)
       {
           string strRecieve = spReceive.ReadExisting();  //获取缓冲区中的数据
           //interfaceUpdateHandle(strRecieve);
          strRecieve = UpdateLabel(strRecieve);
          _strValues = strRecieve;
           if (HandleWeightEvent != null) {
               HandleWeightEvent(strRecieve);
               
           }
       }

       public void ReadData()
       {
           if (spReceive.IsOpen)  //如果串口已打开则关闭重新打开
           {
               spReceive.Close();
               spReceive.Open();
           }
           else  //如果串口没有打开则打开串口
           {
               spReceive.Open();
           }
       }

       

       //protected void UpdateLabel(string text)
       protected string UpdateLabel(string text)
       {
           strWeight = "";
           if (!string.IsNullOrEmpty(text))
           {
               //= "";//txtGrossWeight.Text;
               string QT = "";
               //if (txtGrossWeight.Text != "")
               //{
               //    QT = txtGrossWeight.Text;
               //}
               string strT = "";
               int numss = 0;
               if (text.Length > 20)
               {
                   strT = text;
                   for (int i = 0; i < strT.Length; i++)
                   {
                       int Snum = strT.IndexOf("=");
                       strT = strT.Remove(0, Snum);
                       strT = strT.Substring(0, 8);
                       //char[] strN = text.ToCharArray();
                       strT = strT.Replace("0000", "00").Replace("000", "00").Replace("=", "");




                       //获取小数点的位置  

                       string diana = "";
                       numss = strT.IndexOf(".");
                       diana = strT;
                       if (numss == 0)
                       {
                           text = "0" + text;
                           numss = strT.IndexOf(".");
                           diana = strT.Replace("00", "");
                       }

                       //获取小数点后面的数字 是否有两位 不足两位补足两位  
                       String dianAfters = strT.Substring(0, numss + 1);
                       String afterDatas = strT.Replace(dianAfters, "");
                       if (afterDatas.Length < 2)
                       {
                           afterDatas = afterDatas + "0";
                       }
                       else
                       {
                           afterDatas = afterDatas;
                       }
                       text = dianAfters + afterDatas;


                       text = ReverseString(text);
                       if (text.Length >= 5)
                       {
                           text = text.Replace("000", "0");
                           text = text.Replace("00", "0");
                           if (text.Substring(0, 1) == "0")
                           {
                               text = text.Remove(0, 1);
                           }
                       }
                       if (text.Length == 3)
                       {
                           text = text + "0";
                       }

                       //txtGrossWeight.Text = text;
                       strWeight = text;
                       QT = strWeight;// txtGrossWeight.Text;

                       break;

                   }

                   //int Snum = strT.IndexOf("=");

                   // strT = strT.Substring(Snum, 8);


               }
               else
               {
                   if (QT != "")
                   {
                       strWeight = QT;
                   }
                   #region
                   else
                   {
                       ASCIIEncoding asc = new ASCIIEncoding();
                       byte[] bytest = asc.GetBytes(text);
                       int NumV = 0;
                       foreach (byte c in bytest)
                       {
                           if (c < 48 || c > 57)                          //判断是否为数字 .10jinzhi 46-8jinzhi56=10jinzhi61-8jinzhi75 
                           {
                               if (c == 46 || c == 61)
                               {
                                   NumV = 0;
                               }
                               else
                               {
                                   NumV = 1;
                                   break;
                               }//不是，就返回False
                           }
                           else
                           {
                               NumV = 0;
                           }
                       }
                       if (NumV == 0)
                       {
                           if (text.Length == 8)
                           {
                               if (text.IndexOf("=") >= 5)
                               {
                                   text = text.Replace("0000", "00").Replace("000", "00").Replace("=", "");
                                   if (text.Replace(text.Substring(0, text.IndexOf(".") + 1), "").Length > 4)
                                   {
                                       QT = text.Substring(text.Length - 2, 2);
                                       text = text.Remove(text.Length - 2);
                                   }
                                   else
                                   {
                                       QT = text.Substring(text.Length - 1, 1);
                                       text = text.Remove(text.Length - 1);
                                   }

                                   text = QT + text;
                               }
                               text = text.Replace("0000", "00").Replace("000", "00").Replace("=", "");
                               if (text.IndexOf(".") == text.Length - 1)
                               {
                                   if (text.Length >= 5)
                                   {
                                       QT = "";
                                       QT = text.Substring(0, 2);
                                       text = text.Remove(0, 2);
                                       text = text + QT;
                                   }
                               }
                               text = ReverseString(text);
                               if (text.Length >= 5)
                               {
                                   if (text.IndexOf(".") == 0)
                                   {
                                       text = text.Replace("0000", "");
                                   }
                                   text = text.Replace("000", "0");
                                   text = text.Replace("00", "0");
                                   //text = "02.201";
                                   //if (text.Substring(0, 1) == "0")
                                   //{
                                   //    if (text.IndexOf(".") == 1)
                                   //    { }
                                   //    else
                                   //    {
                                   //        if (text.Substring(0, text.IndexOf(".")).Length == 2)
                                   //        {
                                   //            text = text.Remove(0, 1);
                                   //        }
                                   //        //int ssss = text.Substring(0, text.IndexOf(".")).Length;
                                   //    }
                                   //}

                               }
                               // text="01.401";
                               if (text.Length == 3)
                               {
                                   if (text.IndexOf(".") == 0)
                                   {
                                       text = "0" + text;
                                   }
                                   else
                                   {
                                       text = text + "0";
                                   }
                               }
                               numss = text.IndexOf(".");
                               if (text.IndexOf(".") == text.Length - 1)
                               {
                                   text = text + "00";
                               }
                               //获取小数点后面的数字 是否有两位 不足两位补足两位  
                               String dianAfter = text.Substring(0, numss + 1);
                               String afterData = "";
                               if (dianAfter.Length > 0)
                               {
                                   afterData = text.Replace(dianAfter, "");
                               }
                               //if (text.Substring(0, 1) == "0")
                               //{
                               //    if (text.IndexOf(".") == 1)
                               //    { }
                               //    else
                               //    {
                               //        if (afterData.Length == 2)
                               //        {
                               //            text = text.Remove(0, 1);
                               //        }
                               //    }
                               //}
                               if (afterData.Length >= 3)
                               {
                                   if (afterData.Length == 3)
                                   {
                                       //取最后一位字符
                                       afterData = text.Substring(text.Length - 1);

                                       text = text.Remove(text.Length - 1);
                                       if (afterData != "0")
                                       {
                                           text = afterData + text;
                                       }
                                   }
                                   if (afterData.Length >= 4)
                                   {
                                       text = text.Replace("0000", "").Replace("000", "").Replace("00", "");
                                       if (afterData.Length == 4)
                                       {
                                           afterData = afterData.Substring(afterData.Length - 2);
                                           text = text.Remove(text.Length - afterData.Length);
                                           afterData = ReverseString(afterData);
                                           if (dianAfter.Length == 3)
                                           {
                                               if (afterData.Substring(afterData.Length - 1) == "0")
                                               {
                                                   afterData = afterData.Remove(afterData.Length - 1);
                                               }
                                               afterData = afterData.Substring(afterData.Length - 1);
                                           }
                                           text = afterData + text;

                                       }
                                   }
                               }
                               if (text.Substring(0, 1) == "0")
                               {
                                   if (text.Substring(0, text.IndexOf(".")).Length >= 2)
                                   {
                                       text = text.Remove(0, 1);
                                   }
                               }
                           }
                       }

                   }
                   #endregion

               }
           }
           return text;
       }

       private string ReverseString(string Rweight)
       {
           char[] chars = Rweight.ToCharArray();
           Array.Reverse(chars);
           return new string(chars);
       }
    }
}
