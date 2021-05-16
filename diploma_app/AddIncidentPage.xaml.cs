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
using System.Data.SqlClient;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for AddIncidentPage.xaml
    /// </summary>
    public partial class AddIncidentPage : Page
    {
        SqlConnection connection = new SqlConnection(App.ConnectionString);

        public AddIncidentPage()
        {
            InitializeComponent();
        }

        private void InsertIncidentAddress()
        {
            connection.Open();
            string SqlInsert = "";

            connection.Close();
        }

        private void GetIncidentAddresId()
        {
            connection.Open();
            string SqlSelect = "";

            connection.Close();
        }

        private void InsertIncident()
        {
            connection.Open();
            string SqlInsert = "";

            connection.Close();
        }

        private void InsertPerson()
        {
            connection.Open();
            string SqlInsert = "";

            connection.Close();
        }

        private void GetIncidentId()
        {
            connection.Open();
            string SqlSelect = "";

            connection.Close();
        }

        private void GetPersonId()
        {
            connection.Open();
            string SqlSelect = "";

            connection.Close();
        }

        private void InsertInvolved()
        {
            connection.Open();
            string SqlInsert = "";

            connection.Close();
        }
    }
}
