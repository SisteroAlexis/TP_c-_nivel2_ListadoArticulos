using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Carga
{
    public class AccesoDatos
    {
        //Atributos de coneccion para Sql
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        //Popiedades

        public SqlDataReader Lector
        { get { return lector; } }

        //Constructor para conectar con la base de datos
        public AccesoDatos()
        {
            try
            {
                conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security = true");
                comando = new SqlCommand();
            }
            catch (Exception)
            {

                throw;
            }
        }


        //Metodos:


        //metodo para setear consulta
        //siempre va primero Consulta y despues un Ejecutador dependiendo del tipo de consulta.
        public void Consulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;

        }


        //EjecutarConsulta sirve para leer select y update (lee datos o cambia los existentes)
        //abre la conecccion y ejecuta la lectura
        public void EjecutarLectura() 
        {

            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception )
            {

                throw ;
            }

        }

        //EjecutarAccion se usa para Insert || Delete (inserta o elimina datos)
        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();


            }
            catch (Exception)
            {

                throw;
            }
        }

        //cerra connection y lector -- siempre en el bloque finally

        public void CerrarConeccion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
            //agrego esto y me aseguro de siempre limpiar mis variables.
            //nota: si me olvido un finally y CerrarConeccion puedo hacer que comando elimine cosas o cree caos (lo aprendi por las malas)
            comando.Parameters.Clear();
        }

        //metodo util para lectura de datos
        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);

        }

    }
}
