using Bergs.Bth.Bthstixn;
using Bergs.Bth.Bthstixn.MM4;
using Bergs.Pwx.Pwxoiexn;
using Bergs.Pxc.Pxcbtoxn;
using Bergs.Pxc.Pxcbtoxn.pgm;
using Bergs.Pxc.Pxcscnxn;
using NUnit.Framework;

namespace Bergs.Pxc.Pxcucnxn.Tests
{
    /// <summary>
    /// Contém os métodos de teste da classe Conta
    /// </summary>
    [TestFixture(Description = "Classe de testes para a classe RN Conta.", Author = "T07007")]
    public class ContaTests: AbstractTesteRegraNegocio<Conta>
    {
        private const string CNPJ_DB_TREINAMENTOS_FICTICIO = "43977980000190";
        private const string CPF_JOHN_DOE_FICTICIO = "89276546090";

        ///  <summary>
        /// Realiza o teste padrão para o método Incluir(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Incluir(TOConta)", Author = "T07007")]
        public void US1_CA01_IncluirComSucessoTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            base.TestarIncluir(toConta);
        }

        ///  <summary>
		/// Realiza o teste sem agência para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) sem agência", Author = "T07007")]
        public void US1_CA02_IncluirSemAgenciaTest()
        {
            var toConta = new TOConta
            {
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.AgenciaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste com agência inválida para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) com agência inválida", Author = "T07007")]
        public void US1_CA03_IncluirComAgenciaInvalidaTest()
        {
            var toConta = new TOConta
            {
                Agencia = 999,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.AgenciaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste sem número de conta para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) sem número de conta", Author = "T07007")]
        public void US1_CA04_IncluirSemContaTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste com número de conta inválido para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) com número de conta inválido", Author = "T07007")]
        public void US1_CA05_IncluirComNumeroContaInvalidoTest(double conta)
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 0,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste com uma outra conta bancária já existente para o método Incluir(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Incluir(TOConta) com uma outra conta bancária já existente", Author = "T07007")]
        public void US1_CA06_IncluirContaComOutraJaExistenteTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            RN.Incluir(toConta);

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaAgenciaJaExiste);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste sem saldo informado para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) sem saldo informado", Author = "T07007")]
        public void US1_CA07_IncluirSemSaldoInformadoTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.SaldoInvalido);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste com saldo inválido para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) com saldo inválido", Author = "T07007")]
        public void US1_CA08_IncluirComSaldoInvalidoTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = -0.01M
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.SaldoInvalido);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste com cliente inválido para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) com cliente inválido", Author = "T07007")]
        public void US1_CA09_IncluirComClienteInvalidoTest(string codigoCliente)
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = "12345678",
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = TOUtils.ExtrairMensagemAtributoValidacaoCampoTO<CPFCNPJAttribute>(toConta, nameof(toConta.CodCliente));

            Assert.That(resultado.Mensagem.ParaUsuario, Contains.Substring(mensagemEsperada));
        }

        ///  <summary>
		/// Realiza o teste com tipo de pessoa do cliente inválido para o método Incluir(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Incluir(TOConta) com tipo de pessoa do cliente inválido", Author = "T07007")]
        public void US1_CA10_IncluirComTipoPessoaClienteInvalidoTest(char tipoPessoa)
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = (TipoPessoa)'G',
                Saldo = 1000
            };

            var resultado = RN.Incluir(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.TipoPessoaInvalido);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste sem filtros para o método Listar(TOConta, TOPaginacao)
        /// </summary>
        [Test(Description = "Testa o método Listar(TOConta, TOPaginacao) sem filtros", Author = "T07007")]
        public void US2_CA01_ListarSemFiltrosComSucessoTest()
        {
            Infra.ExecutarSql($"DELETE FROM PXC.BOLETO WHERE COD_BOLETO > 0");

            Infra.ExecutarSql($"DELETE FROM PXC.PIX WHERE COD_PIX > 0");

            Infra.ExecutarSql($"DELETE FROM {TOConta.TABELA} WHERE {TOConta.NUMERO_AGENCIA} > 0");

            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            RN.Incluir(toConta);

            toConta = new TOConta();
            var toPaginacao = new TOPaginacao();

            base.TestarListar(toConta, toPaginacao);
        }

        ///  <summary>
        /// Realiza o teste com filtros para o método Listar(TOConta, TOPaginacao)
        /// </summary>
        [Test(Description = "Testa o método Listar(TOConta, TOPaginacao) com filtros", Author = "T07007")]
        public void US2_CA02_ListarComFiltrosComSucessoTest()
        {
            Infra.ExecutarSql($"DELETE FROM PXC.BOLETO WHERE COD_BOLETO > 0");

            Infra.ExecutarSql($"DELETE FROM PXC.PIX WHERE COD_PIX > 0");

            Infra.ExecutarSql($"DELETE FROM {TOConta.TABELA} WHERE {TOConta.NUMERO_AGENCIA} > 0");

            RN.Incluir(
                new TOConta
                {
                    Agencia = 1000,
                    Conta = 10000010,
                    CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                    IndTpPessoa = TipoPessoa.Juridica,
                    Saldo = 1000
                }
            );

            RN.Incluir(
                new TOConta
                {
                    Agencia = 1000,
                    Conta = 10000011,
                    CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                    IndTpPessoa = TipoPessoa.Juridica,
                    Saldo = 1000
                }
            );

            RN.Incluir(
                new TOConta
                {
                    Agencia = 1000,
                    Conta = 10000012,
                    CodCliente = CPF_JOHN_DOE_FICTICIO,
                    IndTpPessoa = TipoPessoa.Fisica,
                    Saldo = 1000
                }
            );

            RN.Incluir(
                new TOConta
                {
                    Agencia = 1001,
                    Conta = 10000010,
                    CodCliente = CPF_JOHN_DOE_FICTICIO,
                    IndTpPessoa = TipoPessoa.Fisica,
                    Saldo = 1000
                }
            );

            var toConta = new TOConta();

            toConta.CodCliente = CPF_JOHN_DOE_FICTICIO;

            var resultado = RN.Listar(toConta, null);

            Assert.That(resultado.OK, Is.True);

            Assert.That(resultado.Dados.Count, Is.EqualTo(2));
        }

        ///  <summary>
        /// Realiza o teste sem contas bancárias cadastradas para o método Listar(TOConta, TOPaginacao)
        /// </summary>
        [Test(Description = "Testa o método Listar(TOConta, TOPaginacao) sem contas bancárias cadastradas", Author = "T07007")]
        public void US2_CA03_ListarSemContasBancariasCadastradasTest()
        {
            Infra.ExecutarSql($"DELETE FROM PXC.BOLETO WHERE COD_BOLETO > 0");

            Infra.ExecutarSql($"DELETE FROM PXC.PIX WHERE COD_PIX > 0");

            Infra.ExecutarSql($"DELETE FROM {TOConta.TABELA} WHERE {TOConta.NUMERO_AGENCIA} > 0");

            var toConta = new TOConta();

            toConta.Agencia = 1000;

            toConta.CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO;

            var resultado = RN.Listar(toConta, null);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.NaoExistemContasBancariasCadastradas);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste padrão para o método Obter(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Obter(TOConta)", Author = "T07007")]
        public void US3_CA01_ObterComSucessoTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            RN.Incluir(toConta);

            base.TestarObter(toConta);
        }

        ///  <summary>
		/// Realiza o teste sem agência para o método Obter(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Obter(TOConta) sem agência", Author = "T07007")]
        public void US3_CA02_ObterSemAgenciaTest()
        {
            var toConta = new TOConta
            {
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Obter(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.AgenciaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste com agência inválida para o método Obter(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Obter(TOConta) com agência inválida", Author = "T07007")]
        public void US3_CA03_ObterComAgenciaInvalidaTest()
        {
            var toConta = new TOConta
            {
                Agencia = 999,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Obter(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.AgenciaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste sem número de conta para o método Obter(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Obter(TOConta) sem número de conta", Author = "T07007")]
        public void US3_CA04_ObterSemContaTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Obter(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste com número de conta inválido para o método Obter(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Obter(TOConta) com número de conta inválido", Author = "T07007")]
        public void US3_CA05_ObterComNumeroContaInvalidoTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 0,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Obter(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste sem uma conta bancária existente para o método Obter(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Obter(TOConta) sem uma conta bancária existente", Author = "T07007")]
        public void US3_CA06_ObterSemContaExistenteTest()
        {
            Infra.ExecutarSql($"DELETE FROM PXC.BOLETO WHERE COD_BOLETO > 0");

            Infra.ExecutarSql($"DELETE FROM PXC.PIX WHERE COD_PIX > 0");

            Infra.ExecutarSql($"DELETE FROM {TOConta.TABELA} WHERE {TOConta.NUMERO_AGENCIA} > 0");

            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            var resultado = RN.Obter(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.NaoExisteContaBancariaCadastrada);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste padrão para o método Alterar(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Alterar(TOConta)", Author = "T07007")]
        public void US4_CA01_AlterarComSucessoTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                CodCliente = CNPJ_DB_TREINAMENTOS_FICTICIO,
                IndTpPessoa = TipoPessoa.Juridica,
                Saldo = 1000
            };

            RN.Incluir(toConta);

            toConta.IndSituacao = SituacaoConta.Suspensa;

            base.TestarAlterar(toConta);
        }

        ///  <summary>
        /// Realiza o teste sem agência para o método Alterar(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Alterar(TOConta) sem agência", Author = "T07007")]
        public void US4_CA02_AlterarSemAgenciaTest()
        {
            var toConta = new TOConta
            {
                Conta = 1,
                IndSituacao = SituacaoConta.Suspensa
            };

            var resultado = RN.Alterar(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.AgenciaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste com agência inválida para o método Alterar(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Alterar(TOConta) com agência inválida", Author = "T07007")]
        public void US4_CA03_AlterarComAgenciaInvalidaTest()
        {
            var toConta = new TOConta
            {
                Agencia = 999,
                Conta = 1,
                IndSituacao = SituacaoConta.Suspensa
            };

            var resultado = RN.Alterar(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.AgenciaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste sem número de conta para o método Alterar(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Alterar(TOConta) sem número de conta", Author = "T07007")]
        public void US4_CA04_AlterarSemContaTest()
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                IndSituacao = SituacaoConta.Suspensa
            };

            var resultado = RN.Alterar(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste com número de conta inválido para o método Alterar(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Alterar(TOConta) com número de conta inválido", Author = "T07007")]
        public void US4_CA05_AlterarComNumeroContaInvalidoTest(double conta)
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 0,
                IndSituacao = SituacaoConta.Suspensa
            };

            var resultado = RN.Alterar(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.ContaInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
		/// Realiza o teste com situação atualizada inválida para o método Alterar(TOConta)
		/// </summary>
		[Test(Description = "Testa o método Alterar(TOConta) com situação atualizada inválida", Author = "T07007")]
        public void US4_CA06_AlterarComSituacaoAtualizadaInvalidaTest(char situacao)
        {
            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                IndSituacao = (SituacaoConta)'G',
            };

            var resultado = RN.Alterar(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.SituacaoInvalida);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }

        ///  <summary>
        /// Realiza o teste sem uma conta bancária existente para o método Alterar(TOConta)
        /// </summary>
        [Test(Description = "Testa o método Alterar(TOConta) sem uma conta bancária existente", Author = "T07007")]
        public void US4_CA07_AlterarSemContaExistenteTest()
        {
            Infra.ExecutarSql($"DELETE FROM PXC.BOLETO WHERE COD_BOLETO > 0");

            Infra.ExecutarSql($"DELETE FROM PXC.PIX WHERE COD_PIX > 0");

            Infra.ExecutarSql($"DELETE FROM {TOConta.TABELA} WHERE {TOConta.NUMERO_AGENCIA} > 0");

            var toConta = new TOConta
            {
                Agencia = 1000,
                Conta = 1,
                IndSituacao = SituacaoConta.Suspensa
            };

            var resultado = RN.Alterar(toConta);

            Assert.That(resultado.OK, Is.False);

            var mensagemEsperada = new ContaMensagem(TipoMensagem.NaoExisteContaBancariaCadastrada);

            MMAssert.FalhaComMensagem<ContaMensagem>(resultado, mensagemEsperada.Identificador);
        }
        
        /// <summary>
        /// Executa uma ação UMA vez por classe, ANTES do início da execução dos métodos de teste
        /// </summary>
        protected override void BeforeAll()
        { }

        /// <summary>
        /// Executa uma ação ANTES de cada método de teste da classe
        /// </summary>
        protected override void BeforeEach()
        { }

        /// <summary>
        /// Executa uma ação UMA vez por classe, DEPOIS do término da execução dos métodos de teste
        /// </summary>
        protected override void AfterAll()
        { }

        /// <summary>
        /// Executa uma ação DEPOIS de cada método de teste da classe
        /// </summary>
        protected override void AfterEach()
        { }

        /// <summary>
        /// Método para setar os dados necessários para conexão com o PHA no servidor de build
        /// </summary>
        /// <returns>TO com dados necessários para conexão no servidor de build</returns>
        protected override TOPhaServidorBuild SetarDadosServidorBuild()
        {
            var grupoPha = "GESTAG"; 
            var nomeProdutoPha = "TREINAMENTO MM5";

            return new TOPhaServidorBuild(grupoPha, nomeProdutoPha);
        }
    }
}
