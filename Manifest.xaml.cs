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
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Data.SqlClient;

namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for Manifest.xaml
    /// </summary>
    public partial class Manifest : Window
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
        public Manifest()
        {
            InitializeComponent();
            notifier.ShowInformation("قبل از ثبت اطلاعات جدید دکمه ی " +
                "\n Delete pervious info \n" +
                "را بزنید");


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Database1Entities db = new Database1Entities();
            manifest_datagrid.ItemsSource = db.tbl_manifest.ToList();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new manifest_add().Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Database1Entities db = new Database1Entities();
            manifest_datagrid.ItemsSource = db.tbl_manifest.ToList();
        }
        string Id;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();
                var del = db.tbl_manifest.Where(c => c.Id.ToString() == Id).SingleOrDefault();
                db.tbl_manifest.Remove(del);
                db.SaveChanges();
                manifest_datagrid.ItemsSource = db.tbl_manifest.ToList();
            }
            catch { }
        }

        private void Manifest_datagrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                object Item = manifest_datagrid.SelectedItem;
                Id = (manifest_datagrid.SelectedCells[0].Column.GetCellContent(Item) as TextBlock).Text;
            }
            catch { notifier.ShowError("لطفا روی رکورد مورد نظر راست کلیک کنید"); }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }



        private void Del_rows_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();

                var all = from c in db.tbl_manifest select c;
                db.tbl_manifest.RemoveRange(all);
                db.SaveChanges();
                manifest_datagrid.ItemsSource = db.tbl_manifest.ToList();
            }
            catch { }
        }

        private void Sub_info_Click(object sender, RoutedEventArgs e)
        {
            if (master_text.Text == "" || FromPort_text.Text == "" || PortOf_text.Text == "" || Date_text.Text == "" || voyageNo_text.Text == "" || GeneralManifest_text.Text == "")
            {
                notifier.ShowError("تمامی فیلد ها را پر کنید");

            }
            else
            {
                Database1Entities db = new Database1Entities();
                tbl_ManifestInfo Nmanifest = new tbl_ManifestInfo()
                {
                    maniMaster = master_text.Text,
                    fromPortMani = FromPort_text.Text,
                    portofMani = PortOf_text.Text,
                    DateMani = Date_text.Text,
                    voyage_mani = voyageNo_text.Text,
                    general_mani = GeneralManifest_text.Text

                };

                db.tbl_ManifestInfo.Add(Nmanifest);
                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");
            }
        }

        private void Del_info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database1Entities db = new Database1Entities();

                var all = from c in db.tbl_ManifestInfo select c;
                db.tbl_ManifestInfo.RemoveRange(all);
                db.SaveChanges();
                notifier.ShowWarning("اطلاعات قبلی با موفقیت پاک شد . بعد از وارد کردن اطلاعات جدید " +
                    "\n submit info \n " +
                    "را بزنید");
            }
            catch { }
        }

        private void VoyageNo_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
