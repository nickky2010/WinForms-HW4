using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using WinFormsHW4_1.Interfaces;
using WinFormsHW4_1.Models;

namespace WinFormsHW4_1.Readers
{
    class ReaderJSON : IReader<Student>
    {
        public List<Student> Read(string filename)
        {
            List<Student> students = new List<Student>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Student>));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                students = (List<Student>)jsonFormatter.ReadObject(fs);
            }
            return students;
        }
    }
}
