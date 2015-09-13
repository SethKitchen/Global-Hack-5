using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Template
{
    public sealed partial class Payment : Page
    {
        MainPage rootPage = MainPage.Current;

        public Payment()
        {
            this.InitializeComponent();
            paypalPic.Source= new BitmapImage(new Uri("ms-appx:///Assets/PayPal_2014_logo.svg.png"));
        }

        public async void checkCitationsForPrices()
        {
            // Get the file.
            StorageFolder install = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var citationNumber = await install.OpenStreamForReadAsync(@"Assets\violations.csv");

            // Read the data.
            using (StreamReader streamReader = new StreamReader(citationNumber))
            {
                string line = "";
                string[] partsOfLine = null;
                bool hasCaughtID = false;
                while (!streamReader.EndOfStream && !hasCaughtID)
                {
                    if (streamReader.EndOfStream)
                    {
                        MessageDialog msgbox = new MessageDialog("Citation Number not found!", "Court Connection");

                        //msgbox.Commands.Clear();
                        //msgbox.Commands.Add(new UICommand { Label = "OK", Id = 0 });

                        var res = await msgbox.ShowAsync();

                        /*
                        if ((int)res.Id == 0)
                        {
                            MessageDialog msgbox2 = new MessageDialog("Hello to you too! :)", "User Response");
                            await msgbox2.ShowAsync();
                        }

                        if ((int)res.Id == 1)
                        {
                            MessageDialog msgbox2 = new MessageDialog("Oh well, too bad! :(", "User Response");
                            await msgbox2.ShowAsync();
                        }

                        if ((int)res.Id == 2)
                        {
                            MessageDialog msgbox2 = new MessageDialog("Nevermind then... :|", "User Response");
                            await msgbox2.ShowAsync();
                        }*/
                        return;
                    }
                    line = streamReader.ReadLine();
                    partsOfLine = line.Split(',');
                    if (partsOfLine[1].Equals(citationID.Text))
                    {
                        float tickPrice = float.Parse(partsOfLine[8].Replace("$", ""));
                        float totalPrice = float.Parse(partsOfLine[9].Replace("$", ""))+tickPrice;
                        priceBox.Text = totalPrice + "";
                        hasCaughtID = true;
                        MessageDialog msgbox = new MessageDialog("We have sent an email requesting money through Paypal.", "Court Connection");

                        //msgbox.Commands.Clear();
                        //msgbox.Commands.Add(new UICommand { Label = "OK", Id = 0 });

                        var res = await msgbox.ShowAsync();
                    }
                }
            }
            citationNumber.Dispose();
        }

        private void submitBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            checkCitationsForPrices();
        }
    }
}
