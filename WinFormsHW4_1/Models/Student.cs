using System.Runtime.Serialization;
using System.Text;

namespace WinFormsHW4_1.Models
{
    [DataContract]
    public class Student
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public int[] Marks { get; set; }
        public override string ToString()
        {
            StringBuilder temp = new StringBuilder(Name).Append(" ").Append(Age).Append(" оценки:");
            foreach (int item in Marks)
            {
                temp.Append(" ").Append(item);
            }
            return  temp.ToString();
        }
    }
}
