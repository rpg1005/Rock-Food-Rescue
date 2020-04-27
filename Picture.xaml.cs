using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using FFImageLoading.Forms;
using FFImageLoading;

namespace rockfoodrescue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Picture : ContentPage
    {

        //View Picture
        public Picture(string url)
        {
            InitializeComponent();

            //Code adapted from https://github.com/luberda-molinet/FFImageLoading/wiki/Xamarin.Forms-API
            ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);

            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=foodphotostorage;AccountKey=yoG4v5Uhpd24PQO8EPvnTHyIFuNM8PKjvsxCiYwECaRADSWt+r9E36oVfk46sK90K9aOrq/45g3f4BHxPa3CRw==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("images");
            var name = url;
            var blockBlob = new CloudBlockBlob(new Uri(@name));
            var fileStream = new MemoryStream();
            blockBlob.DownloadToStreamAsync(fileStream);
            var image = ImageSource.FromStream(() => fileStream);
            var interval = TimeSpan.Zero;
            CachedImage cachedImage = null;

            cachedImage = new CachedImage()
            {
                Source = image,
                Aspect = Aspect.AspectFill,
                DownsampleToViewSize = true,
                MinimumHeightRequest = 300,
                MinimumWidthRequest = 300,
                HeightRequest = 300,
                WidthRequest = 300,
                DownsampleHeight = 300,
                DownsampleWidth = 300,
                DownsampleUseDipUnits = true,
                FadeAnimationEnabled = false,
                IsOpaque = false,
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalOptions = LayoutOptions.Center,
                RetryCount = 100,
                RetryDelay = 1000,
                ErrorPlaceholder = "logo.jpg",
                LoadingPlaceholder = "logo.jpg",
                CacheKeyFactory = new CustomCacheKeyFactory(),
                CacheDuration = interval
            };

            Button backBtn = new Button
            {
                Text = "Back",
                BackgroundColor = Color.FromHex("#FFCB0B"),
                Margin = 10
            };
            backBtn.Clicked += (sender, e) =>
            {
                ImageService.Instance.InvalidateCacheEntryAsync(cachedImage.CacheKeyFactory.GetKey(image, this.BindingContext), FFImageLoading.Cache.CacheType.All, true);
                Navigation.PopAsync();
                Navigation.PushAsync(new TodoList());
            };

            Button refreshBtn = new Button
            {
                Text = "Refresh",
                BackgroundColor = Color.FromHex("#FFCB0B"),
                Margin = 10
            };
            refreshBtn.Clicked += (sender, e) =>
            {
                ImageService.Instance.InvalidateCacheEntryAsync(cachedImage.CacheKeyFactory.GetKey(image, this.BindingContext), FFImageLoading.Cache.CacheType.All, true);
                Navigation.PopAsync();
                Navigation.PushAsync(new Picture(url));
            };

            Content = new StackLayout
            {
                BackgroundColor = Color.FromHex("#007055"),

                Children =
                {
                   cachedImage,
                   backBtn,
                   refreshBtn
                }
            };
        }
        public class CustomStreamImageSource : StreamImageSource
        {
            public string Key { get; set; }
        }

        public class CustomCacheKeyFactory : ICacheKeyFactory
        {
            public string GetKey(ImageSource imageSource, object bindingContext)
            {
                var keySource = imageSource as CustomStreamImageSource;

                if (keySource == null)
                    return null;

                return keySource.Key;
            }
        }
    }
}