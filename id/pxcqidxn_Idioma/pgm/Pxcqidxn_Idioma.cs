using Bergs.Pwx.Pwxodaxn;
using Bergs.Pwx.Pwxodaxn.Excecoes;
using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.BD;
using Bergs.Pwx.Pwxoiexn.Mensagens;
using Bergs.Pxc.Pxcbtoxn;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bergs.Pxc.Pxcqidxn
{
    /// <summary>
    /// Classe que possui os métodos de manipulação de dados da tabela IDIOMA da base de dados PXC
    /// </summary>
    public class Idioma: AplicacaoDados
    {
        private static readonly string Alias = string.Empty;

        /// <summary>
        /// Método alterar referente à tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Alterar(TOIdioma toIdioma) // US3 - Alteração de dados de idioma
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"UPDATE {TOIdioma.TABELA}");

                Sql.MontarCampoSet(TOIdioma.DESCRICAO_IDIOMA, toIdioma.DescIdioma);
                Sql.MontarCampoSet(TOIdioma.CODIGO_USUARIO, toIdioma.CodUsuario);

                Sql.MontarCampoSet(TOIdioma.DATA_HORA_ULTIMA_ALTERACAO); // SAC - Refresh para um novo token
                Sql.Comando.Append(BDUtils.BD_CURRENT_TIMESTAMP);

                Sql.MontarCampoWhere(TOIdioma.CODIGO_IDIOMA, toIdioma.CodIdioma);
                Sql.MontarCampoWhere(TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA, toIdioma.CodigoIsoCombinado);

                Sql.MontarCampoWhere(TOIdioma.DATA_HORA_ULTIMA_ALTERACAO, toIdioma.DthrUltAtu); // SAC - Uso do token atual

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
        /// Método contar referente à tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<long> Contar(TOIdioma toIdioma) // Extra (sem US vinculada)
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"SELECT COUNT(*) FROM {QualificarTabela(TOIdioma.TABELA)}");

                /*
                 * Só vão ser agregados ao comando os campos abaixo do TO que estiverem em estados
                 * SETADO COM CONTEÚDO ou SETADO SEM CONTEÚDO
                 * - Lembrando dos estados possíveis: SETADO COM CONTEÚDO, NÃO SETADO e 
                 * SETADO SEM CONTEÚDO (exclusivo para campos CampoOpcional<T>)
                */
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_IDIOMA), toIdioma.CodIdioma);
                //Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA), toIdioma.CodigoIsoCombinado);
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.DESCRICAO_IDIOMA), toIdioma.DescIdioma);
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_USUARIO), toIdioma.CodUsuario);
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.DATA_HORA_ULTIMA_ALTERACAO), toIdioma.DthrUltAtu);

                var quantidadeRegistros = ContarDados();

                return Infra.RetornarSucesso(quantidadeRegistros);
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<long>(exception);
            }
        }

        /// <summary>
        /// Método excluir referente à tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Excluir(TOIdioma toIdioma) // US2 - Remoção de idioma
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"DELETE FROM {TOIdioma.TABELA}");

                Sql.MontarCampoWhere(TOIdioma.CODIGO_IDIOMA, toIdioma.CodIdioma);

                Sql.MontarCampoWhere(TOIdioma.DATA_HORA_ULTIMA_ALTERACAO, toIdioma.DthrUltAtu); // SAC - Uso do token atual

                var registrosAfetados = ExcluirDados();

                if (registrosAfetados == 0)
                    return Infra.RetornarFalha<int>(new ConcorrenciaMensagem());

                return Infra.RetornarSucesso(registrosAfetados);
            }
            catch (ChaveEstrangeiraReferenciadaException exception)
            {
                return Infra.RetornarFalha<int>(new ChaveEstrangeiraReferenciadaMensagem(exception));
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<int>(exception);
            }
        }

        /// <summary>
        /// Método incluir referente à tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<int> Incluir(TOIdioma toIdioma) // US1 - Inclusão de novo idioma
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"INSERT INTO {TOIdioma.TABELA} (");

                Sql.MontarCampoInsert(TOIdioma.CODIGO_IDIOMA, toIdioma.CodIdioma);
                Sql.MontarCampoInsert(TOIdioma.DESCRICAO_IDIOMA, toIdioma.DescIdioma);
                Sql.MontarCampoInsert(TOIdioma.CODIGO_USUARIO, toIdioma.CodUsuario);

                Sql.MontarCampoInsert(TOIdioma.DATA_HORA_ULTIMA_ALTERACAO); // SAC - Inicialização do token
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
        /// Método listar referente à tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <param name="toPaginacao">Classe da infra-estrutura contendo as informações de paginação</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<List<TOIdioma>> Listar(TOIdioma toIdioma, TOPaginacao toPaginacao) // US4 - Listagem de idiomas
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"SELECT * FROM {QualificarTabela(TOIdioma.TABELA)}");

                /*
                 * Só vão ser agregados ao comando os campos abaixo do TO que estiverem em estados
                 * SETADO COM CONTEÚDO ou SETADO SEM CONTEÚDO
                 * - Lembrando dos estados possíveis: SETADO COM CONTEÚDO, NÃO SETADO e 
                 * SETADO SEM CONTEÚDO (exclusivo para campos CampoOpcional<T>)
                */
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_IDIOMA), toIdioma.CodIdioma);
                //Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA), toIdioma.CodigoIsoCombinado);
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.DESCRICAO_IDIOMA), toIdioma.DescIdioma);
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_USUARIO), toIdioma.CodUsuario);
                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.DATA_HORA_ULTIMA_ALTERACAO), toIdioma.DthrUltAtu);

                var listaTOs = toPaginacao != null
                    ? ListarPaginaTOsIdioma(toPaginacao)
                    : ListarTodosTOsIdioma();

                return Infra.RetornarSucesso(listaTOs);
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<List<TOIdioma>>(exception);
            }
        }

        /// <summary>
        /// Método obter referente à tabela IDIOMA
        /// </summary>
        /// <param name="toIdioma">Transfer Object de entrada referente à tabela IDIOMA</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro</returns>
        public virtual Retorno<TOIdioma> Obter(TOIdioma toIdioma) // US5 - Consulta de idioma por código
        {
            try
            {
                ResetarControlesComando();

                Sql.Comando.Append($"SELECT * FROM {QualificarTabela(TOIdioma.TABELA)}");

                Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_IDIOMA), toIdioma.CodIdioma);
                //Sql.MontarCampoWhere(QualificarCampo(TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA), toIdioma.CodigoIsoCombinado);


                var registro = ObterDados();

                if (registro == null)
                    return Infra.RetornarFalha<TOIdioma>(new RegistroInexistenteMensagem());

                var toIdiomaConsultado = GerarTOIdioma(registro);

                return Infra.RetornarSucesso(toIdiomaConsultado);
            }
            catch (Exception exception)
            {
                return Infra.TratarExcecao<TOIdioma>(exception);
            }
        }

        /// <summary>
        /// Método responsável por definir a relação entre o campo do TO e o parâmetro SQL correspondente
        /// </summary>
        /// <param name="nomeCampo">Nome do campo a ser parametrizado</param>
        /// <param name="conteudo">Conteúdo na propriedade do TO respectiva ao campo a ser parametrizado</param>
        /// <returns>Parâmetro recém-criado</returns>
        protected override Parametro CriarParametro(string nomeCampo, object conteudo)
        {
            var parametro = new Parametro();

            switch (nomeCampo)
            {
                case TOIdioma.CODIGO_IDIOMA:
                    {
                        parametro.DbType = DbType.Int32;
                        parametro.Size = 8;
                        parametro.Precision = 8;

                        break;
                    }
                case TOIdioma.CODIGO_ISO_COMBINADO_IDIOMA:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 6;
                        parametro.Precision = 6;

                        break;
                    }
                case TOIdioma.DESCRICAO_IDIOMA:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 50;
                        parametro.Precision = 50;

                        break;
                    }
                case TOIdioma.CODIGO_USUARIO:
                    {
                        parametro.DbType = DbType.String;
                        parametro.Size = 6;
                        parametro.Precision = 6;

                        break;
                    }
                case TOIdioma.DATA_HORA_ULTIMA_ALTERACAO:
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
        /// <returns>Lista completa de TOs de idioma</returns>
        private List<TOIdioma> ListarTodosTOsIdioma()
        {
            var listaCompletaTOs = new List<TOIdioma>();

            using (var listaConectada = ListarDados())
            {
                while (listaConectada.Ler())
                {
                    var toRetorno = GerarTOIdioma(listaConectada.LinhaAtual);

                    listaCompletaTOs.Add(toRetorno);
                }
            }

            return listaCompletaTOs;
        }

        /// <summary>
        /// Auxiliar que faz a listagem de TOs com base no objeto de paginação
        /// </summary>
        /// <param name="toPaginacao">Objeto com as configurações da página a ser listada</param>
        /// <returns>Lista de TOs de idioma com base na página selecionada</returns>
        private List<TOIdioma> ListarPaginaTOsIdioma(TOPaginacao toPaginacao)
        {
            var paginaTOs = new List<TOIdioma>();

            var listaDesconectada = ListarDados(toPaginacao);

            foreach (var linhaAtual in listaDesconectada.Linhas)
            {
                var toRetorno = GerarTOIdioma(linhaAtual);

                paginaTOs.Add(toRetorno);
            }

            return paginaTOs;
        }

        /// <summary>
        /// Auxiliar que efetua a criação de um novo TO de idiomas com base em um registro oriundo da base de dados
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Instância de TOIdioma</returns>
        private TOIdioma GerarTOIdioma(Linha registro)
        {
            var toIdioma = new TOIdioma();

            toIdioma.PopularRetorno(registro);

            return toIdioma;
        }
    }
}
