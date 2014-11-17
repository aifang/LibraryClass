using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WindowBasis
{
    /// <summary>
    /// Linq to XML
    /// </summary>
    public class XMLHelper
    {

        public static void constructXML()
        {
            //创建XML文件
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Books",
                    new XElement("Book",
                        new XAttribute("ID", "104"),
                        new XElement("No", "0004"),
                        new XElement("Name", "Book 0004"),
                        new XElement("Price", "300"),
                        new XElement("Remark", "This is a book 0004.")
                                   ),
                    new XElement("Advertisements",
                        new XComment("this is a website."),       //注释
                        new XElement("Ad",
                        new XAttribute("ID", "1"),
                        new XElement("Name", "w3c"),
                        new XElement("Url", "http://www.w3c.com")
                        )
                        ),
                    new XElement("Time",
                        new XComment("this is a website."),       //注释
                        new XElement("Ad",
                        new XAttribute("ID", "1"),
                        new XElement("Name", "w3c"),
                        new XElement("Url", "http://www.w3c.com")
                        )
                        )
                            )
                                 );
            doc.Save(@"C:\temp\LinqToXML.xml");
            
            //读取XML文件
            XDocument xd = XDocument.Load(@"C:\temp\LinqToXML.xml");
            XElement xe = XElement.Load(@"C:\temp\LinqToXML.xml");

            //查询根节点 ”Books“
            IEnumerable<XElement> elements = from e in doc.Elements("Books") select e;
            foreach(XElement item in elements)
            {
                //输出名字
                System.Diagnostics.Debug.WriteLine(item.Name);                
            }

            ///查询元素
            IEnumerable<XElement> elements1 = from e in doc.Elements("Book")
                                              where (string)e.Element("Name") == "Book 0004"
                                              select e;
            //组合查询
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };
            
            var pairs = from a in numbersA  from  b in numbersB 
                        where a==b
                        select new {a,b};
            foreach (var pair in pairs)
                System.Diagnostics.Debug.WriteLine(pair.a+"="+pair.b);

            }

    }
}
