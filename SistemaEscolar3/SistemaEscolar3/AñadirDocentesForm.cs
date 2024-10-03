using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
            // Crea una instancia de AñadirDatosDocente y asigna los datos al DataGrid
            AñadirDatosDocente addTD = new AñadirDatosDocente();
            Datagrid_Docentes.DataSource = addTD.DatosDocentes();

        }

        // Evento que se activa al hacer clic en el botón de añadir docente

        private void BtnAñadir_docente_Click(object sender, EventArgs e)
        {
            // Verifica si alguno de los campos está vacío
            if (Id_Docente.Text == ""
                || NombreCompleto_docente.Text == ""
                || generos_docente.Text == ""
                || direccion_docente.Text == ""
                || ciudad_docente.Text == ""
                || status_docente.Text == ""
                || foto_docente == null
                || imagePath == null)
            {
                // Muestra un mensaje de error si faltan campos
                MessageBox.Show("Por favor rellene todos los campos en blanco", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Abre la conexión si aún no está abierta
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open(); // Abre la conexión a la base de datos

                        // Instancia de la clase AñadirDatosDocente para obtener los datos
                        AñadirDatosDocente addTD = new AñadirDatosDocente();

                        // Asigna los datos obtenidos por el método DatosDocentes al DataGridView
                        Datagrid_Docentes.DataSource = addTD.DatosDocentes();

                        // Consulta SQL para comprobar si el ID del docente ya existe
                        string ComprobarIdDocente = "SELECT id_docente, nombre_docente, genero_docente, direccion_docente, status_docente, insertar_fecha FROM docentes";

                        // Usa SqlCommand para ejecutar la consulta
                        using (SqlCommand checkTID = new SqlCommand(ComprobarIdDocente, connect))
                        {
                            checkTID.Parameters.AddWithValue("@IdDocente", Id_Docente.Text.Trim());
                            int count = (int)checkTID.ExecuteScalar(); // Obtiene el resultado de la consulta

                            // Si el ID del docente ya existe, muestra un mensaje de error
                            if (count >= 1)
                            {
                                MessageBox.Show("Docente ID: " + Id_Docente.Text.Trim() + " ya existe",
                                    "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                // Si el ID no existe, inserta los nuevos datos del docente
                                DateTime today = DateTime.Today;

                                // Consulta SQL para insertar los datos del docente
                                string InsertarDatos = "INSERT INTO docentes " +
                                    "(id_docente, nombre_docente, genero_docente, direccion_docente, " +
                                    "foto_docente, status_docente, insertar_fecha) " +
                                    "VALUES (@id_docente, @NombreDocente, @GeneroDocente, @DireccionDocente, " +
                                    "@ImagenesDocente, @StatusDocentes, @InsertarFecha)";

                                // Define la ruta donde se almacenará la imagen del docente
                                string path = Path.Combine(@"C:\Users\Yhavet\Source\Repos\yhavet\SistemaAcademico\SistemaEscolar3\SistemaEscolar3\Directorio_Docentes\", Id_Docente.Text.Trim() + ".jpg");
                                string directoryPath = Path.GetDirectoryName(path);

                                // Crea el directorio si no existe
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                // Copia la imagen del docente a la ruta especificada
                                File.Copy(imagePath, path, true);

                                // Usa SqlCommand para ejecutar la inserción
                                using (SqlCommand cmd = new SqlCommand(InsertarDatos, connect))
                                {
                                    // Asigna los valores de los campos a los parámetros de la consulta
                                    cmd.Parameters.AddWithValue("@id_docente", Id_Docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@NombreDocente", NombreCompleto_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@GeneroDocente", generos_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@DireccionDocente", direccion_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@StatusDocentes", status_docente.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ImagenesDocente", path.Trim());
                                    cmd.Parameters.AddWithValue("@InsertarFecha", today.ToString("yyyy-MM-dd"));

                                    // Ejecuta la consulta y muestra un mensaje de éxito
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Datos insertados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Actualiza el DataGrid con los nuevos datos
                                    DatosVisualesDocente();

                                    // Limpia los campos del formulario
                                    LimpiarCampos();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Muestra un mensaje de error si ocurre una excepción
                        MessageBox.Show("Error al conectarse a la base de datos: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close(); // Cierra la conexión a la base de datos
                    }
                }
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
                            string directoryPath = Path.GetDirectoryName(path);

                            // Crea el directorio si no existe
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            // Copia la nueva imagen a la ruta especificada
                            File.Copy(imagePath, path, true);

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
                                MessageBox.Show("Datos actualizados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Actualiza el DataGrid
                                DatosVisualesDocente();

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

    }

}
