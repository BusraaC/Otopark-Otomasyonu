using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OtoparkOto
{
    public partial class Frm3AraçGiriş : Form
    {
        public Frm3AraçGiriş()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-GKQSMMP;Initial Catalog=otoparksistemi;Integrated Security=True");


        private void Frm3AraçGiriş_Load(object sender, EventArgs e)
        {
            BoşAraçlar();
            MarkaGetir();
            if (comboMarka.Items.Count > 0)
            {
               // comboMarka.SelectedIndex = 0; // otomatik ilk markayı seç
                //ModelGetir(comboMarka.SelectedItem.ToString());
            }


        }

        private void MarkaGetir()
        {
            comboMarka.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT DISTINCT Marka FROM AracBılgılerı", baglanti); // ya da Markalar tablosu varsa oradan
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboMarka.Items.Add(dr["Marka"].ToString());
            }
             
             baglanti.Close();           
        }

        private void ModelGetir(string secilenMarka)
        {
            comboModel.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT DISTINCT Model FROM AracBılgılerı WHERE Marka = @Marka", baglanti);
            komut.Parameters.AddWithValue("@Marka", secilenMarka);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboModel.Items.Add(dr["Model"].ToString());
            }
            baglanti.Close();
        }


        private void BoşAraçlar()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select*from ParkDurumları WHERE Durumu='BOŞ'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboParkYerı.Items.Add(read["ParkYerı"].ToString());
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("INSERT INTO Musterıler (Ad, Soyad,Telefon) VALUES (@Ad, @Soyad,@Telefon)", baglanti);
            komut1.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut1.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut1.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
            


            komut1.ExecuteNonQuery();

      
            SqlCommand komut2 = new SqlCommand("INSERT INTO AracBılgılerı (Plaka, Renk, Marka, Model, ParkYerı,Tarih) VALUES (@Plaka, @Renk, @Marka, @Model, @ParkYerı,@Tarih)", baglanti);
            komut2.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            komut2.Parameters.AddWithValue("@Renk", txtRenk.Text);
            komut2.Parameters.AddWithValue("@Marka", comboMarka.Text);
            komut2.Parameters.AddWithValue("@Model", comboModel.Text);
            komut2.Parameters.AddWithValue("@ParkYerı", comboParkYerı.Text);
            DateTime tarih;
            if (DateTime.TryParse(txtTarih.Text, out tarih))
            {
                komut2.Parameters.AddWithValue("@Tarih", tarih);
            }
            else
            {
                MessageBox.Show("Geçerli bir tarih girin (örn. 15.05.2025)");
                baglanti.Close();
                return;
            }


            komut2.ExecuteNonQuery();

            SqlCommand komut4 = new SqlCommand("INSERT INTO ParkDurumları (ParkYerı, Durumu, Plaka) VALUES (@ParkYerı, 'DOLU', @Plaka)", baglanti);
            komut4.Parameters.AddWithValue("@ParkYerı", comboParkYerı.Text);
            komut4.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            komut4.ExecuteNonQuery();

          

            // Park yerini güncelle
            SqlCommand komut3 = new SqlCommand("UPDATE ParkDurumları SET Durumu = 'DOLU' WHERE ParkYerı = @ParkYeri", baglanti);
            komut3.Parameters.AddWithValue("@ParkYeri", comboParkYerı.Text);
            komut3.ExecuteNonQuery();

            baglanti.Close();

            MessageBox.Show("Araç kaydı oluşturuldu.", "Kayıt");

            // Form temizleme
            comboParkYerı.Items.Clear();
            comboMarka.Items.Clear();
            BoşAraçlar();

            foreach (Control item in grupKişi.Controls)
                if (item is TextBox) item.Text = "";

            foreach (Control item in grupAraç.Controls)
                if (item is TextBox) item.Text = "";

            foreach (Control item in grupAraç.Controls)
                if (item is ComboBox) item.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboMarka_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModelGetir(comboMarka.SelectedItem.ToString());
        }
    }
}
