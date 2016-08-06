using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinAuthTest
{
    public partial class Profile : ContentPage
    {
        public Profile(string message)
        {
            InitializeComponent();

            lblMessage.Text = message;
        }
    }
}
