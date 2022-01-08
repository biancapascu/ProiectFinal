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
    /// Interaction logic for PageRezervari.xaml
    /// </summary>
    enum ActionState1
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class PageRezervari : Page
    {
        ActionState1 action = ActionState1.Nothing;
        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource clientiVSource;
        CollectionViewSource contracteVSource;
        CollectionViewSource cazareVSource;
        CollectionViewSource transportVSource;
        public PageRezervari()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            clientiVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiViewSource")));
            clientiVSource.Source = ctx.Clienti.Local;
            contracteVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("contracteViewSource")));
            //contracteVSource.Source = ctx.Contracte.Local;
            cazareVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cazareViewSource")));
            cazareVSource.Source = ctx.Cazare.Local;
            transportVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("transportViewSource")));
            transportVSource.Source = ctx.Transport.Local;
            BindDataGrid();

            ctx.Clienti.Load();
            ctx.Contracte.Load();
            ctx.Cazare.Load();
            ctx.Transport.Load();

            cmbClient.ItemsSource = ctx.Clienti.Local;
            //cmbClient.DisplayMemberPath = "id_client";
            cmbClient.SelectedValuePath = "id_client";

            cmbCazare.ItemsSource = ctx.Cazare.Local;
            //cmbCazare.DisplayMemberPath = "id_cazare";
            cmbCazare.SelectedValuePath = "id_cazare";

            cmbTransport.ItemsSource = ctx.Transport.Local;
            //cmbTransport.DisplayMemberPath = "id_transport";
            cmbTransport.SelectedValuePath = "id_transport";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState1.New;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState1.Edit;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState1.Delete;
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
            SaveContract();
            ReInitialize();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            contracteVSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            contracteVSource.View.MoveCurrentToPrevious();
        }

        private void BindDataGrid()
        {
            var queryContract = from con in ctx.Contracte
                                join cli in ctx.Clienti on con.id_client equals
                                cli.id_client
                                join caz in ctx.Cazare on con.id_cazare
                    equals caz.id_cazare
                                join tra in ctx.Transport on con.id_transport
                   equals tra.id_transport
                                select new
                                {
                                    con.id_contract,
                                    con.id_client,
                                    con.id_cazare,
                                    con.id_transport,
                                    cli.nume,
                                    cli.prenume,
                                    caz.nume_cazare,
                                    caz.tip_cazare,
                                    tra.nume_firma,
                                    tra.tip_transport
                                };
            contracteVSource.Source = queryContract.ToList();
        }

        private void SaveContract()
        {
            Contracte contract = null;
            if (action == ActionState1.New)
            {
                try
                {
                    Clienti client = (Clienti)cmbClient.SelectedItem;
                    Cazare cazare = (Cazare)cmbCazare.SelectedItem;
                    Transport transport = (Transport)cmbTransport.SelectedItem;
                    //instantiem 
                    contract = new Contracte()
                    {
                        id_cazare = cazare.id_cazare,
                        id_client = client.id_client,
                        id_transport = transport.id_transport
                    };
                    //adaugam entitatea nou creata
                    ctx.Contracte.Add(contract);
                    //salvam modificarile
                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState1.Edit)
            {
                dynamic selectedContract = contractDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedContract.id_contract;
                    var editedContract = ctx.Contracte.FirstOrDefault(s => s.id_contract == curr_id);
                    if (editedContract != null)
                    {
                        editedContract.id_client = Int32.Parse(cmbClient.SelectedValue.ToString());
                        editedContract.id_cazare = Convert.ToInt32(cmbCazare.SelectedValue.ToString());
                        editedContract.id_transport = Convert.ToInt32(cmbTransport.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                clientiVSource.View.MoveCurrentTo(selectedContract);
            }
            else if (action == ActionState1.Delete)
            {
                try
                {
                    dynamic selectedContract = contractDataGrid.SelectedItem;
                    int curr_id = selectedContract.id_contract;
                    var deletedContract = ctx.Contracte.FirstOrDefault(s => s.id_contract == curr_id);
                    if (deletedContract != null)
                    {
                        ctx.Contracte.Remove(deletedContract);
                        ctx.SaveChanges();
                        MessageBox.Show("Contract Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
