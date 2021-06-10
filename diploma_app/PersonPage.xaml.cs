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
    /// Interaction logic for PersonPage.xaml
    /// </summary>
    public partial class PersonPage : Page
    {
        SqlDataAdapter adapter;
        DataTable Person;
        SqlConnection connection = new SqlConnection(App.ConnectionString);
        public PersonPage()
        {
            InitializeComponent();
        }

        private void FillListView()
        {
            connection.Open();
            string sqlSelectQuery = "Select [last_name], [first_name], [patronymic], " +
                "[registration_address], [phone], [diploma_Citizenship].citizenship, " +
                "[diploma_CriminalRecord].criminal_record, diploma_Involved.attitude, " +
                "diploma_KRSP.incident_date, diploma_KRSP.summary " +
                "From " +
                "diploma_Person join diploma_Citizenship " +
                "on diploma_Person.id_citizenship = diploma_Citizenship.id_citizenship " +
                "left join diploma_Convicts " +
                "on diploma_Person.id_person = diploma_Convicts.id_person " +
                "left join diploma_CriminalRecord " +
                "on diploma_Convicts.id_criminalrecord = diploma_CriminalRecord.criminal_record " +
                "join diploma_Involved " +
                "on diploma_Person.id_person = diploma_Involved.id_person " +
                "join diploma_KRSP " +
                "on diploma_Involved.id_incident = diploma_KRSP.id_incident " +
                "order by incident_date desc";
            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapter = new SqlDataAdapter(command);
            Person = new DataTable();
            adapter.Fill(Person);
            lstview_Persons.ItemsSource = Person.DefaultView;
            connection.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FillListView();
        }

        private void cmbbx_SortAttitude_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FillListView();
        }

        private void cmbbx_SortSitizenship_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtbx_PersonSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbbx_IncidentFilterCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbbx_PersonSearchby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
