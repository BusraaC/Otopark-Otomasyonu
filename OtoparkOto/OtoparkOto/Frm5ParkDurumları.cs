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
    public partial class Frm5ParkDurumları : Form
    {
        public Frm5ParkDurumları()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=otoparksistemi;Integrated Security=True;Encrypt=False");
        public string SeciliParkYeriAdi { get; set; }

        private void Frm5ParkDurumları_Load(object sender, EventArgs e)
        {
            BoşParkYerleri();
            DoluParkYerleri();

        }

        private void BoşParkYerleri()
        {
            int sayac = 1;
            foreach (Control item in Controls)
            {
                if (item is Button)
                {
                    item.Text = "A" + sayac;
                    item.Name = "A" + sayac;
                    item.BackColor = Color.Green;
                    item.Click += new EventHandler(ParkYerı_Click); 
                    sayac++;
                }
            }
        }

        private void ParkYerı_Click(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;

            if (tiklananButon != null && tiklananButon.BackColor == Color.Red) 
            {
                SeciliParkYeriAdi = tiklananButon.Text;  

                Frm4AraçÇıkış frm = new Frm4AraçÇıkış();
                frm.SeciliParkYeri = SeciliParkYeriAdi;
                frm.Show();
                this.Hide();
            }
        }




        private void DoluParkYerleri()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select*from ParkDurumları", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                foreach (Control item in Controls)
                {
                    if (item is Button)
                    {
                        if (item.Text == read["ParkYerı"].ToString() && read["Durumu"].ToString() == "DOLU")
                        {
                            item.BackColor = Color.Red;
                        }
                    }
                }
            }
            baglanti.Close();
        }

        private void btnAraçÇıkış(object sender, EventArgs e)
        {
            Frm4AraçÇıkış çıkışFormu = new Frm4AraçÇıkış();
            çıkışFormu.SeciliParkYeri = SeciliParkYeriAdi;
            çıkışFormu.ShowDialog();
        }


    }
}
