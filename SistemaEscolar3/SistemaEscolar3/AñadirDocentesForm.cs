using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace SistemaEscolar3
{
    public partial class AñadirDocentesForm : UserControl
    {
        // Definición de la conexión a la base de datos usando SQL Server
        SqlConnection connect = new SqlConnection("Data Source=YHAVET\\SQLEXPRESS;Initial Catalog=Tecnica3;Integrated Security=True;Connect Timeout=30");

        // Constructor del formulario
        public AñadirDocentesForm()
        {
            InitializeComponent();
            // Llama al método que carga los datos visuales de los docentes en el DataGrid
            DatosVisualesDocente();
        }

        // Método para cargar los datos de los docentes en el DataGrid


        public void DatosVisualesDocente()
        {
            try
            {
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open(); // Abre la conexión si no está abierta
                }

                // Consulta SQL para obtener los datos de los docentes
                string query = "SELECT id_docente, nombre_docente, genero_docente, direccion_docente, foto_docente, status_docente, insertar_fecha FROM docentes";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);

                // Crea una tabla para almacenar los datos
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Asigna la tabla como fuente de datos del DataGridView
                Datagrid_Docentes.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close(); // Cierra la conexión
            }
        }

        // Evento para añadir un docente
        private void BtnAñadir_docente_Click(object sender, EventArgs e)
        {
            if (Id_Docente.Text == ""
                || NombreCompleto_docente.Text == ""
                || generos_docente.Text == ""
                || direccion_docente.Text == ""
                || ciudad_docente.Text == ""
                || status_docente.Text == ""
                || foto_docente == null
                || imagePath == null)
            {
                MessageBox.Show("Por favor rellene todos los campos en blanco", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open(); // Abre la conexión a la base de datos
                }

                // Verificar si el ID ya existe
                string ComprobarIdDocente = "SELECT COUNT(*) FROM docentes WHERE id_docente = @IdDocente";
                using (SqlCommand checkTID = new SqlCommand(ComprobarIdDocente, connect))
                {
                    checkTID.Parameters.AddWithValue("@IdDocente", Id_Docente.Text.Trim());
                    int count = (int)checkTID.ExecuteScalar();

                    if (count >= 1)
                    {
                        MessageBox.Show("Docente ID: " + Id_Docente.Text.Trim() + " ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Insertar los datos del docente
                string InsertarDatos = "INSERT INTO docentes " +
                                       "(id_docente, nombre_docente, genero_docente, direccion_docente, foto_docente, status_docente, insertar_fecha) " +
                                       "VALUES (@id_docente, @NombreDocente, @GeneroDocente, @DireccionDocente, @ImagenesDocente, @StatusDocentes, @InsertarFecha)";

                string path = Path.Combine(@"C:\Directorio_Docentes\", Id_Docente.Text.Trim() + ".jpg");
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.Copy(imagePath, path, true);

                using (SqlCommand cmd = new SqlCommand(InsertarDatos, connect))
                {
                    cmd.Parameters.AddWithValue("@id_docente", Id_Docente.Text.Trim());
                    cmd.Parameters.AddWithValue("@NombreDocente", NombreCompleto_docente.Text.Trim());
                    cmd.Parameters.AddWithValue("@GeneroDocente", generos_docente.Text.Trim());
                    cmd.Parameters.AddWithValue("@DireccionDocente", direccion_docente.Text.Trim());
                    cmd.Parameters.AddWithValue("@StatusDocentes", status_docente.Text.Trim());
                    cmd.Parameters.AddWithValue("@ImagenesDocente", path.Trim());
                    cmd.Parameters.AddWithValue("@InsertarFecha", DateTime.Today.ToString("yyyy-MM-dd"));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Datos insertados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresca el DataGridView
                    DatosVisualesDocente();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close(); // Cierra la conexión
            }
        }

        // Método para limpiar los campos del formulario
        private void LimpiarCampos()
        {
            Id_Docente.Text = "";
            NombreCompleto_docente.Text = "";
            generos_docente.Text = "";
            direccion_docente.Text = "";
            ciudad_docente.Text = "";
            status_docente.Text = "";
            foto_docente.Image = null;
            imagePath = null;
        }

        // Variable para almacenar la ruta de la imagen
        private string imagePath;
        private string connectionString;

        // Evento que se activa al hacer clic en el botón para seleccionar una imagen
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog(); 
            open.Filter = "Image files (.jpg; *.png)|.jpg;*.png"; // Filtro para seleccionar solo archivos de imagen

            // Si el usuario selecciona un archivo, guarda la ruta y muestra la imagen en el formulario
            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                foto_docente.ImageLocation = imagePath;
            }
        }

        // Evento que se activa al hacer clic en el botón para limpiar los campos
        private void btnLimpiar_docente_Click(object sender, EventArgs e)
        {
            LimpiarCampos(); // Llama al método para limpiar los campos
        }

        // Evento que se activa al hacer clic en el botón para actualizar los datos de un docente
        private void btnActualizar_Docente_Click(object sender, EventArgs e)
        {
            // Verifica si algún campo está vacío antes de proceder con la actualización
            if (Id_Docente.Text == ""
              || NombreCompleto_docente.Text == ""
              || generos_docente.Text == ""
              || direccion_docente.Text == ""
              || ciudad_docente.Text == ""
              || status_docente.Text == ""
              || foto_docente == null
              || imagePath == null)
            {
                MessageBox.Show("Por favor rellene todos los campos en blanco", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Si la conexión no está abierta, la abre
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open(); // Abre la conexión


                        DialogResult check = MessageBox.Show("Estas seguro de que quieres actualizar la identificacion del estudiante: "
                            + Id_Docente.Text.Trim() + "?", "Confirmar mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                        DialogResult Check = MessageBox.Show("¿Estás seguro que quieres actualizar esta información del docente "
                        + Id_Docente.Text.Trim() + "?", "Confirmar mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        DialogResult checkStudent = MessageBox.Show("¿Estás seguro de que quieres actualizar la identificación del estudiante: "
                            + Id_Docente.Text.Trim() + "?", "Confirmar mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                        DateTime today = DateTime.Today;

                        if (check == DialogResult.Yes)
                        {
                            // Consulta SQL para actualizar los datos del docente
                            String UpdateData = "UPDATE docentes SET " +
                                "nombre_docente = @NombreDocente, genero_docente = @GeneroDocente" +
                                ", direccion_docente = @DireccionDocente, foto_docente = @ImagenesDocente " +
                                ", status_docente = @StatusDocentes" +
                                ", insertar_fecha = @InsertarFecha WHERE id_docente = @id_docente";

                            // Define la ruta donde se almacenará la imagen actualizada
                            string path = Path.Combine(@"C:\Users\Yhavet\Source\Repos\yhavet\SistemaAcademico\SistemaEscolar3\SistemaEscolar3\Directorio_Docentes\", Id_Docente.Text.Trim() + ".jpg");

                            // Usa SqlCommand para ejecutar la actualización
                            using (SqlCommand cmd = new SqlCommand(UpdateData, connect))
                            {
                                // Asigna los valores a los parámetros de la consulta
                                cmd.Parameters.AddWithValue("@NombreDocente", NombreCompleto_docente.Text.Trim());
                                cmd.Parameters.AddWithValue("@GeneroDocente", generos_docente.Text.Trim());
                                cmd.Parameters.AddWithValue("@DireccionDocente", direccion_docente.Text.Trim());
                                cmd.Parameters.AddWithValue("@StatusDocentes", status_docente.Text.Trim());
                                cmd.Parameters.AddWithValue("@ImagenesDocente", path);
                                cmd.Parameters.AddWithValue("@InsertarFecha", today.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@id_docente", Id_Docente.Text.Trim());

                                // Ejecuta la consulta y muestra un mensaje de éxito
                                cmd.ExecuteNonQuery();
                                DatosVisualesDocente();

                                MessageBox.Show("Datos actualizados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                   

                                // Limpia los campos
                                LimpiarCampos();
                            }
                        }
                        else
                        {
                            // Si el usuario cancela, muestra un mensaje
                            MessageBox.Show("Operación cancelada.", "Mensaje de informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LimpiarCampos(); // Limpia los campos
                        }
                    }
                    catch (Exception ex)
                    {
                        // Muestra un mensaje de error si ocurre una excepción
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close(); // Cierra la conexión
                    }
                }
            }
        }

        // Evento que se activa cuando se hace clic en una celda del DataGrid

        private void Datagrid_Docentes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que el índice de la fila no sea -1 (que indica que no se seleccionó una fila válida)
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = Datagrid_Docentes.Rows[e.RowIndex];

                // Carga los datos de la fila seleccionada en los campos del formulario
                Id_Docente.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
                NombreCompleto_docente.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
                generos_docente.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
                direccion_docente.Text = row.Cells[4].Value?.ToString() ?? string.Empty;
                imagePath = row.Cells[5].Value?.ToString() ?? string.Empty; // Obtiene la ruta de la imagen

                string ImageData = row.Cells[5].Value?.ToString();

                // Si hay una imagen válida, la carga en el control de imagen
                if (!string.IsNullOrEmpty(ImageData) && File.Exists(ImageData))
                {
                    try
                    {
                        foto_docente.Image = Image.FromFile(ImageData); // Carga la imagen
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        foto_docente.Image = null; // Deja la imagen en blanco en caso de error
                    }
                }
                else
                {
                    foto_docente.Image = null; // Si no hay imagen, deja el control vacío
                }

                // Carga los datos restantes
                status_docente.Text = row.Cells[6].Value?.ToString() ?? string.Empty;
            }
        }

        private void btnBorrar_docente_Click(object sender, EventArgs e)
        {
            if(Id_Docente.Text == "")
            {
                MessageBox.Show("Por favor selecciona el elemento primero", "Error Mensaje",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    DialogResult check = MessageBox.Show("............"

                        + Id_Docente.Text + "?", "Confirmar Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    DateTime today = DateTime.Today;

                    if (check == DialogResult.Yes)
                    {
                        try
                        {
                            connect.Open();

                            string BorrarDatos = "UPDATE docentes SET actualizar_fecha = @ActualizarFecha " +
                                "WHERE id_docente = @IdDocente";

                            using (SqlCommand cmd = new SqlCommand(BorrarDatos, connect))

                            {
                                cmd.Parameters.AddWithValue("ActualizarFecha", today);
                                cmd.Parameters.AddWithValue("@IdDocente", Id_Docente.Text.Trim());

                                cmd.ExecuteNonQuery();
                                DatosVisualesDocente();

                                MessageBox.Show("Registro eliminado de forma exitosa!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Limpia los campos
                                LimpiarCampos();


                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al conectarse a la base de datos: " + ex, "Error Mensaje",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Cancelado", "Informacion Mensaje",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
    }

}
