using LiveCharts;
using System.Collections.Generic;
using System.ComponentModel;

namespace Warfare
{
    public class Sensor : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public LinkedList<Point> Points { get; set; }
        public ChartValues<Point> Values { get; set; }

        public void AddPoint(Point point)
        {
            if (Points.Count == 6000)
            {
                Points.RemoveLast();
            }

            if(Values.Count == 200)
            {
                Values.RemoveAt(0);
            }

            Points.AddFirst(point);
            Values.Add(point);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Points"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Values"));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
