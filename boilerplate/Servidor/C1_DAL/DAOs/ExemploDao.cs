using PseudoFramework.ServidorUtils;
using C1_DAL.Models;
using System;
using System.IO;

namespace C1_DAL.DAOs
{
    public class ExemploDao: ObjetoPersistenciaJson<ExemploModel>
    {
        private const string ARQUIVO = "exemplos.json";

        private static string RaizProjeto = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName, "data"); // Alterar conforme conveniencia

        public ExemploDao() : base(Path.Combine(RaizProjeto, ARQUIVO)) { }
    }
}
