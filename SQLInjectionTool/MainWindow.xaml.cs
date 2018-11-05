using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace HakowankoPlanII
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static readonly HttpClient client = new HttpClient();

        private bool run = false;


        public MainWindow()
        {
            InitializeComponent();

        }

        public async void GetData()
        {
            try
            {
                int iterator = 1;

                while (run)
                {
                    string response = await client.GetStringAsync($"http://plan.ii.us.edu.pl/left_menu_feed.php?type=1&branch=8401 UNION {InputQuery.Text} LIMIT {iterator}, 1&link=1");

                    string output = trimName(response);

                    if (string.IsNullOrWhiteSpace(output)) break;

                    QueryText.Text += trimName(response) + Environment.NewLine;

                    ++iterator;
                }

                StatusText.Content = "Finished";
                StatusText.Foreground = Brushes.Purple;
                run = false;

            }
            catch (Exception e)
            {
                QueryText.Text = e.Message;
                StatusText.Content = "Finished";
                StatusText.Foreground = Brushes.Purple;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!run)
            {
                QueryText.Text = string.Empty;

                StatusText.Content = "Running";
                StatusText.Foreground = Brushes.Green;

                run = true;
                GetData();
            }
            else
            {
                run = false;
            }

        }

        private string trimName(string input)
        {
            var a = input.Substring(0, input.IndexOf("</a></span>"));

            var b = a.Substring(a.LastIndexOf(">") + 1);

            return b;
        }
    }
}
