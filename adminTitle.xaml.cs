using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace foodadmin
{
    public partial class Title : ContentPage
    {
        TodoItemManager manager;
        public Title()
        {
            InitializeComponent();
            manager = TodoItemManager.DefaultManager;
        }
        private async void Enter_Clicked(object sender, EventArgs e)
        {
            ObservableCollection<TodoItem> userList = await manager.GetLoginAsync();
            foreach (TodoItem item in userList)
            {
                if (username.Text == item.UserName && password.Text == item.PassWord)
                {
                    await Navigation.PushAsync(new TabbedPage1());
                }
                else
                {
                    await DisplayAlert("Alert", "Incorrect Username or Password", "OK");
                }
            }
        }
    }
}