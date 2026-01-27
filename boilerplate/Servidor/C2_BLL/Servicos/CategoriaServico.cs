using C1_DAL.DAOs;
using C1_DAL.Models;
using C2_BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C2_BLL.Servicos
{
    public class CategoriaServico
    {
        private static readonly CategoriaDao _dao = new CategoriaDao();

        public IEnumerable<CategoriaDto> ListarCategorias()
        {
            return _dao
                .Listar()
                .Select(categoriaModel => new CategoriaDto
                {
                    Id = categoriaModel.Id,
                    Descricao = categoriaModel.Descricao,
                    UsuarioUltimaAlteracao = categoriaModel.UsuarioUltimaAlteracao,
                    DataHoraUltimaAlteracao = categoriaModel.DataHoraUltimaAlteracao
                });
        }

        public CategoriaDto ConsultarCategoria(string id)
        {
            return ConsultarCategoria(Convert.ToInt32(id));
        }

        public CategoriaDto ConsultarCategoria(int id)
        {
            var categoriaModelUnica = _dao
                .Listar()
                .FirstOrDefault(categoriaModel => categoriaModel.Id == id);

            if (categoriaModelUnica == null)
                return null;

            return new CategoriaDto
            {
                Id = categoriaModelUnica.Id,
                Descricao = categoriaModelUnica.Descricao,
                UsuarioUltimaAlteracao = categoriaModelUnica.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = categoriaModelUnica.DataHoraUltimaAlteracao
            };
        }

        public int? IncluirCategoria(CategoriaDto categoriaDto)
        {
            var categoriaDtoConsultada = ConsultarCategoria(categoriaDto.Id);

            if (categoriaDtoConsultada != null)
                return null;

            var categoriaModelIncluir = new CategoriaModel
            {
                Id = categoriaDto.Id,
                Descricao = categoriaDto.Descricao
            };

            var categoriaModelIncluida = _dao.Incluir(categoriaModelIncluir);

            if (categoriaModelIncluida == null)
                return 0;

            return categoriaModelIncluida.Id;
        }

        public bool? AlterarCategoria(CategoriaDto categoriaDto)
        {
            var categoriaDtoConsultada = ConsultarCategoria(categoriaDto.Id);

            if (categoriaDtoConsultada == null)
                return null;

            var categoriaModelAlterar = new CategoriaModel
            {
                Descricao = categoriaDto.Descricao,
                UsuarioUltimaAlteracao = categoriaDto.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = DateTime.Now
            };

            return _dao.Alterar(categoriaModel => categoriaModel.Id == categoriaDtoConsultada.Id, categoriaModelAlterar);
        }

        public bool? RemoverCategoria(string id)
        {
            var categoriaDtoConsultada = ConsultarCategoria(id);

            if (categoriaDtoConsultada == null)
                return null;

            return _dao.Remover(categoriaModel => categoriaModel.Id == categoriaDtoConsultada.Id);
        }
    }
}
