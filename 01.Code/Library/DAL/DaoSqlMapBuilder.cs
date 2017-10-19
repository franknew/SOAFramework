using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace SOAFramework.Library
{
    public class DaoSqlMapBuilder : DomSqlMapBuilder
    {
        public new ISqlMapper ConfigureAndWatch(string resource, ConfigureHandler configureDelegate)
        {
            XmlDocument document;
            if (resource.StartsWith("file://"))
            {
                document = Resources.GetUrlAsXmlDocument(resource.Remove(0, 7));
            }
            else
            {
                document = Resources.GetResourceAsXmlDocument(resource);
            }
            ConfigWatcherHandler.ClearFilesMonitored();
            ConfigWatcherHandler.AddFileToWatch(Resources.GetFileInfo(resource));
            TimerCallback onWhatchedFileChange = new TimerCallback(DomSqlMapBuilder.OnConfigFileChange);
            StateConfig state = default(StateConfig);
            state.FileName = resource;
            state.ConfigureHandler = configureDelegate;
            ISqlMapper result = this.Build(document, true);
            new ConfigWatcherHandler(onWhatchedFileChange, state);
            return result;
        }
    }
}
