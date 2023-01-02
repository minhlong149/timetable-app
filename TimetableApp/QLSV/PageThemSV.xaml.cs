using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp.QLSV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageThemSV : ContentPage
    {
        public PageThemSV()
        {
            InitializeComponent();
        }
        public PageThemSV(string MaLop)
        {
            InitializeComponent();
            txtClass.Text = MaLop;
        }
    }
}