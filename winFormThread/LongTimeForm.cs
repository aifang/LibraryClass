using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace winFormThread
{
    public partial class LongTimeForm : Form
    {
        public LongTimeForm()
        {
            InitializeComponent();
            Debug.Listeners.Add(new ConsoleTraceListener());
        }
        private string QueryDataBase(IFeatureClass abc)
        {

            Thread.Sleep(5 * 1000);
            return "yuyijq";
        }
        private string QueryDataBase()
        {
            Thread.Sleep(5 * 1000);
            return "yuyijq";
        }

        private void SetText(string ret)
        {
            this.lblResult.Text = ret;
        }

        private void btnLongTime_Click(object sender, EventArgs e)
        {
            
            //IFeatureClass fc=null;
            //string txt="";
            //Func<IFeatureClass, string> ac = () => QueryDataBase();
            //ac.BeginInvoke
            Func<string> func = () => QueryDataBase();
            //从数据库里获取结果后会更新lblResult.Text属性
            func.BeginInvoke((result) =>
            {
                string ret = func.EndInvoke(result);
                //注意这里调用Control.BeginInvoke()
                Action<string> abc = new Action<string>(SetText);
                this.BeginInvoke(abc, ret);
            }, null);

            //IAsyncResult ar = this.BeginInvoke(updateTxt, n);
            ////这里就是你要的返回数据
            //string result = this.EndInvoke(ar).ToString();

            /*
             * 当异步操作完成后，上面代码中用lambda表达式表示的一个回调方法就会执行，
             * 在这里调用EndInvoke获取耗时操作的结果。在这里想想为什么用lambda，
             * 如果不用lambda也不用匿名方法（不管你用啥，实际上就是形成一个闭包）你要怎么做？
             */
        }

        //既然这个WndProc是Win32中处理消息的方法的.Net版，那么我们应该在这里可以监视到所有用户操作的“消息”
        //protected override void WndProc(ref Message m)
        //{
        //    Debug.WriteLine(m.Msg.ToString());
        //    base.WndProc(ref m);
        //}
    }
}
