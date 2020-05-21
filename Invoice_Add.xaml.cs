using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
    public partial class InvoiceAdd : Window
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
        public InvoiceAdd()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (quantity_text.Text == "" || description_text.Text == "" || unitprice_text.Text == "" || amount_text.Text == "")
            { notifier.ShowError("لطفا تمامی فیلدها را پر کنید"); }
            else
            {
                Database1Entities db = new Database1Entities();
                tbl_invoice tinvoice = new tbl_invoice()
                {
                    quantity = int.Parse(quantity_text.Text),
                    description = description_text.Text,
                    unitprice = unitprice_text.Text,
                    amount = int.Parse(amount_text.Text)
                };
                db.tbl_invoice.Add(tinvoice);
                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");

                quantity_text.Text = String.Empty;
                amount_text.Text = String.Empty;

            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Quantity_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Amount_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
