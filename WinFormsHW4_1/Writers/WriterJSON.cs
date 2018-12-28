using System.Collections.Generic;
using WinFormsHW4_1.Interfaces;
using WinFormsHW4_1.Models;
using System.IO;
using System.Runtime.Serialization.Json;

namespace WinFormsHW4_1.Writers
{
    class WriterJSON : IWriter<Student>
    {
        public void Write(List<Student> data, string filename)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Student>));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, data);
            }
        }
    }
}
