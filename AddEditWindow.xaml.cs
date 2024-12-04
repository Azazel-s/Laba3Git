using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace Demo4
{
    public partial class AddEditWindow : Window
    {
        private Requests request = new Requests();
        private Equipment equipment = new Equipment();
        private Clients client = new Clients();
        private FaultTypes faultType = new FaultTypes();

        public AddEditWindow()
        {
            InitializeComponent();
            DataContext = request;
            ComboStatus.ItemsSource = TechnoServiceEntities.GetContext().RequestStatus.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (request.application_number == null)
                error.AppendLine("Укажите номер заявки!");
            else if (!int.TryParse(request.application_number.ToString(), out int applicationNumber) || applicationNumber < 0)
                error.AppendLine("Номер заявки должен быть целочисленным и неотрицательным значением!");
            else if (TechnoServiceEntities.GetContext().Requests.Any(row => row.application_number == request.application_number))
                error.AppendLine("Номер заявки уже существует!");

            if (request.request_date == null || request.request_date == DateTime.MinValue)
                error.AppendLine("Укажите дату!");

            if (string.IsNullOrWhiteSpace(request.problem_description))
                error.AppendLine("Укажите описание проблемы!");

            if (ComboStatus.SelectedItem != null && ComboStatus.SelectedItem is RequestStatus selectedStatus) request.status_id = selectedStatus.status_id;
                else error.AppendLine("Выберите статус!");

            if(string.IsNullOrWhiteSpace(EquipmentTextBox.Text))
                error.AppendLine("Укажите оборудование!");

            if(string.IsNullOrWhiteSpace(ClientTextBox.Text))
                error.AppendLine("Укажите имя клиента!");

            if (string.IsNullOrWhiteSpace(FaultTypeTextBox.Text))
                error.AppendLine("Укажите тип неисправности!");

            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(),"Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var context = TechnoServiceEntities.GetContext();

                equipment.equipment_name = EquipmentTextBox.Text;
                client.client_name = ClientTextBox.Text;
                faultType.fault_type_name = FaultTypeTextBox.Text;

                context.Equipment.Add(equipment);
                context.Clients.Add(client);
                context.FaultTypes.Add(faultType);
                context.SaveChanges(); 
             
                var equipmentId = equipment.equipment_id;
                var clientId = client.client_id;
                var faultTypeId = faultType.fault_type_id;

                request.equipment_id = equipmentId;
                request.client_id = clientId;
                request.fault_type_id = faultTypeId;

                context.Requests.Add(request);
                context.SaveChanges();


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
