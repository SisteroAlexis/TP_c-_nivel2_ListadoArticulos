using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo : IComparable<Articulo>
    {
        //atributos-prop

        public int Id { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; } //validarlo futuramente. codigo no correllativo(3) 1 letra 2 numeros
        //intente validarlo aca pero da algunos errores
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }

        public int CompareTo(Articulo other)
        {

            int result = this.Precio.CompareTo(other.Precio);

            return result;
        }


    }
}
