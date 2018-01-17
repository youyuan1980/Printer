using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
using Printer.Cs.EntityConfig;
using System.Reflection;
using Printer.Cs.PageEntityInfo;

namespace Printer.Cs
{
    class PrinterHelper : System.Drawing.Printing.PrintDocument
    {
        private XmlNode xmldata = null;

        private PageEntity pageentity = null;

        private const string Page_Default_Font_Name = "Arial";
        private const decimal Page_Default_Line_Height = 0.5M;


        #region 打印事件
        protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e)
        {
            for (int i = 0; i < xmldata.ChildNodes.Count; i++)
            {
                XmlNode row = xmldata.ChildNodes[i];
                string name = row.Name;
                switch (name)
                {
                    case "text":
                        textConfig(row, e);
                        break;
                    case "line":
                        LineEntity lineentity = PageEntityManager<LineEntity>.Getentity(row);
                        decimal s_x = lineentity.S_x + pageentity.Margin_Left;
                        decimal s_y = lineentity.S_y + pageentity.Margin_Top;
                        decimal e_x = lineentity.E_x + pageentity.Margin_Left;
                        decimal e_y = lineentity.E_y + pageentity.Margin_Top;
                        e.Graphics.DrawLine(new Pen(Brushes.Black, lineentity.Width), GetPoint(s_x, s_y), GetPoint(e_x, e_y));
                        break;
                    case "loop":
                        for (int j = 0; j < row.ChildNodes.Count; j++)
                        {
                            XmlNode row_loop = row.ChildNodes[j];
                            string name_loop = row_loop.Name;
                            switch (name_loop)
                            {
                                case "text":
                                    textConfig(row_loop, e);
                                    break;
                                default: break;
                            }
                        }
                        break;
                    default: break;
                }
            }
            base.OnPrintPage(e);
        }
        #endregion

        #region 文字型配置
        /// <summary>
        /// 文字型配置
        /// </summary>
        /// <param name="row"></param>
        /// <param name="e"></param>
        private void textConfig(XmlNode row, System.Drawing.Printing.PrintPageEventArgs e)
        {

            string Font_Name = row.Attributes["Font_Name"] != null ? row.Attributes["Font_Name"].Value : Page_Default_Font_Name;
            float Font_Size = 0;
            float.TryParse(row.Attributes["Font_Size"].Value, out Font_Size);
            string fontStyle = Convert.ToString(row.Attributes["FontStyle"]!=null?row.Attributes["FontStyle"].Value:"");
            decimal x = 0;
            decimal.TryParse(row.Attributes["x"].Value, out x);
            x += pageentity.Margin_Left;
            decimal y = 0;
            decimal.TryParse(row.Attributes["y"].Value, out y);
            y += pageentity.Margin_Top;
            int MaxLength = 0;
            int.TryParse(row.Attributes["MaxLength"].Value, out MaxLength);
            string value = Convert.ToString(row.Attributes["value"].Value).Replace("\r\n\r\n", "\r\n");
            decimal line_height = 0;
            decimal.TryParse(row.Attributes["Line_Height"]!=null?row.Attributes["Line_Height"].Value:"0.5", out line_height);
            int rowsize = value.Length / MaxLength;
            int rowmaxlength = value.Length;
            if (Font_Name == "C39HrP24DhTt")
            {
                Cs.BarCode39 barcode = new Cs.BarCode39();
                barcode.BarCode = value;
                Bitmap bmp = barcode.getBarCode();
                e.Graphics.DrawImage(bmp, GetPoint(x, y));
                return;
            }
            for (int p = 0; p < rowsize; p++)
            {
                switch (fontStyle)
                {
                    case "Bold":
                        e.Graphics.DrawString(value.Substring(MaxLength * p, MaxLength) + '\r' + '\n', new Font(Font_Name, Font_Size, FontStyle.Bold), Brushes.Black, GetPoint(x, y));
                        break;
                    case "Underline":
                        e.Graphics.DrawString(value.Substring(MaxLength * p, MaxLength) + '\r' + '\n', new Font(Font_Name, Font_Size, FontStyle.Underline), Brushes.Black, GetPoint(x, y));
                        break;
                    default: e.Graphics.DrawString(value.Substring(MaxLength * p, MaxLength) + '\r' + '\n', new Font(Font_Name, Font_Size, FontStyle.Regular), Brushes.Black, GetPoint(x, y));
                        break;
                }
                y += line_height;
            }
            switch (fontStyle)
            {
                case "Bold":
                    e.Graphics.DrawString(value.Substring(MaxLength * rowsize, rowmaxlength - MaxLength * rowsize) + '\r' + '\n', new Font(Font_Name, Font_Size, FontStyle.Bold), Brushes.Black, GetPoint(x, y));
                    break;
                case "Underline":
                    e.Graphics.DrawString(value.Substring(MaxLength * rowsize, rowmaxlength - MaxLength * rowsize) + '\r' + '\n', new Font(Font_Name, Font_Size, FontStyle.Underline), Brushes.Black, GetPoint(x, y));
                    break;
                default: e.Graphics.DrawString(value.Substring(MaxLength * rowsize, rowmaxlength - MaxLength * rowsize) + '\r' + '\n', new Font(Font_Name, Font_Size, FontStyle.Regular), Brushes.Black, GetPoint(x, y));
                    break;
            }
        }
        #endregion

        #region 打印
        public void StartPrint(EntityConfigBase data, string text, string caption)
        {
            XmlNode config = GetConfig(data.pageConfig);
            DataFillToXml(data, ref config);
            xmldata = config;
            //初始化页面实体
            pageentity = PageEntityManager<PageEntity>.Getentity(xmldata);
            DefaultPageSettings.Landscape = pageentity.Landscape;
            int PageWidth = CmToPix(pageentity.Width);
            int PageHeigth = CmToPix(pageentity.Height);
            PaperSize p1 = null;
            for (int i = 0; i < PrinterSettings.PaperSizes.Count; i++)
            {
                if (PrinterSettings.PaperSizes[i].PaperName == pageentity.PageName)
                {
                    p1 = PrinterSettings.PaperSizes[i];
                    break;
                }
            }
            if (p1 == null)
            {
                p1 = new PaperSize(pageentity.PageName, PageWidth, PageHeigth);
            }
            DefaultPageSettings.PaperSize = p1;
            PrintShowDialog(text, caption);
        }

        public void StartPrint(EntityConfigBase data)
        {
            StartPrint(data, string.Empty, string.Empty);
        }

        public void PrintShowDialog(string text, string caption)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(caption))
            {
                if (DialogResult.Yes == MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    //调试用
                    if (Form1.IsDeBug)
                    {
                        PrintPreviewDialog ppd = new PrintPreviewDialog();
                        ppd.Document = this;
                        ppd.ShowDialog();
                    }
                    else
                    {
                        this.Print();
                    }
                }
            }
            else
            {
                //调试用
                if (Form1.IsDeBug)
                {
                    PrintPreviewDialog ppd = new PrintPreviewDialog();
                    ppd.Document = this;
                    ppd.ShowDialog();
                }
                else
                {
                    this.Print();
                }
            }
        }

        public void PrintShowDialog()
        {
            PrintShowDialog(string.Empty, string.Empty);
        }
        #endregion

        #region 配置
        protected Point GetPoint(decimal x, decimal y)
        {
            return new Point((int)(x * 0.3937m * 100), (int)(y * 0.3937m * 100));
        }

        protected int CmToPix(decimal Cm)
        {
            return (int)(Cm * 0.3937m * 100);
        }

        /// <summary>
        /// 获取配置xml文件
        /// </summary>
        /// <param name="pageconfig"></param>
        /// <returns></returns>
        protected XmlNode GetConfig(PageConfig pageconfig)
        {
            XmlNode data = null;
            XmlDocument PageConfigXML = new XmlDocument();
            string filename = Application.StartupPath + "\\PageConfig\\PageConfig.xml";
            PageConfigXML.Load(filename);
            XmlNodeList d = PageConfigXML.GetElementsByTagName("Page");
            for (int i = 0; i < d.Count; i++)
            {
                if (Convert.ToInt32(pageconfig) == Convert.ToInt32(d[i].Attributes["id"].Value))
                {
                    data = d[i];
                    break;
                }
            }
            return data;
        }

        protected void DataFillToXml(object data, ref XmlNode config)
        {
            PropertyInfo[] zz = data.GetType().GetProperties();
            for (int n = 0; n < config.ChildNodes.Count; n++)
            {
                string configType = config.ChildNodes[n].Name;
                switch (configType)
                {
                    case "text":
                        foreach (PropertyInfo x in zz)
                        {
                            if (config.ChildNodes[n].Attributes["DataFieldName"]!=null)
                            {
                                if (Convert.ToString(config.ChildNodes[n].Attributes["DataFieldName"].Value) == x.Name)
                                {
                                    config.ChildNodes[n].Attributes["value"].Value = Convert.ToString(x.GetValue(data, null));
                                    break;
                                }
                            }
                        }
                        break;
                    case "loop":
                        List<XmlNode> l = new List<XmlNode>();
                        foreach (PropertyInfo x in zz)
                        {
                            if (Convert.ToString(config.ChildNodes[n].Attributes["DataFieldName"].Value) == x.Name)
                            {
                                EntityConfigBase[] list = (EntityConfigBase[])x.GetValue(data, null);
                                if (list != null)
                                {
                                    //数据的偏移量
                                    decimal y = 0;
                                    if (list.Length > 0)
                                    {
                                        PropertyInfo[] mm = list[0].GetType().GetProperties();
                                        for (int i = 0; i < list.Length; i++)
                                        {
                                            //每行数据的初始偏移量
                                            decimal yyy = y;
                                            EntityConfigBase d = list[i];
                                            foreach (XmlNode r in config.ChildNodes[n].ChildNodes)
                                            {
                                                //记录该行最大的偏移量
                                                decimal yy = yyy;
                                                foreach (PropertyInfo z in mm)
                                                {
                                                    if (r.Attributes["DataFieldName"]!=null)
                                                    {
                                                        if (Convert.ToString(r.Attributes["DataFieldName"].Value) == z.Name)
                                                        {
                                                            FormatXML_Loop(r, ref yy, ref l, Convert.ToString(z.GetValue(d, null)));
                                                            XmlNode b = r.Clone();
                                                            l.Add(b);
                                                            if (yy > y)
                                                            {
                                                                y = yy;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        config.ChildNodes[n].RemoveAll();
                        foreach (XmlNode newxml in l)
                        {
                            config.ChildNodes[n].AppendChild(newxml);
                        }
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 循环类型的换行
        /// </summary>
        /// <param name="r"></param>
        /// <param name="loop_y"></param>
        /// <param name="l"></param>
        /// <param name="text"></param>
        private void FormatXML_Loop(XmlNode r, ref decimal loop_y, ref List<XmlNode> l, string text)
        {
            string Font_Name = r.Attributes["Font_Name"] != null ? r.Attributes["Font_Name"].Value : Page_Default_Font_Name;
            float Font_Size = float.Parse(r.Attributes["Font_Size"].Value);
            decimal x = Convert.ToDecimal(r.Attributes["x"].Value);
            if (loop_y == 0)
            {
                loop_y = Convert.ToDecimal(r.Attributes["y"].Value);
            }
            int MaxLength = Convert.ToInt32(r.Attributes["MaxLength"].Value);
            string value = text;
            decimal line_height = Convert.ToDecimal(r.Attributes["Line_Height"]!=null?r.Attributes["Line_Height"].Value:"0.5");
            int rowsize = value.Length / MaxLength;
            int rowmaxlength = value.Length;
            for (int p = 0; p < rowsize; p++)
            {
                r.Attributes["value"].Value = value.Substring(MaxLength * p, MaxLength) + '\r' + '\n';
                r.Attributes["y"].Value = loop_y.ToString();
                XmlNode b = r.Clone();
                l.Add(b);
                loop_y += line_height;
            }
            if (value.Substring(MaxLength * rowsize, rowmaxlength - MaxLength * rowsize).Length > 0)
            {
                r.Attributes["value"].Value = value.Substring(MaxLength * rowsize, rowmaxlength - MaxLength * rowsize) + '\r' + '\n';
                r.Attributes["y"].Value = loop_y.ToString();
                XmlNode c = r.Clone();
                l.Add(c);
                loop_y += line_height;
            }
        }
        #endregion
    }
}
