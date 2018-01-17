using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer.Cs.PageEntityInfo
{
    #region 页面实体类
    /// <summary>
    /// 页面实体类
    /// </summary>
    public class PageEntity
    {
        private int _ID = 0;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Caption = "";

        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }
        private decimal _Margin_Top = 0;

        public decimal Margin_Top
        {
            get { return _Margin_Top; }
            set { _Margin_Top = value; }
        }
        private decimal _Margin_Left = 0;

        public decimal Margin_Left
        {
            get { return _Margin_Left; }
            set { _Margin_Left = value; }
        }
        private bool _Landscape = false;

        public bool Landscape
        {
            get { return _Landscape; }
            set { _Landscape = value; }
        }
        private decimal _Width = 20.9M;

        public decimal Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private decimal _Height = 29.6M;

        public decimal Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        private string _PageName = "A4";

        public string PageName
        {
            get { return _PageName; }
            set { _PageName = value; }
        }
    }
    #endregion
}
