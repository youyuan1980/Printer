using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer.Cs.EntityConfig
{
    public enum PageConfig
    {
        INFO = 1
    }

    public class EntityConfigBase
    {
        private PageConfig _pageConfig;
        public PageConfig pageConfig
        {
            get { return _pageConfig; }
            set { _pageConfig = value; }
        }

        private string _TXM;
        /// <summary>
        /// 条形码
        /// </summary>
        public string TXM
        {
            get { return _TXM; }
            set { _TXM = value; }
        }
    }

    /// <summary>
    /// 用于各附记、备注等
    /// </summary>
    public class NoteList : EntityConfigBase
    {
        private string _Content;

        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
    }
}
