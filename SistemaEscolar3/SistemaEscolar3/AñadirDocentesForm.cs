using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient; 

namespace SistemaEscolar3
{
    public partial class AñadirDocentesForm : UserControl
    {

        SqlConnection connect = new SqlConnection("Data Source=YHAVET\\SQLEXPRESS;Initial Catalog=Tecnica3;Integrated Security=True;Connect Timeout=30");

        public AñadirDocentesForm()
        {
            InitializeComponent();

            DatosVisualesDocente();
        }

        public void DatosVisualesDocente()
        {
            AñadirDatosDocente addTD = new AñadirDatosDocente();

            Datagrid_Docentes.DataSource = addTD.DatosDocentes();
        }

        private void BtnAñadir_docente_Click(object sender, EventArgs e)
        {

        }
    }
}
