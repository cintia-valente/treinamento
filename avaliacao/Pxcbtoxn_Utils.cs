using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.Validacoes;
using System;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Bergs.Pxc.Pxcbtoxn.pgm
{
    /// <summary>
    /// Utilitários para objetos BD da camada Q
    /// </summary>
    public static class BDUtils
    {
        /// <summary>
        /// Constante contendo a cláusula a ser chamada quando se quer uma atribuição de data e hora atuais em um campo dentro do contexto da base de dados
        /// </summary>
        public const string BD_CURRENT_TIMESTAMP = "CURRENT_TIMESTAMP";

        /// <summary>
        /// Método para checar se uma propriedade tem configuração de parâmetros equivalentes a de um valor numérico com casas decimais
        /// </summary>
        /// <param name="tipo">Tipo de parâmetro</param>
        /// <param name="casasDecimais">Número de casas decimais do parâmetro</param>
        /// <returns></returns>
        public static bool IsParametroPontoFlutuante(DbType tipo, byte casasDecimais)
        {
            if (tipo == DbType.DateTime)
                return false;

            return casasDecimais > 0;
        }

        /// <summary>
        /// Método para converter propriedades numéricas com casas decimais para parâmetros compatíveis da base de dados
        /// </summary>
        /// <returns></returns>
        public static string FormatarParametroPontoFlutuante(object conteudo, byte casasDecimais)
        {
            return string.Format(CultureInfo.InvariantCulture, $"{{0:F{casasDecimais}}}", conteudo);
        }
    }

    /// <summary>
    /// Utilitários para objetos TO
    /// </summary>
    public static class TOUtils
    {
        /// <summary>
        /// Método para extrair uma mensagem de validação definida através de um atributo de validação
        /// do campo do TO
        /// </summary>
        /// <typeparam name="T">Tipo do atributo de validação desejado</typeparam>
        /// <param name="to">Objeto TO o qual se deseja extrair a mensagem de validação</param>
        /// <param name="nomeCampo">
        /// Nome do campo (propriedade) do objeto TO o qual se deseja extrair a mensagem de validação
        /// <para>Use <c>nameof()</c> para obter o nome string de uma propriedade</para>
        /// </param>
        /// <returns>Mensagem string contida na propriedade MensagemErro do atributo de validação, no campo especificado do TO especificado</returns>
        public static string ExtrairMensagemAtributoValidacaoCampoTO<T>(object to, string nomeCampo)
            where T : ValidacaoAttribute
        {
            var campo = to.GetType().GetProperty(nomeCampo);

            if (campo == null)
                return null;

            var atributoValidacao = campo
                .GetCustomAttributes(typeof(T), inherit: true)
                .Cast<T>()
                .FirstOrDefault();

            return atributoValidacao?.MensagemErro;
        }
    }

    /// <summary>
    /// Validador combinado de CPF/CNPJ para checagem de formato para propriedades ou campos que podem se comportar tanto como CPF quanto como CNPJ
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class CPFCNPJAttribute: ValidacaoAttribute
    {
        private readonly CPFAttribute _cpf;
        private readonly CNPJAttribute _cnpj;

        /// <summary>
        /// Construtor de um novo validador de CPF/CNPJ
        /// </summary>
        /// <param name="mensagemErro">Mensagem a ser exibida se inválido. Default: Propriedade ou campo não é um CPF ou CNPJ válido.</param>
        /// <param name="contextos">Contexto(s) em que deve ser validado (mais de um contexto deve-se separar com ponto-e-vírgula). Default: null</param>
        /// <param name="validarSomenteSetado">Validar somente se campo opcional foi setado. Default: true</param>
        public CPFCNPJAttribute(
            string mensagemErro = "Propriedade ou campo não é um CPF ou CNPJ válido.",
            string contextos = null,
            bool validarSomenteSetado = true)
            : base(mensagemErro, contextos, validarSomenteSetado)
        {
            _cpf = new CPFAttribute(mensagemErro, contextos, validarSomenteSetado);

            _cnpj = new CNPJAttribute(mensagemErro, contextos, validarSomenteSetado);
        }

        /// <summary>
        /// Método de checagem do validador de CPF/CNPJ
        /// </summary>
        /// <param name="valor">Valor a ser checado.</param>
        /// <returns>True caso o valor seja válido para pelo menos um dos dois (CPF ou CNPJ), caso contrário, False.</returns>
        public override bool Validar(ValorMembro<object> valor)
        {
            if (_cpf.Validar(valor)) return true;

            if (_cnpj.Validar(valor)) return true;

            return false;
        }
    }
}
