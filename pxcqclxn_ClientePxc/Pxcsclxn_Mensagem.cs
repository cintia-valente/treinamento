using Bergs.Pxc.Pxcbtoxn;
using System;

namespace Bergs.Pxc.Pxcsclxn
{
    /// <summary>Mensagens previstas para o componente</summary>
    public enum TipoMensagem
    {
        /// <summary>Falha da regra de negócio</summary>
        Falha,
        /// <summary>
        /// Mensagem de aviso de falha para inclusão de categoria devido à já existência de um outra categoria com o mesmo código na base de dados
        /// </summary>,
        FalhaRnIncluirCategoriaJaExistente,
        /// <summary>
        /// Mensagem de aviso de falha para um código de cliente não informado
        /// </summary>,
        FalhaRnIncluirCodClienteNaoInformado,
        /// <summary>
        /// Mensagem de aviso de falha para um cliente não informado
        /// </summary>,
        FalhaRnIncluirClienteNaoInformado,
        /// <summary>Mensagem de aviso de falha para inclusão de agência inválida</summary>
        FalhaRnIncluirAgenciaInvalida,
        /// <summary>
        /// Mensagem de aviso de falha para inclusão de tipo pessoa inválido
        /// </summary>,
        FalhaRnIncluirTipoPessoaInvalido,
        /// <summary>
        /// Mensagem de aviso de falha para inclusão de tipo nome inválido
        /// </summary>,
        FalhaNomeInvalido,
        /// <summary>
        /// Mensagem de aviso de falha para uma agência não informada
        /// </summary>,
        FalhaRnIncluirAgenciaNaoInformada,
        /// <summary>
        /// Mensagem de aviso de falha para inclusão de categoria devido à já existência de um outra categoria com o mesmo código na base de dados
        /// </summary>,
        FalhaRnIncluirCodigoClienteEtipoPessoaJaExistente,
    }

    class Mensagem : Bergs.Pwx.Pwxoiexn.Mensagens.Mensagem
    {
        /// <summary>
        /// Mensagem
        /// </summary>
        private string mensagem;

        /// <summary>
        /// Tipo de mensagem
        /// </summary>
        private Pxcsclxn.TipoMensagem tipoMensagem;

        /// <summary>
        /// Mensagem para o usuário
        /// </summary>
        public override string ParaUsuario
        {
            get { return this.ParaOperador; }
        }

        /// <summary>
        /// Mensagem para o operador
        /// </summary>
        public override string ParaOperador
        {
            get { return this.mensagem; }
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
        /// <param name="mensagem">Mensagem</param>
        /// <param name="argumentos">Argumentos</param>
        public Mensagem(Pxcsclxn.TipoMensagem mensagem, params string[] argumentos)
        {
            tipoMensagem = mensagem;

            switch (mensagem)
            {
                case Pxcsclxn.TipoMensagem.Falha:
                    this.mensagem = string.Empty;
                    break;
                case Pxcsclxn.TipoMensagem.FalhaRnIncluirAgenciaInvalida:
                    this.mensagem = "A agência informada não existe no sistema!";
                    break;
                //case Pxcscaxn.TipoMensagem.FalhaRnIncluirCategoriaJaExistente:
                //    this.mensagem = "Categoria já existente.";
                //    break;
                case Pxcsclxn.TipoMensagem.FalhaRnIncluirCodClienteNaoInformado:
                    this.mensagem = "Um código válido deve ser informado.";
                    break;
                case Pxcsclxn.TipoMensagem.FalhaNomeInvalido:
                    this.mensagem = TOClientePxc.MENSAGEM_ALFANUMERICO;
                    break;
                case Pxcsclxn.TipoMensagem.FalhaRnIncluirTipoPessoaInvalido:
                    this.mensagem = "Um tipo pessoa válido(F ou J) deve ser informado.";
                    break;
                case Pxcsclxn.TipoMensagem.FalhaRnIncluirAgenciaNaoInformada:
                    this.mensagem = "Uma agência válida(515, 590, 4022 ou 9008) deve ser informada.";
                    break;
                case Pxcsclxn.TipoMensagem.FalhaRnIncluirCodigoClienteEtipoPessoaJaExistente:
                    this.mensagem = "Já existe na base de dados um cliente com a combinação de código cliente e tipo pessoa informados.";
                    break;
            }
        }
    }
}
