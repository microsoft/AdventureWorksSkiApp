using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp
{
    public class MainPageContext : INotifyPropertyChanged
    {
        private string _message;
        private int _tapCount;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }
        }

        public int TapCount
        {
            get
            {
                return _tapCount;
            }
            set
            {
                if (_tapCount != value)
                {
                    _tapCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TapCount"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public partial class MainPage : ContentPage
    {
        private MainPageContext context;

        public MainPage()
        {
            context = Init();
            BindingContext = context;
            InitializeComponent();
        }

        private MainPageContext Init()
        {
            return new MainPageContext
            {
                Message = "Hello!",
                TapCount = 0
            };
        }

        public void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            context.Message = "Tapping!";
            context.TapCount++;
        }
    }

}
