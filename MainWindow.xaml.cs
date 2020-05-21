using Stimulsoft.Report;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;


namespace PishgamanFormsAssistant
{

    public partial class MainWindow : Window
    {
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: -230,
                offsetY: -90);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(30),
                maximumNotificationCount: MaximumNotificationCount.FromCount(100));

            cfg.Dispatcher = Application.Current.Dispatcher;

        });
        public MainWindow()
        {
            InitializeComponent();

        }
        // calling database
        Database1Entities db = new Database1Entities();


        private void Popupexit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void invoice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Invoice_form().Show();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {

        }

        private void crew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new CrewList().Show();
        }

        private void manifest_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Manifest().Show();
        }

        private void cargodec_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new CargoDeclearation().Show();
        }

        private void gencargo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new ImoGeneralDeclearation().Show();
        }

        private void bill_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Bill().Show();
        }

       



        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new DeliveryOrder().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ReportMenu().Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
