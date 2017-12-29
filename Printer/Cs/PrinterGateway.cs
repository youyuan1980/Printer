using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Printer.Cs.EntityConfig;
using System.Data;
namespace Printer.Cs
{
    public sealed class PrinterGateway
    {
        public static PrinterGateway Default;

        #region 打印方法
        public void Printer<EntityType>(EntityType obj, string text, string caption) where EntityType : EntityConfigBase
        {
            PrinterHelper printerHelper = new PrinterHelper();
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(caption))
            {
                printerHelper.StartPrint(obj, text, caption);
            }
            else
            {
                printerHelper.StartPrint(obj);
            }
        }

        public void Printer<EntityType>(EntityType obj) where EntityType : EntityConfigBase
        {
            Printer<EntityType>(obj, string.Empty, string.Empty);
        }
        #endregion

        #region 赋值方法

        public void FillData<EntityType>(EntityType obj, DataRow row) where EntityType : EntityConfigBase
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (DataColumn Column in row.Table.Columns)
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.Name.ToUpper() == Column.ColumnName.ToUpper())
                    {
                        if (Column.ColumnName.ToUpper() == "FJ")
                        {
                            p.SetValue(obj, GetNoteList(Convert.ToString(row[Column.ColumnName])), null);
                        }
                        else
                        {
                            if (row[Column.ColumnName] != null)
                            {
                                p.SetValue(obj, Convert.ToString(row[Column.ColumnName]), null);
                            }
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        #region 返回换行分组后的附记记录集
        /// <summary>
        /// 返回换行分组后的附记记录集
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<NoteList> GetNoteList(string str)
        {
            List<NoteList> list = new List<NoteList>();
            string[] strs = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string text in strs)
            {
                NoteList note = new NoteList();
                note.Content = text;
                list.Add(note);
            }
            return list;
        }
        #endregion
    }
}
