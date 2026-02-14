using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carga
{
    public class ArticuloCarga
    {
        //termine prefiriendo ponerlo en cada metodo para evitar errores en un futuro
        //private AccesoDatos datos = new AccesoDatos(); 
        /*variable para siempre acceder a los datos (modificando algunas cosas para evitar errores)
        //Desconosco si genera conflicto usar una variable global pero me ahorra declararla mil veces y como esta todo
        //en try y la consulta se renueva creo que no genera conflicto. mi dudas es con el metodo Consulta() y
        //SetearParametros() nose si se guarda el valor y puede generar problemas.
        //por las dudas planeo crear un destructo (Me sirve para practica ya que no lo use mucho....) //al final no lo cree
        No me genero conflicto pero si arrastre de "informacion basura" //mentira!!! si puede generar conflicto
        Lo "Arregle" en el metodo CerrarConecccion*/

        //metodo para cargar en el dgv los Articulos
        public List<Articulo> Listar()
        {
            AccesoDatos datos = new AccesoDatos();
            List <Articulo> lista = new List<Articulo>();

            try
            {

                datos.Consulta("Select a.Id,Codigo,Nombre,a.Descripcion,m.Descripcion 'Marca',c.Descripcion 'Categoria',ImagenUrl,Precio,m.Id,c.Id from ARTICULOS a, MARCAS m, CATEGORIAS c\r\nwhere \r\na.IdMarca = m.Id\r\nand \r\na.IdCategoria = c.Id;");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                //Lectura de datos uso el Get... pero se puede hacer con un (string)datos.Lector["Nombre"]
                {
                    Articulo aux = new Articulo();
                    aux.Id = datos.Lector.GetInt32(0);
                    aux.Codigo = datos.Lector.GetString(1);
                    aux.Nombre = datos.Lector.GetString(2);
                    aux.Descripcion = datos.Lector.GetString(3);
                    //esto es un objeto por asociacion por lo que tenemos que instanciarlo primero.
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = datos.Lector.GetString(4);
                    //lo mis pero con Categoria
                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = datos.Lector.GetString(5);
                    //Nos asegurames que la imagen no sea null para evitar conflictos al cargar la imagen. 
                    //aunque tambien utilizo otras medidas de seguridad para la imagen. Despues veo si se puede sacar este if
                    if (!(datos.Lector["ImagenUrl"] is DBNull)) // aca si uso []
                    { aux.ImagenUrl = datos.Lector.GetString(6); }

                    /*trabajo con decimal pero nose si es el mejor tipo de dato. Vi que en el foro se podia generar una especie
                    //de formato para redondear ej: 69,999 => 70,00  . Pero no estoy seguro de si usarlo.
                    arreglado. Lo dejo como anotacion */
                    aux.Precio = datos.Lector.GetDecimal(7);
                    aux.Marca.Id = datos.Lector.GetInt32(8);
                    aux.Categoria.Id = datos.Lector.GetInt32(9);


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

        //Agregar 
        public void Agregar (Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.Consulta("Insert into ARTICULOS Values(@Codigo, @Nombre, @Desc, @IdMarca, @IdCategoria, @Img, @Precio);");

                datos.SetearParametro("@Codigo", nuevo.Codigo);
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Desc", nuevo.Descripcion);
                datos.SetearParametro("@IdMarca", nuevo.Marca.Id);
                datos.SetearParametro("@IdCategoria", nuevo.Categoria.Id);
                datos.SetearParametro("@Img", nuevo.ImagenUrl);
                datos.SetearParametro("@Precio", nuevo.Precio);

                datos.EjecutarAccion();


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

        //Modificar

        public void Modificar (Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //cargo todo por parametro evita posibles errores
                datos.Consulta("update ARTICULOS set Codigo = @Codigo,Nombre = @Nombre,Descripcion = @Descripcion,IdMarca = @IdMarca,IdCategoria =@IdCategoria,ImagenUrl = @ImagenUrl,Precio = @Precio\r\nwhere Id = @Id;");
                datos.SetearParametro("@Codigo",art.Codigo);
                datos.SetearParametro("@Nombre",art.Nombre);
                datos.SetearParametro("@Descripcion",art.Descripcion);
                datos.SetearParametro("@IdMarca",art.Marca.Id);
                datos.SetearParametro("@IdCategoria", art.Categoria.Id);
                datos.SetearParametro("@ImagenUrl", art.ImagenUrl);
                datos.SetearParametro("@Precio",art.Precio);
                datos.SetearParametro("@Id",art.Id);
                datos.EjecutarLectura();
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

        //EliminarFisico es super corto gracias a todo lo ya construido:D
        public void EliminarFisico(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.Consulta("Delete from ARTICULOS Where Id = @id;");
                datos.SetearParametro("@id",articulo.Id);
                datos.EjecutarAccion();
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

       
        //Filtro Avanzado (Va a la BD)

        public List<Articulo> Filtro(string campo, string criterio, string filtro)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Articulo> articulos = new List<Articulo>();
            
            //esto puede que sea mejor seteralo por parametros futuramente lo veo. aunque de esta forma no da errores. pero es mas tedioso de modificar.
            try
            {
                string consul = ("Select a.Id,Codigo,Nombre,a.Descripcion,m.Descripcion 'Marca',c.Descripcion 'Categoria',ImagenUrl,Precio,m.Id,c.Id from ARTICULOS a, MARCAS m, CATEGORIAS c\r\nwhere \r\na.IdMarca = m.Id\r\nand \r\na.IdCategoria = c.Id ");

                switch (campo)
                {
                    case "Precio":
                        {
                            switch (criterio)
                            {
                                case "Mayor a: ":
                                    consul += " and Precio >" + filtro;
                                    break;
                                case "Menor a: ":
                                    consul += " and Precio <" + filtro;
                                    break;
                                case "Igual a: ":
                                    consul += " and Precio =" + filtro;
                                    break;
                            }
                            break;
                        }
                       

                    case "Nombre":
                        {
                            switch (criterio)
                            {
                                case "Comienza con:  ":
                                    consul += " and Nombre like '" + filtro + "%'";
                                    break;
                                case "Termina con:  ":
                                    consul += " and Nombre like '%" + filtro + "'";
                                    break;
                                case "Contiene: ":
                                    consul += " and Nombre like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        }

                    case "Marca":
                        {
                            switch (criterio)
                            {
                                case "Comienza con:  ":
                                    consul += " and m.Descripcion like '" + filtro + "%'";
                                    break;
                                case "Termina con:  ":
                                    consul += " and m.Descripcion like '%" + filtro + "'";
                                    break;
                                case "Contiene: ":
                                    consul += " and m.Descripcion like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        }

                    case "Categoria":
                        {
                            switch (criterio)
                            {
                                case "Comienza con:  ":
                                    consul += " and c.Descripcion like '" + filtro + "%'";
                                    break;
                                case "Termina con:  ":
                                    consul += " and c.Descripcion like '%" + filtro + "'";
                                    break;
                                case "Contiene: ":
                                    consul += " and c.Descripcion like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        }



                }

                datos.Consulta(consul);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                
                {
                    Articulo aux = new Articulo();
                    aux.Id = datos.Lector.GetInt32(0);
                    aux.Codigo = datos.Lector.GetString(1);
                    aux.Nombre = datos.Lector.GetString(2);
                    aux.Descripcion = datos.Lector.GetString(3);
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = datos.Lector.GetString(4);
                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = datos.Lector.GetString(5);
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    { aux.ImagenUrl = datos.Lector.GetString(6); }
                    aux.Precio = datos.Lector.GetDecimal(7);
                    aux.Marca.Id = datos.Lector.GetInt32(8);
                    aux.Categoria.Id = datos.Lector.GetInt32(9);


                    articulos.Add(aux);
                }
                    return articulos;
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
