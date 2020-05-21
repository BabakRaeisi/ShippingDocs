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
using System.Data.SqlClient;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;


namespace PishgamanFormsAssistant
{
    /// <summary>
    /// Interaction logic for Crew_add.xaml
    /// </summary>
    public partial class Crew_add : Window


    {
        //inputs defined 
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

        public Crew_add()
        {
            InitializeComponent();


        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (name_text.Text == "" || rank_text.Text == "" || nationality_text.Text == "" || cdcNUM_text.Text == "" || CDCExpiry_text.Text == "" || passportNum_text.Text == "" || passport_expiry_tetxt.Text == "" || datebirth_text.Text == "" || placebirth_text.Text == "")

            {
                notifier.ShowError("لطفا تمامی فیلدها را پر کنید");

            }
            else
            {
                Database1Entities db = new Database1Entities();
                tbl_crw tcrew = new tbl_crw()
                {
                    name = name_text.Text,
                    ranking = rank_text.Text,
                    nationality = nationality_text.Text,
                    cdc_num = cdcNUM_text.Text,
                    cdc_exp = cdcNUM_text.Text,
                    passport_num = passportNum_text.Text,
                    passport_exp = passport_expiry_tetxt.Text,
                    date_birth = datebirth_text.Text,
                    birthplace = placebirth_text.Text
                };
                db.tbl_crw.Add(tcrew);
                db.SaveChanges();
                notifier.ShowSuccess("اطلاعات با موفقیت ثبت گردید");

                name_text.Text = String.Empty;
                rank_text.Text = String.Empty;
                nationality_text.Text = String.Empty;
                cdcNUM_text.Text = String.Empty;
                CDCExpiry_text.Text = String.Empty;
                passportNum_text.Text = String.Empty;
                passport_expiry_tetxt.Text = String.Empty;
                datebirth_text.Text = String.Empty;
                placebirth_text.Text = String.Empty;




            }


            //update function later on .... 
            //else if {}

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Database1Entities db = new Database1Entities();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


    }
}