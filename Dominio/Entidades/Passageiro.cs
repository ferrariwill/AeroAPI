using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Passageiro
    {
        public int? codigo { get; set; }
        public string descricao { get; set; }
        public int? Id { get; set; }
        public string Nome { get; set; }
        public int? Idade { get; set; }
        public string Celular { get; set; }

        public static string PassageiroInsProc = "PassageiroInsProc";
        public static string PassageiroAltProc = "PassageiroAltProc";
        public static string PassageiroDelProc = "PassageiroDelProc";
        public static string PassageiroConProc = "PassageiroConProc";
    }
}
