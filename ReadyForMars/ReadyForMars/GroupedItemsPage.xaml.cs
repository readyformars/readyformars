using ReadyForMars.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.ServiceModel.Dispatcher;
using Windows.UI.Core;
using Windows.System.Threading;
using Windows.UI.Popups;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace ReadyForMars
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage : ReadyForMars.Common.LayoutAwarePage
    {
        private DataRetriever retriever;





        public GroupedItemsPage()
        {
            this.InitializeComponent();
           this.retriever = new DataRetriever();

        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
           /*var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;*/


            
                        
          
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

             /*private async void Page_Loaded(object sender, RoutedEventArgs e)
              {
                  await this.retriever.GetFirstPage();
                  RecordMetadata rd = this.retriever.LastData;
                  //this.txtStatus.Text = rd.results[0].sol.ToString();
                  Display_Data(rd.results[0]);
              }*/
      
           private void Display_Data(Record results)
           {
                 this.sol_txt.Text = (results.sol.ToString() != null) ? "Sol " + results.sol.ToString() : "N/A";
                 this.max_value.Text = (results.max_temp.ToString() != null) ? results.max_temp.ToString() : "N/A";
                 this.min_value.Text = (results.min_temp.ToString() != null) ? results.min_temp.ToString() : "N/A";
                 this.pressure_value.Text = (results.pressure.ToString() != null) ? results.pressure.ToString() : "N/A";
                 this.pressure_str.Text = (results.pressure_string.ToString() != null) ? results.pressure_string.ToString() + "Than Nominal" : "N/A";
                 this.humidity_value.Text = (results.abs_humidity != null) ? results.abs_humidity : "N/A";
                 this.month.Text = (results.season != null) ? results.season : "N/A";
                 this.ls_value.Text = (results.ls != null) ? "LS " + results.ls.ToString() : "N/A";

                 if (results.terrestrial_date != null)
                 {
                     String[] longDate = results.terrestrial_date.GetDateTimeFormats('D');
                     this.terrestrial_date.Text = longDate[0].ToString();
                    
                 }
                 else
                 {
                     this.terrestrial_date.Text = "N/A";
                 }

                 if (results.wind_speed != null)
                 {
                     float? kmval = results.wind_speed * 3600 / 1000;
                     this.wind_speed_value.Text = kmval.ToString() + "Km/h";
                 }
                 else
                 {
                     this.wind_speed_value.Text = "N/A";
                 }


                 if (results.wind_direction != null)
                 {
                     if (results.wind_direction != null)
                     {
                         if (results.wind_direction == "E")
                         {
                             this.wind_dir_value.Text = "East";
                         }
                         else if (results.wind_direction == "S")
                         {
                             this.wind_dir_value.Text = "South";
                         }
                         else if (results.wind_direction == "W")
                         {
                             this.wind_dir_value.Text = "West";
                         }
                         else
                         {
                             this.wind_dir_value.Text = "North";
                         }
                     }

                 }
                 else
                 {
                     this.wind_dir_value.Text = "N/A";
                 }

                 if (results.sunrise != null)
                 {
                     String[] date = results.sunrise.Split('T');
                     DateTime sunrisef = DateTime.ParseExact(date[0], "yyyy-MM-dd", null);

                     string[] hour = date[1].Split('Z');
                     DateTime sunriseh = DateTime.ParseExact(hour[0], "HH:mm:ss", null);

                     TimeSpan ts = new TimeSpan(sunriseh.Hour, sunriseh.Minute, sunriseh.Second);
                     sunrisef = sunrisef.Date + ts;
                     String[] sunriseflongDate = sunrisef.GetDateTimeFormats('g');
                     this.sunrise_date.Text = sunriseflongDate[0].ToString();
                 }
                 else
                 {
                     this.sunrise_date.Text = "N/A";
                 }


                 if (results.sunset != null)
                 {
                     String[] sunsetdate = results.sunset.Split('T');
                     DateTime sunsetf = DateTime.ParseExact(sunsetdate[0], "yyyy-MM-dd", null);

                     string[] hour = sunsetdate[1].Split('Z');
                     DateTime sunseth = DateTime.ParseExact(hour[0], "HH:mm:ss", null);

                     TimeSpan ts = new TimeSpan(sunseth.Hour, sunseth.Minute, sunseth.Second);
                     sunsetf = sunsetf.Date + ts;
                     String[] sunsetflongDate = sunsetf.GetDateTimeFormats('g');
                     this.sunset_date.Text = sunsetflongDate[0].ToString();
                 }
                 else
                 {
                     this.sunset_date.Text = "N/A";
                 }
           }

           async private void pageRoot_Loaded(object sender, RoutedEventArgs e)
           {
               await this.retriever.GetFirstPage();
               RecordMetadata rd = this.retriever.LastData;
               //this.txtStatus.Text = rd.results[0].sol.ToString();
               Display_Data(rd.results[0]);

           }

           private void sol_txt_Tapped(object sender, TappedRoutedEventArgs e)
           {
               MessageDialog messageDialog = new MessageDialog("Clouds of water ice form high in the Martian air, driven by winds that can gust up to 50 mph on the surface.", "Weather");
               messageDialog.ShowAsync();
           }

           private void month_Tapped(object sender, TappedRoutedEventArgs e)
           {
               MessageDialog messageDialog = new MessageDialog("Yet coupled with the tenuous air and Mars' daily rotation, that energy produces some remarkable weather.", "Terrestrial Date");
               messageDialog.ShowAsync();
           }

           private void ls_value_Tapped(object sender, TappedRoutedEventArgs e)
           {
               MessageDialog messageDialog = new MessageDialog("The thin, chill blanket of the Martian atmosphere has an average density less than one-hundredth of the Earth's. At 142 million miles from the Sun, Mars receives less than half the solar energy that reaches the Earth.", "Mars Season");
               messageDialog.ShowAsync();
           }

    }
}
