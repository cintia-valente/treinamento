using PseudoFramework.SharedUtils;
using C2_BLL.DTOs;
using C2_BLL.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C3_UI.Rotas
{
    public class IdiomaRota: IRota
    {
        public const string RECURSO = "idiomas";

        private static readonly IdiomaServico _servico = new IdiomaServico();
 
        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetIdioma(id); // US5

                            return GetIdiomas(); // US4
                        }
                    case "POST":
                        return PostIdioma(json); // US1

                    case "PUT":
                        return PutIdioma(id, json); // US3

                    case "DELETE":
                        return DeleteIdioma(id); // US2

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de idioma não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<IdiomaDto>> GetIdiomas()
        {
            var listaIdiomasDto = _servico.ListarIdiomas();
              
            if (!listaIdiomasDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaIdiomasDto, "Lista de idiomas está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaIdiomasDto);
        }

        private static RespostaWrapper<IdiomaDto> GetIdioma(string id)
        {
            var idiomaDtoConsultado = _servico.ConsultarIdioma(id);

            if (idiomaDtoConsultado == null)
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null, "Idioma não encontrada.");

            return RespostaWrapper.EnveloparSucesso(idiomaDtoConsultado);
        }

        private static RespostaWrapper<IdiomaDto> PostIdioma(string json)
        {
            var idiomaDtoRecebido = ConversorJson.Desserializar<IdiomaDto>(json);

            var resultado = _servico.IncluirIdioma(idiomaDtoRecebido);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null, "Idioma já existe.");
            else if (resultado == 0)
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null);

            var idiomaDtoIncluido = _servico.ConsultarIdioma(resultado.Value);

            return RespostaWrapper.EnveloparSucesso(idiomaDtoIncluido);
        }

        private static RespostaWrapper PutIdioma(string id, string json)
        {
            var idiomaDtoRecebido = ConversorJson.Desserializar<IdiomaDto>(json);

            idiomaDtoRecebido.Id = Convert.ToInt32(id);

            var resultado = _servico.AlterarIdioma(idiomaDtoRecebido);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrada um idioma com o código informado.");
            else if (resultado == false)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteIdioma(string id)
        {
            var resultado = _servico.RemoverIdioma(id);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrada uma categoria com o código informado.");
            else if (resultado == false)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}