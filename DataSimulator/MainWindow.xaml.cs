using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Infrastructure.Data;
using Infrastructure.Redis;

namespace DataSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RedisClient client = RedisClientFactory.GetRedisClient();

            client.Set("wkydev:message", "hello world");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RedisClient client = RedisClientFactory.GetRedisClient();

            MessageBox.Show( client.Get("wkydev:message"));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RedisTypedClient<Setting> client = RedisClientFactory.GetRedisTypedClient<Setting>();
            Setting test  = new Setting { SettingId = 111, Name = "Test", Value = "TestValue"};
            client.Store(test);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RedisTypedClient<Setting> client = RedisClientFactory.GetRedisTypedClient<Setting>();
            var test = client.GetById(111);
            MessageBox.Show(string.Format("Name: {0}  Value: {1}", test.Name, test.Value));

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                using (AppContext db =new AppContext())
                {
                    var setting = db.Settings.Where(s => s.SettingId == 1).SingleOrDefault();
                    setting = db.Settings.Where(s => s.SettingId == 2).SingleOrDefault();
                    setting = db.Settings.Where(s => s.SettingId == 3).SingleOrDefault();
                    setting = db.Settings.Where(s => s.SettingId == 4).SingleOrDefault();
                    setting = db.Settings.Where(s => s.SettingId == 5).SingleOrDefault();
                }
            }

            TimeSpan elapsedTime = DateTime.Now.Subtract(start);

            MessageBox.Show(string.Format("Database: {0}", elapsedTime.TotalSeconds));

            RedisTypedClient<Setting> client = RedisClientFactory.GetRedisTypedClient<Setting>();

            using (AppContext db = new AppContext())
            {
                var setting = db.Settings.Where(s => s.SettingId == 1).SingleOrDefault();
                client.Store(setting);
                setting = db.Settings.Where(s => s.SettingId == 2).SingleOrDefault();
                client.Store(setting);
                setting = db.Settings.Where(s => s.SettingId == 3).SingleOrDefault();
                client.Store(setting);
                setting = db.Settings.Where(s => s.SettingId == 4).SingleOrDefault();
                client.Store(setting);
                setting = db.Settings.Where(s => s.SettingId == 5).SingleOrDefault();
                client.Store(setting);

            }
            start = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {

                var setting = client.GetById(1);
                setting = client.GetById(2);
                setting = client.GetById(3);
                setting = client.GetById(4);
                setting = client.GetById(5);

            }

            elapsedTime = DateTime.Now.Subtract(start);

            MessageBox.Show(string.Format("Redis Cache: {0}", elapsedTime.TotalSeconds));

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var client = RedisClientFactory.GetRedisClient();
            client.Subscribe("wkydev:news", (key, value) =>
                {
                    
                    this.Message = value;
                });
        }


        private string _message = string.Empty;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var client = RedisClientFactory.GetRedisTypedClient<EmailMessage>();

            EmailMessage message = new EmailMessage
            {
                To = "someone@foo.com",
                From = "from@foo.com",
                Subject = DateTime.Now.ToLongTimeString()
            };
            client.Push("wkydev:emailqueue", message);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var client = RedisClientFactory.GetRedisTypedClient<EmailMessage>();

            var message = client.Pop("wkydev:emailqueue");

            if (message != null)
                MessageBox.Show(message.Subject);
        }

        private int currentTeamIndex = 0;
        private System.Timers.Timer scoreTimer;
        private RedisTypedClient<ScoreUpdate> scoreRedisClient;
        private string[] teamIds;
        private void GenerateScoreUpdates_Click(object sender, RoutedEventArgs e)
        {

            scoreRedisClient = RedisClientFactory.GetRedisTypedClient<ScoreUpdate>();

            teamIds = RedisClientFactory.GetRedisTypedClient<Team>().GetAllIds();

            scoreTimer = new System.Timers.Timer(500);
            scoreTimer.Elapsed += scoreTimer_Elapsed;
            scoreTimer.Enabled = true;
        }

        void scoreTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (currentTeamIndex >= teamIds.Length) currentTeamIndex = 0;
            Random pointsGenerator = new Random();

            scoreRedisClient.Publish("scoreupdate", new ScoreUpdate { TeamId = Convert.ToInt32( teamIds[currentTeamIndex]), Points = pointsGenerator.Next(8) });

            currentTeamIndex++;
        }

        private void StopGenerateScoreUpdates_Click(object sender, RoutedEventArgs e)
        {
            if (scoreTimer!= null)
            {
                scoreTimer.Enabled = false;
                scoreTimer = null;
            }
        }


    }
}
