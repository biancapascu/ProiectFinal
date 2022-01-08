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
using DatabaseModel;
using System.Data.Entity;
using System.Data;

namespace Proiect
{
    /// <summary>
    /// Interaction logic for PageTransport.xaml
    /// </summary>
    enum ActionState3
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class PageTransport : Page
    {
        ActionState3 action = ActionState3.Nothing;
        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource transportVSource;
        public PageTransport()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            transportVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("transportViewSource")));
            transportVSource.Source = ctx.Transport.Local;
            ctx.Transport.Load();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState3.New;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState3.Edit;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState3.Delete;
        }

        private void SaveTransport()
        {
            Transport transport = null;
            if (action == ActionState3.New)
            {
                try
                {
                    //instantiem 
                    transport = new Transport()
                    {
                        tip_transport = tip_transportTextBox.Text.Trim(),
                        nume_firma = nume_firmaTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Transport.Add(transport);
                    transportVSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            if (action == ActionState3.Edit)
            {
                try
                {
                    transport = (Transport)transportDataGrid.SelectedItem;
                    transport.tip_transport = tip_transportTextBox.Text.Trim();
                    transport.nume_firma = nume_firmaTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState3.Delete)
            {
                try
                {
                    transport = (Transport)transportDataGrid.SelectedItem;
                    ctx.Transport.Remove(transport);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                transportVSource.View.Refresh();
            }
        }

        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }

        private void ReInitialize()
        {
            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTransport();
            ReInitialize();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            transportVSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            transportVSource.View.MoveCurrentToPrevious();
        }

        
    }
}
