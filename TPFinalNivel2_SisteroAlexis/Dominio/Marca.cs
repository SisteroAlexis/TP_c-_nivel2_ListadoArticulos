using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Marca
    {
        //Atributos--Prop

        public int Id { get; set; }
        public string Descripcion { get; set; }

        //Sobrecarga de la class object-- Evita errores al mostrar la class
        public override string ToString()
        {
            return Descripcion;
        }

    }
}
