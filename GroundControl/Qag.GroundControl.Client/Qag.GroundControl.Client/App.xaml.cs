﻿namespace Qag.GroundControl.Client
{
    using System.Windows;

    public partial class App : Application
    {
        Bootstrapper bootstrapper;

        public App()
        {
            bootstrapper = new Bootstrapper();
        }
    }
}