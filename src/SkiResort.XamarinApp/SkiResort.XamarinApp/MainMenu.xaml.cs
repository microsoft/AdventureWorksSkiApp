﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp
{
    public partial class MainMenu : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public MainMenu()
        {
            InitializeComponent();
            BackgroundColor = Color.FromHex("#141414");

            var masterPageItems = new List<MainMenuItem>();
            masterPageItems.Add(new MainMenuItem
            {
                Title = "Lift Status",
            });
            masterPageItems.Add(new MainMenuItem
            {
                Title = "Rental Reservation",
            });
            masterPageItems.Add(new MainMenuItem
            {
                Title = "Dining",
            });
            masterPageItems.Add(new MainMenuItem
            {
                Title = "Live Webcams",
            });
            masterPageItems.Add(new MainMenuItem
            {
                Title = "Lift Tickets",
            });

            listView.ItemsSource = masterPageItems;
        }
    }

    public class MainMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
    }
}
