using System;
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
    public partial class InscriptionView : ContentPage
    {
        private InscriptionViewModel inscriptionViewModel;
        public InscriptionView()
        {
            InitializeComponent();
            inscriptionViewModel = new InscriptionViewModel();
            MessagingCenter.Subscribe<InscriptionViewModel, string>(this, "InscriptionAlert", (sender, msg) =>
            {

                DisplayAlert("Erreur", msg, "Ok");
            });
            this.BindingContext = inscriptionViewModel;
            emailEntry.Completed += (object sender, EventArgs e) => {
                passwordEntry.Focus();
            };
            passwordEntry.Completed += (object sender, EventArgs e) => {
                firstNameEntry.Focus();
            };
            firstNameEntry.Completed += (object sender, EventArgs e) => {
                lastNameEntry.Focus();
            };
            lastNameEntry.Completed += (object sender, EventArgs e) =>
            {
                inscriptionViewModel.InscriptionCommand.Execute(null);
            };
        }
    }
}