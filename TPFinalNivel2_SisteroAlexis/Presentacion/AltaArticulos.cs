using Carga;
using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Configuration;


namespace Presentacion
{
    public partial class AltaArticulos : Form
    {
        //creamos un articulo nulo //para que no se carge los datos cuando ponemos agregar
        private Articulo articulo = null; //esto seria un atributo tambien 
        private OpenFileDialog archivo = null;


        //constructor por defecto
        public AltaArticulos()
        {
            InitializeComponent();
        }
        //sobrecarga de constructor
        public AltaArticulos(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        //eventos

        //aceptar 
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloCarga carga = new ArticuloCarga();
            try
            {
                if (articulo == null) articulo = new Articulo();

                //valido
                if (ValidarDatos()) return;  //si da true el validar salgo del evento

                //cargo los datos
                articulo.Codigo = txtCodigo.Text.ToUpper(); //siempre quiero que este en mayus la primera letra
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cbMarca.SelectedItem; //casteo explicioto sabemos que lo que hay adentro es una marca
                articulo.Categoria = (Categoria)cbCategoria.SelectedItem;//lo mismo 
                articulo.ImagenUrl = txtImagen.Text;
                articulo.Precio = Convert.ToDecimal(txtPrecio.Text);

                //guardar la imagen
                //me gusto mucho el IsNullOrWhiteSpace verifico si no se borro la informacion en el txt
                if (archivo != null && !(string.IsNullOrWhiteSpace(txtImagen.Text)) && !(txtImagen.Text.ToUpper().Contains("http".ToUpper())))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Imagen"] + archivo.SafeFileName);
                    //antes de lo de arriba se configura el app.Config
                }
                //si la imagen ya existe se va a una exception especial y da su respectivo mensaje

                if (articulo.Id != 0)
                {
                    carga.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    carga.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

          
                Close();
            }
            catch(IOException Iex )
            {
                MessageBox.Show(Iex.Message + "\nError, La imagen que intenta cargar ya fue cargada en otro articulo \n cambiale el nombre a su imagen o cargue otra imagen");
            }
            catch (OverflowException oex)
            {
                MessageBox.Show(oex.Message);
            }
            catch (FormatException exf)
            {
                txtPrecio.Focus(); //es casi seguro que el error se de aca por eso le pongo el focus
                MessageBox.Show(exf.Message);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        //cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //cargar datos para ver si lo modificamos
        private void AltaArticulos_Load(object sender, EventArgs e)
        {
            //Cargo los datos de los comboBox y la imagen(si es necesario)
            //genero las instancias de carga de Marca y Categoria para los cb
            MarcaCarga marca = new MarcaCarga();
            CategoriaCarga categoria = new CategoriaCarga();

            try
            {
                //agrego los values y display para poder configurar los desplegables mejor.
                cbMarca.DataSource = marca.Listar();
                cbMarca.ValueMember = "Id";
                cbMarca.DisplayMember = "Descripcion";
                cbCategoria.DataSource = categoria.Listar();
                cbCategoria.ValueMember = "Id";
                cbCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    //los desplegables tienen un manejor particular
                    cbMarca.SelectedValue = articulo.Marca.Id;
                    cbCategoria.SelectedValue = articulo.Categoria.Id;
                    txtImagen.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = Convert.ToString(articulo.Precio);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //para ver la imagen que cargamos
        private void txtImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagen.Text);
        }

        //btn para cargar imagen de los archivos locales
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);


            }


        }



        //metodos
        private void CargarImagen(string Imagen)
        {
            try
            {

                pbImagen.Load(Imagen);
            }
            catch (Exception ex)
            {
                pbImagen.Load("https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg");

            }
        }


        //validacion
        private bool ValidarDatos()
        {
            //validamos codigo nombre y precio el resto puede ser null

            int aux = txtCodigo.Text.Length; //verifico que el codigo solo sea una cadena de 3
            if (aux != 3)
            {
                DialogResult respuesta;
                //pongo un mensaje largo para practicar la sintaxis
                MessageBox.Show("Ingrese un Codigo de 3 caracteres. \n ejemplo: P66", "Codigo Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txtCodigo.Focus();
                return true;
            }
            //se que txt.Codigo dese aca es un string de 3 caracteres
            int aux2;
            int cont = 0;
            //aca valido un correcto formato de entrada del txtCodigo. Es un poco rebuscado pero funciona
            foreach (char c in txtCodigo.Text)
            {
                if (cont == 0)
                {

                    if (int.TryParse(Convert.ToString(c), out aux2)) //si esto da verdadero significa que la primera letra es un numero
                    {
                        MessageBox.Show("El primer caracter tiene que ser una letra. \n ejemplo: P66", "Codigo Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCodigo.Focus();
                        return true;
                    }
                }
                else
                {
                    if (!(int.TryParse(Convert.ToString(c), out aux2))) //con el ! da true cuando no es un numero
                    {
                        MessageBox.Show("Los ultimos 2 caracteres tienen que ser numericos. \n ejemplo: P66", "Codigo Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCodigo.Focus();
                        return true;
                    }
                }


                cont++;
            }

            //valido que el codigo sea unico lo pongo en un try por las dudas
            try
            {
                ArticuloCarga p = new ArticuloCarga();
                List<Articulo> list = new List<Articulo>();
                list = p.Listar();
                foreach (Articulo a in list)
                {
                    if (a.Codigo == txtCodigo.Text)
                    {
                      DialogResult resultado = MessageBox.Show("El codigo ya existe. \n ¿queres ver los detalles para saber los codigos?", "Codigo Repetido", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (resultado == DialogResult.Yes)
                        {
                            Detalles ventana = new Detalles(list);
                            ventana.ShowDialog();
                            txtCodigo.Focus();
                        }
                        txtCodigo.Focus();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
          
            

            //El nombre no puede ser null o estar en blanco.
            //despues de eso puede llamarse como sea incluso empezar con numeros 

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese por favor un nombre");
                txtNombre.Focus();
                return true;
            }

          

            //valido el precio aunque ya cree un evento para ello. esto es por si se deja en blanco.

            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                MessageBox.Show("Ingrese un precio por favor", "Precio decimal");
                txtPrecio.Focus();
                return true;
            }



            return false;
        }

        //evento que me ayuda a validar el precio. se activa con cada tecla apretada dentro del control
        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo numeros
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir una sola coma
            if (e.KeyChar == ',' && !txtPrecio.Text.Contains(","))
                return;

            // Permitir espacios
            if (e.KeyChar == (char)Keys.Back)
                return;

            // Bloquear cualquier otro caracter
            e.Handled = true;
        }

        //solo existe una forma que esto no valide 100% bien y es que solo se ingrese una "," 
        //pero el try ya anula este error. solo que no pone el focus en el txtPrecio
        //arreglado con el  catch (FormatException exf) 
    }
}
