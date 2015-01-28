using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsService.Model
{
    public class TDB_BACKUPPLANInfo
    {
        public string PLANID { get; set; }   //备份计划编号
        public string PLANNAME { get; set; } //备份计划名称
        public string DATABASEID { get; set; } //备份数据库编号
        public string PLANCREATOR { get; set; } //备份计划创建者
        public DateTime PLANCREATETIME { get; set; } //备份计划创建时间
        public DateTime PLANACTIVETIME { get; set; } //备份计划执行时间
        public int PLANACTIVEMETHOD { get; set; } //备份计划执行方式
        public int EXECUTEFLAG { get; set; } //标志备份计划是否需要执行
    }
}
