using System;
using System.Windows.Input;
using SensorData.Services;
using Xamarin.Forms;

namespace SensorData.ViewModel.PrecisionPredictionTapPage
{
    public class PrecisionPredictionTapPageViewModel : BaseViewModel
    {
        INavService _navService;
        public double XTop = -1;
        public double YTop = -1;
        public double XPlay = -1;
        public double YPlay = -1;
        public PrecisionPredictionTapPageViewModel(INavService navService)
        {
            _navService = navService;
        }

        private double _XLocationFactor;
        public double XLocationFactor
        {
            get
            {
                return _XLocationFactor;
            }
            set
            {
                _XLocationFactor = value;
                OnPropertyChanged("XLocationFactor");
            }
        }

        public void UpdateDotLocation()
        {
            Random random = new Random();
            if(XTop!=-1 && YTop!=-1)
            {
                XLocationFactor = (random.NextDouble() * (.9) + .05)*XTop;
                YLocationFactor = (random.NextDouble() * (.9) + .05)*YTop;
            }
        }

        private double _YLocationFactor;
        public double YLocationFactor
        {
            get
            {
                return _YLocationFactor;
            }
            set
            {
                _YLocationFactor = value;
                OnPropertyChanged("YLocationFactor");
            }
        }

        public Command<Point> PrecisionCommand { get { return new Command<Point>((p) => CheckPrecision(p)); } }

        private void CheckPrecision(Point point)
        {
            double acc = CalculateAccuracy(point);
            //////Show the Accuracy
            _navService.ShowDialog("Accuracy", acc.ToString("#.00"));
            UpdateDotLocation();
        }

        private double CalculateAccuracy(Point point)
        {
            if(XTop != -1 && XPlay != -1 && YTop != -1 && YPlay != -1)
            {
                var x = XLocationFactor / XTop;
                var y = YLocationFactor / YTop;

                var userX = Math.Abs((point.X / XPlay) - x);
                var userY = Math.Abs((point.Y / YPlay) - y);
                _navService.ShowDialog("Horizontal Error", userX.ToString());
                _navService.ShowDialog("Vertical Error", userY.ToString());
                /////Some logic needed to find ou the percentage accuracy from this variance
                return 100 - (userX + userY) * 100;
            }
            return 0.0;
        }
    }
}
