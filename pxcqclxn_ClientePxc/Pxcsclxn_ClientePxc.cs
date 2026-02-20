using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.Mensagens;
using Bergs.Pwx.Pwxoiexn.Relatorios;
using Bergs.Pwx.Pwxoiexn.RN;
using Bergs.Pxc.Pxcbtoxn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bergs.Pxc.Pxcsclxn
{
    /// <summary>Classe que possui as regras de negócio para o acesso da tabela CLIENTE_PXC da base de dados PXC.</summary>
    public class ClientePxc : AplicacaoRegraNegocio
    {
        HashSet<short> agenciasValidas = new HashSet<short> { 515, 590, 4022, 9008 };

        #region Métodos
        /// <summary>Altera registro da tabela CLIENTE_PXC07.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC07.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<int> Alterar(TOClientePxc toClientePxc)
        {
            try
            {
                Pxcqclxn.ClientePxc bdClientePxc;
                Retorno<int> alteracaoClientePxc;

                #region Validação de campos
                //Valida que os campos que fazem parte da chave primária foram informados
                if (!toClientePxc.CodCliente.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new CampoObrigatorioMensagem("COD_CLIENTE"));
                }
                if (!toClientePxc.TipoPessoa.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new CampoObrigatorioMensagem("TIPO_PESSOA"));
                }
                #endregion

                #region Validação de regras de negócio
                #endregion

                bdClientePxc = this.Infra.InstanciarBD<Pxcqclxn.ClientePxc>();

                //Cria escopo transacional para garantir atomicidade
                using (EscopoTransacional escopo = this.Infra.CriarEscopoTransacional())
                {
                    alteracaoClientePxc = bdClientePxc.Alterar(toClientePxc);
                    if (!alteracaoClientePxc.OK)
                    {
                        return alteracaoClientePxc;
                    }

                    escopo.EfetivarTransacao();
                    return this.Infra.RetornarSucesso(alteracaoClientePxc.Dados, new OperacaoRealizadaMensagem("Alteração"));
                }
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<int>(ex);
            }
        }
        /// <summary>Conta quantidade de registros da tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<long> Contar(TOClientePxc toClientePxc)
        {
            try
            {
                Pxcqclxn.ClientePxc bdClientePxc;
                Retorno<long> contagemClientePxc;

                bdClientePxc = this.Infra.InstanciarBD<Pxcqclxn.ClientePxc>();
                contagemClientePxc = bdClientePxc.Contar(toClientePxc);

                if (!contagemClientePxc.OK)
                {
                    return contagemClientePxc;
                }

                return this.Infra.RetornarSucesso(contagemClientePxc.Dados, new OperacaoRealizadaMensagem());
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<long>(ex);
            }
        }
        
        /// <summary>Inclui registro na tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<int> Incluir(TOClientePxc toClientePxc)
        {
            try
            {
                Pxcqclxn.ClientePxc bdClientePxc;
                Retorno<int> inclusaoClientePxc;

                //Valida que os campos obrigatórios foram informados
                if (!toClientePxc.CodCliente.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirCodClienteNaoInformado));
                }
                if (!toClientePxc.NomeCliente.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaNomeInvalido));
                }
                if (!toClientePxc.TipoPessoa.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirTipoPessoaInvalido));
                }
                if (!toClientePxc.Agencia.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirAgenciaNaoInformada));
                }
                // Valida que o campo nome deve conter apenas letras, números e acentuação
                if (!ValidarTO(toClientePxc, out var listaRetornoValidacao))
                    return this.Infra.RetornarFalha<int>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                //Valida que o campo tipo pessoa aceita apenas "F" ou "J"
                if (!(toClientePxc.TipoPessoa == "F") && !(toClientePxc.TipoPessoa == "J"))
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirTipoPessoaInvalido));

                var resultadoConsulta = Obter(toClientePxc);

                //Valida que combinação de código do cliente e tipo de pessoa é único
                if (resultadoConsulta.OK && (resultadoConsulta.Mensagem is RegistroDuplicadoMensagem))
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirCodigoClienteEtipoPessoaJaExistente));

                // Valida que aceita apenas as agências específicas
                if (!(toClientePxc.Agencia == 515) && !(toClientePxc.Agencia == 590) && !(toClientePxc.Agencia == 4022) && !(toClientePxc.Agencia == 9008))
                {
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirAgenciaNaoInformada));
                }

                toClientePxc.VlrCapitalSocial = 0;
                toClientePxc.CodOperador = Infra.Usuario.Matricula;
                
                bdClientePxc = this.Infra.InstanciarBD<Pxcqclxn.ClientePxc>();

                //Cria escopo transacional para garantir atomicidade
                using (EscopoTransacional escopo = this.Infra.CriarEscopoTransacional())
                {
                    inclusaoClientePxc = bdClientePxc.Incluir(toClientePxc);
                    if (!inclusaoClientePxc.OK && (inclusaoClientePxc.Mensagem is RegistroDuplicadoMensagem))
                    {
                        return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.FalhaRnIncluirCategoriaJaExistente));
                    }

                    escopo.EfetivarTransacao();
                    return this.Infra.RetornarSucesso(inclusaoClientePxc.Dados, new OperacaoRealizadaMensagem("Inclusão"));
                }
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<int>(ex);
            }
        }

        /// <summary>Lista registros da tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <param name="toPaginacao">Classe da infra-estrutura contendo as informações de paginação.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<List<TOClientePxc>> Listar(TOClientePxc toClientePxc, TOPaginacao toPaginacao)
        {
            try
            {
                Pxcqclxn.ClientePxc bdClientePxc;
                Retorno<List<TOClientePxc>> listagemClientePxc;

                if (!toClientePxc.NomeCliente.FoiSetado)
                    return this.Infra.RetornarFalha<List<TOClientePxc>>(new Mensagem(TipoMensagem.FalhaNomeInvalido));

                if (!ValidarTO(toClientePxc, out var listaRetornoValidacao))
                    return Infra.RetornarFalha<List<TOClientePxc>>(new ObjetoInvalidoMensagem(listaRetornoValidacao));

                bdClientePxc = this.Infra.InstanciarBD<Pxcqclxn.ClientePxc>();

                listagemClientePxc = bdClientePxc.Listar(toClientePxc, toPaginacao);

                if (!listagemClientePxc.OK)
                {
                    return listagemClientePxc;
                }

                if(listagemClientePxc.Dados.Count == 0)
                    return this.Infra.RetornarFalha<List<TOClientePxc>>(new Mensagem(TipoMensagem.FalhaRnIncluirClienteNaoInformado));

                return this.Infra.RetornarSucesso(listagemClientePxc.Dados, new OperacaoRealizadaMensagem());
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<List<TOClientePxc>>(ex);
            }
        }

        /// <summary>Obtém registro da tabela CLIENTE_PXC.</summary>
        /// <param name="toClientePxc">Transfer Object de entrada referente à tabela CLIENTE_PXC.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<TOClientePxc> Obter(TOClientePxc toClientePxc)
        {
            try
            {
                Pxcqclxn.ClientePxc bdClientePxc;
                Retorno<TOClientePxc> obtencaoClientePxc;

                //Valida que os campos que fazem parte da chave primária foram informados
                if (!toClientePxc.CodCliente.FoiSetado)
                    return this.Infra.RetornarFalha<TOClientePxc>(new Mensagem(TipoMensagem.FalhaRnIncluirCodClienteNaoInformado));

                if (!toClientePxc.TipoPessoa.FoiSetado)
                    return this.Infra.RetornarFalha<TOClientePxc>(new Mensagem(TipoMensagem.FalhaRnIncluirTipoPessoaInvalido));

                bdClientePxc = this.Infra.InstanciarBD<Pxcqclxn.ClientePxc>();

                obtencaoClientePxc = bdClientePxc.Obter(toClientePxc);

                // Validar tentativa de obtencao de categoria inexistente
                if (!obtencaoClientePxc.OK && (obtencaoClientePxc.Mensagem is RegistroInexistenteMensagem))
                    return this.Infra.RetornarFalha<TOClientePxc>(new Mensagem(TipoMensagem.FalhaNomeInvalido));

                if (!obtencaoClientePxc.OK)
                    return obtencaoClientePxc;

                return this.Infra.RetornarSucesso(obtencaoClientePxc.Dados, new OperacaoRealizadaMensagem());
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<TOClientePxc>(ex);
            }
        }

        /// <summary>Exclui registro da tabela CLIENTE_PXC07.</summary>
        /// <param name="toClientePxc07">Transfer Object de entrada referente à tabela CLIENTE_PXC07.</param>
        /// <returns>Classe de retorno contendo as informações de resposta ou as informações de erro.</returns>
        public virtual Retorno<int> Excluir(TOClientePxc toClientePxc07)
        {
            try
            {
                Pxcqclxn.ClientePxc bdClientePxc;
                Retorno<int> exclusaoClientePxc;

                #region Validação de campos
                //Valida que os campos que fazem parte da chave primária foram informados
                if (!toClientePxc07.CodCliente.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new CampoObrigatorioMensagem("COD_CLIENTE"));
                }
                if (!toClientePxc07.TipoPessoa.FoiSetado)
                {
                    return this.Infra.RetornarFalha<int>(new CampoObrigatorioMensagem("TIPO_PESSOA"));
                }
                #endregion

                #region Validação de regras de negócio
                #endregion

                bdClientePxc = this.Infra.InstanciarBD<Pxcqclxn.ClientePxc>();

                //Cria escopo transacional para garantir atomicidade
                using (EscopoTransacional escopo = this.Infra.CriarEscopoTransacional())
                {
                    exclusaoClientePxc = bdClientePxc.Excluir(toClientePxc07);
                    if (!exclusaoClientePxc.OK)
                    {
                        return exclusaoClientePxc;
                    }

                    escopo.EfetivarTransacao();
                    return this.Infra.RetornarSucesso(exclusaoClientePxc.Dados, new OperacaoRealizadaMensagem("Exclusão"));
                }
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<int>(ex);
            }
        }
        /// <summary>Gera relatório da tabela CLIENTE_PXC07.</summary>
        /// <param name="toClientePxc07">Transfer Object de entrada referente à tabela CLIENTE_PXC07.</param>
        /// <returns>Classe de retorno contendo as informações de resposta e o nome do relatório gerado, ou as informações de erro.</returns>
        public virtual Retorno<string> Imprimir(TOClientePxc toClientePxc07)
        {
            try
            {
                Retorno<List<TOClientePxc>> listagemClientePxc07;
                StringBuilder linha;
                
                //Lista registros da tabela
                listagemClientePxc07 = this.Listar(toClientePxc07, null);
                if (!listagemClientePxc07.OK)
                {
                    return this.Infra.RetornarFalha<List<TOClientePxc>, String>(listagemClientePxc07);
                }
                
                //Monta relatório com os dados da listagem
                using (RelatorioPadrao relatorio = new RelatorioPadrao(this.Infra))
                {   
                    //Define colunas do relatório
                    relatorio.Colunas.Add(new Coluna("COD_CLIENTE", 14));
                    relatorio.Colunas.Add(new Coluna("TIPO_PESSOA", 1));
                    relatorio.Colunas.Add(new Coluna("NOME_CLIENTE", 50));
                    relatorio.Colunas.Add(new Coluna("AGENCIA", 2));
                    relatorio.Colunas.Add(new Coluna("DT_ABE_CAD", 26));
                    relatorio.Colunas.Add(new Coluna("NOME_FANTASIA", 30));
                    relatorio.Colunas.Add(new Coluna("VLR_CAPITAL_SOCIAL", 17));
                    relatorio.Colunas.Add(new Coluna("DT_CONSTITUICAO", 26));
                    relatorio.Colunas.Add(new Coluna("NOME_MAE", 30));
                    relatorio.Colunas.Add(new Coluna("COD_OPERADOR", 6));
                    relatorio.Colunas.Add(new Coluna("ULT_ATUALIZACAO", 26));
                    relatorio.Colunas.Add(new Coluna("IND_FUNC_BANRISUL", 1));
                    relatorio.Colunas.Add(new Coluna("ULT_NOSSO_NRO", 4));

                    linha = new StringBuilder();
                    //Monta linhas do relatório
                    foreach(TOClientePxc toSaida in listagemClientePxc07.Dados)
                    {
                        linha.Append(toSaida.CodCliente.ToString().PadRight(relatorio.Colunas["COD_CLIENTE"].Tamanho));
                        linha.Append(toSaida.TipoPessoa.ToString().PadRight(relatorio.Colunas["TIPO_PESSOA"].Tamanho));
                        linha.Append(toSaida.NomeCliente.ToString().PadRight(relatorio.Colunas["NOME_CLIENTE"].Tamanho));
                        linha.Append(toSaida.Agencia.ToString().PadRight(relatorio.Colunas["AGENCIA"].Tamanho));
                        linha.Append(toSaida.DtAbeCad.ToString().PadRight(relatorio.Colunas["DT_ABE_CAD"].Tamanho));
                        linha.Append(toSaida.NomeFantasia.ToString().PadRight(relatorio.Colunas["NOME_FANTASIA"].Tamanho));
                        linha.Append(toSaida.VlrCapitalSocial.ToString().PadRight(relatorio.Colunas["VLR_CAPITAL_SOCIAL"].Tamanho));
                        linha.Append(toSaida.DtConstituicao.ToString().PadRight(relatorio.Colunas["DT_CONSTITUICAO"].Tamanho));
                        linha.Append(toSaida.NomeMae.ToString().PadRight(relatorio.Colunas["NOME_MAE"].Tamanho));
                        linha.Append(toSaida.CodOperador.ToString().PadRight(relatorio.Colunas["COD_OPERADOR"].Tamanho));
                        linha.Append(toSaida.UltAtualizacao.ToString().PadRight(relatorio.Colunas["ULT_ATUALIZACAO"].Tamanho));
                        linha.Append(toSaida.IndFuncBanrisul.ToString().PadRight(relatorio.Colunas["IND_FUNC_BANRISUL"].Tamanho));
                        linha.Append(toSaida.UltNossoNro.ToString().PadRight(relatorio.Colunas["ULT_NOSSO_NRO"].Tamanho));

                        relatorio.AdicionarLinha(linha.ToString());
                        linha.Length = 0;
                    }
                    
                    return this.Infra.RetornarSucesso(relatorio.NomeArquivoVirtual, new OperacaoRealizadaMensagem());
                }                
            }
            catch (Exception ex)
            {
                return this.Infra.TratarExcecao<String>(ex);
            }
        }
        #endregion
    }
}