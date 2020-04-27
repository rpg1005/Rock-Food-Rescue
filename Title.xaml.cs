using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rockfoodrescue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Title : ContentPage
    {
        public Title()
        {
            InitializeComponent();
        }
        private async void Enter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoList());
        }
    }
}