using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Printer.Cs.PageEntityInfo
{
    public class PageEntityManager<T>where T:class,new()
    {
        public static T Getentity(XmlNode node)
        {
            T t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();
            foreach (XmlAttribute item in node.Attributes)
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.Name.ToUpper() == item.Name.ToUpper())
                    {
                        switch (p.PropertyType.Name)
                        {
                            case "Int32":
                                p.SetValue(t, Convert.ToInt32(item.Value), null);
                                break;
                            case "Decimal":
                                p.SetValue(t, Convert.ToDecimal(item.Value), null);
                                break;
                            default:
                                p.SetValue(t, Convert.ToString(item.Value), null);
                                break;
                        }
                    }
                }
            }
            return t;
        }
    }
}
