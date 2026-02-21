using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.Mensagens;
using Bergs.Pwx.Pwxoiexn.RN;
using Bergs.Pxc.Pxcbtoxn;
using Bergs.Pxc.Pxcbtoxn.pgm;
using System;
using System.Collections.Generic;

namespace Bergs.Pxc.Pxcscnxn
{
    /// <summary>
    /// Classe que possui as regras de negócio para o acesso da tabela CONTA da base de dados PXC
    /// </summary>
    public class Conta: AplicacaoRegraNegocio
    {
        /// <summary>
        /// Inclui registro na tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Incluir(TOConta toConta) // US1 - Inclusão de nova conta bancária
        {
            try
            {
                if (!toConta.Agencia.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.AgenciaInvalida));

                if (!toConta.Conta.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.ContaInvalida));

                if (!toConta.Saldo.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.SaldoInvalido));

                // Esta validação foi feita apenas por questão de padronização, não existe regra nem CA que exiga a existência dela portanto não será cobrada
                if (!toConta.CodCliente.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.ClienteInvalido));

                // Esta validação foi feita apenas por questão de padronização, não existe regra nem CA que exiga a existência dela portanto não será cobrada
                if (!toConta.IndTpPessoa.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.TipoPessoaInvalido));

                decimal agencia = Convert.ToDecimal(toConta.Agencia.Conteudo);
                if (!IsAgenciaValida(agencia))
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.AgenciaInvalida));

                decimal conta = Convert.ToInt64(toConta.Conta.Conteudo);
                if (!IsNumeroContaValido(conta))
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.ContaInvalida));

                if (Convert.ToDecimal(toConta.Saldo.Conteudo) < 0)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.SaldoInvalido));

                if (!IsTipoPessoaValido(toConta.IndTpPessoa))
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.TipoPessoaInvalido));

                if (!ValidarTO(toConta, out var listaRetornoValidacao))
                    return Infra.RetornarFalha<int>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                toConta.CodOperador = Infra.Usuario.Matricula;
                toConta.IndSituacao = SituacaoConta.Ativa;

                var bdConta = Infra.InstanciarBD<Pxcqcnxn.Conta>();

                using (EscopoTransacional escopo = Infra.CriarEscopoTransacional())
                {
                    var resultado = bdConta.Incluir(toConta);

                    if (!resultado.OK && (resultado.Mensagem is RegistroDuplicadoMensagem))
                        return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.ContaAgenciaJaExiste));

                    if (!resultado.OK)
                        return resultado;

                    escopo.EfetivarTransacao();

                    return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem("Inclusão"));
                }
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Lista registros da tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <param name="toPaginacao">Classe da infra-estrutura contendo as informações de paginação</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<List<TOConta>> Listar(TOConta toConta, TOPaginacao toPaginacao) // US2 - Listagem de contas bancárias
        {
            try
            {
                var bdConta = Infra.InstanciarBD<Pxcqcnxn.Conta>();

                var resultado = bdConta.Listar(toConta, toPaginacao);

                if (!resultado.OK)
                    return resultado;

                if (resultado.Dados.Count == 0)
                    return Infra.RetornarFalha<List<TOConta>>(new ContaMensagem(TipoMensagem.NaoExistemContasBancariasCadastradas));

                return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem());
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<List<TOConta>>(exception);
            }
        }

        /// <summary>
        /// Obtém registro da tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<TOConta> Obter(TOConta toConta) // US3 - Consulta de conta bancária
        {
            try
            {
                if (!toConta.Agencia.FoiSetado)
                    return Infra.RetornarFalha<TOConta>(new ContaMensagem(TipoMensagem.AgenciaInvalida));

                if (!toConta.Conta.FoiSetado)
                    return Infra.RetornarFalha<TOConta>(new ContaMensagem(TipoMensagem.ContaInvalida));

                decimal agencia = Convert.ToDecimal(toConta.Agencia.Conteudo);
                if (!IsAgenciaValida(agencia))
                    return Infra.RetornarFalha<TOConta>(new ContaMensagem(TipoMensagem.AgenciaInvalida));

                decimal conta = Convert.ToInt64(toConta.Conta.Conteudo);
                if (!IsNumeroContaValido(conta))
                    return Infra.RetornarFalha<TOConta>(new ContaMensagem(TipoMensagem.ContaInvalida));

                var bdConta = Infra.InstanciarBD<Pxcqcnxn.Conta>();

                var resultado = bdConta.Obter(toConta);

                if (!resultado.OK && (resultado.Mensagem is RegistroInexistenteMensagem))
                    return Infra.RetornarFalha<TOConta>(new ContaMensagem(TipoMensagem.NaoExisteContaBancariaCadastrada));

                if (!resultado.OK)
                    return resultado;

                return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem());
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<TOConta>(exception);
            }
        }

        /// <summary>
        /// Altera registro da tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Alterar(TOConta toConta) // US4 - Alteração de situação de conta bancária
        {
            try
            {
                if (!toConta.Agencia.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.AgenciaInvalida));

                if (!toConta.Conta.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.ContaInvalida));

                // Esta validação foi feita apenas por questão de padronização, não existe regra nem CA que exiga a existência dela portanto não será cobrada
                if (!toConta.IndSituacao.FoiSetado)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.SituacaoInvalida));

                decimal agencia = Convert.ToDecimal(toConta.Agencia.Conteudo);
                if (!IsAgenciaValida(agencia))
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.AgenciaInvalida));

                decimal conta = Convert.ToInt64(toConta.Conta.Conteudo);
                if (!IsNumeroContaValido(conta))
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.ContaInvalida));

                if (!IsSituacaoContaValida(toConta.IndSituacao))
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.SituacaoInvalida));

                var resultadoConsulta = Obter(
                    /*
                     * Isola o TO de filtragem da obtenção para evitar qualquer tipo de colisão com as propostas 
                     * de alteração contidas no TO principal
                     */
                    new TOConta()
                    {
                        Agencia = toConta.Agencia,
                        Conta = toConta.Conta
                    }
                );

                if (!resultadoConsulta.OK)
                    return Infra.RetornarFalha<int>(new ContaMensagem(TipoMensagem.NaoExisteContaBancariaCadastrada));

                toConta.UltAtualizacao = resultadoConsulta.Dados.UltAtualizacao; // Pega o token de atualização para SAC
                toConta.CodOperador = Infra.Usuario.Matricula;

                var bdConta = Infra.InstanciarBD<Pxcqcnxn.Conta>();

                using (EscopoTransacional escopo = Infra.CriarEscopoTransacional())
                {
                    var resultado = bdConta.Alterar(toConta);

                    if (!resultado.OK)
                        return resultado;

                    escopo.EfetivarTransacao();

                    return Infra.RetornarSucesso(resultado.Dados, new OperacaoRealizadaMensagem("Alteração"));
                }
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        private bool IsAgenciaValida(decimal agencia)
        {
            return agencia >= 1000 && agencia <= 9999;
        }

        private bool IsNumeroContaValido(decimal conta)
        {
            return conta >= 1 && conta <= 2000000000;
        }

        private bool IsTipoPessoaValido(TipoPessoa tipoPessoa)
        {
            return Enum.IsDefined(typeof(TipoPessoa), tipoPessoa);
        }

        private bool IsSituacaoContaValida(SituacaoConta situacao)
        {
            return Enum.IsDefined(typeof(SituacaoConta), situacao);
        }
    }
}
