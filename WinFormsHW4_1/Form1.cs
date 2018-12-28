using WinFormsHW4_1.Interfaces;
using WinFormsHW4_1.Models;
using WinFormsHW4_1.Readers;
using WinFormsHW4_1.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace WinFormsHW4_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = Directory.GetParent(Directory.GetParent(Application.StartupPath).FullName).FullName;
            saveFileDialog1.InitialDirectory = Directory.GetParent(Directory.GetParent(Application.StartupPath).FullName).FullName;
        }
        void Read<T>(string filename, IReader<T> reader)
        {
            richTextBox1.Clear();
            List<T> data = reader.Read(filename);
            for (int i = 0; i < data.Count; i++)
            {
                richTextBox1.AppendText(data[i] + "\n");
            }
        }
        void Write(string filename, IWriter<Student> writer)
        {
            List<Student> data = new List<Student>(richTextBox1.Lines.Count());
            Regex r = new Regex(" +(?:оценки:)? *");
            foreach (string item in richTextBox1.Lines)
            {
                try
                {
                    string[] temp = r.Split(item);
                    Student st = new Student();
                    st.Name = temp[0];
                    st.Age = int.Parse(temp[1]);
                    st.Marks = new int[temp.Length - 2];
                    for (int i = 0; i < temp.Length - 2; i++)
                    {
                        st.Marks[i] = int.Parse(temp[i + 2]);
                    }
                    data.Add(st);
                }
                catch { }
            }
            writer.Write(data, filename);
        }
        //метод для валидации
        void validateStudentXml(string filename)
        {
            richTextBox1.Clear();
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add("http://tempuri.org/Students.xsd", @"..\..\Students.xsd");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;

            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            XmlReader reader = XmlReader.Create(filename, settings);
            while (reader.Read()) ;
            if (richTextBox1.Lines.Length == 0)
                richTextBox1.Text += "File validation was successful!!!";
        }

        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            richTextBox1.Text += e.Message + "\n";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "xml file|*.xml|json file|*.json";
                openFileDialog1.DefaultExt = "xml |json";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    switch (openFileDialog1.FilterIndex)
                    {
                        case 1:
                            Read<Student>(openFileDialog1.FileName, new ReaderXML());
                            break;
                        case 2:
                            Read<Student>(openFileDialog1.FileName, new ReaderJSON());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сould not read file!!! "+ex.Message);
            }
        }

        private void intoXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "xml file|*.xml";
                saveFileDialog1.DefaultExt = "xml";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Write(saveFileDialog1.FileName, new WriterXML());
                }
            }
            catch { }
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "xml file|*.xml";
                openFileDialog1.DefaultExt = "xml";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    validateStudentXml(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сould not check file!!! " + ex.Message);
            }
        }

        private void intoJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "json file|*.json";
                saveFileDialog1.DefaultExt = "json";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Write(saveFileDialog1.FileName, new WriterJSON());
                }
            }
            catch { }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
