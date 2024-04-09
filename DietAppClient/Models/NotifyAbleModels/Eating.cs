using System.ComponentModel;

namespace DietAppClient.Models
{
    public class Eating : INotifyPropertyChanged
    {
        private string id;
        private string food;
        private int? fat;
        private int? protein;
        private int? carbohydrate;
        private DateTime date;


        public string Id
        {
            get { return id; }
            set { id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id")); }
        }
        public string Food
        {
            get { return food; }
            set { food = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Food")); }
        }
        public int? Fat
        {
            get { return fat; }
            set { fat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fat")); }
        }
        public int? Protein
        {
            get { return protein; }
            set { protein = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Protein")); }
        }
        public int? Carbohydrate
        {
            get { return carbohydrate; }
            set { carbohydrate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Carbohydrate")); }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date")); }
        }

        public Eating()
        {
            Date = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }

        public Eating GetCopy()
        {
            return new Eating()
            {
                Food = this.Food,
                Fat = this.Fat,
                Protein = this.Protein,
                Carbohydrate = this.Carbohydrate,
                Date = this.Date,
                Id = this.Id
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
