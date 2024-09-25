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
        SqlConnection connect = new SqlConnection(@"");

        public int id { set; get; }
        public string Iddocente{ set; get; }
        public string NombreDocente { set; get; }
        public string GeneroDocente { set; get; }
        public string DireccionDocente { set; get; }
        public string ImagenesDocente { set; get; }
        public string StatusDocentes { set; get; }
        public string CursosDocentes { set; get; }

        //public List<AñadirDatosDocente> DatosDocentes()
        //{
        //    List<AñadirDatosDocente> listData = new List<AñadirDatosDocente> ();
        //}



    }
}
