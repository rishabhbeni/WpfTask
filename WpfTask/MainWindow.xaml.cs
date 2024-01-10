using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(MainWindow), new FrameworkPropertyMetadata(OnHTMLChanged));
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string myHTML = "";
            Debug.WriteLine($"Thread No: {Thread.CurrentThread.ManagedThreadId} before run");
            await Task.Run(async () =>
            {
                HttpClient webClient = new HttpClient();
                string html = webClient.GetStringAsync("http://google.com/").Result;
                Debug.WriteLine($"Thread No: {Thread.CurrentThread.ManagedThreadId} during run");
                myHTML = html;
            });
            Debug.WriteLine($"Thread No: {Thread.CurrentThread.ManagedThreadId} after run");
            myButton.Content = "Done";
            myWebBrowser.SetValue(HtmlProperty, myHTML);
        }

        static void OnHTMLChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser webBrowser = dependencyObject as WebBrowser;
            if (webBrowser != null)
            {
                webBrowser.NavigateToString(e.NewValue as string);
            }
        }
    }
}
