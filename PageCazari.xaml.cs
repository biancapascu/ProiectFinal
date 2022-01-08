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
    /// Interaction logic for PageCazari.xaml
    /// </summary>
    enum ActionState2
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class PageCazari : Page
    {
        ActionState2 action = ActionState2.Nothing;
        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource cazareVSource;
        CollectionViewSource destinatiiVSource;
        public PageCazari()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cazareVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cazareViewSource")));
            cazareVSource.Source = ctx.Cazare.Local;
            ctx.Cazare.Load();

            destinatiiVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("destinatiiViewSource")));
            destinatiiVSource.Source = ctx.Destinatii.Local;
            ctx.Destinatii.Load();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState2.New;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState2.Edit;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState2.Delete;
        }

        private void SaveCazare()
        {
            Cazare cazare = null;
            if (action == ActionState2.New)
            {
                try
                {
                    //instantiem 
                    cazare = new Cazare()
                    {
                        nume_cazare = nume_cazareTextBox.Text.Trim(),
                        tip_cazare = tip_cazareTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Cazare.Add(cazare);
                    cazareVSource.View.Refresh();
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
            if (action == ActionState2.Edit)
            {
                try
                {
                    cazare = (Cazare)cazareDataGrid.SelectedItem;
                    cazare.nume_cazare = nume_cazareTextBox.Text.Trim();
                    cazare.tip_cazare = tip_cazareTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState2.Delete)
            {
                try
                {
                    cazare = (Cazare)cazareDataGrid.SelectedItem;
                    ctx.Cazare.Remove(cazare);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cazareVSource.View.Refresh();
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
            SaveCazare();
            ReInitialize();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            cazareVSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            cazareVSource.View.MoveCurrentToPrevious();
        }

       
    }
}
