using Carga;
using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Presentacion
{
    public partial class Presentacion : Form
    {
        //Atributo
        //genero una lista para cargarla en el dgvArticulos
        private List<Articulo> listaArticulo;
        private List<Articulo> listaArticulo_FiltroAvanzado = null; //filtro especial para busquedas avanzadas
                                                                    //regresa a null con el boton limpiar filtros

        //Constructor
        public Presentacion()
        {
            InitializeComponent();

        }

        //Eventos:


        //carga de formulario
        private void Presentacion_Load(object sender, EventArgs e)
        {
            try
            {
                Cargar();
                // Carga de campo

                cboCampo.Items.Add("Precio");
                cboCampo.Items.Add("Nombre");
                cboCampo.Items.Add("Marca");
                cboCampo.Items.Add("Categoria");
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
          
            

        }
        //Seleccionar la imagen al cambiar de articulo
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                //valido el dgv para que al usar el filtro no se quede con una imagen a pesar de no existir nada en el dgv
                if (Validardgv())
                {

                    //la fila actual //Da el objeto enlasado
                    Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    //arriba genere un casteo explicito de pokemons y lo guarde en una nueva variable pokemons
                    CargarImagen(seleccionado.ImagenUrl);
                }
                else pbArticulo.Load("https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        //agregar Articulos
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                AltaArticulos ventana = new AltaArticulos();
                ventana.ShowDialog();
                Cargar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
         
        }

        //modificar Articulos
        private void btnModificar_Click(object sender, EventArgs e)
        {

            try
            {
                //valido tener seleccionado un articulo
                if (Validardgv())
                {
                    //creo un nuevo articulo. no es necesario instanciarlo ya que va a hacer referencia a otro objeto
                    Articulo seleccionado;
                    //cargo los datos del articulo seleccionado
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem; //CurrentRow.DataBoundItem es la fina selecionada con sus datos
                                                                                    //creo el objeto para pasar a otra ventana. le paso el objeto seleccionado ya que sobrecargue el constructor para aceptar un Articulo
                    AltaArticulos ventana = new AltaArticulos(seleccionado);
                    ventana.ShowDialog();
                    Cargar();
                }
                else MessageBox.Show("Por favor seleccione un articulo correctamente");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           

        }

        //eliminarArticulo
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloCarga carga = new ArticuloCarga(); //creo que tenia que poner esto como variable global. aunque nose si da problemas
            

            try
            {

                if (Validardgv())
                {
                    Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem; //carga explicita, sabemos que son articulos
                    DialogResult resultado = MessageBox.Show("¿De verdad quiere eliminar este articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        DialogResult r = MessageBox.Show("De veritas de veritas?", "Seguro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (r == DialogResult.Yes) { carga.EliminarFisico(seleccionado); Cargar(); }
                    }
                }else MessageBox.Show("Por favor seleccione un articulo correctamente");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //Detalles
        private void btnDetalles_Click(object sender, EventArgs e)
        {

            if (Validardgv())
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                Detalles ventana = new Detalles(seleccionado);
                ventana.ShowDialog();
            }
            else MessageBox.Show("Por favor seleccione un articulo correctamente");
        }

        //Filtro Rapido
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {


            try
            {
                List<Articulo> listafiltrada = new List<Articulo>();
                string filtro = txtFiltro.Text;

                /*Primero recargamos la info de la lista de articulos. porque?
                Porque si usamos primero el filtro avanzado y despues este filtro muestra mal la informacion.
                Anlo similar solia pasar con el ordenamiento. pero de esta forma valido todo los posiblles errores.
                es mas que nada porque el dgv puede tener un valor y la listaArticulos otro y eso da a confusiones
                si no hago esto el progrma funciona, pero no como quiero. */
                if (listaArticulo_FiltroAvanzado !=null)
                listaArticulo = listaArticulo_FiltroAvanzado;

                //IsNullOrWhiteSpace similar que IsNullOrEmpty pero mejor para este momento ya que verifica si hay espacios en blanco
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    //filtro findAll busca y encuentra todos los objetos en la lista segun unos parametros.
                    listafiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()));
                    //requiere una exprecion landam. no te voy a mentir tengo que practicar mas esto lo entiendo a medias. si me pasas un video y ejersicios de practica me ayudas mucho.
                }
                else { listafiltrada = listaArticulo; }

                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = listafiltrada;

                OcultarColumnas();
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        //Cambio de campo
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opc = cboCampo.SelectedItem.ToString();
            if (opc == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a: ");
                cboCriterio.Items.Add("Menor a: ");
                cboCriterio.Items.Add("Igual a: ");

            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con:  ");
                cboCriterio.Items.Add("Termina con:  ");
                cboCriterio.Items.Add("Contiene: ");
            }
        }

        //evento para realizar la busqueda por filtro avanzado
        private void btnFiltroAvanzado_Click(object sender, EventArgs e)
        {
            ArticuloCarga carga = new ArticuloCarga();

            try
            {
                listaArticulo_FiltroAvanzado = new List<Articulo>();

                if (ValidarFiltro()) { return; } //el return hace que se salga del evento si no cumple la condicion
                string campo = cboCampo.Text;
                string criterio = cboCriterio.Text;
                string filtro = txtFiltroAvanzado.Text;


                listaArticulo_FiltroAvanzado = carga.Filtro(campo, criterio, filtro);
                dgvArticulos.DataSource = listaArticulo_FiltroAvanzado;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //limpieza de filtros y ordenamientos
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                Cargar();

                //limpieza
                cboCampo.Items.Clear();
                cboCriterio.Items.Clear();
                txtFiltro.Clear();
                txtFiltroAvanzado.Clear();

                //recarga
                cboCampo.Items.Add("Precio");
                cboCampo.Items.Add("Nombre");
                cboCampo.Items.Add("Marca");
                cboCampo.Items.Add("Categoria");

                //limpieza de la lista avanzada
                listaArticulo_FiltroAvanzado = null; 
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

        //ordenamiento por precios
        private void btnOrdenP_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validardgv() ) //si existe un dgv con info ordeno. sino no pierdo el tiempo
                {
                    List<Articulo> listaOrdenada = new List<Articulo>();
                    listaOrdenada = (List<Articulo>)dgvArticulos.DataSource; //lo que este cargado en en el dgv
                                                                             //arriba hice un casteo explicito ya que sabemos que lo que esta adentro del dgv es una lista de articulos.//validar
                    listaOrdenada.Sort(); //lo odenamos y procedemos a mostrarlo
                    dgvArticulos.DataSource = null;
                    dgvArticulos.DataSource = listaOrdenada;

                    OcultarColumnas();
                    CargarImagen(listaOrdenada[0].ImagenUrl);
                    //aca limito el formato del decimal y tambien lo redondea solo un genio Maxi en el foro.
                    dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                dgvArticulos.Focus(); //si no pongo no tiene ningun articulo seleccionado y puede dar error con algunos botones
                                      //creo que ya lo arregle era una prop del dgv que toque pero dejo esto comentado, no lastima a nadie
            }




        }
       

        //Metodos--Funsiones para facilitar el uso de eventos.

        private void Cargar()
        {
            ArticuloCarga carga = new ArticuloCarga();
            try
            {
                listaArticulo = carga.Listar();
                dgvArticulos.DataSource = listaArticulo;
                OcultarColumnas();
                CargarImagen(listaArticulo[0].ImagenUrl);
                //aca limito el formato del decimal y tambien lo redondea solo un genio Maxi en el foro.
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void OcultarColumnas()
        {
            dgvArticulos.Columns[0].Visible = false;
            dgvArticulos.Columns[1].Visible = false;
            dgvArticulos.Columns[6].Visible = false;
        }
        private void CargarImagen(string Imagen)
        {
            try
            {

                pbArticulo.Load(Imagen);
            }
            catch (Exception)
            {
                pbArticulo.Load("https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg");

            }
        }

     


        //validaciones

        //validaciones para filtro avanzado
        private bool ValidarFiltro()
        {
            //regresamos true para que entre a un return y salga del evento

            if (cboCampo.SelectedIndex == -1)
            {
                MessageBox.Show("Ingrese porfavor el Campo");
                return true;
            }
            if (cboCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Ingrese porfavor el Criterio");
                return true;

            }

            //si es null o esta en blanco
            if (string.IsNullOrWhiteSpace(txtFiltroAvanzado.Text))
            {
                MessageBox.Show("Debes cargar el filtro para realizar una busqueda");
                return true;
            }
            if (cboCampo.Text == "Precio")
            {
                return (SoloNumeros(txtFiltroAvanzado.Text));
            }


            return false;
        }

        //los numeros me gusta validar con un TryParse
        private bool SoloNumeros(string cadena)
        {
            int aux;
           
                if (int.TryParse(cadena, out aux))
                {

                    return false;
                }
               else
                {
                    MessageBox.Show("Ingrese solo numeros");
                    return true;
                }



        }

        //validacion de dgv, puse muchas cosas porque me dio problemas en general esto.
        private bool Validardgv ()
        {
            if (dgvArticulos.DataSource == null  ) return false; //verifico que el dgv tenga datos
            if (dgvArticulos.Rows.Count == 0) return false; //verifico que si hay filas
            if (dgvArticulos.CurrentRow == null) return false; //verifico de si hay una fila seleccionada
            if (dgvArticulos.CurrentRow.DataBoundItem == null) return false; //por las dudas
            return true;
        }
    }
}
