using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MapAndNotes.ViewModels;
using MapAndNotes.Dtos;

namespace MapAndNotes.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceView : ContentPage
    {
        private PlaceViewModel placeViewModel;
        public PlaceView()
        {
            InitializeComponent();
            placeViewModel = new PlaceViewModel();
            MessagingCenter.Subscribe<PlaceViewModel, string>(this, "PlaceAlert", (sender, msg) =>
            {
                DisplayAlert("Infos", msg, "Ok");
            });
            this.BindingContext = placeViewModel;

        }
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            // la vue ne devrais pas faire ca..  
            PlaceItemSummary item = e.SelectedItem as PlaceItemSummary;
            Navigation.PushModalAsync(new NavigationPage(new DetailView(item)));
            //App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new DetailView(item)));
        }
    }
}