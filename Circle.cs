using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Fourier
{
    public class Circle : INotifyPropertyChanged
    {
        private double radius;
        private double time;

        public double Radius
        {
            get => radius;
            //ustawiamy recznie setter i wolamy propertychanged
            set
            {
                if (radius != value)
                {
                    radius = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Time
        {
            get => time;
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
