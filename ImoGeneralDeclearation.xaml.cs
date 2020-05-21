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
using System.Data.SqlClient;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for Imo General Declearation.xaml
    /// </summary>
    public partial class ImoGeneralDeclearation : Window
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
        public ImoGeneralDeclearation()
        {
            InitializeComponent();
            notifier.ShowInformation("قبل از ثبت اطلاعات جدید دکمه ی " +
                "\n Delete pervious info \n" +
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
            if (Acheck.IsChecked == false & Dcheck.IsChecked == true)
            {
                ADtxt.Text = "Departure";

            }
            else if (Acheck.IsChecked == true & Dcheck.IsChecked == false)
            {
                ADtxt.Text = "Arrival";
            }
            else if (Acheck.IsChecked == true & Dcheck.IsChecked == true)
            {
                notifier.ShowError("لطفا یک گزینه را انتخاب کنید");
            }
            else if (Acheck.IsChecked == false & Dcheck.IsChecked == false)
            {
                notifier.ShowError("وضعیت ورود یا خروج را مشخص کنید");
            }
            else { }

            Database1Entities db = new Database1Entities();
            tbl_ImoGen tGen = new tbl_ImoGen()

            {
                nameship = nameship_text.Text,
                ArrDepa = ADtxt.Text,
                flag = flag_text.Text,
                voyage_num = voyage_num_txt.Text,
                imo_num = imo_text.Text,
                callsign = call_sign_text.Text,
                master_name = master_text.Text,
                agent_ship = agent_text.Text,
                portA = porta_text.Text,
                portdes = portd_text.Text,
                dateAD = dateAD_text.Text,
                brifeVoy = brife_text.Text,
                arrivedDes = portDeparture_text.Text,
                position_ship = shipposition_text.Text,
                passenger_number = passenger_num_tetxt.Text,
                crew_number = numCrew_text.Text,
                gross_ton = gross_text.Text,
                net_ton = nettonnage_text.Text,
                brife_cargo = brifeCargo_text.Text,
                registry = registry_text.Text,
                remarks = remark_tetxt.Text,
                attached_doc = attachedDocpage_text.Text,
                ship_dec = storedeclaration_text.Text,
                passenger_list = passengerlistpage_text.Text,
                cargo_dec = cargoDEclaration_text.Text,
                crew_list = crewlist_text.Text,
                crew_effect = effect_tetxt.Text,
                maritime_dec = maritime_tetxt.Text,
                ships_req = requirement_tetxt.Text

            };
            db.tbl_ImoGen.Add(tGen);
            db.SaveChanges();
            notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Database1Entities db = new Database1Entities();

            var all = from c in db.tbl_ImoGen select c;
            db.tbl_ImoGen.RemoveRange(all);
            db.SaveChanges();
            notifier.ShowWarning("اطلاعات قبلی با موفقیت حذف گردید . اطلاعات جدید را وارد و سپس " +
                "\n submit \n" +
                "را بزنید");
        }
    }
}
