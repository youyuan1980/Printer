using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Printer.Cs.EntityConfig;

namespace Printer.Cs.Dal
{
    public class EntityConfigDAL
    {
        public static Info GetInfo() {
            Info info = new Info();

            info.FWZL = "桑园北村";
            info.TXM = "123";

            List<OWNERINFO> owners = new List<OWNERINFO>();

            owners.Add(new OWNERINFO() { 
                OWNER = "尤圆",
                CNUM = "123456"
            });

            owners.Add(new OWNERINFO()
            {
                OWNER = "新桓结衣",
                CNUM = "123456"
            });

            info.Owners = owners.ToArray();

            info.FJ = "abcdef121212121212121212ghijklmn\r\n34563456\r\n";
            return info;
        }
    }
}
