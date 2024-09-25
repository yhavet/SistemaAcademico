using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;


namespace SistemaEscolar3
{
    public partial class FormularioInicioSesion : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-JMH1N0F\\SQLEXPRESS;Initial Catalog=TEC3;Integrated Security=True;Connect Timeout=30");




        public FormularioInicioSesion()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if(username.Text == "" || password.Text == "")
            {
                MessageBox.Show("Tienes los campos en blanco, no te olvides de completarlos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }else
            {

                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM users WHERE username = @username AND password = @password";

                  
                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@username", username.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", password.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count >= 1)
                        {
                          MessageBox.Show("Iniciaste sesion en tu cuenta de forma exitosa!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                          MainFormulario mForm = new MainFormulario();
                          mForm.Show();
                          this.Hide();
                        }

                        else
                        {
                            MessageBox.Show("Usuario/Contraseña incorrecta, revisa bien los campos!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                catch(Exception ex) 
                {
                   MessageBox.Show("Error al conectarse a la base de datos: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }

            }

        }

        private void showPass_CheckedChanged(object sender, EventArgs e)
        {
            password.PasswordChar = showPass.Checked ? '\0' : '*';
        }
    }
}
