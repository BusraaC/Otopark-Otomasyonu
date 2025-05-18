using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtoparkOto
{
    static class Programm
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Hata burada oluşuyordu, artık Form ismini açıkça belirtiyoruz
            Application.Run(new OtoparkOto.Form1());
        }
    }
}

