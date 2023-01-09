using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTKBSV : ContentPage
    {
        public PageTKBSV()
        {
            InitializeComponent();
            timeTable.ShownDate = DateTime.Today;
            timeTable.SelectedDate = DateTime.Today;
            BindingContext = new PageTKBSV_VM();
        }
    }
}