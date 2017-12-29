using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Printer.Cs.EntityConfig;

namespace Printer.Cs.Certificate
{
    /// <summary>
    /// 打印业务抽像类
    /// </summary>
    public abstract class AbstractCertificate
    {
        protected PrinterGateway printerGateway;

        protected string caption = string.Empty;

        protected abstract void PrintCertificate<T>(T data)
            where T : EntityConfigBase,new();
    }
}
