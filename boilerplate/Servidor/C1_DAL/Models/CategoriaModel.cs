using PseudoFramework.ServidorUtils;
using System;

namespace C1_DAL.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        [CampoArquivoJson("descricao-categoria")]
        public string Descricao { get; set; }
        [CampoArquivoJson("usuario-ult-alteracao-categoria")]
        public string UsuarioUltimaAlteracao { get; set; }
        [CampoArquivoJson("Data-hora-ult-alteracao-categoria")]
        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
