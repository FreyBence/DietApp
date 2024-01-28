using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Models
{
    public class User : INotifyPropertyChanged
    {
        private string name;
        private int age;
        private string sex;
        private int height;
        private int weight;
        private string workActivity;
        private string freeTimeActivity;

        public string Name
        {
            get { return name; }
            set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); }
        }

        public int Age
        {
            get { return age; }
            set { age = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Age")); }
        }
        public string Sex
        {
            get { return sex; }
            set { sex = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sex")); }
        }

        public int Height
        {
            get { return height; }
            set { height = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Height")); }
        }
        public int Weight
        {
            get { return weight; }
            set { weight = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Weight")); }
        }
        public string WorkActivity
        {
            get { return workActivity; }
            set
            {
                workActivity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WorkActivity"));
            }
        }
        public string FreeTimeActivity
        {
            get { return freeTimeActivity; }
            set
            {
                freeTimeActivity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FreeTimeActivity"));
            }
        }
        public User GetCopy()
        {
            return new User()
            {
                Name = this.Name,
                Age = this.Age,
                Sex = this.Sex,
                Height = this.Height,
                Weight = this.Weight,
                FreeTimeActivity = this.FreeTimeActivity,
                WorkActivity = this.WorkActivity,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
