using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metodos_Validaciones
{
    public class Helper
    {
        ////Metodos--Funsiones para facilitar el uso de eventos.

        //private void Cargar()
        //{
        //    ArticuloCarga carga = new ArticuloCarga();
        //    try
        //    {
        //        listaArticulo = carga.Listar();
        //        dgvArticulos.DataSource = listaArticulo;
        //        OcultarColumnas();
        //        CargarImagen(listaArticulo[0].ImagenUrl);
        //        //aca limito el formato del decimal y tambien lo redondea solo un genio Maxi en el foro.
        //        dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";


        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void OcultarColumnas()
        //{
        //    dgvArticulos.Columns[0].Visible = false;
        //    dgvArticulos.Columns[1].Visible = false;
        //    dgvArticulos.Columns[6].Visible = false;
        //}
        //private void CargarImagen(string Imagen)
        //{
        //    try
        //    {

        //        pbArticulo.Load(Imagen);
        //    }
        //    catch (Exception ex)
        //    {
        //        pbArticulo.Load("https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg");

        //    }
        //}



        ////validaciones

        ////validaciones para filtro avanzado
        //private bool ValidarFiltro()
        //{
        //    //regresamos true para que entre a un return y salga del evento

        //    if (cboCampo.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Ingrese porfavor el Campo");
        //        return true;
        //    }
        //    if (cboCriterio.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Ingrese porfavor el Criterio");
        //        return true;

        //    }

        //    //si es null o esta en blanco
        //    if (string.IsNullOrWhiteSpace(txtFiltroAvanzado.Text))
        //    {
        //        MessageBox.Show("Debes cargar el filtro para realizar una busqueda");
        //        return true;
        //    }
        //    if (cboCampo.Text == "Precio")
        //    {
        //        return (SoloNumeros(txtFiltroAvanzado.Text));
        //    }


        //    return false;
        //}

        ////los numeros me gusta validar con un TryParse
        //private bool SoloNumeros(string cadena)
        //{
        //    int aux;

        //    if (int.TryParse(cadena, out aux))
        //    {

        //        return false;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Ingrese solo numeros");
        //        return true;
        //    }



        //}

    }
}
