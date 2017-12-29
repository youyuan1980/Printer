using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Printer.Cs.EntityConfig;
using Printer.Cs.Dal;

namespace Printer.Cs.Certificate
{
    public class InfoCertificate:CertificateBase,ICertificate
    {
        public InfoCertificate()
        {
            caption = "现在准备打印,\r\n请放入空白 A4 纸 ! 谢谢配合!";
        }

        public void Print()
        {
            Info info = EntityConfigDAL.GetInfo();
            PrintCertificate<Info>(info);
        }
    }
}
