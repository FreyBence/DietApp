using System.ComponentModel;

namespace DietAppClient.Models
{
    public class Record : INotifyPropertyChanged
    {
        private string id;
        private double? calories;
        private DateTime date;

        public string Id
        {
            get { return id; }
            set
            {
                id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
            }
        }

        public double? Calories
        {
            get { return calories; }
            set
            {
                calories = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Calories"));
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }

        public Record()
        {
            Date = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }

        public Record GetCopy()
        {
            return new Record()
            {
                Id = this.Id,
                Calories = this.Calories,
                Date = this.Date
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
