using Bergs.Pxc.Pxcbtoxn;
using Bergs.Pwx.Pwxodaxn;
using Bergs.Pwx.Pwxodaxn.Excecoes;
using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.BD;
using Bergs.Pwx.Pwxoiexn.Mensagens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace Bergs.Pxc.Pxcqclxn
{
    /// <summary>Classe que possui os métodos de manipulação de dados da tabela CLIENTE_PXC da base de dados PXC.</summary>
    public class ClientePxc : AplicacaoDados
    {
        private static readonly string Alias = "CLI";

        #region Métodos  
        /// <summary>Método alterar referente à tabela CLIENTE_PXC07.</summary>
        /// <param name="toClientePxc07">Transfer Object de entrada referente à tabela CLIENTE_PXC07.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<int> Alterar(TOClientePxc toClientePxc07)
        {
            try
            {
                int registrosAfetados;

                //Limpa as propriedades utilizadas para a montagem do comando
                this.Sql.Comando.Length = 0;
                this.Parametros.Clear();
                toClientePxc07.CodOperador = Infra.Usuario.Matricula;

                //Inicia montagem do comando
                this.Sql.Comando.Append("UPDATE PXC.CLIENTE_PXC");
                //Monta campos que serão modificados
                this.MontarSet(toClientePxc07);
                //Filtra a alteração pelas chaves da tabela
                this.MontarWhereChaves(toClientePxc07, String.Empty);
                //Filtra a alteração pelo campo de controle de acessos concorrentes
                this.Sql.MontarCampoWhere("ULT_ATUALIZACAO", toClientePxc07.UltAtualizacao);

                //Executa o comando
                registrosAfetados = this.AlterarDados();
                if (registrosAfetados == 0)
                {
                    return this.Infra.RetornarFalha<int>(new ConcorrenciaMensagem());
                }

                return this.Infra.RetornarSucesso(registrosAfetados);
            }
            catch (ChaveEstrangeiraInexistenteException ex)
            {
                return this.Infra.RetornarFalha<int>(new ChaveEstrangeiraInexistenteMensagem(ex));
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<int>(ex);
            }
        }

        /// <summary>Método contar referente à tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<long> Contar(TOClientePxc toClientePxc)
        {
            try
            {
                //Limpa as propriedades utilizadas para a montagem do comando
                ResetarControlesComando();

                //Inicia montagem do comando
                this.Sql.Comando.Append($"SELECT COUNT(*) FROM {QualificarTabela(TOClientePxc.TABELA)}");
                //Filtra consulta pelos dados informados no TO
                Sql.MontarCampoWhere(QualificarCampo(TOClientePxc.NOME_CLIENTE), toClientePxc.NomeCliente);

                //Executa o comando
                var quantidadeRegistros = this.ContarDados();

                return this.Infra.RetornarSucesso(quantidadeRegistros);
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<long>(ex);
            }
        }
           
        /// <summary>Método incluir referente à tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<int> Incluir(TOClientePxc toClientePxc)
        {
            try
            { 
                
                //Limpa as propriedades utilizadas para a montagem do comando
                ResetarControlesComando();

                //Inicia montagem do comando
                this.Sql.Comando.Append($"INSERT INTO {TOClientePxc.TABELA} (");

                //Monta campos que serão inseridos
                //this.MontarInsert(toClientePxc);
                Sql.MontarCampoInsert(TOClientePxc.CODIGO_CLIENTE, toClientePxc.CodCliente);
                Sql.MontarCampoInsert(TOClientePxc.NOME_CLIENTE, toClientePxc.NomeCliente);
                Sql.MontarCampoInsert(TOClientePxc.TIPO_PESSOA, toClientePxc.TipoPessoa);
                Sql.MontarCampoInsert(TOClientePxc.AGENCIA_CLIENTE, toClientePxc.Agencia);
                Sql.MontarCampoInsert(TOClientePxc.VALOR_CAPITAL_SOCIAL, toClientePxc.VlrCapitalSocial);
                //Sql.MontarCampoInsert(TOClientePxc.DATA_ABERTURA_CADASTRO, toClientePxc.DtAbeCad);

                Sql.MontarCampoInsert(TOClientePxc.DATA_ABERTURA_CADASTRO);
                Sql.Temporario.Append("CURRENT_DATE");

                Sql.MontarCampoInsert(TOClientePxc.CODIGO_OPERADOR, toClientePxc.CodOperador); // SAC
                Sql.MontarCampoInsert(TOClientePxc.DATA_HORA_ULTIMA_ALTERACAO); // SAC - Inicialização do token
                Sql.Temporario.Append("CURRENT_TIMESTAMP");

                //Une os buffers de montagem do comando
                this.Sql.Comando.Append(") VALUES (");                
                this.Sql.Comando.Append(this.Sql.Temporario);
                
                this.Sql.Comando.Append(")");

                //Executa o comando
                var registrosAfetados = this.IncluirDados();

                return this.Infra.RetornarSucesso(registrosAfetados);
            }
			catch (RegistroDuplicadoException ex)
            {
                return this.Infra.RetornarFalha<int>(new RegistroDuplicadoMensagem(ex));
            }
			catch (ChaveEstrangeiraInexistenteException ex)
            {
                return this.Infra.RetornarFalha<int>(new ChaveEstrangeiraInexistenteMensagem(ex));
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<int>(ex);
            }
        }
    
        /// <summary>Método listar referente à tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <param name="toPaginacao">Classe da infra-estrutura contendo as informações de paginação.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<List<TOClientePxc>> Listar(TOClientePxc toClientePxc, TOPaginacao toPaginacao)
        {
            try
            {
                List<TOClientePxc> dados;
                TOClientePxc toRetorno;
                
                //Limpa as propriedades utilizadas para a montagem do comando
                ResetarControlesComando();

                //Inicia montagem do comando
                this.Sql.Comando.Append("SELECT ");
                this.Sql.Comando.Append("CLI.COD_CLIENTE, ");
                this.Sql.Comando.Append("CLI.NOME_CLIENTE, ");
                this.Sql.Comando.Append("CLI.TIPO_PESSOA, ");
                this.Sql.Comando.Append("CLI.AGENCIA, ");
                this.Sql.Comando.Append("CLI.VLR_CAPITAL_SOCIAL, ");
                this.Sql.Comando.Append("CLI.COD_OPERADOR, ");
                this.Sql.Comando.Append("CLI.ULT_ATUALIZACAO ");
                this.Sql.Comando.Append($"FROM {QualificarTabela(TOClientePxc.TABELA)}");

                //Filtra consulta pelos dados informados no TO
                Sql.MontarCampoWhere(QualificarCampo(TOClientePxc.NOME_CLIENTE), toClientePxc.NomeCliente);

                dados = new List<TOClientePxc>();

                if (toPaginacao == null)
                {
                    //Executa o comando sem utilizar paginação
                    using (ListaConectada listaConectada = this.ListarDados())
                    {
                        //Cria TO para cada tupla retornada
                        while (listaConectada.Ler())
                        {
                            toRetorno = new TOClientePxc();
                            toRetorno.PopularRetorno(listaConectada.LinhaAtual);
                            dados.Add(toRetorno);
                        }
                    }
                }
                else
                {
                    //Executa o comando utilizando paginação
                    ListaDesconectada listaDesconectada = this.ListarDados(toPaginacao);

                    //Cria TO para cada tupla retornada
                    foreach (Linha linha in listaDesconectada.Linhas)
                    {
                        toRetorno = new TOClientePxc();
                        toRetorno.PopularRetorno(linha);
                        dados.Add(toRetorno);
                    }
                }

                return this.Infra.RetornarSucesso(dados);
            }    
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<List<TOClientePxc>>(ex);
            }
        }

        /// <summary>Método excluir referente à tabela CLIENTE_PXC07.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC07.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<int> Excluir(TOClientePxc toClientePxc)
        {
            try
            {
                int registrosAfetados;

                //Limpa as propriedades utilizadas para a montagem do comando
                this.Sql.Comando.Length = 0;
                this.Parametros.Clear();
                toClientePxc.CodOperador = Infra.Usuario.Matricula;

                //Inicia montagem do comando
                this.Sql.Comando.Append("DELETE FROM PXC.CLIENTE_PXC");
                //Filtra a exclusão pelas chaves da tabela
                this.MontarWhereChaves(toClientePxc, String.Empty);
                //Filtra a exclusão pelo campo de controle de acessos concorrentes
                this.Sql.MontarCampoWhere("ULT_ATUALIZACAO", toClientePxc.UltAtualizacao);

                //Executa o comando
                registrosAfetados = this.ExcluirDados();
                if (registrosAfetados == 0)
                {
                    return this.Infra.RetornarFalha<int>(new ConcorrenciaMensagem());
                }

                return this.Infra.RetornarSucesso(registrosAfetados);
            }
            catch (ChaveEstrangeiraReferenciadaException ex)
            {
                return this.Infra.RetornarFalha<int>(new ChaveEstrangeiraReferenciadaMensagem(ex));
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<int>(ex);
            }
        }

        /// <summary>Método obter referente à tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<TOClientePxc> Obter(TOClientePxc toClientePxc)
        {
            try
            {
                Linha linha;
                TOClientePxc dados;
                
                //Limpa as propriedades utilizadas para a montagem do comando
                ResetarControlesComando();

                //Inicia montagem do comando
                this.Sql.Comando.Append("SELECT ");
                this.Sql.Comando.Append("CLI.COD_CLIENTE, ");
                this.Sql.Comando.Append("CLI.TIPO_PESSOA, ");
                this.Sql.Comando.Append("CLI.COD_OPERADOR, ");
                this.Sql.Comando.Append("CLI.ULT_ATUALIZACAO ");
                this.Sql.Comando.Append($"FROM {QualificarTabela(TOClientePxc.TABELA)}");
                //Filtra consulta pelos dados informados no TO

                Sql.MontarCampoWhere(QualificarCampo(TOClientePxc.CODIGO_CLIENTE), toClientePxc.CodCliente);
                Sql.MontarCampoWhere(QualificarCampo(TOClientePxc.TIPO_PESSOA), toClientePxc.TipoPessoa);

                //Executa o comando
                linha = this.ObterDados();
                if (linha == null)
                {
                    return this.Infra.RetornarFalha<TOClientePxc>(new RegistroInexistenteMensagem());
                }
                
                //Cria TO para a tupla retornada
                dados = new TOClientePxc();
                dados.PopularRetorno(linha);

                return this.Infra.RetornarSucesso(dados);
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<TOClientePxc>(ex);
            }
        }
    
        /// <summary>Monta campos para cláusula WHERE.</summary>
        /// <param name="toClientePxc">TO contendo os campos.</param>
        /// <param name="alias">Alias da tabela ClientePxc.</param>
        private void MontarWhere(TOClientePxc toClientePxc, String alias)
        {
            //Monta no WHERE todos os campos da tabela que foram informados
            
            this.MontarWhereChaves(toClientePxc, alias);
            this.MontarCampos(this.Sql.MontarCampoWhere, toClientePxc, alias);
            
			this.Sql.MontarCampoWhere(alias + "ULT_ATUALIZACAO", toClientePxc.UltAtualizacao);
        }
        
        /// <summary>Monta campos chave para cláusula WHERE.</summary>
        /// <param name="toClientePxc">TO contendo os campos.</param>
        /// <param name="alias">Alias da tabela ClientePxc.</param>
        private void MontarWhereChaves(TOClientePxc toClientePxc, String alias)
        {
            //Monta no WHERE todos os campos chave da tabela
            
            this.MontarCamposChave(this.Sql.MontarCampoWhere, toClientePxc, alias);
        }
        
        /// <summary>Monta campos para cláusula SET.</summary>
        /// <param name="toClientePxc">TO contendo os campos.</param>
        private void MontarSet(TOClientePxc toClientePxc)
        {
            //Monta no SET todos os campos não chave da tabela que foram informados
            
            this.MontarCampos(this.Sql.MontarCampoSet, toClientePxc, String.Empty);
            this.Sql.MontarCampoSet("ULT_ATUALIZACAO");
            this.Sql.Comando.Append("CURRENT_TIMESTAMP");
        }
        
        /// <summary>Monta campos para cláusula INSERT.</summary>
        /// <param name="toClientePxc">TO contendo os campos.</param>
        private void MontarInsert(TOClientePxc toClientePxc)
        {
            //Monta no INSERT todos os campos da tabela que foram informados
            
            this.MontarCamposChave(this.Sql.MontarCampoInsert, toClientePxc, String.Empty);
            this.MontarCampos(this.Sql.MontarCampoInsert, toClientePxc, String.Empty);
            this.Sql.MontarCampoInsert("ULT_ATUALIZACAO");
            this.Sql.Temporario.Append("CURRENT_TIMESTAMP");
        }
        
        /// <summary>Executa uma ação nos campos chave de um TO.</summary>
        /// <param name="montagem">Ação a ser executada.</param>
        /// <param name="toClientePxc">TO alvo das ações.</param>
        /// <param name="alias">Alias da tabela ClientePxc.</param>
        private void MontarCamposChave(ConstrutorSql.MontarCampo montagem, TOClientePxc toClientePxc, String alias)
        {   
            //Invoca qualquer comando simples de montagem nos campos chave da tabela
            
            montagem.Invoke(alias + "COD_CLIENTE", toClientePxc.CodCliente);
            montagem.Invoke(alias + "TIPO_PESSOA", toClientePxc.TipoPessoa);
        }
        
        /// <summary>Executa uma ação nos campos não chave de um TO.</summary>
        /// <param name="montagem">Ação a ser executada.</param>
        /// <param name="toClientePxc">TO alvo das ações.</param>
        /// <param name="alias">Alias da tabela ClientePxc.</param>
        private void MontarCampos(ConstrutorSql.MontarCampo montagem, TOClientePxc toClientePxc, String alias)
        {   
            //Invoca qualquer comando simples de montagem nos campos não chave da tabela, exceto no que faz controle de acessos concorrentes
            
            montagem.Invoke(alias + "AGENCIA", toClientePxc.Agencia);
            montagem.Invoke(alias + "COD_OPERADOR", toClientePxc.CodOperador);
            montagem.Invoke(alias + "DT_ABE_CAD", toClientePxc.DtAbeCad);
            montagem.Invoke(alias + "DT_CONSTITUICAO", toClientePxc.DtConstituicao);
            montagem.Invoke(alias + "IND_FUNC_BANRISUL", toClientePxc.IndFuncBanrisul);
            montagem.Invoke(alias + "NOME_CLIENTE", toClientePxc.NomeCliente);
            montagem.Invoke(alias + "NOME_FANTASIA", toClientePxc.NomeFantasia);
            montagem.Invoke(alias + "NOME_MAE", toClientePxc.NomeMae);
            montagem.Invoke(alias + "ULT_NOSSO_NRO", toClientePxc.UltNossoNro);
            montagem.Invoke(alias + "VLR_CAPITAL_SOCIAL", toClientePxc.VlrCapitalSocial);
        }

        /// <summary>Cria um parâmetro para a instrução SQL.</summary>
        /// <param name="nomeCampo">Nome do campo da tabela.</param>
        /// <param name="conteudo">Valor para o parâmetro.</param>
        /// <returns>Parâmetro recém-criado.</returns>
        protected override Parametro CriarParametro(String nomeCampo, Object conteudo)
        {
            Parametro parametro = new Parametro();
            switch (nomeCampo)
            {   
                #region Chaves Primárias
                case "COD_CLIENTE":
                    parametro.Precision = 14;
                    parametro.Size = 14;
                    parametro.DbType = DbType.String;
                    break;
                case "TIPO_PESSOA":
                    parametro.Precision = 1;
                    parametro.Size = 1;
                    parametro.DbType = DbType.String;
                    break;                        
                #endregion

                #region Campos Obrigatórios
                case "AGENCIA":
                    parametro.Precision = 2;
                    parametro.Size = 2;
                    parametro.DbType = DbType.Int16;
                    break;
                case "COD_OPERADOR":
                    parametro.Precision = 6;
                    parametro.Size = 6;
                    parametro.DbType = DbType.String;
                    break;
                case "DT_ABE_CAD":
                    parametro.Precision = 4;
                    parametro.Size = 4;
                    parametro.DbType = DbType.Date;
                    break;
                case "NOME_CLIENTE":
                    parametro.Precision = 50;
                    parametro.Size = 50;
                    parametro.DbType = DbType.String;
                    break;
                case "ULT_ATUALIZACAO":
                    parametro.Precision = 10;
                    parametro.Scale = 6;
                    parametro.Size = 10;
                    parametro.DbType = DbType.DateTime;
                    break;
                #endregion

                #region Campos Opcionais
                case "DT_CONSTITUICAO":
                    parametro.Precision = 4;
                    parametro.Size = 4;
                    parametro.DbType = DbType.Date;
                    break;
                case "IND_FUNC_BANRISUL":
                    parametro.Precision = 1;
                    parametro.Size = 1;
                    parametro.DbType = DbType.String;
                    break;
                case "NOME_FANTASIA":
                    parametro.Precision = 30;
                    parametro.Size = 30;
                    parametro.DbType = DbType.String;
                    break;
                case "NOME_MAE":
                    parametro.Precision = 30;
                    parametro.Size = 30;
                    parametro.DbType = DbType.String;
                    break;
                case "ULT_NOSSO_NRO":
                    parametro.Precision = 4;
                    parametro.Size = 4;
                    parametro.DbType = DbType.Int32;
                    break;
                case "VLR_CAPITAL_SOCIAL":
                    parametro.Precision = 15;
                    parametro.Scale = 2;
                    parametro.Size = 15;
                    parametro.DbType = DbType.Decimal;
                    break;

#if DEBUG
                default:
                    //Força um erro em modo debug para alertar o programador caso tenha caido no default
                    //Todo parâmetro deve cair em um case neste switch
                    parametro = null;
                    break;
#endif
                #endregion                
            }
            parametro.Direction = ParameterDirection.Input;
            parametro.SourceColumn = nomeCampo;
            
            if (parametro.Scale > 0 && conteudo != null &&  parametro.DbType != DbType.DateTime)
            {
                parametro.Value = String.Format(CultureInfo.InvariantCulture, "{0:F" + parametro.Scale + "}", conteudo);
            }
            else
            {
                parametro.Value = conteudo;
            }
            
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
        #endregion
    }
}