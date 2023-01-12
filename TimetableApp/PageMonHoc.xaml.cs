using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMonHoc : ContentPage
    {
        List<MonHoc> mons;
        List<Loai> loais;
        public PageMonHoc()
        {
            InitializeComponent();
            Title = "Danh sách các môn học";
            ListViewInit();
            CacLoai();
        }
        async void ListViewInit()
        {
            mons= new List<MonHoc>();
			HttpClient httpClient = new HttpClient();
            var lstMon = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/MonHoc");
            var lstMonConverted = JsonConvert.DeserializeObject<List<MonHoc>>(lstMon);
            mons = lstMonConverted;
            LstMonHoc.ItemsSource = lstMonConverted;
        }

        void CacLoai()
        {
            loais = new List<Loai>();
            loais.Add(new Loai { ID=1, Name = "Tất cả môn" });
			loais.Add(new Loai { ID=2, Name = "Môn đã đăng ký" });
			loais.Add(new Loai { ID=3, Name = "Môn chưa đăng ký" });
            Picker.ItemsSource = loais;
		}

        private void LstMonHoc_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MonHoc monHoc = (MonHoc)e.Item;
            Navigation.PushAsync(new PageChiTietLop(monHoc));
        }

		private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var texto = searchBar.Text;
			LstMonHoc.ItemsSource = mons.Where(x => x.TenMon.ToLower().Contains(texto));
		}

		private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectrow = picker.SelectedIndex;
            HttpClient httpClient = new HttpClient();
            if(selectrow == 0 )
            {
                ListViewInit();
            }    
            else if (selectrow == 1)
            {
				var lstMon = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/MonHoc");
				var lstMonConverted = JsonConvert.DeserializeObject<List<MonHoc>>(lstMon);

                List<MonHoc> mondk = new List<MonHoc>();
				var lstLopdk = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
				var lstlopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLopdk);
                foreach(MonHoc mon in lstMonConverted)
                {
					var t = 0;
					foreach (LopHoc lop in lstlopConverted)
					{
						if (lop.MaLop.Substring(0,5) == mon.MaMon)
						{
							t++;
						}
					}
					if (t == 1)
						mondk.Add(new MonHoc { MaMon = mon.MaMon, SoTC = mon.SoTC, TenMon = mon.TenMon });
				}
                LstMonHoc.ItemsSource = mondk;
			}    
            else if(selectrow == 2)
            {
				var lstMon = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/MonHoc");
				var lstMonConverted = JsonConvert.DeserializeObject<List<MonHoc>>(lstMon);

				List<MonHoc> mondk = new List<MonHoc>();
				var lstLopdk = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
				var lstlopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLopdk);
				foreach (MonHoc mon in lstMonConverted)
				{
					var t = 0;
					foreach (LopHoc lop in lstlopConverted)
					{
						if (lop.MaLop.Substring(0, 5) == mon.MaMon)
						{
                            t++;
						}
					}
                    if (t==0)
						mondk.Add(new MonHoc { MaMon = mon.MaMon, SoTC = mon.SoTC, TenMon = mon.TenMon });
				}
				LstMonHoc.ItemsSource = mondk;
			}    
		}
	}
}