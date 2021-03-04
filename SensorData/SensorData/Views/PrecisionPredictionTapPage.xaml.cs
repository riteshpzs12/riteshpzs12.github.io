using System;
using System.Collections.Generic;
using SensorData.ContainerHelper;
using SensorData.ViewModel.PrecisionPredictionTapPage;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class PrecisionPredictionTapPage : ContentPage
    {
        public PrecisionPredictionTapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            IIoCContainer container = Builder.Build();
            ViewModel = container.Resolve<IBaseViewModel>() as PrecisionPredictionTapPageViewModel;
        }

        public PrecisionPredictionTapPageViewModel ViewModel
        {
            get { return BindingContext as PrecisionPredictionTapPageViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            ViewModel.XTop = CheckParent.Width;
            ViewModel.YTop = CheckParent.Height;
            ViewModel.XPlay = PlayTest.Width;
            ViewModel.YPlay = PlayTest.Height;
            ViewModel.UpdateDotLocation();
        }
    }
}
