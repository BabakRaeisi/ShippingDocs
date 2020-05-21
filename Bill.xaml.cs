using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Data.SqlServerCe;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Data.Entity;

namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for Imo General Declearation.xaml
    /// </summary>
    public partial class Bill : Window
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








        public Bill()
        {
            InitializeComponent();
            notifier.ShowInformation("قبل از ثبت اطلاعات جدید دکمه ی " +
                "Delete pervious info" + "\n" +
                "را بزنید");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             Database1Entities  db = new  Database1Entities();
            tbl_bill tbill = new tbl_bill
            {
                bl_num = bl_txt.Text,
                shipper_name = shipper_txt.Text,
                shipper_tel = ship_tel_txt.Text,
                shipper_adr = shiper_ad_txt.Text,
                consignee_name_ = con_txt.Text,
                consignee_tel = con_tel_txt.Text,
                notify_adr = noti_txt.Text,
                vessel = vesel_txt.Text,
                voyage_num = voyage_txt.Text,
                port_del = del_txt.Text,
                port_load = load_txt.Text,
                port_dis = dis_txt.Text,
                shipper_good = goods_txt.Text,
                gross = gross_txt.Text,
                time_used = timeload_txt.Text,
                place_issue = place_issue_txt.Text,
                name_agent_dis = agent_dis_txt.Text,
                agent_dis_add = agent_add_txt.Text,
                phone_tel = agent_phone.Text,
                cpt = cpt_txt.Text

            };
            db.tbl_bill.Add(tbill);
            db.SaveChanges();
            notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            Database1Entities db = new Database1Entities();

            var all = from c in db.tbl_bill select c;
            db.tbl_bill.RemoveRange(all);
            db.SaveChanges();


            notifier.ShowWarning("اطلاعات قبلی با موفقیت پاک شد .بعد از اضافه کردن اطلاعات جدید " +
               "\n submit info \n" +
               "را بزنید");
        }
    }
}

