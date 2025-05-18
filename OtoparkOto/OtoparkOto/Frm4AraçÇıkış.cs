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
    public partial class Frm4AraçÇıkış : Form
    {
        public Frm4AraçÇıkış()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-GKQSMMP;Initial Catalog=otoparksistemi;Integrated Security=True");
        public string SeciliParkYeri { get; set; }

        private void Frm4AraçÇıkış_Load(object sender, EventArgs e)
        {
            Plakalar();           
            DoluYerler();        

            timer1.Enabled = true;

       
            if (!string.IsNullOrEmpty(SeciliParkYeri))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT Plaka FROM ParkDurumları WHERE ParkYerı = @yer AND Durumu = 'DOLU'", baglanti);
                komut.Parameters.AddWithValue("@yer", SeciliParkYeri);
                object plaka = komut.ExecuteScalar();
                baglanti.Close();

                if (plaka != null)
                {
                    comboPlaka.SelectedItem = plaka.ToString();

                 
                    comboPlaka_SelectedIndexChanged(comboPlaka, EventArgs.Empty);
                }
            }
        }



        private void DoluYerler()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select*from ParkDurumları where Durumu ='DOLU'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
         
            baglanti.Close();
        }

        private void Plakalar()
        {
            comboPlaka.Items.Clear();

            baglanti.Open();
            SqlCommand komut = new SqlCommand(@"
             SELECT DISTINCT a.Plaka 
             FROM AracBılgılerı a
             INNER JOIN ParkDurumları p ON a.ParkYerı = p.ParkYerı
             WHERE p.Durumu = 'DOLU'", baglanti);

            SqlDataReader read = komut.ExecuteReader();

            while (read.Read())
            {
                comboPlaka.Items.Add(read["Plaka"].ToString());
            }

            read.Close();
            baglanti.Close();
        }



        private void comboPlaka_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM AracBılgılerı WHERE Plaka = @Plaka", baglanti);
            komut.Parameters.AddWithValue("@Plaka", comboPlaka.Text);
            SqlDataReader read = komut.ExecuteReader();

            if (read.Read())
            {
                txtParkYeri.Text = read["ParkYerı"] != DBNull.Value ? read["ParkYerı"].ToString() : "";
                txtMarka.Text = read["Marka"] != DBNull.Value ? read["Marka"].ToString() : "";
                txtModel.Text = read["Model"] != DBNull.Value ? read["Model"].ToString() : "";
                txtRenk.Text = read["Renk"] != DBNull.Value ? read["Renk"].ToString() : "";

                if (read["Tarih"] != DBNull.Value)
                {
                    DateTime giriş = Convert.ToDateTime(read["Tarih"]);
                    lblGirişTarihi.Text = giriş.ToString("dd.MM.yyyy HH:mm:ss");

                    DateTime çıkış = DateTime.Now;
                    TimeSpan fark = çıkış - giriş;
                    lblSüre.Text = fark.TotalHours.ToString("F2") + " saat";

                    double toplamTutar = fark.TotalHours * 7;
                    lblToplam.Text = toplamTutar.ToString("C2");
                }
                else
                {
                    lblGirişTarihi.Text = "Bilinmiyor";
                    lblSüre.Text = "0 saat";
                    lblToplam.Text = "₺0,00";
                }
            }

            read.Close();
            baglanti.Close();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lblÇıkışTarihi.Text = DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            // 1. AracBılgılerı tablosundan aracı sil
            SqlCommand komut = new SqlCommand("DELETE FROM AracBılgılerı WHERE Plaka = @Plaka", baglanti);
            komut.Parameters.AddWithValue("@Plaka", comboPlaka.Text);
            komut.ExecuteNonQuery();

            // 2. ParkDurumları tablosunda park yerini BOŞ yap ve plakayı sil
            SqlCommand komut1 = new SqlCommand("UPDATE ParkDurumları SET Durumu = 'BOŞ', Plaka = NULL WHERE ParkYerı = @ParkYerı", baglanti);
            komut1.Parameters.AddWithValue("@ParkYerı", txtParkYeri.Text);
            komut1.ExecuteNonQuery();

            baglanti.Close();

            MessageBox.Show("Araç çıkışı yapıldı.");

            // Formu temizle
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                    item.Text = "";
            }

            comboPlaka.Text = "";
            comboPlaka.Items.Clear();
            DoluYerler();
            Plakalar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
