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
using System.Data;
using System.Data.SqlClient;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for AddPersonPage.xaml
    /// </summary>
    public partial class AddPersonPage : Page
    {
        string whereCity = "";
        string whereStatus = "";
        string whereDate = "";
        string whereSummary = "Where [summary] like '%%' ";

        string incidentId = "";

        SqlDataAdapter adapter;
        DataTable Incidents;
        SqlConnection connection = new SqlConnection(App.ConnectionString);

        public AddPersonPage()
        {
            InitializeComponent();
        }

        //Привязка данных к ListView
        private void FillListView()
        {
            connection.Open();
            string sqlSelectQuery = "Select distinct [diploma_KRSP].[id_incident], [incident_date], [summary], " +
                "[diploma_IncidentAddress].city, [diploma_IncidentAddress].street, " +
                "[diploma_IncidentAddress].building, [diploma_IncidentAddress].comment, " +
                "[diploma_IncidentType].incident_type, [diploma_FixationForm].fixation_form, " +
                "[diploma_Person].last_name, [diploma_Person].first_name, " +
                "[diploma_Person].patronymic, [diploma_Citizenship].citizenship, " +
                "[diploma_Involved].attitude, [diploma_IncidentStatus].[incident_status] " +
                "From[diploma_KRSP] join[diploma_IncidentAddress] " +
                "on[diploma_KRSP].id_incident_address = [diploma_IncidentAddress].id_incident_address" +
                " join[diploma_IncidentType] on [diploma_KRSP].id_incident_type = [diploma_IncidentType].id_incident_type " +
                "join[diploma_FixationForm] on[diploma_KRSP].id_fixation_from = [diploma_FixationForm].id_fixation_from" +
                " join[diploma_Involved] on[diploma_KRSP].id_incident = [diploma_Involved].id_incident" +
                " join[diploma_Person] on[diploma_Involved].id_person = [diploma_Person].id_person" +
                " join[diploma_Citizenship] on[diploma_Person].id_citizenship = [diploma_Citizenship].id_citizenship" +
                " join [diploma_Decree] on [diploma_KRSP].[id_incident] = [diploma_Decree].[id_incident]" +
                " join [diploma_IncidentStatus] on [diploma_Decree].[id_incidentstatus] = [diploma_IncidentStatus].[id_incidentstatus]" +
                " " + whereSummary + " " + whereCity + " " + whereStatus + " " + whereDate + " " +
                " order by [incident_date] desc";
            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapter = new SqlDataAdapter(command);
            Incidents = new DataTable();
            adapter.Fill(Incidents);
            lstview_Incidents.ItemsSource = Incidents.DefaultView;
            connection.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new StartPage());
        }

        //Вставка Физ. Лица
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
            string SqlSelect = "Select [id_person] " +
                "From diploma_Person " +
                "Where [last_name] = '" + txtbx_last_name.Text + "' and [first_name] = '" + txtbx_first_name.Text + "' " +
                "and [patronymic] = '" + txtbx_patronymic.Text + "' " +
                "and [registration_address] = '" + txtbx_registration_address.Text + "' " +
                "and [phone] = '" + txtbx_phone.Text + "' and [id_citizenship] = '" + cmbbx_Citizenship.SelectedIndex + "'";
            SqlCommand command = new SqlCommand(SqlSelect, connection);

            string PersonId = command.ExecuteScalar().ToString();

            connection.Close();

            return PersonId;
        }

        //Вставка причастных к происшествию
        private void InsertInvolved(string attitude)
        {
            connection.Open();
            string SqlInsert = "Insert Into [diploma_Involved] ([id_incident], [id_person], [attitude]) " +
                "Values ('" + txtbx_incidentId.Text + "', '" + GetPersonId() + "', '"+ attitude + "')";
            SqlCommand command = new SqlCommand(SqlInsert, connection);
            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtbx_last_name.Text == "" || txtbx_first_name.Text == "" ||
                txtbx_patronymic.Text == "" || txtbx_registration_address.Text == "" ||
                txtbx_phone.Text == "" || txtbx_incidentId.Text == "" ||
                cmbbx_Citizenship.SelectedIndex == 0)
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                if (rdbtn_suspect.IsChecked == true)
                {
                    InsertPerson();
                    InsertInvolved(rdbtn_suspect.Content.ToString());
                    MessageBox.Show("Успешно");
                }
                else if (rdbtn_witness.IsChecked == true)
                {
                    InsertPerson();
                    InsertInvolved(rdbtn_witness.Content.ToString());
                    MessageBox.Show("Успешно");
                }
                else
                {
                    MessageBox.Show("Выберите кем является физ. лицо");
                }
            }
        }

        private void lstview_Incidents_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DataRowView)lstview_Incidents.SelectedItem == null)
            {
                return;
            }

            DataRowView drv = (DataRowView)lstview_Incidents.SelectedItem;
            incidentId = drv["id_incident"].ToString();

            txtbx_incidentId.Text = incidentId;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FillListView();
        }

        //фильтр по городу
        private void cmbbx_IncidentFilterCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_IncidentFilterCity.SelectedIndex != 0)
            {
                if (cmbbx_IncidentFilterCity.SelectedIndex == 1)
                {
                    whereCity = "";
                    FillListView();
                }
                else
                {
                    whereCity = " and city = " +
                        "'" + cmbbx_IncidentFilterCity.SelectedValue.ToString().Substring(38) + "'";
                    FillListView();
                }
            }
        }

        //Фильтр по статусу
        private void cmbbx_IncidentFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_IncidentFilterStatus.SelectedIndex != 0)
            {
                if (cmbbx_IncidentFilterStatus.SelectedIndex == 1)
                {
                    whereStatus = "";
                    FillListView();
                }
                else
                {
                    whereStatus = " and [incident_status] = " +
                        "'" + cmbbx_IncidentFilterStatus.SelectedValue.ToString().Substring(38) + "'";
                    FillListView();
                }
            }
        }

        //Фильтр по дате
        private void dtpcr_IncidentFilterDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            whereDate = " and " +
                "(DATEPART(yy, incident_date) = " + dtpcr_IncidentFilterDate.Text.Substring(6) + " " +
                "and DATEPART(mm, incident_date) = " + dtpcr_IncidentFilterDate.Text.Substring(3).Remove(2) + " " +
                "and DATEPART(dd, incident_date) = " + dtpcr_IncidentFilterDate.Text.Remove(2) + ") ";
            FillListView();
        }

        //Поиск по фабуле происшествия
        private void txtbx_IncidentSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            whereSummary = "Where[summary] like '%" + txtbx_IncidentSearch.Text + "%'";
            FillListView();
        }

        //Отмена фильтра по дате
        private void btn_IncidentFilterDateCancel_Click(object sender, RoutedEventArgs e)
        {
            whereDate = "";
            dtpcr_IncidentFilterDate.Text = "Выберите дату";
            FillListView();
        }
    }
}
