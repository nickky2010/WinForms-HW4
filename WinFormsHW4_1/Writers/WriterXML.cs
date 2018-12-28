using WinFormsHW4_1.Interfaces;
using WinFormsHW4_1.Models;
using System.Collections.Generic;
using System.Xml;

namespace WinFormsHW4_1.Writers
{
    class WriterXML : IWriter<Student>
    {
        public void Write(List<Student> data, string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode xmlD = doc.CreateXmlDeclaration("1.0", "", "");
            doc.AppendChild(xmlD);
            XmlNode root = doc.CreateElement("Students", "http://tempuri.org/Students.xsd");
            foreach (Student st in data)
            {
                XmlNode newSt = doc.CreateElement("Student");

                XmlNode elemName = doc.CreateElement("Name");
                XmlNode textName = doc.CreateTextNode(st.Name);
                elemName.AppendChild(textName);
                newSt.AppendChild(elemName);

                XmlNode elemAge = doc.CreateElement("Age");
                XmlNode textAge = doc.CreateTextNode(st.Age.ToString());
                elemAge.AppendChild(textAge);
                newSt.AppendChild(elemAge);

                XmlNode elemMark = doc.CreateElement("Marks");
                foreach (int item in st.Marks)
                {
                    XmlNode elemBall = doc.CreateElement("Ball");
                    XmlNode textBall = doc.CreateTextNode(item.ToString());
                    elemBall.AppendChild(textBall);
                    elemMark.AppendChild(elemBall);
                }
                newSt.AppendChild(elemMark);
                root.AppendChild(newSt);
            }
            doc.AppendChild(root);
            doc.Save(filename);
        }
    }
}
