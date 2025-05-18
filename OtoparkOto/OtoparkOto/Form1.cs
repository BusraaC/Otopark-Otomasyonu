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
    public partial class Form1 : Form
    {
        public string conString = "Data Source=DESKTOP-GKQSMMP;Initial Catalog=otoparksistemi;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection db = new SqlConnection(conString))
            {
                db.Open();
                string sorgu = "SELECT * FROM Kullanıcılar WHERE Kullanıcı = @kullanici AND Sıfre = @sifre";
                SqlCommand cmd = new SqlCommand(sorgu, db);
                cmd.Parameters.AddWithValue("@kullanici", textBox1.Text);
                cmd.Parameters.AddWithValue("@sifre", textBox2.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("GİRİŞ BAŞARILI");

                    Frm2AnaEkran anaForm = new Frm2AnaEkran();
                    anaForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("GİRİŞ BAŞARISIZ");
                }
            }
        }
    }
}

