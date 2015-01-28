using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsService.Model
{
    /// <summary>
    /// 记录备份所需要的所有信息
    /// </summary>
    public class BackupInfo
    {
        //备份基本信息
        public string PlanID { get; set; }   //备份数据计划ID
        public string PlanName { get; set; } //备份计划名称
        public string PlanCreator { get; set; } //备份计划创建者
        public DateTime PlanCreateTime { get; set; } //备份计划创建时间
        public DateTime PlanActiveTime { get; set; } //备份计划执行时间
        public int PlanActiveMethod { get; set; } //备份计划执行方式
        public int ExecuteFlag { get; set; } //标志备份计划是否需要执行

        //Source数据SDE信息
        public string SourceSdeIP { get; set; } //
        public string SourceSdeInstance { get; set; } //
        public string SourceSdeUser { get; set; } //
        public string SourceSdePassword { get; set; } //
        public string SourceSdeVersion { get; set; } //
        public string SourceSdeFeatureDataSet { get; set; } //

        //Target数据SDE信息
        public string TargetSdeIP { get; set; } //
        public string TargetSdeInstance { get; set; } //
        public string TargetSdeUser { get; set; } //
        public string TargetSdePassword { get; set; } //
        public string TargetSdeVersion { get; set; } //
        public string TargetSdeFeatureDataSet { get; set; } //
        
    }
}
