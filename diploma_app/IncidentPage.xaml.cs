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
    /// Interaction logic for IncidentPage.xaml
    /// </summary>
    public partial class IncidentPage : Page
    {
        string whereCity = "";
        string whereStatus = "";
        string whereDate = "";
        string whereSummary = "Where [summary] like '%%' ";

        SqlDataAdapter adapter;
        DataTable Incidents;
        SqlConnection connection = new SqlConnection(App.ConnectionString);

        public IncidentPage()
        {
            InitializeComponent();
        }

        private void FillListView()
        {
            connection.Open();
            string sqlSelectQuery = "Select [diploma_KRSP].[id_incident], [incident_date], [summary], " +
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
                " "+ whereSummary + " "+whereCity+" "+whereStatus+" "+whereDate+" " +
                " order by [incident_date] desc";
            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapter = new SqlDataAdapter(command);
            Incidents = new DataTable();
            adapter.Fill(Incidents);
            lstview_Incidents.ItemsSource = Incidents.DefaultView; 
            connection.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FillListView();
        }

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

        private void dtpcr_IncidentFilterDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            whereDate = " and " +
                "(DATEPART(yy, incident_date) = "+ dtpcr_IncidentFilterDate.Text.Substring(6) + " " +
                "and DATEPART(mm, incident_date) = " + dtpcr_IncidentFilterDate.Text.Substring(3).Remove(2) + " " +
                "and DATEPART(dd, incident_date) = " + dtpcr_IncidentFilterDate.Text.Remove(2) + ") ";
            FillListView();
        }

        private void txtbx_IncidentSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            whereSummary = "Where[summary] like '%" + txtbx_IncidentSearch.Text + "%'";
            FillListView();
        }

        private void btn_IncidentFilterDateCancel_Click(object sender, RoutedEventArgs e)
        {
            whereDate = "";
            dtpcr_IncidentFilterDate.Text = "Выберите дату";
            FillListView();
        }

        private string CheckIfDecreeHadOfficial(string id)
        {
            string result;
            connection.Open();
            string SqlInsert = "Select [id_official] From [diploma_Decree] " +
                "where [id_incident] = "+id+ " and id_official = "+App.CurrentUserId+"";
            SqlCommand com = new SqlCommand(SqlInsert, connection);

            //Console.WriteLine(com.ExecuteNonQuery());
            //Console.WriteLine(com.ExecuteScalar().ToString());

            if (com.ExecuteNonQuery() == -1 && com.ExecuteScalar().ToString() == "") //
            {
                result = "null";
            }
            else
            {
                result = com.ExecuteScalar().ToString();
            }
            connection.Close();

            return result;
        }

        private void lstview_Incidents_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DataRowView)lstview_Incidents.SelectedItem == null)
            {
                return;
            }

            DataRowView drv = (DataRowView)lstview_Incidents.SelectedItem;
            string incidentStatus = drv["incident_status"].ToString();

            string incidentId = drv["id_incident"].ToString();
            string incidentDate = drv["incident_date"].ToString();
            string incidentSummary = drv["summary"].ToString();

                if (CheckIfDecreeHadOfficial(incidentId) == "null" || CheckIfDecreeHadOfficial(incidentId) != App.CurrentUserId.ToString())
                {
                    string message = "Открыть происшетсвие для редактирования?" +
                       "\nВы сразу станете должнастным лицом этого происшествия.";
                    string caption = "";
                    MessageBoxButton buttons = MessageBoxButton.YesNo;
                    string result;

                    result = Convert.ToString(MessageBox.Show(message, caption, buttons, MessageBoxImage.Question));

                    if (result == Convert.ToString(MessageBoxResult.Yes))
                    {
                        if (CheckIfDecreeHadOfficial(incidentId) == "null")
                        {
                            connection.Open();
                            string SqlInsert = "Update [diploma_Decree] " +
                                "set [id_official] = " + App.CurrentUserId + " " +
                                "Where [id_incident] = " + incidentId + "";
                            SqlCommand com = new SqlCommand(SqlInsert, connection);
                            com.ExecuteNonQuery();
                            connection.Close();
                        }
                        else if (CheckIfDecreeHadOfficial(incidentId) != App.CurrentUserId.ToString())
                        {
                            connection.Open();
                            string SqlInsert = "Insert into [diploma_Decree] ([decree_date],[id_incident], [id_official]) " +
                                "Values('" + DateTime.Now + "', '" + incidentId + "', '" + App.CurrentUserId + "')";
                            SqlCommand com = new SqlCommand(SqlInsert, connection);
                            com.ExecuteNonQuery();
                            connection.Close();
                        }

                        IncidentInfoWindow IIW = new IncidentInfoWindow(incidentId);
                        IIW.Show();
                    }
                }
                else if (CheckIfDecreeHadOfficial(incidentId) == App.CurrentUserId.ToString())
                {
                    IncidentInfoWindow IIW = new IncidentInfoWindow(incidentId);
                    IIW.Show();
                }
        }
    }
}
