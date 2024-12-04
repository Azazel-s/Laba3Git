using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Windows;

namespace Demo4
{
    public partial class RefreshWindow : Window
    {
        private Requests request = new Requests();
        public RefreshWindow(Requests selectedRequest)
        {
            InitializeComponent();

            if (selectedRequest != null)
            {
                request = selectedRequest;
            }
            DataContext = request;
            ComboStatus.ItemsSource = TechnoServiceEntities.GetContext().RequestStatus.ToList();
            ComboWorker.ItemsSource = TechnoServiceEntities.GetContext().Workers.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(request.problem_description))
                error.AppendLine("Укажите описание проблемы!");

            if (ComboStatus.SelectedItem != null && ComboStatus.SelectedItem is RequestStatus selectedStatus) request.status_id = selectedStatus.status_id;
            else error.AppendLine("Выберите статус!");

            if (ComboWorker.SelectedItem != null && ComboWorker.SelectedItem is Workers selectedWorker) request.worker_id = selectedWorker.worker_id;
            else error.AppendLine("Выберите сотрудника!");

            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                TechnoServiceEntities.GetContext().Requests.AddOrUpdate(request);
                TechnoServiceEntities.GetContext().SaveChanges();

                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
