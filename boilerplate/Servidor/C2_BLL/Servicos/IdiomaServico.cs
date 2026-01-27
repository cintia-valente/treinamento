using C1_DAL.DAOs;
using C1_DAL.Models;
using C2_BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C2_BLL.Servicos
{
    public class IdiomaServico
    {
        private static readonly IdiomaDao _dao = new IdiomaDao();

        public IEnumerable<IdiomaDto> ListarIdiomas()
        {
            return _dao
                .Listar()
                .Select(idiomaModel => new IdiomaDto
                {
                    Id = idiomaModel.Id,
                    Descricao = idiomaModel.Descricao,
                    UsuarioUltimaAlteracao = idiomaModel.UsuarioUltimaAlteracao,
                    DataHoraUltimaAlteracao = idiomaModel.DataHoraUltimaAlteracao
                });
        }

        public IdiomaDto ConsultarIdioma(string id)
        {
            return ConsultarIdioma(Convert.ToInt32(id));
        }

        public IdiomaDto ConsultarIdioma(int id)
        {
            var idiomaModelUnica = _dao
                .Listar()
                .FirstOrDefault(idiomaModel => idiomaModel.Id == id);

            if (idiomaModelUnica == null)
                return null;

            return new IdiomaDto
            {
                Id = idiomaModelUnica.Id,
                Descricao = idiomaModelUnica.Descricao,
                UsuarioUltimaAlteracao = idiomaModelUnica.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = idiomaModelUnica.DataHoraUltimaAlteracao
            };
        }

        public int? IncluirIdioma(IdiomaDto idiomaDto)
        {
            var idiomaDtoConsultada = ConsultarIdioma(idiomaDto.Id);

            if (idiomaDtoConsultada != null)
                return null;

            var idiomaModelIncluir = new IdiomaModel
            {
                Id = idiomaDto.Id,
                Descricao = idiomaDto.Descricao
            };

            var idiomaModelIncluida = _dao.Incluir(idiomaModelIncluir);

            if (idiomaModelIncluida == null)
                return 0;

            return idiomaModelIncluida.Id;
        }

        public bool? AlterarIdioma(IdiomaDto idiomaDto)
        {
            var idiomaDtoConsultada = ConsultarIdioma(idiomaDto.Id);

            if (idiomaDtoConsultada == null)
                return null;

            var idiomaModelAlterar = new IdiomaModel
            {
                Descricao = idiomaDto.Descricao,
                UsuarioUltimaAlteracao = idiomaDto.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = DateTime.Now
            };

            return _dao.Alterar(idiomaModel => idiomaModel.Id == idiomaDtoConsultada.Id, idiomaModelAlterar);
        }

        public bool? RemoverIdioma(string id)
        {
            var idiomaDtoConsultada = ConsultarIdioma(id);

            if (idiomaDtoConsultada == null)
                return null;

            return _dao.Remover(idiomaModel => idiomaModel.Id == idiomaDtoConsultada.Id);
        }
    }
}
