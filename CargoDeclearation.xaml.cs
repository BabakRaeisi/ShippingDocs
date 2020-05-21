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
using System.Data.SqlClient;
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
    /// Interaction logic for cargo declearation.xaml
    /// </summary>
    public partial class CargoDeclearation : Window
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
        public CargoDeclearation()
        {
            InitializeComponent();
            notifier.ShowInformation("\nقبل از ثبت اطلاعات جدید دکمه ی " +
                "Delete pervious info" + "\n" +
                "را بزنید");

        }

        private void IMO_num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Database1Entities db = new Database1Entities();
            cargo_datagrid.ItemsSource = db.tbl_cargo.ToList();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Database1Entities db = new Database1Entities();
            cargo_datagrid.ItemsSource = db.tbl_cargo.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new cargodec_add().Show();
        }

        
        string Id;
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string constr = "metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlServerCe.4.0;provider connection string=&quot;Data Source=";
                constr = constr + System.Reflection.Assembly.GetExecutingAssembly().Location + "\\Debug\\PishDB.sdf" + "; Max Database Size = 4000 & quot;  providerName = 'System.Data.EntityClient' />";

                MessageBox.Show(constr);
                Database1Entities db = new Database1Entities();
                    var del = db.tbl_cargo.Where(c => c.Id.ToString() == Id).SingleOrDefault();
                db.tbl_cargo.Remove(del);
                db.SaveChanges();
                cargo_datagrid.ItemsSource = db.tbl_cargo.ToList();
            }
            catch { notifier.ShowError("رکوردی موجود نیست"); }

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Cargo_datagrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                object Item = cargo_datagrid.SelectedItem;
                Id = (cargo_datagrid.SelectedCells[0].Column.GetCellContent(Item) as TextBlock).Text;
            }
            catch { notifier.ShowError("لطفا روی رکورد مورد نظر کلیک کنید"); }
        }


        private void Submit_info_Click(object sender, RoutedEventArgs e)
        {
            if (Acheck.IsChecked == false & Dcheck.IsChecked == true)
            {
                txtblck_AD.Text = "Departure";

            }
            else if (Acheck.IsChecked == true & Dcheck.IsChecked == false)
            {
                txtblck_AD.Text = "Arrival";
            }
            else if (Acheck.IsChecked == true & Dcheck.IsChecked == true)
            {
                notifier.ShowError("یک وضعیت فقط قابل انتخاب است");
            }
            else if (Acheck.IsChecked == false & Dcheck.IsChecked == false)
            {
                notifier.ShowError("لطفا ورود یا خروج را تعیین کنید");

            }

            if (call_sign_txt.Text == "" || IMO_num.Text == "" || ship_name.Text == "" || portAD.Text == "" || flag_text.Text == "" || masters.Text == "" || lastport.Text == "" || dateAD.Text == "" || txtblck_AD.Text == "")
            {
                notifier.ShowError("لطفا تمامی فیلد ها را پر کنید");
            }
            else
            {
                Database1Entities db = new Database1Entities();
                tbl_CargoInf tcargoInf = new tbl_CargoInf()
                {
                    call_sign = call_sign_txt.Text,
                    name_ship = ship_name.Text,
                    imo_number = int.Parse(IMO_num.Text),
                    port_report = portAD.Text,
                    flag_ship = flag_text.Text,
                    masters_name = masters.Text,
                    portOfLoading = lastport.Text,
                    portOfDischarge = dateAD.Text,
                    ArrivalDeparture = txtblck_AD.Text


                };

                db.tbl_CargoInf.Add(tcargoInf);
                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");
            }


        }

        private void DEl_info_Click(object sender, RoutedEventArgs e)
        {
            Database1Entities db = new Database1Entities();
            var all = from c in db.tbl_CargoInf select c;
            db.tbl_CargoInf.RemoveRange(all);
            
            db.SaveChanges();
            notifier.ShowWarning("اطلاعات قبلی با موفقیت پاک شد. بعد از اضافه کردن اطلاعات جدید " +
                "\n submit info \n" +
                "را بزنید");
        }

        private void DEl_all_Click(object sender, RoutedEventArgs e)
        {
            Database1Entities db = new Database1Entities();
            var all = from c in db.tbl_cargo select c;
            db.tbl_cargo.RemoveRange(all);
            
            db.SaveChanges();
            cargo_datagrid.ItemsSource = db.tbl_cargo.ToList();
        }


    }
}
