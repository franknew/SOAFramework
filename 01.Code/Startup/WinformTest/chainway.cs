using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using WinformTest.RemoteServer;

namespace WinformTest
{
    public class chainway
    {
        public void test()
        {
        }

        public void go(string username, string password, string data, string url)
        {
            try
            {
                Uri uri = new Uri(url);
                DynamicService service = new DynamicService(url, "http://tempuri.org/");
                string str_qdn = data;
                str_qdn = str_qdn.Substring(0, str_qdn.Length - 1);
                //Object result_qdn = service.Invoke("SaveHunanGps_pxjlByStr", new Object[] { username, password, str_qdn });
                //int resultNum = Convert.ToInt32(result_qdn.ToString().Substring(13));
                Service1Client client = new Service1Client();
                client.Endpoint.Address = new EndpointAddress(url);
                string str = client.SaveHunanGps_pxjlByStr(username, password, str_qdn);
                //System.out.println("成功数量：" + resultNum)
            }
            catch (Exception e)
            {
            }

        }
    }
}
