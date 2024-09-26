using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient; 

namespace SistemaEscolar3
{
    class AñadirDatosDocente
    {
        SqlConnection connect = new SqlConnection("Data Source=YHAVET\\SQLEXPRESS;Initial Catalog=Tecnica3;Integrated Security=True;Connect Timeout=30");

        public int id { set; get; }
        public string IdDocente{ set; get; }
        public string NombreDocente { set; get; }
        public string GeneroDocente { set; get; }
        public string DireccionDocente { set; get; }
        public string ImagenesDocente { set; get; }
        public string StatusDocentes { set; get; }
        public string CursosDocentes { set; get; }

        public List<AñadirDatosDocente> DatosDocentes()
        {
            List<AñadirDatosDocente> listData = new List<AñadirDatosDocente>();

            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string SQL = "Select * from docentes WHERE date_delete IS NULL";
                    using(SqlCommand cmd = new SqlCommand(SQL, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while(reader.Read()) 
                        { 
                            id = (int)reader["id"];
                            IdDocente = reader["id_docente"].ToString();
                            NombreDocente = reader["id_docente"].ToString();
                            GeneroDocente = reader["genero_docente"].ToString();
                            DireccionDocente = reader["direccion_docente"].ToString();

                        }
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectarse a la base de datos: " + ex);
                }
                finally
                {
                    connect.Close();
                }

            }
            
        }



    }
}
