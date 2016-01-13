using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public class HtmlOutputGenerator
{
    public class Style
    {
        public string BGColor { get; set; }
        public string TextAlign { get; set; }
        public int Padding { get; set; }
        public string VerticalAlign { get; set; }
    }

    private Dictionary<string, string> columns;
    private IEnumerable<object> objects;

    public Style HeaderStyle { get; set; }
    public Style ItemStyle { get; set; }

    public HtmlOutputGenerator(IEnumerable<object> objects)
    {
        this.columns = new Dictionary<string, string>();
        this.objects = objects;

        this.HeaderStyle = new Style()
        {
            BGColor = "#d9d9d9",
            Padding = 10,
            TextAlign = "center",
            VerticalAlign = "center"
        };
        this.ItemStyle = new Style()
        {
            BGColor = "lightgray",
            Padding = 10,
            TextAlign = "center",
            VerticalAlign = "center"
        };
    }

    public void AppendColumn(string headerName, string objectPropertyName)
    {
        if (!columns.ContainsKey(headerName))
        {
            this.columns.Add(headerName, objectPropertyName);
        }
    }
    public void GenerateHtmlOutput(StringBuilder output)
    {
        output.AppendFormat("<table border = \"1 \" style=\"width:100%; text-align: {0} \" >", HeaderStyle.TextAlign);
        output.AppendFormat("<tr style=\"background-color: {0}; font-size:14pt; text-align: {1} \">", HeaderStyle.BGColor, HeaderStyle.TextAlign);

        foreach (string header in columns.Keys)
        {
            output.AppendFormat("<td valign=\"{0}\" style=\"padding:{1}px; \">{2}</td>", HeaderStyle.VerticalAlign, HeaderStyle.TextAlign, header);
        }

        output.Append("</tr>");

        foreach (object item in this.objects)
        {
            output.AppendFormat("<tr style=\"background-color:{0}; \">", ItemStyle.BGColor);

            foreach (string property in this.columns.Values)
            {
                string value;
                try
                {
                    object valueAsObject = GetPropertyValue(item, property);
                    value = valueAsObject != null ? valueAsObject.ToString() : string.Empty;
                }
                catch (Exception)
                {
                    value = string.Empty;
                }

                output.AppendFormat("<td valign=\"{0}\" style=\"padding:{1}px; text-align:{2}; \">{3}</td>", ItemStyle.VerticalAlign, ItemStyle.Padding, ItemStyle.TextAlign, value);
            }

            output.Append("</tr>");
        }

        output.Append("</table>");
    }

    private object GetPropertyValue(Object fromObject, string propertyName)
    {
        Type objectType = fromObject.GetType();
        PropertyInfo propInfo = objectType.GetProperty(propertyName);
        if (propInfo == null && propertyName.Contains('.'))
        {
            string firstProp = propertyName.Substring(0, propertyName.IndexOf('.'));
            propInfo = objectType.GetProperty(firstProp);
            if (propInfo == null)//property name is invalid
            {
                throw new ArgumentException(string.Format("Property {0} is not a valid property of {1}.", firstProp, fromObject.GetType().ToString()));
            }

            return GetPropertyValue(propInfo.GetValue(fromObject, null), propertyName.Substring(propertyName.IndexOf('.') + 1));
        }
        else
        {
            return propInfo.GetValue(fromObject, null);
        }
    }
}
