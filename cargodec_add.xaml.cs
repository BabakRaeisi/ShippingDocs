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
using System.Text.RegularExpressions;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Data.SqlClient;

namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for cargodec_add.xaml
    /// </summary>
    public partial class cargodec_add : Window
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
        public cargodec_add()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tedad_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Vazn_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        //adding
        private void Submit_Click(object sender, RoutedEventArgs e)

        {

            if (vazn_text.Text == "" || alaem_text.Text == "" || kala_text.Text == "" || vazn_unit_txt.Text == "" || no_text.Text == "" || tedad_text.Text == "" || andaze_text.Text == "" || sharh.Text == "")
            { notifier.ShowError("لطفا تمامی فیلدها را پر کنید"); }
            else
            {
                Database1Entities db = new Database1Entities();
                tbl_cargo tcargo = new tbl_cargo()
                {
                    alaem_va_shomareha = alaem_text.Text,
                    kala_girande = kala_text.Text,
                    andaze = andaze_text.Text,
                    noe_baste = no_text.Text,
                    vazn_nakhales = float.Parse(vazn_text.Text),
                    vazn_unit = vazn_unit_txt.Text,
                    tedad = int.Parse(tedad_text.Text),
                    sharh_kala = sharh.Text

                };


                db.tbl_cargo.Add(tcargo);

                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");


                alaem_text.Text = String.Empty;

                vazn_text.Text = String.Empty;




            }
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


    }
}
