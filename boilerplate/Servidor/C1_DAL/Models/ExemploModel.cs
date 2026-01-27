using PseudoFramework.ServidorUtils;

namespace C1_DAL.Models
{
    public class ExemploModel // Model de exemplo de objeto que é persistido
    {
        public int Id { get; set; }
        [CampoArquivoJson("caracteristica-1-exemplo")]
        public string Caracteristica1 { get; set; }
        [CampoArquivoJson("caracteristica-2-exemplo")]
        public int Caracteristica2 { get; set; }
        [CampoArquivoJson("caracteristica-3-exemplo")]
        public bool Caracteristica3 { get; set; }
        [CampoArquivoJson("caracteristica-4-exemplo")]
        public double Caracteristica4 { get; set; }
    }
}
