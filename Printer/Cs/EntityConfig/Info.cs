using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer.Cs.EntityConfig
{
    public class Info : EntityConfigBase
    {
        public Info()
        {
            pageConfig = PageConfig.INFO;
        }

        private string _FWZL;
        /// <summary>
        /// 房屋座落
        /// </summary>
        public string FWZL
        {
            get { return _FWZL; }
            set { _FWZL = value; }
        }

        public OWNERINFO[] Owners { get; set; }

        
        public string FJ { get; set; }

        
    }

    public class OWNERINFO:EntityConfigBase {

        public string OWNER { get; set; }

        public string CNUM { get; set; }
    }
}
