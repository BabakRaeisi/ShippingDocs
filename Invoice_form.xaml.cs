using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class Invoice_form : Window
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
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;

        });

        public Invoice_form()
        {
            InitializeComponent();
            notifier.ShowInformation("قبل از ثبت اطلاعات جدید دکمه ی " +
                "\n Delete pervious info\n" +
                "را بزنید");


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            Database1Entities db = new Database1Entities();
            invoice_datagrid.ItemsSource = db.tbl_invoice.ToList();

        }
        private void Window_Activated(object sender, EventArgs e)
        {

            Database1Entities db = new Database1Entities();
            invoice_datagrid.ItemsSource = db.tbl_invoice.ToList();

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void ADd_Click(object sender, RoutedEventArgs e)
        {
            new InvoiceAdd().Show();
        }

        string Id;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                var del = db.tbl_invoice.Where(c => c.Id.ToString() == Id).SingleOrDefault();
                db.tbl_invoice.Remove(del);
                db.SaveChanges();
                invoice_datagrid.ItemsSource = db.tbl_invoice.ToList();
            }
            catch { notifier.ShowError("رکوردی موجود نیست"); }

        }
        private void Invoice_datagrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                object Item = invoice_datagrid.SelectedItem;
                Id = (invoice_datagrid.SelectedCells[0].Column.GetCellContent(Item) as TextBlock).Text;
            }
            catch { notifier.ShowError("لطفا روی رکورد مورد نظر راست کلیک کنید"); }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { };
        }

        private void Submit_info_Click(object sender, RoutedEventArgs e)
        {
            if (delivery_text.Text == "" || vessel_text.Text == "" || voyage_num.Text == "" || INVOICE_NUM.Text == "" || date_text.Text == "")

            {
                notifier.ShowError("لطفا تمامی فیلد ها را پر کنید");
            }
            else
            {
                try
                {
                    Database1Entities db = new Database1Entities();
                    tbl_InvInfo tinvinfo = new tbl_InvInfo()
                    {
                        delivery = delivery_text.Text,
                        vessel = vessel_text.Text,
                        voyage = int.Parse(voyage_num.Text),
                        invoice_no = INVOICE_NUM.Text,
                        date_inv = date_text.Text


                    };
                    db.tbl_InvInfo.Add(tinvinfo);
                    db.SaveChanges();
                    notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");
                }
                catch { }
            }
        }

        private void Del_info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                var all = from c in db.tbl_InvInfo select c;
                db.tbl_InvInfo.RemoveRange(all);

                db.SaveChanges();

                notifier.ShowWarning("اطلاعات قبلی با موفقیت پاک شد . بعد از وارد کردن اطلاعات جدید" +
                    "\n submit info \n" +
                    "را بزنید");
            }
            catch { }
        }

        private void Del_rows_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();

                
                var all = from c in db.tbl_invoice select c;
                db.tbl_invoice.RemoveRange(all);

                db.SaveChanges();
                invoice_datagrid.ItemsSource = db.tbl_invoice.ToList();
            }
            catch { notifier.ShowError("رکوردی موجود نیست"); }
        }



        private void Voyage_num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

}
