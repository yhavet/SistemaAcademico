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
using System.IO;

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
            if (Id_Docente.Text == ""
                || NombreCompleto_docente.Text == ""
                || generos_docente.Text == ""
                || direccion_docente.Text == ""
                || ciudad_docente.Text == ""
                || status_docente.Text == ""
                || Cursos_docente.Text == ""
                || foto_docente == null
                || imagePath == null)
            {
                MessageBox.Show("Por favor rellene todos los campos en blanco", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open) // Abre conexión solo si no está abierta
                {
                    try
                    {
                        connect.Open();

                        string ComprobarIdDocente = "SELECT COUNT(*) FROM docentes WHERE id_docente = @IdDocente";

                        using (SqlCommand checkTID = new SqlCommand(ComprobarIdDocente, connect))
                        {
                            checkTID.Parameters.AddWithValue("@IdDocente", Id_Docente.Text.Trim());
                            int count = (int)checkTID.ExecuteScalar();

                            if (count >= 1)
                            {
                                MessageBox.Show("Docente ID: " + Id_Docente.Text.Trim() + " ya existe",
                                    "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;

                                string InsertarDatos = "INSERT INTO docentes " +
                                    "(id_docente, nombre_docente, genero_docente, direccion_docente, " +
                                    "foto_docente, cursos_docente, status_docente, insertar_fecha) " +
                                    "VALUES (@id_docente, @NombreDocente, @GeneroDocente, @DireccionDocente, " +
                                    "@ImagenesDocente, @CursosDocentes, @StatusDocentes, @InsertarFecha)";

                                string path = Path.Combine(@"C:\Users\Yhavet\Source\Repos\yhavet\SistemaAcademico\SistemaEscolar3\SistemaEscolar3\Directorio_Docentes\", Id_Docente.Text.Trim() + ".jpg");
                                string directoryPath = Path.GetDirectoryName(path);

                                if (!Directory.Exists(directoryPath)) // Crear directorio si no existe
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                File.Copy(imagePath, path, true);

                                using (SqlCommand cmd = new SqlCommand(InsertarDatos, connect))
                                {
                                    cmd.Parameters.AddWithValue("@id_docente", Id_Docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@NombreDocente", NombreCompleto_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@GeneroDocente", generos_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@DireccionDocente", direccion_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@StatusDocentes", status_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ImagenesDocente", path.Trim());
                                    cmd.Parameters.AddWithValue("@CursosDocentes", Cursos_docente.Text.Trim());
                                    //cmd.Parameters.AddWithValue("@InsertarFecha", today.ToString());
                                    cmd.Parameters.AddWithValue("@InsertarFecha", today.ToString("yyyy-MM-dd"));


                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Datos insertados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Actualiza la visualización de los datos después de insertar
                                    DatosVisualesDocente();

                                    // Limpia los campos
                                    LimpiarCampos();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al conectarse a la base de datos: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close(); // Cierra la conexión
                    }
                }
            }
        }

        private void LimpiarCampos() // Método para limpiar campos
        {
            Id_Docente.Text = "";
            NombreCompleto_docente.Text = "";
            generos_docente.Text = "";
            direccion_docente.Text = "";
            ciudad_docente.Text = "";
            status_docente.Text = "";
            Cursos_docente.Text = "";
            foto_docente.Image = null;
            imagePath = null;
        }

        private string imagePath;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.jpg; *.png)|*.jpg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                foto_docente.ImageLocation = imagePath;
            }
        }
    }
}
