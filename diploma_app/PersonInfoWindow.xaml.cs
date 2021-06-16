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
    /// Interaction logic for PersonInfoWindow.xaml
    /// </summary>
    public partial class PersonInfoWindow : Window
    {
        SqlDataAdapter adapterPerson;
        SqlDataAdapter adapterPersonAndIncidents;
        DataTable person;
        DataTable personAndIncidents;
        SqlConnection connection = new SqlConnection(App.ConnectionString);
        string id;

        public PersonInfoWindow(string PersonID)
        {
            InitializeComponent();

            id = PersonID;
        }

        private void FillDataGridPerson()
        {
            connection.Open();

            string sqlSelectQuery = "Select * From [diploma_Person] " +
                "Where [diploma_Person].[id_person] = " + id + "";

            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapterPerson = new SqlDataAdapter(command);
            person = new DataTable();
            adapterPerson.Fill(person);
            dtgrid_PersonInfo.ItemsSource = person.DefaultView;
            connection.Close();
        }

        private void FillDataGridPersonAndIncidents()
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
                "Where [diploma_Person].[id_person] = " + id + "";

            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            adapterPersonAndIncidents = new SqlDataAdapter(command);
            personAndIncidents = new DataTable();
            adapterPersonAndIncidents.Fill(personAndIncidents);
            dtgrid_PersonAndIncidentsInfo.ItemsSource = personAndIncidents.DefaultView;
            connection.Close();
        }

        private void UpdateDB()
        {
            try
            {
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapterPerson);
                adapterPerson.Update(person);

                MessageBox.Show("Сохранено.");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошбка\nВвод некорректных данных:\nПроверьте тип или длину вводимых данных");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGridPerson();
            FillDataGridPersonAndIncidents();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }
    }
}
