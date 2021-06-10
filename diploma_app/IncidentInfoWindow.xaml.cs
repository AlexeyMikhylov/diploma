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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for IncidentInfoWindow.xaml
    /// </summary>
    public partial class IncidentInfoWindow : Window
    {
        SqlDataAdapter adapter;
        DataTable Incidents;
        SqlConnection connection = new SqlConnection(App.ConnectionString);
        string id;

        public IncidentInfoWindow(string incidentID)
        {
            InitializeComponent();

            id = incidentID;
        }

        private void FillDataGrid(string id)
        {
            connection.Open();

            string sqlSelectQuery = "Select diploma_KRSP.[id_incident], [incident_date], " +
                "[summary], [incident_status], diploma_Official.last_name, diploma_Official.first_name " +
                "From diploma_KRSP join diploma_Decree on diploma_KRSP.id_incident = diploma_Decree.id_incident " +
                "join diploma_IncidentStatus on diploma_Decree.id_incidentstatus = diploma_IncidentStatus.id_incidentstatus " +
                "join diploma_Official on diploma_Decree.id_official = diploma_Official.id_official " +
                "Where diploma_KRSP.[id_incident] = "+id+"";

            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapter = new SqlDataAdapter(command);
            Incidents = new DataTable();
            adapter.Fill(Incidents);
            dtgrid_IncidentInfo.ItemsSource = Incidents.DefaultView;
            connection.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid(id);
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            string sqlSelectQuery = "Update diploma_Decree " +
                "Set id_incidentstatus = "+cmbbx_IncidentStatus.SelectedIndex+" " +
                "Where [id_incident] = "+id+ " and [id_official] = "+App.CurrentUserId+"";
            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapter = new SqlDataAdapter(command);
            Incidents = new DataTable();
            adapter.Fill(Incidents);
            dtgrid_IncidentInfo.ItemsSource = Incidents.DefaultView;
            connection.Close();
            MessageBox.Show("Успешно.");
            Close();
        }
    }
}
