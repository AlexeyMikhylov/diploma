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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_showPswrd_Click(object sender, RoutedEventArgs e) //показать пароль
        {
            if (txtbx_password.Visibility == Visibility.Collapsed && pswrdbx_password.Visibility == Visibility.Visible)
            {
                pswrdbx_password.Visibility = Visibility.Collapsed;
                txtbx_password.Visibility = Visibility.Visible;
                txtbx_password.Text = pswrdbx_password.Password;
                btn_showPswrd.Content = "👁";
            }
            else
            {
                pswrdbx_password.Visibility = Visibility.Visible;
                txtbx_password.Visibility = Visibility.Collapsed;
                pswrdbx_password.Password = txtbx_password.Text;
                btn_showPswrd.Content = "⌒";
            }
        }

        private void btn_signIn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(App.ConnectionString);
            connection.Open();

            string SqlSelect = "Select login, password, role, [last_name], [first_name], [id_official] " +
                "From [diploma_Official] " +
                "Where login = '" + txtbx_login.Text + "' and password = '" + pswrdbx_password.Password + "'";
            SqlCommand command = new SqlCommand(SqlSelect, connection);

            SqlDataReader dataReader = command.ExecuteReader();
            string login = "";
            string password = "";
            string role = "";
            string name = "";
            int id = 0;

            while (dataReader.Read())
            {
                login = dataReader.GetString(0);
                password = dataReader.GetString(1);
                role = dataReader.GetString(2);
                name = dataReader.GetString(3) + " " + dataReader.GetString(4);
                id = dataReader.GetInt32(5);
            }

            if (login == "" || password == "")
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            else
            {
                if (role == "user")
                {
                    App.CurrentUserId = id;
                    MainPageWindow MPW = new MainPageWindow(name);
                    MPW.Show();
                    Close();
                }
                // to do: for role == "admin"
            }
        }
    }
}
