﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCN : ContentPage
    {
        public PageCN()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}