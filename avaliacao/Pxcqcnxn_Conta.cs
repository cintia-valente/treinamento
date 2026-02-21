using Bergs.Pwx.Pwxodaxn;
using Bergs.Pwx.Pwxodaxn.Excecoes;
using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.BD;
using Bergs.Pwx.Pwxoiexn.Mensagens;
using Bergs.Pxc.Pxcbtoxn;
using Bergs.Pxc.Pxcbtoxn.pgm;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bergs.Pxc.Pxcqcnxn
{
    /// <summary>
    /// Classe que possui os métodos de manipulação de dados da tabela CONTA da base de dados PXC
    /// </summary>
    public class Conta: AplicacaoDados
    {
        private static readonly string Alias = string.Empty;

        /// <summary>
        /// Método incluir referente à tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Incluir(TOConta toConta) // US1 - Inclusão de nova conta bancária
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"INSERT INTO {TOConta.TABELA} (");

                Sql.MontarCampoInsert(TOConta.NUMERO_AGENCIA, toConta.Agencia);
                Sql.MontarCampoInsert(TOConta.NUMERO_CONTA, toConta.Conta);
                Sql.MontarCampoInsert(TOConta.SALDO_CONTA, toConta.Saldo);
                Sql.MontarCampoInsert(TOConta.CODIGO_CLIENTE, toConta.CodCliente);
                Sql.MontarCampoInsert(TOConta.TIPO_PESSOA_CLIENTE, toConta.IndTpPessoa);
                Sql.MontarCampoInsert(TOConta.SITUACAO, toConta.IndSituacao);
                Sql.MontarCampoInsert(TOConta.CODIGO_USUARIO, toConta.CodOperador);

                Sql.MontarCampoInsert(TOConta.DATA_HORA_ULTIMA_ALTERACAO); // SAC - Inicialização do token
                Sql.Temporario.Append(BDUtils.BD_CURRENT_TIMESTAMP);

                Sql.Comando.Append(") VALUES (");
                Sql.Comando.Append(Sql.Temporario);
                Sql.Comando.Append(")");

                var registrosAfetados = IncluirDados();

                return Infra.RetornarSucesso(registrosAfetados);
            }
            catch (RegistroDuplicadoException exception)
            {
                return Infra.RetornarFalha<int>(new RegistroDuplicadoMensagem(exception));
            }
            catch (ChaveEstrangeiraInexistenteException exception)
            {
                return Infra.RetornarFalha<int>(new ChaveEstrangeiraInexistenteMensagem(exception));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Método listar referente à tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <param name="toPaginacao">Classe da infra-estrutura contendo as informações de paginação</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<List<TOConta>> Listar(TOConta toConta, TOPaginacao toPaginacao) // US2 - Listagem de contas bancárias
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"SELECT * FROM {QualificarTabela(TOConta.TABELA)}");

                Sql.MontarCampoWhere(QualificarCampo(TOConta.NUMERO_AGENCIA), toConta.Agencia);
                Sql.MontarCampoWhere(QualificarCampo(TOConta.NUMERO_CONTA), toConta.Conta);
                Sql.MontarCampoWhere(QualificarCampo(TOConta.CODIGO_CLIENTE), toConta.CodCliente);
                Sql.MontarCampoWhere(QualificarCampo(TOConta.TIPO_PESSOA_CLIENTE), toConta.IndTpPessoa);
                Sql.MontarCampoWhere(QualificarCampo(TOConta.SITUACAO), toConta.IndSituacao);

                var listaTOs = toPaginacao != null
                    ? ListarPaginaTOsConta(toPaginacao)
                    : ListarTodosTOsConta();

                return Infra.RetornarSucesso(listaTOs);
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<List<TOConta>>(exception);
            }
        }

        /// <summary>
        /// Método obter referente à tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<TOConta> Obter(TOConta toConta) // US3 - Consulta de conta bancária
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"SELECT * FROM {QualificarTabela(TOConta.TABELA)}");

                Sql.MontarCampoWhere(QualificarCampo(TOConta.NUMERO_AGENCIA), toConta.Agencia);
                Sql.MontarCampoWhere(QualificarCampo(TOConta.NUMERO_CONTA), toConta.Conta);

                var registro = ObterDados();

                if (registro == null)
                    return Infra.RetornarFalha<TOConta>(new RegistroInexistenteMensagem());

                var toContaConsultado = GerarTOConta(registro);

                return Infra.RetornarSucesso(toContaConsultado);
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<TOConta>(exception);
            }
        }

        /// <summary>
        /// Método alterar referente à tabela CONTA
        /// </summary>
        /// <param name="toConta">Transfer Object de entrada referente à tabela CONTA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Alterar(TOConta toConta) // US4 - Alteração de situação de conta bancária
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"UPDATE {TOConta.TABELA}");

                Sql.MontarCampoSet(TOConta.SITUACAO, toConta.IndSituacao);
                Sql.MontarCampoSet(TOConta.CODIGO_USUARIO, toConta.CodOperador);

                Sql.MontarCampoSet(TOConta.DATA_HORA_ULTIMA_ALTERACAO); // SAC - Refresh para um novo token
                Sql.Comando.Append(BDUtils.BD_CURRENT_TIMESTAMP);

                Sql.MontarCampoWhere(TOConta.NUMERO_AGENCIA, toConta.Agencia);
                Sql.MontarCampoWhere(TOConta.NUMERO_CONTA, toConta.Conta);

                Sql.MontarCampoWhere(TOConta.DATA_HORA_ULTIMA_ALTERACAO, toConta.UltAtualizacao); // SAC - Uso do token atual

                var registrosAfetados = AlterarDados();

                if (registrosAfetados == 0)
                    return Infra.RetornarFalha<int>(new ConcorrenciaMensagem());

                return Infra.RetornarSucesso(registrosAfetados);
            }
            catch (ChaveEstrangeiraInexistenteException exception)
            {
                return Infra.RetornarFalha<int>(new ChaveEstrangeiraInexistenteMensagem(exception));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Método responsável por definir a relação entre o campo do TO e o parâmetro SQL correspondente
        /// </summary>
        /// <param name="nomeCampo">Nome do campo a ser parametrizado</param>
        /// <param name="conteudo">Conteúdo na propriedade do TO respectiva ao campo a ser parametrizado</param>
        /// <returns>Parâmetro recém-criado.</returns>
        protected override Parametro CriarParametro(string nomeCampo, object conteudo)
        {
            Parametro parametro = new Parametro();
            switch (nomeCampo)
            {
                case TOConta.NUMERO_AGENCIA:
                    {
                        parametro.DbType = DbType.Decimal;
                        parametro.Size = 4;
                        parametro.Precision = 4;

                        break;
                    }
                case TOConta.NUMERO_CONTA:
                    {
                        parametro.DbType = DbType.Decimal;
                        parametro.Size = 10;
                        parametro.Precision = 10;

                        break;
                    }
                case TOConta.SALDO_CONTA:
                    {
                        parametro.DbType = DbType.Decimal;
                        parametro.Size = 15;
                        parametro.Precision = 15;
                        parametro.Scale = 2;

                        break;
                    }
                case TOConta.CODIGO_CLIENTE:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 14;
                        parametro.Precision = 14;

                        break;
                    }
                case TOConta.TIPO_PESSOA_CLIENTE:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 1;
                        parametro.Precision = 1;

                        break;
                    }
                case TOConta.SITUACAO:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 1;
                        parametro.Precision = 1;

                        break;
                    }
                case TOConta.CODIGO_USUARIO:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 6;
                        parametro.Precision = 6;

                        break;
                    }
                case TOConta.DATA_HORA_ULTIMA_ALTERACAO:
                    {
                        parametro.DbType = DbType.DateTime;
                        parametro.Size = 10;
                        parametro.Precision = 10;
                        parametro.Scale = 6;

                        break;
                    }
                default:
                    {
#if DEBUG // Diretiva de compilação condicional - Só vai constar na aplicação quando rodando em modo debug
                        parametro = null; // Força um erro para alertar o desenvolvedor, pois todo parâmetro deve ser tratado no switch
#endif
                        break;
                    }
            }

            parametro.Direction = ParameterDirection.Input;

            parametro.SourceColumn = nomeCampo;

            if (BDUtils.IsParametroPontoFlutuante(parametro.DbType, parametro.Scale) && conteudo != null)
                parametro.Value = BDUtils.FormatarParametroPontoFlutuante(conteudo, parametro.Scale);
            else
                parametro.Value = conteudo;

            return parametro;
        }

        /// <summary>
        /// Auxiliar que reseta/limpa os controles utilizados para a montagem dos comandos (operação de segurança)
        /// </summary>
        private void ResetarControlesComando()
        {
            Sql.Comando.Clear();

            Sql.Temporario.Clear();

            Parametros.Clear();
        }

        /// <summary>
        /// Auxiliar que controla a inserção ou não do alias SQL nos nomes de tabelas durante a montagem dos comandos
        /// </summary>
        /// <returns>Nome qualificado da tabela</returns>
        private string QualificarTabela(string nomeTabela)
        {
            return !string.IsNullOrWhiteSpace(Alias)
                ? $"{nomeTabela} {Alias}"
                : nomeTabela;
        }

        /// <summary>
        /// Auxiliar que controla a inserção ou não do alias SQL nos nomes de campo durante a montagem dos comandos
        /// </summary>
        /// <param name="nomeCampo">Nome do campo a ser qualificado</param>
        /// <returns>Nome qualificado do campo</returns>
        private string QualificarCampo(string nomeCampo)
        {
            return !string.IsNullOrWhiteSpace(Alias)
                ? $"{Alias}.{nomeCampo}"
                : nomeCampo;
        }

        /// <summary>
        /// Auxiliar que faz a listagem completa de TOs
        /// </summary>
        /// <returns>Lista completa de TOs de conta</returns>
        private List<TOConta> ListarTodosTOsConta()
        {
            var listaCompletaTOs = new List<TOConta>();

            using (var listaConectada = ListarDados())
            {
                while (listaConectada.Ler())
                {
                    var toRetorno = GerarTOConta(listaConectada.LinhaAtual);

                    listaCompletaTOs.Add(toRetorno);
                }
            }

            return listaCompletaTOs;
        }

        /// <summary>
        /// Auxiliar que faz a listagem de TOs com base no objeto de paginação
        /// </summary>
        /// <param name="toPaginacao">Objeto com as configurações da página a ser listada</param>
        /// <returns>Lista de TOs de conta com base na página selecionada</returns>
        private List<TOConta> ListarPaginaTOsConta(TOPaginacao toPaginacao)
        {
            var paginaTOs = new List<TOConta>();

            var listaDesconectada = ListarDados(toPaginacao);

            foreach (var linhaAtual in listaDesconectada.Linhas)
            {
                var toRetorno = GerarTOConta(linhaAtual);

                paginaTOs.Add(toRetorno);
            }

            return paginaTOs;
        }

        /// <summary>
        /// Auxiliar que efetua a criação de um novo TO de conta com base em um registro oriundo da base de dados
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Instância de TOConta</returns>
        private TOConta GerarTOConta(Linha registro)
        {
            var toConta = new TOConta();

            toConta.PopularRetorno(registro);

            return toConta;
        }
    }
}