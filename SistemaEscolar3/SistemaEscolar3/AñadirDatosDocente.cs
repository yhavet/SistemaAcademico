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
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-OHA95C8\\MSSQLSERVER01;Initial Catalog=TEC3;Integrated Security=True;Connect Timeout=30");

        public int id { set; get; }
        public string IdDocente { set; get; }
        public string NombreDocente { set; get; }
        public string GeneroDocente { set; get; }
        public string DireccionDocente { set; get; }
        public string CiudadDocente { set; get; }
        public string ImagenesDocente { set; get; }
        public string StatusDocentes { set; get; }
        public string InsertarFecha { set; get; }

        public string ActualizarFecha { set; get; }

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
                            AñadirDatosDocente addTD = new AñadirDatosDocente();
                            addTD.id = (int)reader["id"];
                            addTD.IdDocente = reader["id_docente"].ToString();
                            addTD.NombreDocente = reader["id_docente"].ToString();
                            addTD.GeneroDocente = reader["genero_docente"].ToString();
                            addTD.DireccionDocente = reader["direccion_docente"].ToString();
                            addTD.CiudadDocente = reader["ciudad_docente"].ToString();
                            addTD.StatusDocentes = reader["status_docente"].ToString();
                            addTD.ImagenesDocente = reader["foto_docente"].ToString();
                            addTD.InsertarFecha = reader["insertar_fecha"].ToString();
                            addTD.ActualizarFecha = reader["actualizar_fecha"].ToString();

                            listData.Add(addTD);

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
            return listData; 
            
        }



    }
}
