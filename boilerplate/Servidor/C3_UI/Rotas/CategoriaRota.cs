using C2_BLL.DTOs;
using C2_BLL.Servicos;
using PseudoFramework.SharedUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C3_UI.Rotas
{
    public class CategoriaRota: IRota
    {
        public const string RECURSO = "categorias";

        private static readonly CategoriaServico _servico = new CategoriaServico();

        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetCategoria(id);

                            return GetCategorias();
                        }
                    case "POST":
                        return PostCategoria(json);

                    case "PUT":
                        return PutCategoria(id, json);

                    case "DELETE":
                        return DeleteCategoria(id);

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de categoria não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<CategoriaDto>> GetCategorias()
        {
            var listaCategoriasDto = _servico.ListarCategorias();

            if (!listaCategoriasDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaCategoriasDto, "Lista de categorias está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaCategoriasDto);
        }

        private static RespostaWrapper<CategoriaDto> GetCategoria(string id)
        {
            var categoriaDtoConsultada = _servico.ConsultarCategoria(id);

            if (categoriaDtoConsultada == null)
                return RespostaWrapper.EnveloparInsucesso<CategoriaDto>(null, "Categoria não encontrada.");

            return RespostaWrapper.EnveloparSucesso(categoriaDtoConsultada);
        }

        private static RespostaWrapper<CategoriaDto> PostCategoria(string json)
        {
            var categoriaDtoRecebida = ConversorJson.Desserializar<CategoriaDto>(json);

            var resultado = _servico.IncluirCategoria(categoriaDtoRecebida);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso<CategoriaDto>(null, "Categoria já existe.");
            else if (resultado == 0)
                return RespostaWrapper.EnveloparInsucesso<CategoriaDto>(null);

            var categoriaDtoIncluida = _servico.ConsultarCategoria(resultado.Value);

            return RespostaWrapper.EnveloparSucesso(categoriaDtoIncluida);
        }

        private static RespostaWrapper PutCategoria(string id, string json)
        {
            var categoriaDtoRecebida = ConversorJson.Desserializar<CategoriaDto>(json);

            categoriaDtoRecebida.Id = Convert.ToInt32(id);

            var resultado = _servico.AlterarCategoria(categoriaDtoRecebida);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrada uma categoria com o código informado.");
            else if (resultado == false)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteCategoria(string id)
        {
            var resultado = _servico.RemoverCategoria(id);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrada uma categoria com o código informado.");
            else if (resultado == false)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}
