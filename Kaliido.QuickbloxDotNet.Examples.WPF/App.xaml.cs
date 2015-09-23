using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Kaliido.QuickBlox;
using Kaliido;

namespace Kaliido.QuickbloxDotNet.Examples.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public QuickBloxClient QBClient { get; set; }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var apiCredentials = new ApiCredential("27927", "myc6f4MG-j2NRv9", "VqBP2z6VOYKDBG-");
            //var adminCredentials = new UserCredential() { UserLogin = "testuser", Password = "Password1234" };
            var adminCredentials = new UserCredential() { UserLogin = "quotes4transport", Password = "Matt2105*" };
            QuickBloxClient client = new QuickBloxClient(apiCredentials, adminCredentials);

            var response = await client.Login(adminCredentials);

            var user = await client.GetUser(adminCredentials.UserLogin);


            var users = await client.GetUsersClosest(-33.8999389, 151.1766595);

            var userTags = await client.GetUsersByTags("web");
            
        }  
        
 
        


        


    }
}
