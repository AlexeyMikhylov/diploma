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
        string whereCity = "";
        string whereAttitude = "";
        string whereSitizenship = "";
        string whereSearch = "Where [last_name] like '%%' ";
        public PersonPage()
        {
            InitializeComponent();
        }

        private void FillListView()
        {
            connection.Open();
            string sqlSelectQuery = "Select [diploma_Person].[id_person], " +
                "[last_name], [first_name], [patronymic], " +
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
                ""+whereSearch+" "+whereCity+" "+whereAttitude+" "+whereSitizenship+" " +
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

        //фильтр по отношению к происшествию
        private void cmbbx_SortAttitude_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_SortAttitude.SelectedIndex != 0)
            {
                if (cmbbx_SortAttitude.SelectedIndex == 1)
                {
                    whereAttitude = "";
                    FillListView();
                }
                else
                {
                    whereAttitude = " and attitude = '" + cmbbx_SortAttitude.SelectedValue.ToString().Substring(38) + "'";
                    FillListView();
                }
            }
        }

        //фильтр по гражданству
        private void cmbbx_SortSitizenship_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_SortSitizenship.SelectedIndex != 0)
            {
                if (cmbbx_SortSitizenship.SelectedIndex == 1)
                {
                    whereSitizenship = "";
                    FillListView();
                }
                else
                {
                    whereSitizenship = " and citizenship = '"+cmbbx_SortSitizenship.SelectedValue.ToString().Substring(38) + "'";
                    FillListView();
                }
            }
        }

        //фильтр городу
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
                    whereCity = " and registration_address like " +
                        "'%" + cmbbx_IncidentFilterCity.SelectedValue.ToString().Substring(38) + "%'";
                    FillListView();
                }
            }
        }

        //поиск по ФИО, телефону, адресу
        private void txtbx_PersonSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbbx_PersonSearchby.SelectedIndex != 0)
            {
                if (cmbbx_PersonSearchby.SelectedIndex == 1)
                {
                    whereSearch = "Where [last_name] like '%" + txtbx_PersonSearch.Text + "%' " +
                        "or [first_name] like '%" + txtbx_PersonSearch.Text + "%' " +
                        "or [patronymic] like '%" + txtbx_PersonSearch.Text + "%' ";
                    FillListView();
                }
                else if (cmbbx_PersonSearchby.SelectedIndex == 2)
                {
                    whereSearch = "Where [phone] like '%" + txtbx_PersonSearch.Text + "%' ";
                    FillListView();
                }
                else
                {
                    whereSearch = "Where [registration_address] like '%" + txtbx_PersonSearch.Text + "%' ";
                    FillListView();
                }
            }
        }

        //Клик по ListView
        private void lstview_Persons_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DataRowView)lstview_Persons.SelectedItem == null)
            {
                return;
            }

            DataRowView drv = (DataRowView)lstview_Persons.SelectedItem;
            string personID = drv["id_person"].ToString();

            PersonInfoWindow PIW = new PersonInfoWindow(personID);
            PIW.ShowDialog();
        }
    }
}
