using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bibellus_form_1._0
{
    public partial class AvisoPopup : Form
    {
        public AvisoPopup(Exception erro)
        {
            InitializeComponent();
            richTextBox1.Text = erro.Message;
        }
        public AvisoPopup(string mensagem)
        {
            InitializeComponent();
            richTextBox1.Text = mensagem;
        }
    }
}
