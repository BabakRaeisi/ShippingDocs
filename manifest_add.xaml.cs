using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
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

namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for manifest_add.xaml
    /// </summary>
    public partial class manifest_add : Window
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

        public manifest_add()
        {
            InitializeComponent();

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (bl_text.Text == "" || weight_text.Text == "" || marks_text.Text == "" || weight_unit.Text == "" || consignee_text.Text == "" || shipper_text.Text == "" || description_tetxt.Text == "")
            { notifier.ShowError("لطفا تمامی فیلدها را پر کنید"); }
            else
            {
                Database1Entities db = new Database1Entities();

                tbl_manifest tmanifest = new tbl_manifest()
                {
                    BL_no = bl_text.Text,
                    weight = float.Parse(weight_text.Text.ToString()),
                    weight_unit = weight_unit.Text,
                    mark_remark = marks_text.Text,
                    consignee = consignee_text.Text,
                    shipper = shipper_text.Text,
                    description = description_tetxt.Text

                };
                db.tbl_manifest.Add(tmanifest);
                db.SaveChanges();

                bl_text.Text = String.Empty;
                weight_text.Text = String.Empty;
                marks_text.Text = String.Empty;

                notifier.ShowSuccess("اطلاعات با موقیت ثبت گردید");
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void weight_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
