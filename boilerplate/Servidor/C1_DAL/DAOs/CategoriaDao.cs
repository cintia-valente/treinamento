using PseudoFramework.ServidorUtils;
using C1_DAL.Models;
using System;
using System.IO;

namespace C1_DAL.DAOs
{
    public class CategoriaDao: ObjetoPersistenciaJson<CategoriaModel>
    {
        private const string ARQUIVO = "categorias.json";

        private static string RaizProjeto = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName, "data");

        public CategoriaDao() : base(Path.Combine(RaizProjeto, ARQUIVO)) { }
    }
}
