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
    public partial class PageAdThemLop : ContentPage
    {
		private readonly LopHoc _lop;
		MonHoc _mon = new MonHoc();
		List<Phong> listphong;

		public PageAdThemLop(MonHoc monHoc)
		{
			InitializeComponent();
			Title = "Thêm lớp " + monHoc.MaMon;
			_mon = monHoc;
			Phong();
		}
		public PageAdThemLop(LopHoc lop)
		{
			InitializeComponent();
			Phong();
			Title = "Chỉnh sửa lớp ";
			_lop = lop;
			AddGV.Text = lop.GiaoVien;
			AddThu.Text = lop.Thu;
			AddTiet.Text = lop.Tiet;
			AddPhong.Title = lop.PhongHoc;
			NameLabel.Text = lop.PhongHoc;
			Date1.Date = lop.NgayBD;
			Date2.Date = lop.NgayKT;
			AddGV.Focus();
		}
		private async void Save_Clicked(object sender, EventArgs e)
		{
			if(string.IsNullOrWhiteSpace(AddGV.Text) || (string.IsNullOrWhiteSpace(AddThu.Text) || (string.IsNullOrWhiteSpace(AddTiet.Text) || (string.IsNullOrWhiteSpace(AddPhong.Title)))))
				{
				await DisplayAlert("Thông báo", "Vui lòng nhập đầy đủ thông tin!", "OK");
			}
			else 
			if (_lop != null)
			{
				_lop.GiaoVien = AddGV.Text;
				_lop.Thu = AddThu.Text;
				_lop.Tiet = AddTiet.Text;
				_lop.PhongHoc = NameLabel.Text;
				_lop.NgayBD = Date1.Date;
				_lop.NgayKT = Date2.Date;
				HttpClient httpClient = new HttpClient();
				string jsonup = JsonConvert.SerializeObject(_lop);
				StringContent stringContent = new StringContent(jsonup, Encoding.UTF8, "application/json");
				HttpResponseMessage kq;

				kq = await httpClient.PutAsync("http://www.lno-ie307.somee.com/api/LopHoc", stringContent);
				var kqthem = await kq.Content.ReadAsStringAsync();
				if (int.Parse(kqthem.ToString()) > 0)
				{
					await DisplayAlert("Thông báo", "Sửa lớp học " + _lop.MaLop.ToString() + " thành công", "OK");
					await Navigation.PopAsync();
				}
				else
					await DisplayAlert("Thông báo", "Sửa lớp học không thành công", "Thử lại");
			}

			else
			{
				LopHoc lopHoc = new LopHoc();
				lopHoc.MaMon = _mon.MaMon;
				lopHoc.GiaoVien = AddGV.Text;
				lopHoc.Thu = AddThu.Text;
				lopHoc.Tiet = AddTiet.Text;
				lopHoc.PhongHoc = NameLabel.Text;
				lopHoc.NgayBD = Date1.Date;
				lopHoc.NgayKT = Date2.Date; /*Tạo sẵn list các phòng và để picker*/


				HttpClient httpClient = new HttpClient();
				string json = JsonConvert.SerializeObject(lopHoc);
				StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage kq;

				kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/LopHoc", stringContent);
				var kqthem = await kq.Content.ReadAsStringAsync();
				if (int.Parse(kqthem.ToString()) > 0)
				{
					await DisplayAlert("Thông báo", "Thêm lớp thành công", "OK");
					await Navigation.PopAsync();
				}
				else
					await DisplayAlert("Thông báo", "Thêm lớp không thành công", "Thử lại");
			}
			
		}
		
		void Phong ()
		{
			listphong = new List<Phong>();
			listphong.Add( new Phong { Ten = "A205" } );
			listphong.Add(new Phong { Ten = "A213" });
			listphong.Add(new Phong { Ten = "GĐ1 (A1)" });
			listphong.Add(new Phong { Ten = "GĐ2 (A2)" });
			listphong.Add(new Phong { Ten = "GĐ3 (A3)" });
			listphong.Add(new Phong { Ten = "B106" });
			listphong.Add(new Phong { Ten = "B206" });
			listphong.Add(new Phong { Ten = "C113" });
			AddPhong.ItemsSource = listphong;
		}
		private void AddPhong_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectrow = picker.SelectedIndex;
			
			if (selectrow == 0)
				NameLabel.Text = listphong.First().Ten;
			else if (selectrow == 1)
				NameLabel.Text = listphong[selectrow].Ten;
			else if (selectrow == 2)
				NameLabel.Text = listphong[selectrow].Ten;
			else if (selectrow == 3)
				NameLabel.Text = listphong[selectrow].Ten;
			else if (selectrow == 4)
				NameLabel.Text = listphong[selectrow].Ten;
			else if (selectrow == 5)
				NameLabel.Text = listphong[selectrow].Ten;
			else if (selectrow == 6)
				NameLabel.Text = listphong[selectrow].Ten;
			else if (selectrow == 7)
				NameLabel.Text = listphong[selectrow].Ten;

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();



		}
		private void Date1_DateSelected(object sender, DateChangedEventArgs e)
		{
			Date1.Date = DateTime.Today;
			Date1.MaximumDate = DateTime.Now.AddMonths(7);
		}
		private void Date2_DateSelected(object sender, DateChangedEventArgs e)
		{
			Date2.MinimumDate = Date1.Date;
			Date2.MaximumDate = DateTime.Now.AddMonths(7);
		}

		
	}
	}