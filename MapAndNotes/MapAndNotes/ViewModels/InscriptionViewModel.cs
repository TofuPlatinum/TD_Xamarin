using System;
using System.Collections.Generic;
using System.Text;
using Storm.Mvvm;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using MapAndNotes.Dtos;
using Xamarin.Forms;
using MonkeyCache.SQLite;
using MapAndNotes.View;
using MapAndNotes.Models;

namespace MapAndNotes.ViewModels
{
    class InscriptionViewModel : ViewModelBase
    {

        private string _email;
        private string _password;
        private string _firstName;
        private string _lastName;
        public string Email
        {
            get { return _email; }
            set { SetProperty<string>(ref _email, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty<string>(ref _password, value); }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty<string>(ref _firstName, value); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty<string>(ref _lastName, value); }
        }

        public ICommand InscriptionCommand { get; set; }

        public InscriptionViewModel()
        {
            InscriptionCommand = new Command(OnSubmit);
            Barrel.ApplicationId = "coucou";
        }

        public void OnSubmit()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                MessagingCenter.Send(this, "InscriptionAlert", "Les champs ne peuvent etre vide");
            }
            else
            {

                RegisterRequest user = new RegisterRequest() { Email = Email, Password = Password, LastName = LastName, FirstName = FirstName };
                RegisterRequest(user);

            }
        }
		
        public async void RegisterRequest(RegisterRequest data)
        {
            ApiClient api = new ApiClient();
            HttpResponseMessage response = await api.Execute(HttpMethod.Post, "https://td-api.julienmialon.com/auth/register",data,null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
				
                LoginResult result = JsonConvert.DeserializeObject<Response<LoginResult>>(content).Data;
              
                // MonkeyCache
          
                Barrel.Current.Add(key: "AccessToken", data: result.AccessToken, expireIn: TimeSpan.FromDays(result.ExpiresIn));
                Barrel.Current.Add(key: "RefreshToken", data: result.RefreshToken, expireIn: TimeSpan.FromDays(result.ExpiresIn));

                await App.Current.MainPage.Navigation.PushModalAsync(new PlaceView());
            }
            else
            {
                MessagingCenter.Send(this, "LoginAlert", "Echec");

            }
        }
    }
}
