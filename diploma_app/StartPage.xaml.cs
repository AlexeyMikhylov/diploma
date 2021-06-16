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
using System.Globalization;
using System.Runtime;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        SqlDataAdapter adapter;
        DataTable incidents;
        SqlDataAdapter adapterGraph;
        DataTable incidentsGraph;
        SqlConnection connection = new SqlConnection(App.ConnectionString);

        string monday, sunday, month;

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

        //Происшествий вчера
        private string CountIncidentsYesterday()
        {
            connection.Open();

            string SqlSel = "Select count(diploma_KRSP.id_incident) as 'incidents' " +
                "From diploma_KRSP join diploma_IncidentAddress on " +
                "diploma_KRSP.id_incident_address = diploma_IncidentAddress.id_incident_address " +
                "group by year(incident_date), month(incident_date), day(incident_date) " +
                "having " +
                "Year(diploma_KRSP.incident_date) = 2021 " +
                " and month(incident_date) = " + month + " " +
                " and day(incident_date) = " + DateTime.Now.AddDays(-1).ToString().Remove(2) + " ";
            SqlCommand com = new SqlCommand(SqlSel, connection);

            string result;

            if (com.ExecuteScalar() == null)
            {
                result = "0";
            }
            else if (com.ExecuteScalar().ToString() == "")
            {
                result = "0";
            }
            else
            {
                result = com.ExecuteScalar().ToString();
            }

            connection.Close();

            return result;
        }

        //Происшествий сегодня
        private string CountIncidentsToday()
        {
            connection.Open();
            string SqlSel = "Select count(diploma_KRSP.id_incident) as 'incidents' " +
                "From diploma_KRSP join diploma_IncidentAddress on " +
                "diploma_KRSP.id_incident_address = diploma_IncidentAddress.id_incident_address " +
                "group by year(incident_date), month(incident_date), day(incident_date) " +
                "having " +
                "Year(diploma_KRSP.incident_date) = 2021 " +
                "and month(incident_date) = " + month + " " +
                "and day(incident_date) = " + DateTime.Now.ToString().Remove(2) + " ";
            SqlCommand com = new SqlCommand(SqlSel, connection);

            string result;

            if (com.ExecuteScalar() == null)
            {
                result = "0";
            }
            else if (com.ExecuteScalar().ToString() == "")
            {
                result = "0";
            }
            else
            {
                result = com.ExecuteScalar().ToString();
            }

            connection.Close();

            return result;
        }

        private void CountIncidentsWeek()
        {
            // to do sql query

            //return numbers
        }

        //Форматирование даты
        private void DateFormat()
        {
            ((DataGridTextColumn)dtgrid_incidents.Columns[0]).Binding.StringFormat = "dd.MM.yyyy HH:mm";
        }

        private void SqlQueryCountIncidents()
        {
            myCmb1.Items.Clear();
            myCmb2.Items.Clear();
            SqlConnection connection = new SqlConnection(App.ConnectionString);
            connection.Open();
            string SqlSel = "Select count(diploma_KRSP.id_incident) as 'incidents', " +
                "CONCAT(year(incident_date), '/', month(incident_date), '/', day(incident_date)) as 'date' " +
                "From diploma_KRSP join diploma_IncidentAddress on " +
                "diploma_KRSP.id_incident_address = diploma_IncidentAddress.id_incident_address " +
                "group by year(incident_date), month(incident_date), day(incident_date) " +
                "having Year(diploma_KRSP.incident_date) = 2021 " +
                "and month(incident_date) = " + month + " " +
                "and day(incident_date) between " + monday + " and " + sunday + " " +
                "order by 'date' asc";
            SqlCommand com = new SqlCommand(SqlSel, connection);
            adapterGraph = new SqlDataAdapter(com);
            incidentsGraph = new DataTable();
            adapterGraph.Fill(incidentsGraph);
            for (int i = 0; i < incidentsGraph.Rows.Count; i++)
            {
                myCmb1.Items.Add(incidentsGraph.Rows[i]["incidents"].ToString());
                myCmb2.Items.Add(incidentsGraph.Rows[i]["date"].ToString());
            }

            connection.Close();
        }

        private void BuildGraph(ComboBox cmbbx1, ComboBox cmbbx2)
        {
            chart.Series["Происшествия"].ChartArea = "Default";
            chart.Series["Происшествия"].ChartType = SeriesChartType.Column;

            string[] axisXData = new string[cmbbx1.Items.Count];
            int[] axisYData = new int[cmbbx2.Items.Count];

            for (int i = 0; i < cmbbx1.Items.Count; i++)
            {
                axisXData[i] = cmbbx1.Items[i].ToString();
            }
            for (int i = 0; i < cmbbx2.Items.Count; i++)
            {
                axisYData[i] = Convert.ToInt32(cmbbx2.Items[i]);
            }

            chart.Series["Происшествия"].Points.DataBindXY(axisXData, axisYData);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentWeekDates();

            SqlQueryCountIncidents();

            lbl_yesterday.Content = "Вчера - " + CountIncidentsYesterday();
            lbl_today.Content = "Сегодня - " + CountIncidentsToday();

            FillDataGrid();

            DateFormat();

            BuildGraph(myCmb2, myCmb1);
        }

        private void CurrentWeekDates()
        {
            DateTime startOfWeek = DateTime.Today.AddDays(
                (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek -
                (int)DateTime.Today.DayOfWeek);

            string result = string.Join("," + Environment.NewLine, Enumerable
              .Range(0, 7)
              .Select(i => startOfWeek
                 .AddDays(i)
                 .ToString("dd-MM-yyyy")));

            monday = result.Substring(0).Remove(2);
            sunday = result.Substring(78).Remove(2);
            month = result.Substring(3).Remove(2);
        }

        //Навигация
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
