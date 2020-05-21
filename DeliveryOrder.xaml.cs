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
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using System.Data.SqlClient;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DeliveryOrder : Window
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
        public DeliveryOrder()
        {


            InitializeComponent();
            notifier.ShowInformation("قبل از ثبت اطلاعات جدید دکمه ی " +
                "Delete pervious info" + "\n" +
                "را بزنید");

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Database1Entities db = new Database1Entities();
            invoice_datagrid.ItemsSource = db.tbl_deliverygrid.ToList();

        }
        private void Window_Activated(object sender, EventArgs e)
        {

            Database1Entities db = new Database1Entities();
            invoice_datagrid.ItemsSource = db.tbl_deliverygrid.ToList();

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void ADd_Click(object sender, RoutedEventArgs e)
        {
            new DeliveryAdd().Show();
        }

        string Id;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                var del = db.tbl_deliverygrid.Where(c => c.Id.ToString() == Id).SingleOrDefault();
                db.tbl_deliverygrid.Remove(del);
                db.SaveChanges();
                invoice_datagrid.ItemsSource = db.tbl_deliverygrid.ToList();
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
            this.DragMove();
        }

        private void Submit_info_Click(object sender, RoutedEventArgs e)
        {
            if (date_text.Text == "" || vessel_txt.Text == "" || voyage_txt.Text == "" || unit_txt.Text == "" || Bl_txt.Text == "" || final_des_txt.Text == "" || good_des_txt.Text == "" || director_txt.Text == "" || expiry_txt.Text == "")

            {
                notifier.ShowError("لطفا تمامی فیلدها را پر کنید");
            }
            else
            {
                try
                {
                    Database1Entities db = new Database1Entities();
                    tbl_deliveryinfo tDeliverInfo = new tbl_deliveryinfo()
                    {
                        date = date_text.Text,
                        BL_no = Bl_txt.Text,
                        director = director_txt.Text,
                        final_destination = final_des_txt.Text,
                        vessel = vessel_txt.Text,
                        voyage = voyage_txt.Text,
                        good_des = good_des_txt.Text,
                        gross_unit = unit_txt.Text,
                        delivery_exp = expiry_txt.Text


                    };
                    db.tbl_deliveryinfo.Add(tDeliverInfo);
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

                var all = from c in db.tbl_deliveryinfo select c;
                db.tbl_deliveryinfo.RemoveRange(all);
                db.SaveChanges();

                notifier.ShowWarning("اطلاعات قبلی با موفقیت ثبت شد . بعد از وارد کردن اطلاعات جدید " +
                    "\n submit info \n " +
                    "را بزنید");
            }
            catch { }
        }

        private void Del_rows_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();


                var all = from c in db.tbl_deliverygrid select c;
                db.tbl_deliverygrid.RemoveRange(all);
                db.SaveChanges();
                invoice_datagrid.ItemsSource = db.tbl_deliverygrid.ToList();
            }
            catch { notifier.ShowWarning("رکوردی موجود نیست"); }
        }



        private void Voyage_num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

}
