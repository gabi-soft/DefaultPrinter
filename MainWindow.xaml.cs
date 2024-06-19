using System.Management;
using System.Runtime.InteropServices;
using System.Windows;

namespace DefaultPrinter;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        var defaultPrinter = "";
        List<string> printers = [];
        var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

        foreach (var printer in printerQuery.Get())
        {
            var name = (string) printer.GetPropertyValue("Name");
            if ((bool) printer.GetPropertyValue("Default"))
                defaultPrinter = name;
            printers.Add(name);
        }

        Printers.ItemsSource = printers;
        Printers.SelectedValue = defaultPrinter;
        Valider.Click += ValidateOnClick;

        void ValidateOnClick(object sender, RoutedEventArgs e)
        {
            SetDefaultPrinter(Printers.SelectedValue.ToString() ?? "");
            Application.Current.Shutdown();
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDefaultPrinter(string printer);
    }
}