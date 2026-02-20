using Bergs.Pwx.Pwxodaxn;
using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.Validacoes;
using Bergs.Pxc.Pxcbtoxn.pgm;
using System;
using System.Data;
using System.Xml.Serialization;

namespace Bergs.Pxc.Pxcbtoxn
{
    /// <summary>
    /// Representa um registro da tabela CONTA da base de dados PXC
    /// </summary>
    public class TOConta: TOTabela
    {
        /*
         * Notas gerais ao avaliador:
         * 
         * - O uso das constantes com os nomes da tabela e dos campos de modo a evitar repetição de "strings mágicas" 
         * é apenas uma facilitação na avaliação modelo (que está exibindo uma implementação ideal). Sua não 
         * implementação nas avaliações dos alunos não deve acarretar em prejuízos de pontuação
         */

        /// <summary>
        /// Constante com o nome da tabela CONTA
        /// </summary>
        public const string TABELA = "PXC.CONTA";

        /// <summary>
        /// Constante com o nome do campo AGENCIA (número da agência) na tabela CONTA
        /// </summary>
        public const string NUMERO_AGENCIA = "AGENCIA";
        /// <summary>
        /// Constante com o nome do campo CONTA (número da conta) na tabela CONTA
        /// </summary>
        public const string NUMERO_CONTA = "CONTA";
        /// <summary>
        /// Constante com o nome do campo SALDO na tabela CONTA
        /// </summary>
        public const string SALDO_CONTA = "SALDO";
        /// <summary>
        /// Constante com o nome do campo COD_CLIENTE na tabela CONTA
        /// </summary>
        public const string CODIGO_CLIENTE = "COD_CLIENTE";
        /// <summary>
        /// Constante com o nome do campo IND_TP_PESSOA na tabela CONTA
        /// </summary>
        public const string TIPO_PESSOA_CLIENTE = "IND_TP_PESSOA";
        /// <summary>
        /// Constante com o nome do campo IND_SITUACAO na tabela CONTA
        /// </summary>
        public const string SITUACAO = "IND_SITUACAO";
        /// <summary>
        /// Constante com o nome do campo COD_OPERADOR na tabela CONTA
        /// </summary>
        public const string CODIGO_USUARIO = "COD_OPERADOR";
        /// <summary>
        /// Constante com o nome do campo ULT_ATUALIZACAO na tabela CONTA
        /// </summary>
        public const string DATA_HORA_ULTIMA_ALTERACAO = "ULT_ATUALIZACAO";

        /// <summary>
        /// Campo AGENCIA da tabela CONTA
        /// </summary>
        [XmlAttribute("agencia")]
        [CampoTabela(NUMERO_AGENCIA, Chave = true, Obrigatorio = true, TipoParametro = DbType.Decimal, Tamanho = 4, Precisao = 4)] // Valores de 1000 até 9999
        public CampoObrigatorio<decimal> Agencia { get; set; }

        /// <summary>
        /// Campo CONTA da tabela CONTA
        /// </summary>
        [XmlAttribute("conta")]
        [CampoTabela(NUMERO_CONTA, Chave = true, Obrigatorio = true, TipoParametro = DbType.Decimal, Tamanho = 10, Precisao = 10)] // Valores de 1 até 2000000000 (dois bilhões)
        public CampoObrigatorio<decimal> Conta { get; set; }

        /// <summary>
        /// Campo SALDO da tabela CONTA
        /// </summary>
        [XmlAttribute("saldo")]
        [CampoTabela(SALDO_CONTA, Obrigatorio = true, TipoParametro = DbType.Decimal, Tamanho = 15, Precisao = 15, Escala = 2)]
        public CampoObrigatorio<decimal> Saldo { get; set; }

        /// <summary>
        /// Campo COD_CLIENTE da tabela CONTA
        /// </summary>
        [XmlAttribute("cod_cliente")]
        [CampoTabela(CODIGO_CLIENTE, Obrigatorio = true, TipoParametro = DbType.String, Tamanho = 14, Precisao = 14)]
        [CPFCNPJ(mensagemErro: "Um CPF/CNPJ válido deve ser informado.")]
        public CampoObrigatorio<string> CodCliente { get; set; }

        /// <summary>
        /// Campo IND_TP_PESSOA da tabela CONTA
        /// </summary>
        [XmlAttribute("ind_tp_pessoa")]
        [CampoTabela(TIPO_PESSOA_CLIENTE, Obrigatorio = true, TipoParametro = DbType.String, Tamanho = 1, Precisao = 1)]
        public CampoObrigatorio<TipoPessoa> IndTpPessoa { get; set; }

        /// <summary>
        /// Campo IND_SITUACAO da tabela CONTA
        /// </summary>
        [XmlAttribute("ind_situacao")]
        [CampoTabela(SITUACAO, Obrigatorio = true, TipoParametro = DbType.String, Tamanho = 1, Precisao = 1)]
        public CampoObrigatorio<SituacaoConta> IndSituacao { get; set; }

        /// <summary>
        /// Campo COD_OPERADOR da tabela CONTA
        /// </summary>
        [XmlAttribute("cod_operador")]
        [CampoTabela(CODIGO_USUARIO, Obrigatorio = true, TipoParametro = DbType.String, Tamanho = 6, Precisao = 6)]
        public CampoObrigatorio<string> CodOperador { get; set; }

        /// <summary>
        /// Campo ULT_ATUALIZACAO da tabela CONTA
        /// </summary>
        [XmlAttribute("ult_atualizacao")]
        [CampoTabela(DATA_HORA_ULTIMA_ALTERACAO, Obrigatorio = true, UltAtualizacao = true, TipoParametro = DbType.DateTime, Tamanho = 10, Precisao = 10, Escala = 6)]
        public CampoObrigatorio<DateTime> UltAtualizacao { get; set; }

        /// <summary>
        /// Popula os atributos da classe a partir de uma linha de dados
        /// </summary>
        /// <param name="linha">Registro de dados retornado pelo acesso à base de dados</param>
        public override void PopularRetorno(Linha linha)
        {
            // Percorre os campos que foram retornados pela consulta e converte seus valores para tipos do .NET
            foreach (Campo campo in linha.Campos)
            {
                switch (campo.Nome)
                {
                    case NUMERO_AGENCIA:
                        {
                            Agencia = Convert.ToDecimal(campo.Conteudo);

                            break;
                        }
                    case NUMERO_CONTA:
                        {
                            Conta = Convert.ToDecimal(campo.Conteudo);

                            break;
                        }
                    case SALDO_CONTA:
                        {
                            Saldo = Convert.ToDecimal(campo.Conteudo);

                            break;
                        }
                    case CODIGO_CLIENTE:
                        {
                            CodCliente = Convert.ToString(campo.Conteudo).Trim();

                            break;
                        }
                    case TIPO_PESSOA_CLIENTE:
                        {
                            IndTpPessoa = (TipoPessoa)Convert.ToString(campo.Conteudo).Trim()[0];

                            break;
                        }
                    case SITUACAO:
                        {
                            IndSituacao = (SituacaoConta)Convert.ToString(campo.Conteudo).Trim()[0];

                            break;
                        }
                    case CODIGO_USUARIO:
                        {
                            CodOperador = Convert.ToString(campo.Conteudo).Trim();

                            break;
                        }
                    case DATA_HORA_ULTIMA_ALTERACAO:
                        {
                            UltAtualizacao = Convert.ToDateTime(campo.Conteudo);

                            break;
                        }
                    default:
                        {
                            // TODO: Tratar situação em que a coluna da tabela não tiver sido mapeada para uma propriedade do TO

                            break;
                        }
                }
            }
        }
    }
}
