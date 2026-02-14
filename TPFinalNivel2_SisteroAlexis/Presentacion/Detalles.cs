using Carga;
using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class Detalles : Form
    {
        //atributo
       private Articulo articulo;
        private List<Articulo> l_articulo;
        //constructor
        public Detalles()
        {
            InitializeComponent();
        }
        //sobrecarga de constructor
        public Detalles(Articulo nuevo)
        {
            InitializeComponent();
            articulo = nuevo;
        }

        //sobrecarga de listado de articulos
        public Detalles(List<Articulo> nuevo)
        {
            InitializeComponent();
            l_articulo = nuevo;
        }


        private void Detalles_Load(object sender, EventArgs e)
        {
            if (articulo != null)
            {
                // dgvArticulos.DataSource = articulo; //esto no funciona pasando el articulo tengo que pasarle una coleccion
                List<Articulo> list = new List<Articulo>();
                list.Add(articulo);
                dgvArticulos.DataSource = list;
                //con esto el dgv piensa que tiene una colleccion de objetos 
            }
            if (l_articulo != null)
            {
                dgvArticulos.DataSource = l_articulo;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
