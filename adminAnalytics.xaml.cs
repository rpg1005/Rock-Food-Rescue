using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace foodadmin
{
    public partial class Analytics : ContentPage
    {
        TodoItemManager manager;

        public Analytics()
        {
            InitializeComponent();

            manager = TodoItemManager.DefaultManager;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            enteredNum.IsVisible = false;
            newItemName.IsVisible = false;
            local.IsVisible = false;           
            overTime.IsVisible = false;
            startTime.IsVisible = false;            
        }

        // Data methods

        public async void Go_Clicked(object sender, EventArgs e)
        {
            int i = 0;
            ObservableCollection<TodoItem> taskList = await manager.GetAllTasksAsync();
            foreach (TodoItem item in taskList)
            {
                DateTime Create = item.Created.Date;
                if (Create.Date >= fromDate.Date && Create.Date <= toDate.Date)
                {
                    i++;
                }
                count.Text = i.ToString();
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