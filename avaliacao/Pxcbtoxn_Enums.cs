using Bergs.Pwx.Pwxoiexn;

namespace Bergs.Pxc.Pxcbtoxn.pgm
{
    /// <summary>
    /// Enum para as situações de contas
    /// </summary>
    [CharEnum]
    public enum SituacaoConta
    {
        /// <summary>
        /// Conta Ativa
        /// </summary>
        Ativa = 'A',
        /// <summary>
        /// Conta Inativa
        /// </summary>
        Inativa = 'I',
        /// <summary>
        /// Conta Suspensa
        /// </summary>
        Suspensa = 'S'
    }

    /// <summary>
    /// Enum para os tipos de pessoas de clientes
    /// </summary>
    [CharEnum]
    public enum TipoPessoa
    {
        /// <summary>
        /// Cliente Pessoa Física
        /// </summary>
        Fisica = 'F',
        /// <summary>
        /// Cliente Pessoa Jurídica
        /// </summary>
        Juridica = 'J',
    }
}