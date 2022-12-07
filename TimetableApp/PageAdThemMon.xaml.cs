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
    public partial class PageAdThemMon : ContentPage
    {
        MonHoc _monHoc;
        public PageAdThemMon()
        {
            InitializeComponent();
        }
        public PageAdThemMon(MonHoc monHoc)
        {
            InitializeComponent();
            _monHoc = monHoc;
            AddMaMon.Text = monHoc.MaMon;
            AddTenMon.Text = monHoc.TenMon;
            AddTC.Text = monHoc.SoTC.ToString();
            AddMaMon.Focus();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (_monHoc != null)
            {
               _monHoc.MaMon = AddMaMon.Text;
               _monHoc.TenMon = AddTenMon.Text;
               _monHoc.SoTC = int.Parse(AddTC.Text.ToString());
                /*post dữ liệu lên teams 23/11*/
            }
        }
    }
}