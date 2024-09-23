using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaEscolar3
{
    public partial class MainFormulario : Form
    {
        public MainFormulario()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("¿Estas seguro que quieres salir del sistema?", "Mensaje de confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {

                FormularioInicioSesion lForm = new FormularioInicioSesion();
                lForm.Show();
                this.Hide();
            }
        }
    }
}
