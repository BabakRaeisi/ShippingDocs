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
    /// Interaction logic for CrewList.xaml
    /// </summary>
    public partial class CrewList : Window
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
        public CrewList()
        {
            InitializeComponent();
            notifier.ShowInformation("قبل از ثبت اطلاعات جدید دکمه ی " +
                "Delete pervious info" + "\n" +
                "را بزنید");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                crew_datagrid.ItemsSource = db.tbl_crw.ToList();
            }
            catch { }

        }
        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                crew_datagrid.ItemsSource = db.tbl_crw.ToList();
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Crew_add().Show();
        }



        private void Crew_datagrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                object Item = crew_datagrid.SelectedItem;
                Id = (crew_datagrid.SelectedCells[0].Column.GetCellContent(Item) as TextBlock).Text;
            }
            catch { notifier.ShowError("لطفا روی رکورد مورد نظر راست کلیل کنید"); }
        }
        string Id;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                var del = db.tbl_crw.Where(c => c.Id.ToString() == Id).SingleOrDefault();
                db.tbl_crw.Remove(del);
                db.SaveChanges();
                crew_datagrid.ItemsSource = db.tbl_crw.ToList();
            }
            catch { }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Delete_all_rows_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                var all = from c in db.tbl_crw select c;
                db.tbl_crw.RemoveRange(all);
                db.SaveChanges();
                crew_datagrid.ItemsSource = db.tbl_crw.ToList();
            }
            catch { }
        }

        private void Delete_info_Click(object sender, RoutedEventArgs e)
        {

            Database1Entities db = new Database1Entities();
            var all = from c in db.tbl_crwInf select c;
            db.tbl_crwInf.RemoveRange(all);
            db.SaveChanges();
            
            notifier.ShowWarning("اطلاعات قبلی با موقیت پاک شد.بعد از وارد کردن اطلاعات جدید " +
                "\n submit info \n" +
                "را بزنید");

        }

        private void Submit_info_Click(object sender, RoutedEventArgs e)
        {
            if (AcheckcheckBox.IsChecked == false & Dcheck.IsChecked == true)
            {
                ADtxt.Text = "Departure";

            }
            else if (AcheckcheckBox.IsChecked == true & Dcheck.IsChecked == false)
            {
                ADtxt.Text = "Arrival";
            }
            else if (AcheckcheckBox.IsChecked == true & Dcheck.IsChecked == true)
            {
                notifier.ShowError("فقط یک وضعیت قابل انتخاب است");
            }
            else
            {
                notifier.ShowError("ورود یا خروج را تعیین کنید");
            }
            if (call_sign.Text == "" || ship_name.Text == "" || voyage_num.Text == "" || portAD.Text == "" || flag.Text == "" || imo_num.Text == "" || lastport.Text == "" || dateAD.Text == "" || ADtxt.Text == "")
            {
                notifier.ShowError("تمامی فیلدها را پر کنید");

            }

            else
            {

                Database1Entities db = new Database1Entities();
                tbl_crwInf newcrewinf = new tbl_crwInf()
                {
                    crw_arr_depar = ADtxt.Text,
                    call_signcrw = call_sign.Text,
                    name_shipcrw = ship_name.Text,
                    voyage_crw = voyage_num.Text,
                    port_arr_dep = portAD.Text,
                    flag_crw = flag.Text,
                    imo_crw = imo_num.Text,
                    lastReportCall_crw = lastport.Text,
                    date_arr_depar = dateAD.Text,

                };
                db.tbl_crwInf.Add(newcrewinf);
                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");
            }
        }

        private void Imo_num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }



}
