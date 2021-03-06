﻿using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace rockfoodrescue
{
    //Get messages
    public partial class TodoList : ContentPage
    {
        TodoItemManager manager;

        public TodoList()
        {
            InitializeComponent();

            manager = TodoItemManager.DefaultManager;
        //    if (Device.RuntimePlatform == Device.UWP)
        //    {
        //        var refreshButton = new Button
        //        {
        //            Text = "Refresh",
        //            HeightRequest = 30
        //        };
        //        refreshButton.Clicked += OnRefreshItems;
        //        buttonsPanel.Children.Add(refreshButton);
        //        if (manager.IsOfflineEnabled)
        //        {
        //            var syncButton = new Button
        //            {
        //                Text = "Sync items",
        //                HeightRequest = 30
        //            };
        //            syncButton.Clicked += OnSyncItems;
        //            buttonsPanel.Children.Add(syncButton);
        //        }
        //    }
        }
        void Map_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("http://rockpride.sru.edu/map/"));
        }
        private async void Text_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TextList());
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            newItemName.IsVisible = false;
            overDate.IsVisible = false;
            local.IsVisible = false;
            startDate.IsVisible = false;
            overTime.IsVisible = false;
            startTime.IsVisible = false;

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true, syncItems: true);
        }

        // Data methods
        async Task AddItem(TodoItem item)
        {
            await manager.SaveTaskAsync(item);
            todoList.ItemsSource = await manager.GetTodoItemsAsync();
        }

        async Task CompleteItem(TodoItem item)
        {           
            todoList.ItemsSource = await manager.GetTodoItemsAsync();
        }

        //Get messages
        public async void OnAdd(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            var todo = new TodoItem { Name = newItemName.Text, Over = now, Start = now, Place = local.Text };
            //await AddItem(todo);

            newItemName.Text = string.Empty;
            local.Text = string.Empty;
            overDate.Date = DateTime.Now;
            overTime.Time = DateTime.Now.TimeOfDay;
            startDate.Date = DateTime.Now;
            startTime.Time = DateTime.Now.TimeOfDay;
            newItemName.Unfocus();
            local.Unfocus();
            todoList.ItemsSource = await manager.GetTodoItemsAsync();
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todo = e.SelectedItem as TodoItem;
            if (Device.RuntimePlatform != Device.iOS && todo != null)
            {
                // Not iOS - the swipe-to-delete is discoverable there
                if (Device.RuntimePlatform == Device.Android)
                {
                    await DisplayAlert(todo.Name, "Press-and-hold to complete task " + todo.Name, "Got it!");
                }
                else
                {
                    // Windows, not all platforms support the Context Actions yet
                    if (await DisplayAlert("Mark completed?", "Do you wish to complete " + todo.Name + "?", "Complete", "Cancel"))
                    {
                        await CompleteItem(todo);
                    }
                }
            }

            // prevents background getting highlighted
            todoList.SelectedItem = null;
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        
        //Options Menu
        public void OnComplete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var todo = mi.CommandParameter as TodoItem;
            //Button deleteBtn = new Button
            //{
            //    Text = "Delete Post",
            //    BackgroundColor = Color.FromHex("#FFCB0B"),
            //    Margin = 10
            //};

            //deleteBtn.Clicked += (async (sender1, e1) => await CompleteItem(todo));

            Button photoBtn = new Button
            {
                Text = "View Picture",
                BackgroundColor = Color.FromHex("#FFCB0B"),
                Margin = 10
            };
            photoBtn.Clicked += (sender2, e2) => DownloadImage(todo.Image);

            Button backBtn = new Button
            {
                Text = "Back",
                BackgroundColor = Color.FromHex("#FFCB0B"),
                Margin = 10
            };
            backBtn.Clicked += (sender3, e3) => Navigation.PushAsync(new TodoList());

            Image seal = new Image
            {
                Source = "srumodernseal.png",
                BackgroundColor = Color.White,
                Margin = 70
            };

            Content = new StackLayout
            {
                BackgroundColor = Color.FromHex("#007055"),

                Children =
                {
                   //deleteBtn,
                   photoBtn,
                   backBtn,
                   seal
                }
            };
        }
        public async void DownloadImage(string url)
        {
            await Navigation.PushAsync(new Picture(url));
        }

    // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
    public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        public async void OnRefreshItems(object sender, EventArgs e)
        {
            await RefreshItems(true, false);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                todoList.ItemsSource = await manager.GetTodoItemsAsync(syncItems);
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}

