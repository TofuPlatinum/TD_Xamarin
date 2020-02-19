using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using MapAndNotes.View;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MapAndNotes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new LoginView();
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
