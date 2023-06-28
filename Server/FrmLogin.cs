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

namespace Server
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text
            };
            user = UserRepository.Instance.VratiUsera(user);
            if(user != null)
            {
                UserRepository.Instance.CurrentUser = user;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Admin ne ostoji");
            }
        }
    }
}
