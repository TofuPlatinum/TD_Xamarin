using System;
using System.Collections.Generic;
using System.Text;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using MapAndNotes.View;
using MapAndNotes.Dtos;
using System.Windows.Input;

namespace MapAndNotes.ViewModels
{
    class DetailViewModel : ViewModelBase
    {
        private PlaceItemSummary _place;
        public PlaceItemSummary Place
        {
            get { return _place; }
            set { SetProperty<PlaceItemSummary>(ref _place, value); }
        }
        private string _imageURL;
        public string ImageURL
        {
            get { return _imageURL; }
            set { SetProperty<string>(ref _imageURL, value); }
        }
        public DetailViewModel(PlaceItemSummary place)
        {
            Place = place;
            ImageURL = "https://td-api.julienmialon.com/images/" + place.ImageId;
            GoToMapCommand = new Command(goToMap);

            
        }

        public ICommand GoToMapCommand { get; set; }

        private void goToMap()
        {
            Position position = new Position(Place.Latitude, Place.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            Map map = new Map(mapSpan);
            map.IsShowingUser = true;

            var pin = new Pin()
            {
                Position = new Position(Place.Latitude, Place.Longitude),
                Label = Place.Title
            };
            map.Pins.Add(pin);

            var rootPage = new ContentPage();
            rootPage.Content = map;
            App.Current.MainPage.Navigation.PushModalAsync(rootPage);
        }
    }
}
