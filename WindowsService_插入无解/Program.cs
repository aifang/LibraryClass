using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
//using ESRI.ArcGIS

namespace WindowsService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new FHService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
