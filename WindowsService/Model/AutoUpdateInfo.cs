using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsService.Model
{
    public class AutoUpdateInfo
    {
        public int ID { get; set; }
        public string SourceDataset { get; set; } //
        public string SourceDataLayer { get; set; } //
        public string TargetDataset { get; set; } //
        public string TargetDataLayer { get; set; } //
        public string UpdataTime { get; set; } //
        public string CreateTime { get; set; } //
        public string LastUpdataTime { get; set; } //

        
        //Source数据库SDE信息
        public string SourceSdeIP { get; set; } //
        public string SourceSdeInstance { get; set; } //
        public string SourceSdeUser { get; set; } //
        public string SourceSdePassword { get; set; } //
        public string SourceSdeVersion { get; set; } //

        //Source Backup数据库SDE信息
        public string SourceBackupSdeIP { get; set; } //
        public string SourceBackupSdeInstance { get; set; } //
        public string SourceBackupSdeUser { get; set; } //
        public string SourceBackupSdePassword { get; set; } //
        public string SourceBackupSdeVersion { get; set; } //

        //Target数据库SDE信息
        public string TargetSdeIP { get; set; } //
        public string TargetSdeInstance { get; set; } //
        public string TargetSdeUser { get; set; } //
        public string TargetSdePassword { get; set; } //
        public string TargetSdeVersion { get; set; }

        //Target Backup数据库SDE信息
        public string TargetBackupSdeIP { get; set; } //
        public string TargetBackupSdeInstance { get; set; } //
        public string TargetBackupSdeUser { get; set; } //
        public string TargetBackupSdePassword { get; set; } //
        public string TargetBackupSdeVersion { get; set; }

    }
}
