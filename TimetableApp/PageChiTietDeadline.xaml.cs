using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageChiTietDeadline : ContentPage
    {
        public PageChiTietDeadline(Deadline deadline)
        {

            InitializeComponent();
        }

        private void Add_Invoked(object sender, EventArgs e)
        {

        }
    }
}