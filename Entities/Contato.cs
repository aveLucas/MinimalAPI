using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_API.Entities
{
    public class Contato
    {
        public int Id { get; set;}
        public string Nome { get; set; }
        public string Numero { get; set; }
        public EnumStatusContato Status { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime DataDeRegistro { get; set; }

    }
}