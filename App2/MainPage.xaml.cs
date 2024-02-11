using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
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

namespace App2
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
      doneBtn.IsEnabled = false;
    }

    ShareOperation _Operation;
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      _Operation = e.Parameter as ShareOperation;

      Task.Factory.StartNew(async () =>
      {
        string text = null, html = null;
        if (_Operation.Data.Contains(StandardDataFormats.Text))
        {
          text = await _Operation.Data.GetTextAsync();
        }
        if (_Operation.Data.Contains(StandardDataFormats.Html))
        {
          html = await _Operation.Data.GetHtmlFormatAsync();
        }

        string custom = null;
        if (_Operation.Data.Contains("myappuniqueid"))
        {
          custom = (string)await _Operation.Data.GetDataAsync("myappuniqueid");
        }

        _Operation.ReportDataRetrieved();

        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
          doneBtn.IsEnabled = true;
          this.Text1.Text = text ?? "";
          this.Text2.Text = html ?? "";
          this.Text3.Text = custom ?? "";
        });
      });
    }

    private void doneBtn_Click(object sender, RoutedEventArgs e)
    {
      _Operation?.ReportCompleted();
    }
  }
}
