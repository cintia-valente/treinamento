using C1_DAL.DAOs;
using C1_DAL.Models;
using C2_BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C2_BLL.Servicos
{
    public class ExemploServico
    {
        private static readonly ExemploDao _dao = new ExemploDao();

        public IEnumerable<ExemploSaidaDto> ListarExemplos()
        {
            return _dao
                .Listar()
                .Select(exemploModel => new ExemploSaidaDto
                {
                    Id = exemploModel.Id,
                    Caracteristica1 = exemploModel.Caracteristica1,
                    Caracteristica2 = exemploModel.Caracteristica2,
                    Caracteristica3 = exemploModel.Caracteristica3,
                    Caracteristica4 = exemploModel.Caracteristica4
                });
        }

        public ExemploSaidaDto ConsultarExemplo(string id)
        {
            var exemploModelUnico = _dao
                .Listar()
                .FirstOrDefault(exemploModel => exemploModel.Id == Convert.ToInt32(id));

            if (exemploModelUnico == null)
                return null;

            return new ExemploSaidaDto
            {
                Id = exemploModelUnico.Id,
                Caracteristica1 = exemploModelUnico.Caracteristica1,
                Caracteristica2 = exemploModelUnico.Caracteristica2,
                Caracteristica3 = exemploModelUnico.Caracteristica3,
                Caracteristica4 = exemploModelUnico.Caracteristica4
            };
        }

        public ExemploSaidaDto IncluirExemplo(ExemploEntradaDto exemploDto)
        {
            var listaExemplosModel = _dao.Listar();

            var proximoId = listaExemplosModel.Any()
                ? listaExemplosModel.Max(exemploModel => exemploModel.Id) + 1
                : 1;

            var exemploModelIncluir = new ExemploModel
            {
                Id = proximoId,
                Caracteristica1 = exemploDto.Caracteristica1,
                Caracteristica2 = exemploDto.Caracteristica2,
                Caracteristica3 = exemploDto.Caracteristica3,
                Caracteristica4 = exemploDto.Caracteristica4
            };

            var exemploModelIncluido = _dao.Incluir(exemploModelIncluir);

            return new ExemploSaidaDto
            {
                Id = exemploModelIncluido.Id,
                Caracteristica1 = exemploModelIncluido.Caracteristica1,
                Caracteristica2 = exemploModelIncluido.Caracteristica2,
                Caracteristica3 = exemploModelIncluido.Caracteristica3,
                Caracteristica4 = exemploModelIncluido.Caracteristica4
            };
        }

        public bool? AlterarExemplo(string id, ExemploEntradaDto exemploDto)
        {
            var exemploDtoConsultado = ConsultarExemplo(id);

            if (exemploDtoConsultado == null)
                return null;

            var exemploModelAlterar = new ExemploModel
            {
                Caracteristica1 = exemploDto.Caracteristica1,
                Caracteristica2 = exemploDto.Caracteristica2,
                Caracteristica3 = exemploDto.Caracteristica3,
                Caracteristica4 = exemploDto.Caracteristica4
            };

            return _dao.Alterar(exemploModel => exemploModel.Id == exemploDtoConsultado.Id, exemploModelAlterar);
        }

        public bool? RemoverExemplo(string id)
        {
            var exemploDtoConsultado = ConsultarExemplo(id);

            if (exemploDtoConsultado == null)
                return null;

            return _dao.Remover(exemploModel => exemploModel.Id == exemploDtoConsultado.Id);
        }
    }
}
