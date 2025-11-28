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
                databasesys = new DaO("server=localhost;password=root;user=root;port=3306;database=bibellus");
            }
            catch (Exception Erro)
            {
                new AvisoPopup(Erro).ShowDialog();
            }
        }

        private void LoginButton(object sender, EventArgs e)
        {
            try
            {

                string[] search = databasesys.MYSQLSelect("socios", $"nome LIKE '%{User_id.Text}%' AND cpf LIKE '%{User_pwd.Text}%'");
                if (search.Length == 0 || User_id.Text == "" || User_pwd.Text == "")
                {
                    new AvisoPopup("Login errado!").ShowDialog();
                }
                else
                {
                    this.Hide();
                    new Vendas_panel().Show();
                }
            }
            catch (Exception Erro)
            {
                new AvisoPopup(Erro).ShowDialog();
            }
        }
    }
}
