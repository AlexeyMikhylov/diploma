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
using System.Windows.Forms.DataVisualization.Charting;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        SqlDataAdapter adapter;
        DataTable incidents;
        SqlConnection connection = new SqlConnection(App.ConnectionString);

        public StartPage()
        {
            InitializeComponent();

            // Все графики находятся в пределах области построения ChartArea, создадим ее
            chart.ChartAreas.Add(new ChartArea("Default"));

            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series("Происшествия"));
        }

        private void FillDataGrid()
        {
            connection.Open();
            string sqlSelectQuery = "Select [incident_date], [summary], " +
                "[diploma_IncidentAddress].city, [diploma_IncidentAddress].street, " +
                "[diploma_IncidentAddress].building, [diploma_IncidentAddress].comment, " +
                "[diploma_IncidentType].incident_type, [diploma_FixationForm].fixation_form, " +
                "[diploma_Person].last_name, [diploma_Person].first_name, " +
                "[diploma_Person].patronymic, [diploma_Citizenship].citizenship, " +
                "[diploma_Involved].attitude " +
                "From[diploma_KRSP] join[diploma_IncidentAddress] " +
                "on[diploma_KRSP].id_incident_address = [diploma_IncidentAddress].id_incident_address" +
                " join[diploma_IncidentType] on [diploma_KRSP].id_incident_type = [diploma_IncidentType].id_incident_type " +
                "join[diploma_FixationForm] on[diploma_KRSP].id_fixation_from = [diploma_FixationForm].id_fixation_from" +
                " join[diploma_Involved] on[diploma_KRSP].id_incident = [diploma_Involved].id_incident" +
                " join[diploma_Person] on[diploma_Involved].id_person = [diploma_Person].id_person" +
                " join[diploma_Citizenship] on[diploma_Person].id_citizenship = [diploma_Citizenship].id_citizenship" +
                " order by [incident_date]";
            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapter = new SqlDataAdapter(command);
            incidents = new DataTable();
            adapter.Fill(incidents);
            dtgrid_incidents.ItemsSource = incidents.DefaultView;
            connection.Close();
        }

        private void btn_toReportPage_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ReportPage());
        }

        private void CountIncidentsYesterday()
        {
            // to do sql query

            //return number
        }

        private void CountIncidentsToday()
        {
            // to do sql query

            //return number
        }

        private void CountIncidentsWeek()
        {
            // to do sql query

            //return numbers
        }

        private void DateFormat()
        {
            ((DataGridTextColumn)dtgrid_incidents.Columns[0]).Binding.StringFormat = "dd.MM.yyyy HH:mm";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();

            DateFormat();

            chart.Series["Происшествия"].ChartArea = "Default";
            //chart.Series["Series1"].ChartType = SeriesChartType.Line;
            chart.Series["Происшествия"].ChartType = SeriesChartType.Line;

            // добавим данные линии
            string[] axisXData = new string[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            double[] axisYData = new[] { 65.0, 57.0, 89.0, 14.0, 51.0, 44.0, 70.0 };
            chart.Series["Происшествия"].Points.DataBindXY(axisXData, axisYData);
        }

        private void btn_IncidentAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddIncidentPage());
        }

        private void btn_IncidentCatalog_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new IncidentPage());
        }

        private void btn_PersonCatalog_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PersonPage());
        }

        private void btn_PersonAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddPersonPage());
        }
    }
}
