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
    public partial class DetailView : ContentPage
    {
        private DetailViewModel detailViewModel;
        public DetailView(PlaceItemSummary place)
        {
            InitializeComponent();
            detailViewModel = new DetailViewModel(place);
            this.BindingContext = detailViewModel;

        }
    }
}