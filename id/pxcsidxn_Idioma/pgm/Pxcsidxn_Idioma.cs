using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.Mensagens;
using Bergs.Pwx.Pwxoiexn.Relatorios;
using Bergs.Pwx.Pwxoiexn.RN;
using Bergs.Pxc.Pxcbtoxn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bergs.Pxc.Pxcsidxn
{
    /// <summary>
    /// Classe que possui as regras de negócio para o acesso da tabela IDIOMA da base de dados PXC
    /// </summary>
    public class Idioma: AplicacaoRegraNegocio
    {
        /// <summary>
        /// Altera registro da tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Alterar(TOIdioma toIdioma) // US3 - Alteração de dados de idioma
        {
            try
            {
                if (!toIdioma.CodIdioma.FoiSetado && !toIdioma.CodigoIsoCombinado.FoiSetado)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarExistenciaCodIsoOuCodNumerico));

                if (!toIdioma.DescIdioma.FoiSetado)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarExistenciaDescricao));

                if (!ValidarTO(toIdioma, out var listaRetornoValidacao))
                    return Infra.RetornarFalha<int>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                var reconciliacaoCodigos = ReconciliarCodigosIdioma(toIdioma);

                if (!reconciliacaoCodigos.OK)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem((IdiomaMensagem)reconciliacaoCodigos.Mensagem));

                /*
                 * Isola o TO de filtragem da obtenção para evitar qualquer tipo de colisão com as propostas 
                 * de alteração contidas no TO principal
                */
                var resultadoConsulta = Obter(new TOIdioma() { CodIdioma = toIdioma.CodIdioma });

                if (!resultadoConsulta.OK)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConsultarIdiomaBaseDados, resultadoConsulta.Mensagem.ParaUsuario));

                toIdioma.DthrUltAtu = resultadoConsulta.Dados.DthrUltAtu; // Pega o token de atualização para SAC
                toIdioma.CodUsuario = Infra.Usuario.Matricula;

                var bdIdioma = Infra.InstanciarBD<Pxcqidxn.Idioma>();

                using (EscopoTransacional escopo = Infra.CriarEscopoTransacional())
                {
                    var resultado = bdIdioma.Alterar(toIdioma);

                    if (!resultado.OK)
                        return resultado;

                    escopo.EfetivarTransacao();

                    return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem("Alteração"));
                }
            }
            catch (ArgumentException exception)
            {
                return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConverterCodIsoParaCodNumerico, toIdioma.CodigoIsoCombinado, exception.Message));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Conta quantidade de registros da tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<long> Contar(TOIdioma toIdioma) // Extra (sem US vinculada)
        {
            try
            {
                var bdIdioma = Infra.InstanciarBD<Pxcqidxn.Idioma>();

                var resultado = bdIdioma.Contar(toIdioma);

                if (!resultado.OK)
                    return resultado;

                return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem());
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<long>(exception);
            }
        }

        /// <summary>
        /// Exclui registro da tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Excluir(TOIdioma toIdioma) // US2 - Remoção de idioma
        {
            try
            {
                if (!toIdioma.CodIdioma.FoiSetado && !toIdioma.CodigoIsoCombinado.FoiSetado)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarExistenciaCodIsoOuCodNumerico));

                if (!ValidarTO(toIdioma, out var listaRetornoValidacao))
                    return Infra.RetornarFalha<int>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                var reconciliacaoCodigos = ReconciliarCodigosIdioma(toIdioma);

                if (!reconciliacaoCodigos.OK)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem((IdiomaMensagem)reconciliacaoCodigos.Mensagem));

                /*
                 * Isola o TO de filtragem da obtenção para evitar qualquer tipo de colisão com os dados 
                 * de exclusão contidos no TO principal
                */
                var resultadoConsulta = Obter(new TOIdioma() { CodIdioma = toIdioma.CodIdioma });

                if (!resultadoConsulta.OK)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConsultarIdiomaBaseDados, resultadoConsulta.Mensagem.ParaUsuario));

                toIdioma.DthrUltAtu = resultadoConsulta.Dados.DthrUltAtu; // Pega o token de atualização para SAC

                var bdIdioma = Infra.InstanciarBD<Pxcqidxn.Idioma>();

                using (EscopoTransacional escopo = Infra.CriarEscopoTransacional())
                {
                    var resultado = bdIdioma.Excluir(toIdioma);

                    if (!resultado.OK)
                        return resultado;

                    escopo.EfetivarTransacao();

                    return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem("Exclusão"));
                }
            }
            catch (ArgumentException exception)
            {
                return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConverterCodIsoParaCodNumerico, toIdioma.CodigoIsoCombinado, exception.Message));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Gera relatório da tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta e o nome do relatório gerado, ou as informações de erro</returns>
        public virtual Retorno<string> Imprimir(TOIdioma toIdioma) // Extra (sem US vinculada)
        {
            try
            {
                var resultado = Listar(toIdioma, null);

                if (!resultado.OK)
                    return Infra.RetornarFalha<List<TOIdioma>, string>(resultado);

                using (var relatorio = new RelatorioPadrao(Infra))
                {
                    relatorio.Colunas.Add(new Coluna(TOIdioma.CODIGO_IDIOMA, 8));
                    relatorio.Colunas.Add(new Coluna(TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA, 6));
                    relatorio.Colunas.Add(new Coluna(TOIdioma.DESCRICAO_IDIOMA, 50));

                    var relatorioBuilder = new StringBuilder();

                    foreach (TOIdioma toIdiomaAtual in resultado.Dados)
                    {
                        var definicaoColunas = relatorio.Colunas;

                        relatorioBuilder.Append(
                            toIdiomaAtual.CodIdioma
                                .ToString()
                                .PadRight(definicaoColunas[TOIdioma.CODIGO_IDIOMA].Tamanho)
                        );

                        relatorioBuilder.Append(
                            toIdiomaAtual.CodigoIsoCombinado
                                .ToString()
                                .PadRight(definicaoColunas[TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA].Tamanho)
                        );

                        relatorioBuilder.Append(
                            toIdiomaAtual.DescIdioma
                                .ToString()
                                .PadRight(definicaoColunas[TOIdioma.DESCRICAO_IDIOMA].Tamanho)
                        );

                        relatorio.AdicionarLinha(relatorioBuilder.ToString());

                        relatorioBuilder.Clear();
                    }

                    return Infra.RetornarSucesso(relatorio.NomeArquivoVirtual, new OperacaoRealizadaMensagem());
                }
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<string>(exception);
            }
        }

        /// <summary>
        /// Inclui registro na tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Incluir(TOIdioma toIdioma) // US1 - Inclusão de novo idioma
        {
            try
            {
                if (toIdioma.CodIdioma.FoiSetado)
                    toIdioma.CodIdioma = new CampoObrigatorio<int>(); // Desseta porque senão vai cair na validação do ValidarTO

                if (!toIdioma.CodigoIsoCombinado.FoiSetado)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarExistenciaCodIso));

                if (!toIdioma.DescIdioma.FoiSetado)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarExistenciaDescricao));

                if (!ValidarTO(toIdioma, out var listaRetornoValidacao))
                    return Infra.RetornarFalha<int>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                toIdioma.CodIdioma = Feconid.IsoToCodigo(toIdioma.CodigoIsoCombinado);

                var resultadoConsulta = Obter(toIdioma);

                if (!resultadoConsulta.OK && !(resultadoConsulta.Mensagem is RegistroInexistenteMensagem))
                    return Infra.RetornarFalha<int>(resultadoConsulta.Mensagem);

                if (resultadoConsulta.OK)
                    return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnIncluirIdiomaJaExistente));

                toIdioma.CodUsuario = Infra.Usuario.Matricula;

                var bdIdioma = Infra.InstanciarBD<Pxcqidxn.Idioma>();

                using (EscopoTransacional escopo = Infra.CriarEscopoTransacional())
                {
                    var resultado = bdIdioma.Incluir(toIdioma);

                    if (!resultado.OK)
                        return resultado;

                    escopo.EfetivarTransacao();

                    return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem("Inclusão"));
                }
            }
            catch (ArgumentException exception)
            {
                return Infra.RetornarFalha<int>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConverterCodIsoParaCodNumerico, toIdioma.CodigoIsoCombinado, exception.Message));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Lista registros da tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <param name="toPaginacao">Classe da infra-estrutura contendo as informações de paginação</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<List<TOIdioma>> Listar(TOIdioma toIdioma, TOPaginacao toPaginacao) // US4 - Listagem de idiomas
        {
            try
            {
                var bdIdioma = Infra.InstanciarBD<Pxcqidxn.Idioma>();

                var resultado = bdIdioma.Listar(toIdioma, toPaginacao);

                if (!resultado.OK)
                    return resultado;

                return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem());
            }
            catch (ArgumentException exception)
            {
                return Infra.RetornarFalha<List<TOIdioma>>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConverterCodNumericoParaCodIso, toIdioma.CodIdioma.ToString(), exception.Message));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<List<TOIdioma>>(exception);
            }
        }

        /// <summary>
        /// Obtém registro da tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<TOIdioma> Obter(TOIdioma toIdioma) // US5 - Consulta de idioma por código
        {
            try
            {
                if (!toIdioma.CodIdioma.FoiSetado && !toIdioma.CodigoIsoCombinado.FoiSetado)
                    return Infra.RetornarFalha<TOIdioma>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarExistenciaCodIsoOuCodNumerico));

                if (!ValidarTO(toIdioma, out var listaRetornoValidacao))
                    return Infra.RetornarFalha<TOIdioma>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                var reconciliacaoCodigos = ReconciliarCodigosIdioma(toIdioma);

                if (!reconciliacaoCodigos.OK)
                    return Infra.RetornarFalha<TOIdioma>(new IdiomaMensagem((IdiomaMensagem)reconciliacaoCodigos.Mensagem));

                var bdIdioma = Infra.InstanciarBD<Pxcqidxn.Idioma>();

                var resultado = bdIdioma.Obter(toIdioma);

                if (!resultado.OK)
                    return resultado;

                return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem());
            }
            catch (ArgumentException exception)
            {
                return Infra.RetornarFalha<TOIdioma>(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnConverterCodNumericoParaCodIso, toIdioma.CodIdioma.ToString(), exception.Message));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<TOIdioma>(exception);
            }
        }

        private Retorno ReconciliarCodigosIdioma(TOIdioma toIdioma)
        {
            if (toIdioma.CodIdioma.FoiSetado && toIdioma.CodigoIsoCombinado.FoiSetado)
            {
                var codigoNumericoCalculado = Feconid.IsoToCodigo(toIdioma.CodigoIsoCombinado);

                if (toIdioma.CodIdioma != codigoNumericoCalculado)
                    return Infra.RetornarFalha(new IdiomaMensagem(TipoIdiomaMensagem.FalhaRnValidarEquivalenciaCodIsoOuCodNumerico));
            }
            else if (!toIdioma.CodIdioma.FoiSetado && toIdioma.CodigoIsoCombinado.FoiSetado)
            {
                toIdioma.CodIdioma = Feconid.IsoToCodigo(toIdioma.CodigoIsoCombinado);
            }

            return Infra.RetornarSucesso();
        }
    }
}
