using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.HTML.JavaScript
{
    public partial class _Default : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack) linqQueryData(initializeArrayList());
        }

        //初始化数组
        private List<int> initializeArrayList()
        {
            List<int> strList = new List<int>();
            for(int i=0;i<100;i++)
            {
                strList.Add(i);
            }
            return strList;
        }

        private  void linqQueryData(List<int> strList)
        {
            var query = from item in strList where item < 10 select item;
            foreach(var a in query)
            {
                Response.Write(a.ToString() + "<br/>");
            }
        }
    }
}
