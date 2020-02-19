using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Storm.Mvvm;
using Newtonsoft.Json;
using MapAndNotes.Dtos;
using MapAndNotes.View;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MapAndNotes.Models;
using MonkeyCache.SQLite;
namespace MapAndNotes.ViewModels
{
    public class LoginModel : ViewModelBase
    {
        private string _username;
        private string _password;
		
        public string Username
        {
            get { return _username; }
            set { SetProperty<string>(ref _username, value); }
        }
		
        public string Password
        {
            get { return _password; }
            set { SetProperty<string>(ref _password, value); }
        }

        public ICommand SubmitCommand { get; set; }
        public ICommand VersInscriptionCommand { get; set; }


        public LoginModel()
        {
            SubmitCommand = new Command(OnSubmit);
            VersInscriptionCommand = new Command(VersInscription);
            Barrel.ApplicationId = "coucou";
        }
		
        public void VersInscription()
        {
			
            App.Current.MainPage.Navigation.PushModalAsync(new InscriptionView());
        }
		
        public void OnSubmit()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessagingCenter.Send(this, "LoginAlert", "Les champs ne peuvent etre vide");
            }
            else
            {
                 
                LoginRequest user = new LoginRequest() { Email = Username, Password = Password };
                LoginRequest(user);
              
            }
        }
		
        public async void LoginRequest(LoginRequest data)
        {
            HttpClient client = new HttpClient();
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/auth/login");
            
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                
                LoginResult result = JsonConvert.DeserializeObject<Response<LoginResult>>(content).Data;
         
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
