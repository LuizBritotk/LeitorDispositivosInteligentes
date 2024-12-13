using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDI.Dominio.Entidades
{
    public class AutenticacaoResultado
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
        public long Expire_Time { get; set; }
    }
}
