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
using System.Windows.Threading;

namespace diploma_app
{
    /// <summary>
    /// Interaction logic for MainPageWindow.xaml
    /// </summary>
    public partial class MainPageWindow : Window
    {
        public MainPageWindow(string userLogin)
        {
            InitializeComponent();

            lbl_user.Content = userLogin;

            MainFrame.Navigate(new StartPage());

            Manager.MainFrame = MainFrame;
        }

        private void btn_signOut_Click(object sender, RoutedEventArgs e)
        {
            string message = "Вы хотите выйти из учетной записи?";
            string caption = "Выход";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result;

            result = Convert.ToString(MessageBox.Show(message, caption, buttons,
            MessageBoxImage.Question));

            if (result == Convert.ToString(MessageBoxResult.Yes))
            {
                MainWindow SignIn = new MainWindow();
                SignIn.Show();
                this.Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string emoji = "";

            string dateTime = DateTime.Now.ToString("HH");
            if (Convert.ToInt32(dateTime) >= 4 && Convert.ToInt32(dateTime) <= 11)
            {
                emoji = "🌤";
            }
            else if (Convert.ToInt32(dateTime) > 12 && Convert.ToInt32(dateTime) <= 16)
            {
                emoji = "☀";
            }
            else if (Convert.ToInt32(dateTime) > 17 && Convert.ToInt32(dateTime) <= 23)
            {
                emoji = "⛅";
            }
            else
            {
                emoji = "🌙";
            }

            // Обновляем лейбл показывающий текущие день и время
            lbl_dayInfo.Content = DateTime.Now.ToString("dddd") + ", " + DateTime.Now.ToString("HH:mm:ss:ff") + " " + DateTime.Now.ToString("dd.MM.yyyy") + " " + emoji;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //таймер для установления текущей даты и времени
            InitializeComponent();
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += Timer_Tick;
            LiveTime.Start();

            /* \/ graph \/ */
            /*double[] x = new double[200];
            for (int i = 0; i < x.Length; i++)
                x[i] = 3.1415 * i / (x.Length - 1);

            for (int i = 0; i < 25; i++)
            {
                var lg = new LineGraph();
                lines.Children.Add(lg);
                lg.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(i * 10), 0));
                lg.Description = String.Format("Data series {0}", i + 1);
                lg.StrokeThickness = 2;
                lg.Plot(x, x.Select(v => Math.Sin(v + i / 10.0)).ToArray());
            }*/
        }

        public class VisibilityToCheckedConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return ((Visibility)value) == Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
