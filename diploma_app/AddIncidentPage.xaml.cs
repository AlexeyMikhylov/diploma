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
using System.Data.SqlClient;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for AddIncidentPage.xaml
    /// </summary>
    public partial class AddIncidentPage : Page
    {
        SqlConnection connection = new SqlConnection(App.ConnectionString);

        public AddIncidentPage()
        {
            InitializeComponent();
        }

        //если нет в базе адреса
        private void InsertIncidentAddress()
        {            
            connection.Open();
            string SqlInsert = "Insert Into diploma_IncidentAddress (city, street, building, comment) " +
                "Values ('" + cmbbx_city.Text + "', '" + txtbx_street.Text + "', " +
                "'" + txtbx_building.Text + "', '" + txtbx_comment.Text + "')";

            SqlCommand command = new SqlCommand(SqlInsert, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        //если есть такой адрес, то берем его ID
        private string GetIncidentAddresId()
        {
            connection.Close();
            connection.Open();
            string SqlSelect = "Select id_incident_address From diploma_IncidentAddress " +
                "Where city = '" + cmbbx_city.Text + "' and street = '" + txtbx_street.Text + "' " +
                "and building = '" + txtbx_building.Text + "' and comment = '" + txtbx_comment.Text + "'";
            SqlCommand command = new SqlCommand(SqlSelect, connection);

            string id_incident_address;

            if (command.ExecuteScalar() == null)
            {
                id_incident_address = "";
            }
            else
            {
                id_incident_address = command.ExecuteScalar().ToString();
            }

            connection.Close();

            return id_incident_address;
        }

        //Вставка Происшествия
        private void InsertIncident()
        {
            connection.Open();
            string SqlInsert = "Insert Into diploma_KRSP ([incident_date], [summary], " +
                "[id_incident_address], [id_incident_type], [id_fixation_from]) " +
                "Values ('" + dtpckr_incident_date.Text + " " + txtbx_hour.Text + ":" + txtbx_minute.Text + "', " +
                "'" + txtbx_summary.Text + "', " +
                "'" + GetIncidentAddresId() + "', '" + cmbbx_IncidentType.SelectedIndex + "', " +
                "'" + cmbbx_FixationForm.SelectedIndex + "')";
            SqlCommand command = new SqlCommand(SqlInsert, connection);
            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }

        //Берем ID происшествия, по дате и времени (мб еще адрес добавить)
        private string GetIncidentId()
        {
            connection.Close();
            connection.Open();

            string SqlSelect = "Select id_incident " +
                "From diploma_KRSP " +
                "Where(DATEPART(yy, incident_date) = " + dtpckr_incident_date.Text.Substring(6) + " " +
                "and DATEPART(mm, incident_date) = " + dtpckr_incident_date.Text.Substring(3).Remove(2) + " " +
                "and DATEPART(dd, incident_date) = " + dtpckr_incident_date.Text.Remove(2) + " " +
                "and DATEPART(HH, incident_date) = " + txtbx_hour.Text + " " +
                "and DATEPART(MINUTE, incident_date) = " + txtbx_minute.Text + ")";

            SqlCommand command = new SqlCommand(SqlSelect, connection);

            string IncidentId = command.ExecuteScalar().ToString();

            connection.Close();

            return IncidentId;
        }

        //Вставка Лица
        private void InsertPerson()
        {
            connection.Open();
            string SqlInsert = "Insert Into diploma_Person ([last_name], [first_name], " +
                "[patronymic], [registration_address], [phone], [id_citizenship]) " +
                "Values ('" + txtbx_last_name.Text + "', '" + txtbx_first_name.Text + "', " +
                "'" + txtbx_patronymic.Text + "', '" + txtbx_registration_address.Text + "', " +
                "'" + txtbx_phone.Text + "', '" + cmbbx_Citizenship.SelectedIndex + "')";
            SqlCommand command = new SqlCommand(SqlInsert, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        //Берем ID лица
        private string GetPersonId()
        {
            connection.Open();
            string SqlSelect = "Select [id_person] " +
                "From diploma_Person " +
                "Where [last_name] = '" + txtbx_last_name.Text + "' and [first_name] = '" + txtbx_first_name.Text + "' " +
                "and [patronymic] = '" + txtbx_patronymic.Text + "' " +
                "and [registration_address] = '" + txtbx_registration_address.Text + "' " +
                "and [phone] = '" + txtbx_phone.Text + "' and [id_citizenship] = '" + cmbbx_Citizenship.SelectedIndex + "'";
            SqlCommand command = new SqlCommand(SqlSelect, connection);

            string PersonId;

            if (command.ExecuteScalar() == null)
            {
                PersonId = "";
            }
            else
            {
                PersonId = command.ExecuteScalar().ToString();
            }

            connection.Close();

            return PersonId;
        }

        //Вставка причастных к происшествию не анонимно
        private void InsertInvolved()
        {
            connection.Open();
            string SqlInsert = "Insert Into [diploma_Involved] ([id_incident], [id_person], [attitude]) " +
                "Values ('" + GetIncidentId() + "', '" + GetPersonId() + "', 'Заявитель')";
            SqlCommand command = new SqlCommand(SqlInsert, connection);
            connection.Open();
            command.ExecuteNonQuery(); 

            connection.Close();
        }

        //Вставка причастных к происшествию анонимно
        private void InsertInvolvedAnonym()
        {
            connection.Open();
            string SqlInsert = "Insert Into [diploma_Involved] ([id_incident], [id_person], [attitude]) " +
                "Values ('"+GetIncidentId()+"', '4', 'Заявитель')";
            SqlCommand command = new SqlCommand(SqlInsert, connection);
            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }

        //Вставка в постановление
        private void InsertIntoDecree()
        {
            connection.Open();
            string SqlInsert = "Insert Into [diploma_Decree] ([decree_date], [id_incident]) " +
                "Values ('"+DateTime.Now+"', '"+GetIncidentId()+"')";
            SqlCommand command = new SqlCommand(SqlInsert, connection);
            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }

        //сок
        private void btn_IncidentAdd_Click(object sender, RoutedEventArgs e)
        {
            if (rdbtn_anonym.IsChecked == true)
            {
                if (cmbbx_city.Text == "" || txtbx_street.Text == "" ||
                   txtbx_building.Text == "" || txtbx_comment.Text == "" ||
                   txtbx_summary.Text == "" ||
                   txtbx_hour.Text == "" || txtbx_minute.Text == "" ||
                   cmbbx_FixationForm.SelectedIndex == 0 ||
                   cmbbx_IncidentType.SelectedIndex == 0 || txtbx_hour.Text.Length < 2 || txtbx_minute.Text.Length < 2)
                {
                    MessageBox.Show("Заполните все данные.");
                }
            }
            else if (cmbbx_city.Text == "" || txtbx_street.Text == "" ||
                txtbx_building.Text == "" || txtbx_comment.Text == "" ||
                txtbx_last_name.Text == "" || txtbx_first_name.Text == "" ||
                txtbx_patronymic.Text == "" || txtbx_registration_address.Text == "" ||
                txtbx_phone.Text == "" || txtbx_summary.Text == "" ||
                txtbx_hour.Text == "" || txtbx_minute.Text == "" ||
                cmbbx_Citizenship.SelectedIndex == 0 || cmbbx_FixationForm.SelectedIndex == 0 ||
                cmbbx_IncidentType.SelectedIndex == 0 || txtbx_hour.Text.Length < 2 || txtbx_minute.Text.Length < 2)
            {
                MessageBox.Show("Заполните все данные.");
            }
            else
            {
                //вставка адреса
                if (GetIncidentAddresId() == "" || GetIncidentAddresId() == null)
                {
                    InsertIncidentAddress();
                }
                //вставка происшествия
                InsertIncident();
                //Вставка постановления
                InsertIntoDecree();
                //вставка лица
                if (GetPersonId() == "" || GetPersonId() == null)
                {
                    InsertPerson();
                }
                //вставка причастных
                if (rdbtn_anonym.IsChecked == true)
                {
                    GetIncidentId();
                    InsertInvolvedAnonym();
                }
                else
                {
                    GetIncidentId();
                    InsertInvolved();
                }

                MessageBox.Show("Успешно");
            }
        }

        private void dtpckr_incident_date_CalendarClosed(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(dtpckr_incident_date.Text);
        }

        private void btn_IncidentCancel_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new StartPage());
        }

        private void rdbtn_anonym_Checked(object sender, RoutedEventArgs e)
        {
            rdbtn_nonanonym.IsChecked = false;

            txtbx_last_name.IsEnabled = false;
            txtbx_first_name.IsEnabled = false;
            txtbx_patronymic.IsEnabled = false;
            txtbx_phone.IsEnabled = false;
            txtbx_registration_address.IsEnabled = false;
            cmbbx_Citizenship.IsEnabled = false;
        }

        private void rdbtn_nonanonym_Checked(object sender, RoutedEventArgs e)
        {
            rdbtn_anonym.IsChecked = false;

            txtbx_last_name.IsEnabled = true;
            txtbx_first_name.IsEnabled = true;
            txtbx_patronymic.IsEnabled = true;
            txtbx_phone.IsEnabled = true;
            txtbx_registration_address.IsEnabled = true;
            cmbbx_Citizenship.IsEnabled = true;
        }

        //Добавление происшествия и переход на окно добавление подозреваемого
        private void btn_PersonAdd_Click(object sender, RoutedEventArgs e)
        {
            btn_IncidentAdd_Click(sender, e);
            Manager.MainFrame.Navigate(new AddPersonPage());
        }
    }
}
