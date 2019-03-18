using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Warfare
{
    public class SensorViewModel : INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;

        private Sensor _currentSensor;
        private ObservableCollection<Sensor> _sensors;

        public ICommand BackCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand SelectSensorCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        
        public SensorViewModel()
        {
            Sensors = new ObservableCollection<Sensor>
            {
                new Sensor() { Name = "Sensor #1", Points = new LinkedList<Point>(), Values = new ChartValues<Point>() },
                new Sensor() { Name = "Sensor #2", Points = new LinkedList<Point>(), Values = new ChartValues<Point>() },
                new Sensor() { Name = "Sensor #3", Points = new LinkedList<Point>(), Values = new ChartValues<Point>() }
            };

            foreach(var sensor in Sensors)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(Update));
                thread.Start(sensor);
            }

            CartesianMapper<Point> mapper = Mappers.Xy<Point>().X(p => p.Time.Ticks).Y(p => p.Magnitude);
            Charting.For<Point>(mapper);

            XFormatter = v => new DateTime((long)v).ToString("s.fff");
            YFormatter = v => v.ToString();

            AxisStep = TimeSpan.FromMilliseconds(200).Ticks;
            AxisUnit = TimeSpan.TicksPerMillisecond;
            
            BackCommand = new DefaultCommand(BackCommandHandler);
            CloseCommand = new DefaultCommand(CloseCommandHandler);
            SelectSensorCommand = new DefaultCommand<Sensor>(SelectSensorCommandHandler);
        }

        private async void Update(object obj)
        {
            Sensor sensor = obj as Sensor;
            
            Random rnd = new Random();
            DateTime time = DateTime.Now;

            while (true)
            {
                time = time.AddMilliseconds(10);
                sensor.AddPoint(new Point { Time = time, Magnitude = NextGaussian(rnd) });

                if (CurrentSensor == sensor)
                {
                    SetAxisLimits(time);
                }

                await Task.Delay(10);
            }
        }

        private double NextGaussian(Random r, double mean = 0, double sigma = 1)
        {
            var num1 = r.NextDouble();
            var num2 = r.NextDouble();

            var randStandardNormal = Math.Sqrt(-2.0 * Math.Log(num1)) * Math.Sin(2.0 * Math.PI * num2);
            var randNormal = mean + sigma * randStandardNormal;

            return randNormal;
        }

        public void BackCommandHandler()
        {
            CurrentSensor = null;
        }

        public void CloseCommandHandler()
        {
            App.Current.MainWindow.Close();
        }

        public void SelectSensorCommandHandler(Sensor sensor)
        {
            CurrentSensor = sensor;
        }

        public void SetAxisLimits(DateTime time)
        {
            AxisMax = time.Ticks + TimeSpan.FromMilliseconds(100).Ticks;
            AxisMin = time.Ticks - TimeSpan.FromMilliseconds(1500).Ticks;
        }

        public double AxisMax
        {
            get => _axisMax;
            set
            {
                _axisMax = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AxisMax)));
            }
        }

        public double AxisMin
        {
            get => _axisMin;
            set
            {
                _axisMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AxisMin)));
            }
        }

        public ObservableCollection<Sensor> Sensors
        {
            get => _sensors;
            set
            {
                _sensors = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sensors)));
            }
        }

        public Sensor CurrentSensor
        {
            get => _currentSensor;
            set
            {
                _currentSensor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSensor)));
            }
        }
    }
}
