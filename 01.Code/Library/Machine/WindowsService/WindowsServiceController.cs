using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace SOAFramework.Library.Machine
{
    public class WindowsServiceController
    {
        private ManagementClass _managementClass;
        private string _host;
        private string _username;
        private string _password;
        private string _manageScope;
        private string _actionPath;

        public WindowsServiceController(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
            _manageScope = string.Format(@"\\{0}\root\cimv2", _host);
        }

        public bool CheckConnection()
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();
            connectionOptions.Username = _username;
            connectionOptions.Password = _password;
            ManagementScope managementScope = new ManagementScope(_manageScope, connectionOptions);
            try
            {
                managementScope.Connect();
            }
            catch (Exception ex)
            {
            }
            return managementScope.IsConnected;
        }

        public void Connect()
        {
            if (CheckConnection())
            {
                _actionPath = _manageScope + ":Win32_Service";
                _managementClass = new ManagementClass(_actionPath);
                if (_username != null && _username.Length > 0)
                {
                    ConnectionOptions connectionOptions = new ConnectionOptions();
                    connectionOptions.Username = _username;
                    connectionOptions.Password = _password;
                    ManagementScope managementClass = new ManagementScope(_manageScope, connectionOptions);
                    _managementClass.Scope = managementClass;
                }
            }
        }

        public List<ServiceEntity> GetServices(string nameFilter = null)
        {
            List<ServiceEntity> list = new List<ServiceEntity>();
            foreach (ManagementObject mo in _managementClass.GetInstances())
            {
                //services[i, 0] = (string)mo["Name"];
                //services[i, 1] = (string)mo["DisplayName"];
                //services[i, 2] = (string)mo["State"];
                //services[i, 3] = (string)mo["StartMode"];
                //i++;
                ServiceEntity entity = new ServiceEntity
                {

                };
            }
            return list;
        }

        public bool StartService(string name)
        {
            return ActService(name, ServiceActionType.StartService);
        }

        public bool StopService(string name)
        {
            return ActService(name, ServiceActionType.StopService);
        }

        public bool PauseService(string name)
        {
            return ActService(name, ServiceActionType.PauseService);
        }

        public bool ResumeService(string name)
        {
            return ActService(name, ServiceActionType.ResumeService);
        }

        private bool ActService(string name, ServiceActionType action)
        {
            var mo = _managementClass.CreateInstance();
            mo.Path = new ManagementPath(_actionPath + ".Name=\"" + name + "\"");
            mo.InvokeMethod(action.ToString(), null);
            return true;
        }
    }

    public enum ServiceActionType
    {
        StartService,
        PauseService,
        ResumeService,
        StopService
    }
}
