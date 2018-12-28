using WinFormsHW4_1.Interfaces;
using WinFormsHW4_1.Models;
using System.Collections.Generic;
using System.Xml;

namespace WinFormsHW4_1.Readers
{
    class ReaderXML : IReader<Student>
    {
        public List<Student> Read(string filename)
        {
            List<Student> students = new List<Student>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList list = doc.GetElementsByTagName("Student");
            Student currentStudent = null;
            foreach (XmlNode x in list)
            {
                currentStudent = new Student();
                currentStudent.Name = x["Name"].InnerText;
                currentStudent.Age = int.Parse(x["Age"].InnerText);
                XmlNodeList markList = x["Marks"].GetElementsByTagName("Ball");
                currentStudent.Marks = new int[markList.Count];
                for (int i = 0; i < markList.Count; i++)
                {
                    currentStudent.Marks[i] = int.Parse(markList[i].InnerText);
                }
                students.Add(currentStudent);
            }
            return students;
        }
    }
}
