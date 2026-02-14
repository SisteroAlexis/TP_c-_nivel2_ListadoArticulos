using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Carga
{
    public class CategoriaCarga
    {
        private AccesoDatos datos = new AccesoDatos();
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                datos.Consulta("Select Id,Descripcion from Categorias;");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = datos.Lector.GetInt32(0);
                    aux.Descripcion = datos.Lector.GetString(1);

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

    }
}
