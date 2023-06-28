using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain;
using Common;
using Repository;
using System.Diagnostics;

namespace KorisnickiInterfejs
{
    public partial class FrmKlijent : Form
    {
        List<int> kombinacija = new List<int>();
        int brojPogodaka = 0;
        int brojPokusaja = 0;
        public FrmKlijent()
        {
            InitializeComponent();
            Communication.Instance.Connect();
        }

        private void txtBroj11_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj11, 0);
        }

        private void txtBroj12_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj12, 1);
        }

        private void txtBroj13_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj13, 2);
        }

        private void txtBroj21_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj21, 3);
        }

        private void txtBroj22_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj22, 4);
        }

        private void txtBroj23_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj23, 5);
        }

        private void txtBroj31_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj31, 6);
        }

        private void txtBroj32_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj32, 7);
        }

        private void txtBroj33_Click(object sender, EventArgs e)
        {
            Obrada(txtBroj33, 8);
        }

        private void Obrada(TextBox txt, int pozicija)
        {
            Igra igra = SaljiBroj(pozicija);
            txt.Text = igra.Vrijednost.ToString();
            txt.Enabled = false;
            if(txt.Text != "-1")
            {
                kombinacija.Add(int.Parse(txt.Text));
                brojPogodaka++;
            }
            brojPokusaja++;
            if(brojPogodaka == 3)
            {
                MessageBox.Show("Pobijedili ste. Vasa kombinacija je: " + VratiKombinaciju(kombinacija));
                Environment.Exit(0);
            }
            if(brojPokusaja == 5)
            {
                List<int?> odgovori = VratiOdgovore();
                int brojac = 0;
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        string textbox = "txtBroj" + i.ToString() + j.ToString();
                        Controls.Find(textbox, true)[0].Text = (odgovori[brojac] ?? -1).ToString();
                        brojac++;
                        Controls.Find(textbox, true)[0].Enabled = false;
                    }
                }
                MessageBox.Show("Izgubili ste");
                Environment.Exit(0);
            }
        }

        private List<int?> VratiOdgovore()
        {
            Igra igra;
            Request request = new Request
            {
                Operations = Operations.VratiRezultat
            };
            try
            {
                igra = Communication.Instance.SendRequests<Igra>(request);
                return igra.Polja;
            }
            catch(Exception)
            {
                 throw;
            }
        }

        private string VratiKombinaciju(List<int> kombinacija)
        {
            kombinacija.Sort();
            return kombinacija[1].ToString() + kombinacija[2].ToString() + kombinacija[0].ToString();
        }

        private Igra SaljiBroj(int pozicija)
        {
            Igra igra = new Igra
            {
                Pozicija = pozicija
            };
            Request request = new Request
            {
                RequestObject = igra,
                Operations = Operations.Igra
            };
            try
            {
                igra = Communication.Instance.SendRequests<Igra>(request);
                return igra;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
