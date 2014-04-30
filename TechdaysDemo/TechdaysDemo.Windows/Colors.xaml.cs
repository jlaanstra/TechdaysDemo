using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ReactiveUI;
using TechdaysDemo.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TechdaysDemo
{
    public sealed partial class Colors : UserControl, IViewFor<MainViewModel>
    {
        public Colors()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(this.ViewModel, x => x.Red, x => x.Red.Text));
                d(this.Bind(this.ViewModel, x => x.Green, x => x.Green.Text));
                d(this.Bind(this.ViewModel, x => x.Blue, x => x.Blue.Text));

                d(this.OneWayBind(this.ViewModel, x => x.FinalColor, x => x.Color.Background));

                d(this.OneWayBind(this.ViewModel, x => x.FinalColor, x => x.SearchButton.CommandParameter));
                d(this.BindCommand(this.ViewModel, x => x.SearchCommand, x => x.SearchButton));
            });
        }

        public void Bind(Action<IDisposable> d)
        {
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainViewModel), typeof(Colors), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }
    }
}
