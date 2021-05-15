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

            string SqlSelect = "Select login, password, role From diploma_user " +
                "Where login = '" + txtbx_login.Text + "' and password = '" + pswrdbx_password.Password + "'";
            SqlCommand command = new SqlCommand(SqlSelect, connection);

            SqlDataReader dataReader = command.ExecuteReader();
            string login = "";
            string password = "";
            string role = "";

            while (dataReader.Read())
            {
                login = dataReader.GetString(0);
                password = dataReader.GetString(1);
                role = dataReader.GetString(2);
            }

            if (login == "" || password == "")
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            else
            {
                if (role == "user")
                {
                    MainPageWindow MPW = new MainPageWindow(login);
                    MPW.Show();
                    Close();
                }
                // to do: for role == "admin"
            }
        }
    }
}
