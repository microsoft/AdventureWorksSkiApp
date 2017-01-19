using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.ViewModels
{
    class LiftDetailViewModel : BaseViewModel
    {
        private Lift lift { get; set; }
        public Lift Lift
        {
            get
            {
                return lift;
            }
            set
            {
                if (value != lift)
                {
                    lift = value;
                    OnPropertyChanged("Lift");
                }
            }
        }

        public LiftDetailViewModel(Lift lift)
        {
            Lift = lift;
        }
    }
}
