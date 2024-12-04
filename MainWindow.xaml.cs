using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Demo4
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            TechnoService.ItemsSource = TechnoServiceEntities.GetContext().Requests.ToList();
            ComboStatus.ItemsSource = TechnoServiceEntities.GetContext().RequestStatus.ToList();
            Box.Text = TechnoServiceEntities.GetContext().Requests.Count(r => r.status_id == 2).ToString();
            Vis();
        }

        private void TechnoService_Loaded(object sender, RoutedEventArgs e)
        {
            if (TechnoService.Columns.Any())
            {
                TechnoService.Columns.RemoveAt(TechnoService.Columns.Count - 1);
            }
        }

        private void Vis()
        {
            switch (Authorization.authorizationRole)
            {
                case "Админ":
                    break;
                case "Модер":
                    BtnDelet.Visibility = Visibility.Collapsed;
                    break;
                case "Юзер":

                    BtnDelet.Visibility = Visibility.Collapsed;
                    TechnoService.Loaded += TechnoService_Loaded;
                    break;
                default:
                    return;
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            RefreshWindow addEditWindow = new RefreshWindow((sender as Button).DataContext as Requests);
            addEditWindow.ShowDialog();
            TechnoService.ItemsSource = TechnoServiceEntities.GetContext().Requests.ToList();
            Box.Text = TechnoServiceEntities.GetContext().Requests.Count(r => r.status_id == 2).ToString();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow addEditWindow = new AddEditWindow();
            addEditWindow.ShowDialog();
            TechnoService.ItemsSource = TechnoServiceEntities.GetContext().Requests.ToList();
            Box.Text = TechnoServiceEntities.GetContext().Requests.Count(r => r.status_id == 2).ToString();
        }
        private void BtnDelet_Click(object sender, RoutedEventArgs e)
        {
            var servisForRemoving = TechnoService.SelectedItems.Cast<Requests>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {servisForRemoving.Count()} элеме?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    TechnoServiceEntities context = TechnoServiceEntities.GetContext();

                    context.Requests.RemoveRange(servisForRemoving);

                    foreach (var request in servisForRemoving)
                    {
                        var equipmentToRemove = context.Equipment.Where(l => l.equipment_id == request.request_id);
                        context.Equipment.RemoveRange(equipmentToRemove);

                        var clientToRemove = context.Clients.Where(c => c.client_id == request.request_id);
                        context.Clients.RemoveRange(clientToRemove);

                        var faultTypeToRemove = context.FaultTypes.Where(f => f.fault_type_id == request.request_id);
                        context.FaultTypes.RemoveRange(faultTypeToRemove);
                    }
                    context.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    TechnoService.ItemsSource = context.Requests.ToList();
                    Box.Text = TechnoServiceEntities.GetContext().Requests.Count(r => r.status_id == 2).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            TechnoService.ItemsSource = TechnoServiceEntities.GetContext().Requests.ToList();
        }

        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            if (ComboStatus.SelectedItem != null && ComboStatus.SelectedItem is RequestStatus selectedStatus)
            {
                int selectedStatusId = selectedStatus.status_id;
                TechnoService.ItemsSource = TechnoServiceEntities.GetContext().Requests.Where(r => r.status_id == selectedStatusId).ToList();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text;
            TechnoService.ItemsSource = TechnoServiceEntities.GetContext().Requests
                .Where(r =>
                    r.application_number.ToString().Contains(searchText) ||
                    r.Equipment.equipment_name.Contains(searchText) ||
                    r.FaultTypes.fault_type_name.Contains(searchText) ||
                    r.problem_description.Contains(searchText) ||
                    r.Clients.client_name.Contains(searchText) ||
                    r.RequestStatus.status_name.Contains(searchText) ||
                    r.Workers.worker_name.Contains(searchText))
                .ToList();
        }

        private void BtnAuthorization_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }
    }
}
