﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CameraSample.iOS
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            
            InitializeComponent();

            MainPage = new CameraSample.iOS.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
