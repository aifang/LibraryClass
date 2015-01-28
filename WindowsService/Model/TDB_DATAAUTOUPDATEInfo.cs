using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsService.Model
{
    public class TDB_DATAAUTOUPDATEInfo
    {
        public int ID { get; set; }
        public string OLDBACKDATABASEID { get; set; }   //
        public string OLDDATABASEID { get; set; } //
        public string OLDDATASET { get; set; } //
        public string OLDDATALAYER { get; set; } //
        public string TARGETBACKDATABASEID { get; set; } //
        public string TARGETDATABASEID { get; set; } //
        public string TARGETDATASET { get; set; } //
        public string TARGETDATALAYER { get; set; } //
        public string UPDATETIME { get; set; } //
        public string CREATTIME { get; set; } //
        public string LASTUPDATETIME { get; set; } //
    }
}
