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
using Repository;
using Common;
using System.Threading;

namespace Server
{
    public partial class FrmServer : Form
    {
        Server server = new Server();
        public FrmServer()
        {
            InitializeComponent();
            lblUser.Text = $"Admin: {UserRepository.Instance.CurrentUser.Ime} {UserRepository.Instance.CurrentUser.Prezime}";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            Thread serverskaNit = new Thread(server.HandleClients);
            serverskaNit.IsBackground = true;
            serverskaNit.Start();
            if (!Validacija())
            {
                MessageBox.Show("Pogresan unos");
                foreach (Control txt in this.Controls)
                {
                    if(txt is TextBox)
                    {
                        txt.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("Igra je startovana");
            }
        }

        private bool Validacija()
        {
            int brojac = 0;
            int n;
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    string txtBox = "txtBroj" + i.ToString() + j.ToString();
                    if (int.TryParse(Controls.Find(txtBox, true)[0].Text, out n))
                    {
                        server.Polja.Add(n);
                        brojac++;
                    }
                    else
                    {
                        server.Polja.Add(null);
                    }
                }
            }
            if (brojac != 3)
                return false;
            if (server.Polja.Where(x => x != null).ToList().Distinct().Count() != server.Polja.Where(x => x != null).ToList().Count())
                return false;
            return true;
        }
    }
}
