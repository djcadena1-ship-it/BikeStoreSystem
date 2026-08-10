using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Datos
{
    
        public class Bicicleta
        {
            public int IdBicicleta { get; set; }
            public int IdCategoria { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public string Estado { get; set; }
        }
   }

