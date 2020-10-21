using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Camera2Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewPage : ContentPage
    {
        public PreviewPage()
        {
            InitializeComponent();

            Imagem.Source = ImageSource.FromFile(Path.Combine(Path.GetTempPath(), "photo.jpg"));
            Imagem.Aspect = Aspect.AspectFit;
        }
    }
}