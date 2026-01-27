using PseudoFramework.ServidorUtils;
using System;

namespace C1_DAL.Models
{
    public class IdiomaModel
    {
        public int Id { get; set; }
        [CampoArquivoJson("descricao-idioma")]
        public string Descricao { get; set; }
        [CampoArquivoJson("usuario-ult-alteracao-idioma")]
        public string UsuarioUltimaAlteracao { get; set; }
        [CampoArquivoJson("Data-hora-ult-alteracao-idioma")]
        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
