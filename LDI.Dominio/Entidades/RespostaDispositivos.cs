using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDI.Dominio.Entidades
{
    public class RespostaDispositivos
    {
        public List<DispositivoResultado> Result { get; set; }
        public int Code { get; set; }
        public string Msg { get; set; }
    }
}
