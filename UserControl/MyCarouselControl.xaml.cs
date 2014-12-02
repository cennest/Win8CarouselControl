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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace CarouselControl.UserControls
{
    public sealed partial class MyCarouselControl : UserControl
    {
        private const string LeftNavigationButton = "LeftNavButton";
        //temp
        private Double initialOffset;
        private int breakRotation;
        private int SlideCount;

        public List<BitmapImage> Images { get; set; }
        public int CycleTimeInSeconds { get; set; }

        public ResourceDictionary ResourceDictionary { get; set; }
        public string CarouselCircleResourceKey { get; set; }
        public Color SelectedCarouselCircleColor { get; set; }

        public MyCarouselControl()
        {
            this.InitializeComponent();

            Images = new List<BitmapImage>();
            this.ResourceDictionary = new ResourceDictionary();
            
            RotatingImage.PointerPressed += RotatingImage_PointerPressed;
            RotatingImage.PointerReleased += RotatingImage_PointerReleased;
        }

        public void StartRotation()
        {
            try
            {
                if (Images.Count > 0)
                {
                    if (SelectedCarouselCircleColor != Color.FromArgb(0,0,0,0) && CycleTimeInSeconds >= 0)
                    { 
                        ApplyStyleToCarouselCircles();
                        RotateImages();
                    }
                    else
                    {
                        throw new Exception("Please provide Color for selected carousel circle or cycle time in seconds");
                    }
                }
                else
                {
                    throw new Exception("Please add images to the List");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
                    
        private void ApplyStyleToCarouselCircles()
        {
            if (this.ResourceDictionary == null || string.IsNullOrEmpty(this.CarouselCircleResourceKey))
            {
                //here I have added default style
                this.ResourceDictionary.Source = new Uri("ms-appx:///Common/StyleResource.xaml", UriKind.Absolute);

                if (Images.Count > 0)
                {
                    for (int count = 0; count < Images.Count; count++)
                    {
                        //Adding circles dyamically
                        CarouselCircles.Items.Add(
                            new Ellipse
                            {
                                //here I  dynamicly assigning the style 
                                Style = this.ResourceDictionary["CarouselCircle"] as Style
                            });
                    }
                }
            }
            else
            {
                if (Images.Count > 0)
                {
                    for (int count = 0; count < Images.Count; count++)
                    {
                        //Adding circles dyamically
                        CarouselCircles.Items.Add(
                            new Ellipse
                            {
                                //here I  dynamicly assigning the style 
                                Style = this.ResourceDictionary[this.CarouselCircleResourceKey] as Style
                            });
                    }
                }
            }
        }

        /// <summary>
        /// Invoked when user click on Image.
        /// </summary>
        /// <param name="e">Event data that describes that image clicked was released.  The Parameter
        /// property is typically used to Get the pointer position where user last interacted with Image.</param>
        private void RotatingImage_PointerReleased(object sender, PointerRoutedEventArgs args)
        {
            try
            {
                // Minimum amount to declare as a manipulation
                const int moveThreshold = 40;

                // last position
                var clientX = args.GetCurrentPoint(this).Position.X;

                // Here is a "Tap on Item"
                if (!(Math.Abs(clientX - initialOffset) > moveThreshold))
                    return;

                // Here is a manipulation 
                if (clientX < initialOffset)
                {
                    //on swiping right to left
                    this.SlideCount = (this.SlideCount < (this.Images.Count - 1))
                                             ? this.SlideCount + 1
                                             : 0;
                }
                else
                {
                    //on swiping left to right
                    if (this.SlideCount > 0)
                    {
                        this.SlideCount--;
                        Ellipse previous = (Ellipse)CarouselCircles.Items[this.SlideCount + 1];
                        previous.Fill = new SolidColorBrush { Color = Colors.Transparent };
                    }
                    else if (this.SlideCount == 0)
                    {
                        Ellipse previous = (Ellipse)CarouselCircles.Items[0];
                        previous.Fill = new SolidColorBrush { Color = Colors.Transparent };
                        this.SlideCount = Images.Count - 1;
                    }
                }

                initialOffset = clientX;
                breakRotation++;
                RotateImages();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Invoked when user click on Image.
        /// </summary>
        /// <param name="e">Event data that describes that image was clicked.  The Parameter
        /// property is typically used to Get the pointer position where user clicked .</param>
        private void RotatingImage_PointerPressed(object sender, PointerRoutedEventArgs args)
        {
            //gets initial point of interaction
            initialOffset = args.GetCurrentPoint(this).Position.X;
        }

        /// <summary>
        /// Invoked when user click on Left or Right Navigating Button.
        /// </summary>
        /// <param name="e">Event data that describes that navigation button was clicked.  The Parameter
        /// property is typically used to identify which button was clicked .</param>
        private void LeftOrRightNavigationNavButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button navButton = sender as Button;
                if (navButton.Name.ToLower() == LeftNavigationButton.ToLower())
                {
                    if (this.SlideCount == 0)
                    {
                        this.SlideCount = Images.Count - 1;
                        Ellipse previous = (Ellipse)CarouselCircles.Items[0];
                        previous.Fill = new SolidColorBrush { Color = Colors.Transparent };
                    }
                    else
                    {
                        this.SlideCount--;
                        Ellipse previous = (Ellipse)CarouselCircles.Items[this.SlideCount + 1];
                        previous.Fill = new SolidColorBrush { Color = Colors.Transparent };
                    }
                }
                else
                {
                    if (this.SlideCount >= Images.Count - 1)
                    {
                        this.SlideCount = 0;
                    }
                    else
                    {
                        this.SlideCount++;
                    }
                }
                breakRotation++;
                RotateImages();
            }
            catch (Exception ex)
            {
            }
        }

        //Getting Or Returning Image at Index specified
        private BitmapImage GetImageAtIndex(int index)
        {

            try
            {
                int current = index;

                if (current >= Images.Count)
                {
                    current = 0;
                    SlideCount = 0;
                }

                return Images[current];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void RotateImages()
        {
            try
            {
                //assigning true for continous rotating;
                while (true)
                {
                    if (SlideCount == 0)
                    {
                        //assign last circle with transparent value
                        Ellipse ellipse = (Ellipse)CarouselCircles.Items[Images.Count - 1];
                        ellipse.Fill = new SolidColorBrush { Color = Colors.Transparent };
                    }
                    else
                    {
                        //assign previous circle with transparent value
                        Ellipse previous = (Ellipse)CarouselCircles.Items[SlideCount - 1];
                        previous.Fill = new SolidColorBrush { Color = Colors.Transparent };
                    }

                    //Fills current circle with color
                    Ellipse current = (Ellipse)CarouselCircles.Items[this.SlideCount];
                    current.Fill = new SolidColorBrush { Color = this.SelectedCarouselCircleColor };

                    //Changes image source
                    RotatingImage.Source = GetImageAtIndex(SlideCount);

                    //Stops excution for specified seconds before changing
                    TimeSpan time = TimeSpan.FromSeconds(CycleTimeInSeconds);
                    await System.Threading.Tasks.Task.Delay(time);

                    //stops excution
                    if (breakRotation > 0)
                    {
                        breakRotation--;
                        break;
                    }
                    else
                    {
                        if (SlideCount == Images.Count - 1)
                        {
                            SlideCount = 0;
                        }
                        else
                        {
                            SlideCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
      
    }
}
