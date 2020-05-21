using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;


namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for InvoiceAdd.xaml
    /// </summary>
    public partial class DeliveryAdd : Window
    {
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: -230,
                offsetY: -90);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(10),
                maximumNotificationCount: MaximumNotificationCount.FromCount(100));

            cfg.Dispatcher = Application.Current.Dispatcher;

        });
        public DeliveryAdd()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (gross_text.Text == "" || description_text.Text == "" || container_mark_text.Text == "")
            { notifier.ShowError("لطفا تمامی فیلدها را پر کنید"); }
            else
            {
                Database1Entities db = new Database1Entities();
                tbl_deliverygrid tDeliver = new tbl_deliverygrid()
                {
                    container_marks = container_mark_text.Text,
                    des_goods = description_text.Text,
                    gross = gross_text.Text,

                };
                db.tbl_deliverygrid.Add(tDeliver);
                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");
                gross_text.Text = String.Empty;
                description_text.Text = String.Empty;
                container_mark_text.Text = String.Empty;

            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }




    }
}
