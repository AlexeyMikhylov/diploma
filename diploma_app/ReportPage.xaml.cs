using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        SqlDataAdapter adapter;
        DataTable table;
        string whereCity = "";
        string whereDate = "";

        public ObservableCollection<Incident> Incidents { get; set; }

        public ReportPage()
        {
            InitializeComponent();

            var currentSeries = new Series("Происшествия")
            {
                IsValueShownAsLabel = true
            };
            chart.Series.Add(currentSeries);

            // Все графики находятся в пределах области построения ChartArea
            chart.ChartAreas.Add(new ChartArea("Default"));

            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series("Series1"));
        }

        public class Incident
        {
            public string Month { get; set; }
            public int NumberOfIncidents { get; set; }
        }

        private void SqlQuery()
        {
            myCmb1.Items.Clear();
            myCmb2.Items.Clear();
            SqlConnection connection = new SqlConnection(App.ConnectionString);
            connection.Open();
            string SqlSel = "Select count(diploma_KRSP.id_incident) as 'incidents', city, " +
                "CONCAT(year(incident_date), '/', month(incident_date), '/', day(incident_date)) as 'date' " +
                "From diploma_KRSP join diploma_IncidentAddress on " +
                "diploma_KRSP.id_incident_address = diploma_IncidentAddress.id_incident_address " +
                "group by city, year(incident_date), month(incident_date), day(incident_date) " +
                "having " +
                "Year(diploma_KRSP.incident_date) = 2021 and " +
                "month(incident_date) = 06 and " +
                "day(incident_date) between 05 and 11 " +
                " "+whereCity+" ";
            SqlCommand com = new SqlCommand(SqlSel, connection);
            adapter = new SqlDataAdapter(com);
            table = new DataTable();
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                myCmb1.Items.Add(table.Rows[i]["incidents"].ToString());
                myCmb2.Items.Add(table.Rows[i]["date"].ToString());
            }

            connection.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SqlQuery();

            BuildGraph(myCmb2, myCmb1);
        }

        //Построение графика
        private void BuildGraph(ComboBox cmbbx1, ComboBox cmbbx2)
        {
            chart.Series["Series1"].ChartArea = "Default";

            if (cmbbx_2.SelectedIndex == 1)
                chart.Series["Series1"].ChartType = SeriesChartType.Line;
            if (cmbbx_2.SelectedIndex == 2)
                chart.Series["Series1"].ChartType = SeriesChartType.Column;
            if (cmbbx_2.SelectedIndex == 3)
                chart.Series["Series1"].ChartType = SeriesChartType.Area;
            if (cmbbx_2.SelectedIndex == 4)
                chart.Series["Series1"].ChartType = SeriesChartType.Bubble;
            if (cmbbx_2.SelectedIndex == 5)
                chart.Series["Series1"].ChartType = SeriesChartType.Spline; 
            if (cmbbx_2.SelectedIndex == 6)
                chart.Series["Series1"].ChartType = SeriesChartType.SplineArea;

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

            chart.Series["Series1"].Points.DataBindXY(axisXData, axisYData);
        }

        private void cmbbx_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_2.SelectedIndex != 0)
                BuildGraph(myCmb2, myCmb1);

        }

        private void btn_PrintReport_Click(object sender, RoutedEventArgs e)
        {
            //Печать
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(WinFormsHost, "Печать отчета...");
            }
        }

        //Выбор города
        private void cmbbx_ReportCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_ReportCity.SelectedIndex != 0)
            {
                if (cmbbx_ReportCity.SelectedIndex == 1)
                {
                    whereCity = "";
                    SqlQuery();
                    BuildGraph(myCmb2, myCmb1);
                }
                else
                {
                    whereCity = "and city = '" + cmbbx_ReportCity.SelectedValue.ToString().Substring(38) + "'";
                    SqlQuery();
                    BuildGraph(myCmb2, myCmb1);
                }
            }
        }
    }
}
