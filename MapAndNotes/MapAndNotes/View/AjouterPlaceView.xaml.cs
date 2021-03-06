﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MapAndNotes.ViewModels;
namespace MapAndNotes.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AjouterPlaceView : ContentPage
    {
        private AjouterPlaceViewModel ajouterPlaceViewModel;
        public AjouterPlaceView()
        {
            InitializeComponent();
            ajouterPlaceViewModel = new AjouterPlaceViewModel();
            MessagingCenter.Subscribe<AjouterPlaceViewModel, string>(this, "Alert", (sender, msg) =>
            {
                DisplayAlert("Infos", msg, "Ok");
            });
            this.BindingContext = ajouterPlaceViewModel;
        }
    }
}