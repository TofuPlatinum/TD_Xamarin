using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using MapAndNotes.Dtos;
using Storm.Mvvm;
using Xamarin.Forms;
using MapAndNotes.View;
using MapAndNotes.Models;
using MonkeyCache.SQLite;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace MapAndNotes.ViewModels
{
    class AjouterPlaceViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private string token;
        private Image _image;
        private byte[] _fileByte;


        public string Title
        {
            get { return _title; }
            set { SetProperty<string>(ref _title, value); }
        }
        public string Description
        {
            get { return _description; }
            set { SetProperty<string>(ref _description, value); }
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        private bool _enableButtons;
        public bool EnableButtons
        {
            get => _enableButtons;
            set => SetProperty(ref _enableButtons, value);
        }


        public ICommand AjouterCommand { get; set; }

        public ICommand GoPickPhotoCommand { get; }

        public ICommand GoTakePhotoCommand { get; }


        public AjouterPlaceViewModel()
        {
            EnableButtons = true;
            
            token = Barrel.Current.Get<string>(key: "AccessToken");

            AjouterCommand = new Command(OnSubmit);
            GoTakePhotoCommand = new Command(GoTakePhotoAction);
            GoPickPhotoCommand = new Command(GoPickPhotoAction);

        }

        public void OnSubmit()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Description))
            {
                MessagingCenter.Send(this, "Alert", "Les deux champs doivent etre remplie");
            }
            else
            {

                GoSavePlaceAction();

            }
        }
        private async void GoSavePlaceAction()
        {
            IsBusy = true;
            EnableButtons = false;

            // on essai de post l'image dans un premier temps 
            var res = await UploadImage();

            // l'image est uploadée
            if (res != null)
            {
                // on a reussit a poster l'image donc res contient un ImageItem
                CreatePlaceRequest data = new CreatePlaceRequest() { Title = this.Title, Description = this.Description, ImageId = res.Id, Latitude = 0, Longitude = 0 };
                createPlaceRequest(data);
            }
            EnableButtons = true;
            IsBusy = false;
        }

        public async void createPlaceRequest(CreatePlaceRequest data)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    data.Latitude = location.Latitude;
                    data.Longitude = location.Longitude;
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            
            ApiClient api = new ApiClient();
            string json = JsonConvert.SerializeObject(data);
            HttpResponseMessage response = await api.Execute(HttpMethod.Post, "https://td-api.julienmialon.com/places", data, token); 

            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new PlaceView());
                MessagingCenter.Send(this, "Alert", "Requete Reussi");
            }
            else
            {
                var content2 = await response.Content.ReadAsStringAsync();
                MessagingCenter.Send(this, "Alert", JsonConvert.DeserializeObject<Response>(content2).ErrorMessage.ToString());

            }
        }
        public async Task<ImageItem> UploadImage()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/images");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(_fileByte);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            requestContent.Add(imageContent, "file", "file.jpg");

            request.Content = requestContent;

            HttpResponseMessage response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MessagingCenter.Send(this, "Alert", content);
                ImageItem result = JsonConvert.DeserializeObject<Response<ImageItem>>(content).Data;
                return result;
            }
            else
            {
                MessagingCenter.Send(this, "Alert", "Impossible d'upload");
                return null;
            }
        }
        private async void GoPickPhotoAction()
        {
            IsBusy = true;
            PhotoService service = new PhotoService();
            var file = await service.PickPhoto();
            if (file != null)
            {
                _fileByte = service.FileMediaToByteArray(file);
                _image.Source = service.GetImageSource(file);
            }
            IsBusy = false;
        }

        private async void GoTakePhotoAction()
        {
            IsBusy = true;
            PhotoService service = new PhotoService();
            var file = await service.TakePhoto();
            if (file != null)
            {
                _fileByte = service.FileMediaToByteArray(file);
                _image.Source = service.GetImageSource(file);
            }
            IsBusy = false;
        }

    }
}
