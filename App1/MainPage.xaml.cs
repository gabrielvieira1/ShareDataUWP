using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      DataTransferManager dtm = DataTransferManager.GetForCurrentView();
      dtm.DataRequested += Dtm_DataRequested;
    }

    private void Dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
    {
      DataRequest request = args.Request;

      request.Data.Properties.Title = "Burger Truck";
      request.Data.Properties.Description = "Info about the Burger Truck";
      request.Data.SetText("Hello, World!");

      request.Data.SetDataProvider(StandardDataFormats.Html, OnDeferredDataRequestedHandler);

      request.Data.SetData("myappuniqueid", "Some custom data");
    }

    async void OnDeferredDataRequestedHandler(DataProviderRequest request)
    {
      DataProviderDeferral deferral = request.GetDeferral();
      await Task.Delay(3000);

      request.SetData("My very long text...");
      deferral.Complete();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      DataTransferManager dtm = DataTransferManager.GetForCurrentView();
      dtm.DataRequested -= Dtm_DataRequested;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      DataTransferManager.ShowShareUI();
    }
  }
}
