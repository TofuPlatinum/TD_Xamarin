using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Text;
using MapAndNotes.Dtos;
using Storm.Mvvm;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using MonkeyCache.SQLite;

using MapAndNotes.Models;
using MapAndNotes.View;

namespace MapAndNotes.ViewModels
{
    class PlaceViewModel : ViewModelBase
    {
        private string token;
        private ObservableCollection<PlaceItemSummary> _places;
      
        public ObservableCollection<PlaceItemSummary> Places
        {
            get { return _places; }
            set { SetProperty<ObservableCollection<PlaceItemSummary>>(ref _places, value); }
        }


        public ICommand AjouterCommand { get; set; }

        public PlaceViewModel()
        {
            token = Barrel.Current.Get<string>(key: "AccessToken");

            AjouterCommand = new Command(Ajouter);
            Places = new ObservableCollection<PlaceItemSummary>();
            GetPlacesRequest2();
        }

        public void Ajouter()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new AjouterPlaceView());
        }
        public async void GetPlacesRequest()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://td-api.julienmialon.com/places");

            
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<PlaceItemSummary> result = JsonConvert.DeserializeObject<Payload>(content).data;
                int i = 0;
                foreach (var item in result)
                {
                    i++;
                    Places.Add(item);
                }
            }
            else
            {
                MessagingCenter.Send(this, "PlaceAlert", "Echec de la requete vers l'API");

            }
        }
        class Payload
        {
            public List<PlaceItemSummary> data { get; set; }
        }
        public async void GetPlacesRequest2()
        {
            ApiClient api = new ApiClient();
            HttpResponseMessage response = await api.Execute(HttpMethod.Get, "https://td-api.julienmialon.com/places", null,token);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<PlaceItemSummary> result = JsonConvert.DeserializeObject<Payload>(content).data;
                int i = 0;
                foreach (var item in result)
                {
                    i++;
                    Places.Add(item);
                }
            }
            else
            {
                MessagingCenter.Send(this, "PlaceAlert", "Echec de la requete vers l'API");

            }
        }
    }
}
