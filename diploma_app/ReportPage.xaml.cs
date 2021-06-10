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

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ObservableCollection<Incident> Incidents { get; set; }

        public ReportPage()
        {
            InitializeComponent();

            var currentSeries = new Series("Incidents")
            {
                IsValueShownAsLabel = true
            };
            chart.Series.Add(currentSeries);

            // Все графики находятся в пределах области построения ChartArea, создадим ее
            chart.ChartAreas.Add(new ChartArea("Default"));

            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series("Series1"));

            //we gotta send data from
            Incidents = new ObservableCollection<Incident>
            {
                new Incident {Month = "Январь", NumberOfIncidents = 784},
                new Incident {Month = "Февраль", NumberOfIncidents = 792},
                new Incident {Month = "Март", NumberOfIncidents = 874},
                new Incident {Month = "Апрель", NumberOfIncidents = 528},
                new Incident {Month = "Май", NumberOfIncidents = 289}
            };
        }

        public class Incident
        {
            public string Month { get; set; }
            public int NumberOfIncidents { get; set; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //chart.ChartAreas.Add(new ChartArea("Main"));
            //cmbbx_1.ItemsSource = Incidents;
            cmbbx_2.ItemsSource = Enum.GetValues(typeof(SeriesChartType));

            chart.Series["Series1"].ChartArea = "Default";
            //chart.Series["Series1"].ChartType = SeriesChartType.Line;
            chart.Series["Series1"].ChartType = SeriesChartType.Line;

            // добавим данные линии
            //string[] axisXData = new string[] { "a", "b", "c" };
            string[] axisXData = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май" };
            double[] axisYData = new[] { 0.1, 1.5, 1.9, 1.6, 0.2 };
            chart.Series["Series1"].Points.DataBindXY(axisXData, axisYData);
        }

        private void cmbbx_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

        private void cmbbx_ReportCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
