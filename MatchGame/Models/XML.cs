using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MatchGame.Models
{
    public class XML
    {
        /// <summary>
        /// Загрузить данные из XML
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="parent">Корневой элемент</param>
        /// <param name="element">Элемент</param>
        /// <param name="attribute">Атрибут</param>
        /// <returns>Массив строк атрибутов</returns>
        public List<string> LoadAttributs(string fileName, string parent = "data", string element = "info", string attribute = "menuId")
        {
            return XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "/" + fileName).Descendants(parent).Elements(element).Select(x => x.Attribute(attribute).Value.ToString()).ToList();
        }

        public void SaveAttributs(string fileName, string value, string parent = "data", string element = "info", string attribute = "menuId")
        {
            var doc = XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "/" + fileName);
            doc.Descendants(parent).Elements(element).First().Attribute(attribute).SetValue(value);
            doc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "/" + fileName);
        }
    }
}

