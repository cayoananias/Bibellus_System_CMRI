using System;
using System.Windows.Forms;
using Bibellus_BD_Csharp;

namespace Bibellus_form_1._0
{
    public partial class Login : Form
    {
        static DaO databasesys;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_shown(object sender, EventArgs e)
        {
            try
            {
                databasesys = new DaO("server=localhost;user=root;port=337;database=bibellus");
            }
            catch (Exception Erro)
            {
                new AvisoPopup(Erro).ShowDialog();
            }
        }
    }
}
