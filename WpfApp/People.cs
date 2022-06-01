using System;

namespace WpfApp
{
    [Serializable]
    public class People
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public static bool operator == (People people1, People people2)
        {
            return people1.Date == people2.Date 
                && people1.Name == people2.Name 
                && people1.Surname == people2.Surname 
                && people1.Patronymic == people2.Patronymic 
                && people1.City == people2.City 
                && people1.Country == people2.Country;
        }
        public static bool operator != (People people1, People people2)
        {
            return people1.Date != people2.Date
                || people1.Name != people2.Name
                || people1.Surname != people2.Surname
                || people1.Patronymic != people2.Patronymic
                || people1.City != people2.City
                || people1.Country != people2.Country;
        }
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
