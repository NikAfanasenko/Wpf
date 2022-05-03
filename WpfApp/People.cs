using System;

namespace WpfApp
{
    public class People
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public People(string information)
        {
            string[] data = information.Split(';');
            Date = new DateTime(int.Parse(data[0].Substring(0, 4)), int.Parse(data[0].Substring(4, 2)), int.Parse(data[0].Substring(4, 2)));
            Name = data[1];
            Surname = data[2];
            Patronymic = data[3];
            City = data[4];
            Country = data[5];
        }
    }
}
