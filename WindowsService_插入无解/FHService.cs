using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using WindowsService.Model;
using WindowsService.DAL;

namespace WindowsService
{
    public partial class FHService : ServiceBase
    {
        public FHService()
        {
            InitializeComponent();
            
            
        }

        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\temp\\log.txt", true))
            {
                try
                {
                    IAoInitialize m_AoInitialize = new AoInitialize();
                    esriLicenseStatus licenseStatus = esriLicenseStatus.esriLicenseUnavailable;
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
                    if (licenseStatus == esriLicenseStatus.esriLicenseCheckedOut)
                        sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "-----------AO初始化成功，许可状态："+ licenseStatus.ToString());
                    else
                        sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "-----------AO初始化异常，许可状态：" + licenseStatus.ToString());
                }
                catch (Exception err)
                {
                    sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "-----------AO初始化出错-------" + err);
                }

                sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：服务已启动.");
                System.Timers.Timer aTimer = new System.Timers.Timer(60000.0);
                aTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
                aTimer.Enabled = true;
            }
        }

        void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\temp\\log.txt", true))
            {
                
                try
                {
                    sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 拷贝测试开始.");
                    if(Common.AutoUpdateFeatureClass.AutoUpdateFClass())
                    {
                        sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新成功.");
                    }
                    else
                    {
                        sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新失败.");
                    }
                }
                catch(Exception err)
                {
                    sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + err.Message);
                }
                
            }
        }

        protected override void OnStop()
        {
            using(System.IO.StreamWriter sw=new System.IO.StreamWriter ("C:\\temp\\log.txt", true))
            {
                sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Stop.");
            }
        }
    }
}
