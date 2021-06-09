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
            string sqlSelectQuery = "Select [incident_date], [summary], " +
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
                " join [diploma_IncidentStatus] on [diploma_Decree].[id_incidentstatus] = [diploma_IncidentStatus].[id_incidentstatus] " +
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
    }
}
