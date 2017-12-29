using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Printer.Cs.EntityConfig;
using System.Windows.Forms;

namespace Printer.Cs.Certificate
{
    public class CertificateBase : AbstractCertificate
    {
        protected override void PrintCertificate<T>(T data)
        {
            if (PrinterGateway.Default == null)
            {
                PrinterGateway.Default = new PrinterGateway();
            }
            printerGateway = PrinterGateway.Default;

            if (DialogResult.Yes == MessageBox.Show(caption, "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                printerGateway.Printer<T>(data);               
            }
        }
    }
}
