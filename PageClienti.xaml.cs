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
    /// Interaction logic for PageClienti.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class PageClienti : Page
    {
        ActionState action = ActionState.Nothing;
        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource clientiVSource;
        public PageClienti()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            clientiVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiViewSource")));
            clientiVSource.Source = ctx.Clienti.Local;
            ctx.Clienti.Load();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }
        private void SaveClienti()
        {
            Clienti client = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem 
                    client = new Clienti()
                    {
                        nume = numeTextBox.Text.Trim(),
                        prenume = prenumeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Clienti.Add(client);
                    clientiVSource.View.Refresh();
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
            if (action == ActionState.Edit)
            {
                try
                {
                    client = (Clienti)clientiDataGrid.SelectedItem;
                    client.nume = numeTextBox.Text.Trim();
                    client.prenume = prenumeTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    client = (Clienti)clientiDataGrid.SelectedItem;
                    ctx.Clienti.Remove(client);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                clientiVSource.View.Refresh();
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
            SaveClienti();
            ReInitialize();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            clientiVSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            clientiVSource.View.MoveCurrentToPrevious();
        }

        private void SetValidationBinding()
        {
            Binding NullValidationBinding = new Binding();
            NullValidationBinding.Source = clientiVSource;
            NullValidationBinding.Path = new PropertyPath("nume");
            NullValidationBinding.NotifyOnValidationError = true;
            NullValidationBinding.Mode = BindingMode.TwoWay;
            NullValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            NullValidationBinding.ValidationRules.Add(new StringNotEmpty());
            numeTextBox.SetBinding(TextBox.TextProperty, NullValidationBinding);
        }
    }
}
