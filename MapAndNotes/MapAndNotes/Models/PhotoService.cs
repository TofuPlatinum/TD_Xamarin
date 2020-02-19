using Plugin.Media;
using Storm.Mvvm.Forms;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapAndNotes.Models
{
    class PhotoService
    {

        public async Task<Plugin.Media.Abstractions.MediaFile> PickPhoto(BaseContentPage page = null)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                //MessagingCenter.Send(this, "Alert", "Photos Not Supported", ":( Permission not granted to photos.");
                return null;
            }

            //Ouverture du fichier
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
            });

            if (file == null)
                return null;

            return file;
        }


        public async Task<Plugin.Media.Abstractions.MediaFile> TakePhoto(BaseContentPage page = null)
        {
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
               // App.DisplayMessageService.DisplayMessage(page, "Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return null;
            }

            // on ouvre l'appareil photo
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (file == null) return null;

            return file;
        }

        public byte[] FileMediaToByteArray(Plugin.Media.Abstractions.MediaFile file)
        {
            //On transforme le fichier en byte[]
            var memoryStream = new MemoryStream();
            file.GetStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public ImageSource GetImageSource(Plugin.Media.Abstractions.MediaFile file)
        {
            //Permet de recuperer l'image dans le xaml avec l'attribut Source de Image
            if (file != null)
            {
                return ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            else return null;
        }

    }
}
