namespace Bergs.Pxc.Pxcscnxn
{
    /// <summary>
    /// Mensagens previstas para o componente
    /// </summary>
    public enum TipoMensagem
    {
        /// <summary>Falha por agência inválida</summary>
        AgenciaInvalida,
        /// <summary>Falha por tipo pessoa inválido</summary>
        TipoPessoaInvalido,
        /// <summary>Falha por conta inválida</summary>
        ContaInvalida,
        /// <summary>Falha por saldo inválido</summary>
        SaldoInvalido,
        /// <summary>Falha por cliente inválido</summary>
        ClienteInvalido,
        /// <summary>Falha por combinação de conta e agência já existir na base</summary>
        ContaAgenciaJaExiste,
        /// <summary>Falha por não existirem contas cadastradas na base</summary>
        NaoExistemContasBancariasCadastradas,
        /// <summary>Falha por não existir conta cadastrada com as informações dadas na base</summary>
        NaoExisteContaBancariaCadastrada,
        /// <summary>Falha por situação inválida</summary>
        SituacaoInvalida
    }

    /// <summary>
    /// Classe de mensagens
    /// </summary>
    public class ContaMensagem: Pwx.Pwxoiexn.Mensagens.Mensagem
    {
        /// <summary>
        /// Mensagem
        /// </summary>
        private string mensagem;

        /// <summary>
        /// Tipo de mensagem
        /// </summary>
        private TipoMensagem tipoMensagem;

        /// <summary>
        /// Mensagem para o usuário
        /// </summary>
        public override string ParaUsuario
        {
            get { return ParaOperador; }
        }

        /// <summary>
        /// Mensagem para o operador
        /// </summary>
        public override string ParaOperador
        {
            get { return mensagem; }
        }

        /// <summary>
        /// Identificador
        /// </summary>
        public override string Identificador
        {
            get { return tipoMensagem.ToString(); }
        }

        /// <summary>
        /// Construtor da classe Mensagem
        /// </summary>
        /// <param name="tipoMensagem">Tipo de mensagem</param>
        /// <param name="argumentos">Argumentos</param>
        public ContaMensagem(TipoMensagem tipoMensagem, params string[] argumentos)
        {
            this.tipoMensagem = tipoMensagem;

            switch (tipoMensagem)
            {
                case TipoMensagem.AgenciaInvalida:
                    mensagem = "Um número válido de agência deve ser informado (entre 1000 e 9999).";
                    break;
                case TipoMensagem.TipoPessoaInvalido:
                    mensagem = "Um tipo de pessoa válido deve ser informado ('F' para pessoa física ou 'J' para pessoa jurídica).";
                    break;
                case TipoMensagem.ContaInvalida:
                    mensagem = "Um número válido de conta deve ser informado (entre 1 e 9999999999).";
                    break;
                case TipoMensagem.SaldoInvalido:
                    mensagem = "Um saldo válido deve ser informado (não negativo).";
                    break;
                case TipoMensagem.ClienteInvalido:
                    mensagem = "Um CPF/CNPJ válido deve ser informado.";
                    break;
                case TipoMensagem.ContaAgenciaJaExiste:
                    mensagem = "Já existe na base de dados uma conta bancária com a combinação de número de agência e número de conta informados.";
                    break;
                case TipoMensagem.NaoExistemContasBancariasCadastradas:
                    mensagem = "Não existem contas bancárias cadastradas.";
                    break;
                case TipoMensagem.NaoExisteContaBancariaCadastrada:
                    mensagem = "Não existe uma conta bancária cadastrada para os critérios informados.";
                    break;
                case TipoMensagem.SituacaoInvalida:
                    mensagem = "Uma situação válida deve ser informada ('A' para conta ativa, 'I' para inativa e 'S' para suspensa).";
                    break;
                default:
                    mensagem = "Mensagem não definida.";
                    break;
            }
        }
    }
}
