using System;
using System.Linq;

namespace Bergs.Pxc.Pxcsidxn
{
    /// <summary>
    /// Mensagens previstas para o componente
    /// </summary>
    public enum TipoIdiomaMensagem
    {
        /// <summary>
        /// Mensagem de aviso de necessidade de existência do código ISO combinado
        /// </summary>
        FalhaRnValidarExistenciaCodIso,
        /// <summary>
        /// Mensagem de aviso de necessidade de existência de ao menos um dos códigos (ISO combinado ou numérico)
        /// </summary>
        FalhaRnValidarExistenciaCodIsoOuCodNumerico,
        /// <summary>
        /// Mensagem de aviso de necessidade de existência da descrição
        /// </summary>
        FalhaRnValidarExistenciaDescricao,
        /// <summary>
        /// Mensagem de aviso de conflito quando validando igualdade entre códigos (ISO combinado e numérico) em cenário de recepção de ambos
        /// </summary>
        FalhaRnValidarEquivalenciaCodIsoOuCodNumerico,
        /// <summary>
        /// Mensagem de aviso de falha ao converter um código ISO combinado para um código numérico
        /// </summary>
        FalhaRnConverterCodIsoParaCodNumerico,
        /// <summary>
        /// Mensagem de aviso de falha ao converter um código numérico para um código ISO combinado
        /// </summary>
        FalhaRnConverterCodNumericoParaCodIso,
        /// <summary>
        /// Mensagem de aviso de falha ao consultar um idioma existente na base de dados
        /// </summary>
        FalhaRnConsultarIdiomaBaseDados,
        /// <summary>
        /// Mensagem de aviso de falha para inclusão de idioma devido à já existência de um outro idioma com o mesmo código na base de dados
        /// </summary>
        FalhaRnIncluirIdiomaJaExistente,
        /// <summary>
        /// Mensagem genérica de falha indeterminada
        /// </summary>
        FalhaIndeterminada
    }

    /// <summary>
    /// Classe de mapa de mensagens de idiomas
    /// </summary>
    public class IdiomaMensagem: Pwx.Pwxoiexn.Mensagens.Mensagem
    {
        private const string MOTIVO_INDETERMINADO = "Motivo indeterminado.";
        private const string SUFIXO_GENERICO_INDETERMINANCIA = "indeterminado(a)";

        private readonly TipoIdiomaMensagem _tipoMensagem;
        private readonly string _mensagem;

        /// <summary>
        /// Mensagem para o usuário
        /// </summary>
        public override string ParaUsuario
        {
            get { return ParaOperador; }
        }

        /// <summary>
        /// Mensagem para o operador (logs e auditorias internas)
        /// </summary>
        public override string ParaOperador
        {
            get { return _mensagem; }
        }

        /// <summary>
        /// Identificador do tipo de mensagem
        /// </summary>
        public override string Identificador
        {
            get { return _tipoMensagem.ToString(); }
        }

        /// <summary>
        /// Construtor de uma nova mensagem de idioma
        /// </summary>
        /// <param name="tipoMensagem">Identificador do tipo de mensagem</param>
        /// <param name="argumentos">Argumentos extras a serem interpolados na mensagem</param>
        public IdiomaMensagem(TipoIdiomaMensagem tipoMensagem, params string[] argumentos)
        {
            _tipoMensagem = tipoMensagem;

            _mensagem = MapearMensagem(argumentos);
        }

        /// <summary>
        /// Construtor de uma mensagem de idioma a partir de outra mensagem já existente
        /// </summary>
        /// <param name="idiomaMensagem">Objeto de mensagem de idioma</param>
        public IdiomaMensagem(IdiomaMensagem idiomaMensagem)
        {
            _tipoMensagem = (TipoIdiomaMensagem)Enum.Parse(typeof(TipoIdiomaMensagem), idiomaMensagem.Identificador);

            _mensagem = idiomaMensagem.ParaUsuario;
        }

        private string MapearMensagem(params string[] argumentos)
        {
            switch (_tipoMensagem)
            {
                case TipoIdiomaMensagem.FalhaRnValidarExistenciaCodIso:
                    return "O código ISO combinado deve ser informado.";
                case TipoIdiomaMensagem.FalhaRnValidarExistenciaCodIsoOuCodNumerico:
                    return "Ao menos uma das opções de código de idioma (numérico ou ISO combinado) deve ser informada.";
                case TipoIdiomaMensagem.FalhaRnValidarExistenciaDescricao:
                    return "A descrição do idioma deve ser informada.";
                case TipoIdiomaMensagem.FalhaRnValidarEquivalenciaCodIsoOuCodNumerico:
                    return "Ambos os códigos (numérico e ISO combinado) foram informados, porém não são equivalentes. Informe somente um código de idioma.";
                case TipoIdiomaMensagem.FalhaRnConverterCodIsoParaCodNumerico:
                    return $"A conversão do código ISO combinado {argumentos.ElementAtOrDefault(0) ?? SUFIXO_GENERICO_INDETERMINANCIA} para código numérico falhou: {argumentos.ElementAtOrDefault(1) ?? MOTIVO_INDETERMINADO}";
                case TipoIdiomaMensagem.FalhaRnConverterCodNumericoParaCodIso:
                    return $"A conversão do código numérico {argumentos.ElementAtOrDefault(0) ?? SUFIXO_GENERICO_INDETERMINANCIA} para código ISO combinado falhou: {argumentos.ElementAtOrDefault(1) ?? MOTIVO_INDETERMINADO}";
                case TipoIdiomaMensagem.FalhaRnConsultarIdiomaBaseDados:
                    return $"A consulta do idioma na base de dados falhou: {argumentos.ElementAtOrDefault(0) ?? MOTIVO_INDETERMINADO}";
                case TipoIdiomaMensagem.FalhaRnIncluirIdiomaJaExistente:
                    return "Já existe na base de dados um idioma com código equivalente ao código informado.";
                case TipoIdiomaMensagem.FalhaIndeterminada:
                default:
                    return $"Houve uma falha {SUFIXO_GENERICO_INDETERMINANCIA} durante a execução das validações de idioma.";
            }
        }
    }
}
