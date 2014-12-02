using CarouselControl.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CarouselControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            try
            {
                this.InitializeComponent();


                MyCarouselControl carouselControl = new MyCarouselControl()
                {
                    Images = new List<BitmapImage>() 
                {
                    new BitmapImage(new Uri("ms-appx:///Assets/office_ipad01.png", UriKind.Absolute)),
                    new BitmapImage(new Uri("ms-appx:///Assets/office_ipad02.png", UriKind.Absolute)),
                    new BitmapImage(new Uri("ms-appx:///Assets/office_ipad03.png", UriKind.Absolute))
                },
                    CycleTimeInSeconds = 5,
                    SelectedCarouselCircleColor = Colors.DimGray
                };

                container.Children.Add(carouselControl);
                carouselControl.StartRotation();
            }
            catch (Exception ex)
            {
                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(ex.Message);          
                dialog.ShowAsync();
            }
        }
    }
}
