using System;

namespace PseudoFramework.ServidorUtils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CampoArquivoJsonAttribute: Attribute
    {
        public string NomeCampo { get; }

        public CampoArquivoJsonAttribute(string nomeCampo)
        {
            NomeCampo = nomeCampo;
        }
    }
}
