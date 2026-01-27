using C2_BLL.DTOs;
using C2_BLL.Servicos;
using PseudoFramework.SharedUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C3_UI.Rotas
{
    public class ExemploRota: IRota
    {
        public const string RECURSO = "exemplos";

        private static readonly ExemploServico _servico = new ExemploServico();

        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetExemplo(id);

                            return GetExemplos();
                        }
                    case "POST":
                        return PostExemplo(json);

                    case "PUT":
                        return PutExemplo(id, json);

                    case "DELETE":
                        return DeleteExemplo(id);

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de exemplo não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<ExemploSaidaDto>> GetExemplos()
        {
            var listaExemplosDto = _servico.ListarExemplos();

            if (!listaExemplosDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaExemplosDto, "Lista de exemplos está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaExemplosDto);
        }

        private static RespostaWrapper<ExemploSaidaDto> GetExemplo(string id)
        {
            var exemploDtoConsultado = _servico.ConsultarExemplo(id);

            if (exemploDtoConsultado == null)
                return RespostaWrapper.EnveloparInsucesso<ExemploSaidaDto>(null, "Exemplo não encontrado.");

            return RespostaWrapper.EnveloparSucesso(exemploDtoConsultado);
        }

        private static RespostaWrapper<ExemploSaidaDto> PostExemplo(string json)
        {
            var exemploDtoRecebido = ConversorJson.Desserializar<ExemploEntradaDto>(json);

            var exemploDtoIncluido = _servico.IncluirExemplo(exemploDtoRecebido);

            return RespostaWrapper.EnveloparSucesso(exemploDtoIncluido);
        }

        private static RespostaWrapper PutExemplo(string id, string json)
        {
            var exemploDtoRecebido = ConversorJson.Desserializar<ExemploEntradaDto>(json);

            var resultado = _servico.AlterarExemplo(id, exemploDtoRecebido);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrado um exemplo com o id informado.");
            else if (resultado == false)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteExemplo(string id)
        {
            var resultado = _servico.RemoverExemplo(id);

            if (resultado == null)
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrado um exemplo com o id informado.");
            else if (resultado == false)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}
