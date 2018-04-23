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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ServiceReference1;
using System.ServiceModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatServiceCallback
    {        
        private ChatServiceClient proxy;
        private ChatUser clientUser;
        private ChatMessage message;

        public MainWindow()
        {
            InitializeComponent();

            var instanceContext = new InstanceContext(this);
            proxy = new ChatServiceClient(instanceContext);
            
        }       

        public void NewUserConnected(ChatUser user)
        {
            if(!lbConnectedUsers.Items.Contains(user.UserName))
                lbConnectedUsers.Items.Add(user.UserName);
        }

        public void DeliverNewMessage(ChatMessage message)
        {
            string mes = $"({message.Date.ToLongTimeString()}) {message.User.UserName} says: {message.Message}";
            lbMessages.Items.Add(mes); // why does message.ToString() overloaded not work?
        }

        public void GetAllConnected(ChatUser[] users)
        {
            lbConnectedUsers.Items.Clear();
            foreach (var u in users)
                lbConnectedUsers.Items.Add(u.UserName);
        }

        private void bConnect_Click(object sender, RoutedEventArgs e)
        {
            clientUser = new ChatUser() { UserName = tbUserName.Text };
            proxy.ClientConnect(clientUser);
            lConnectionStatus.Content = "connected";
        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(tbNewMessage.Text))
            {
                message = new ChatMessage()
                {                    
                    User = clientUser,
                    Message = tbNewMessage.Text,
                    Date = DateTime.Now
                };
                proxy.SendNewMessage(message);
                tbNewMessage.Text = String.Empty;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            proxy.RemoveUser(clientUser);
        }
    }
}
