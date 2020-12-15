using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Luval.UiPath.NER.Activities
{
    public class OutputParser
    {
        public static DataTable Parse(string content)
        {
            var xml = GetXml(content);
            var dt = new DataTable("NER");
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            var elements = xml.Elements("wi");

            if (!elements.Any()) return dt;

            var phrases = new List<string>();
            var startType = elements.First().Attribute("entity").Value;
            phrases.Add(elements.First().Value);

            foreach (var item in elements.Skip(1))
            {
                var type = item.Attribute("entity").Value;
                if (startType != type)
                {
                    var row = dt.NewRow();
                    row["Type"] = GetClassType(startType);
                    row["Value"] = string.Join(" ", phrases);
                    phrases.Clear();
                    dt.Rows.Add(row);
                    startType = type;
                }
                phrases.Add(item.Value);
            }
            return dt;
        }

        private static string GetClassType(string type)
        {
            return type == "O" ? "PHRASE" : type;
        }

        private static XElement GetXml(string content)
        {
            var sw = new StringWriter();
            sw.WriteLine("<root>");
            sw.WriteLine(content);
            sw.WriteLine("</root>");
            return XElement.Parse(sw.ToString());
        }
    }
}
