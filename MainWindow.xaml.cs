using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Proiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new Home();

        }

        private void HomeClick(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Home();
        }

        private void CazariClick(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new PageCazari();
        }

        private void RezervariClick(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new PageRezervari();
        }

        private void ClientiClick(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new PageClienti();
        }

        private void TransportClick(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new PageTransport();
        }
    }
}
