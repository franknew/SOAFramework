using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WinformTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            config.Servers.Add(new IPEndPoint(IPAddress.Loopback, 11211));
            config.Protocol = MemcachedProtocol.Text;
            config.Authentication.Type = typeof(PlainTextAuthenticator);
            config.Authentication.Parameters["zone"] = "memcached";
            config.Authentication.Parameters["userName"] = "Administrator";
            config.Authentication.Parameters["password"] = "frank";

            var section = (MemcachedClientSection)MemcachedClient.GetSection("enyim.com/memcached");

            config = new MemcachedClientConfiguration();
            foreach (EndPointElement s in section.Servers)
            {
                config.Servers.Add(s.EndPoint);
            }
            config.Protocol = section.Protocol;
            //config1.Protocol = MemcachedProtocol.Text;
            var mc = new MemcachedClient(section);
            //var mc = new MemcachedClient(config);

            for (var i = 0; i < 100; i++)
                mc.Store(StoreMode.Add, "memcached", "World");

        }
    }
}
