using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public class FirewallManager
    {
        private INetFwMgr _mgr = null;

        public FirewallManager()
        {
            _mgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
        }

        public void AddPortToWhiteList(string name, int port, ProtocolEnum protocol)
        {
            INetFwOpenPort objPort = (INetFwOpenPort)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));
            objPort.Name = name;
            objPort.Port = port;
            switch (protocol)
            {
                case ProtocolEnum.TCP:
                    objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                    break;
                default:
                    objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
                    break;
            }
            objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            objPort.Enabled = true;

            bool exist = false;
            //加入到防火墙的管理策略  
            foreach (INetFwOpenPort mPort in _mgr.LocalPolicy.CurrentProfile.GloballyOpenPorts)
            {
                if (objPort == mPort)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist) _mgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(objPort);
        }

        public void DeletePortFromWhiteList(int port, ProtocolEnum protocol)
        {
            var protocolType = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
            switch (protocol)
            {
                case ProtocolEnum.TCP:
                    protocolType = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                    break;
                default:
                    protocolType = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
                    break;
            }
            _mgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, protocolType);
        }

    }

    public enum ProtocolEnum
    {
        TCP,
        UDP
    }
}
