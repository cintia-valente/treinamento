using Bergs.Pwx.Pwxoiexn;
using Bergs.Pwx.Pwxoiexn.IN;
using Bergs.Pwx.Pwxoiexn.IN.Controles;
using Bergs.Pxc.Pxcbtoxn;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace Bergs.Pxc.Pxcwidxn
{
	
	/// <summary>Classe responsável pela tela da transação.</summary>
	[WebService(Namespace = "Bergs.Pxc.Pxcwidxn")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Idioma : AplicacaoTelaHtml5
	{
		/// <summary>Construtor.</summary>
		public Idioma() : 
				base(10)
		{
		}
		/// <summary>Pesquisa dados referentes ao filtro aplicado</summary>
		/// <param name="pagina">Pagina inicial.</param>
		/// <returns>Lista dos dados obtidos.</returns>
		[WebMethod()]
		protected override string Listar(Int32 pagina)
		{
			try
			{
				Pxcsidxn.Idioma rnIdioma = this.Infra.InstanciarRN<Pxcsidxn.Idioma>();
				TOIdioma toIdioma = this.PopularTOIdiomaFiltro();
				Retorno<long> contagemIdioma = rnIdioma.Contar(toIdioma);
				if (!(contagemIdioma.OK && contagemIdioma.Dados > 0))
				{
					return this.Infra.RetornarJson(contagemIdioma);
				}
				this.CalcularPaginacao(contagemIdioma.Dados);
				TOPaginacao toPaginacao = new TOPaginacao(this.Pagina, this.RegistrosPorPagina);
				Retorno<List<TOIdioma>> retorno = rnIdioma.Listar(toIdioma, toPaginacao);
				ListaHtml5<TOIdioma> lista = new ListaHtml5<TOIdioma>(retorno.Dados, toPaginacao);
				return this.Infra.RetornarJson(retorno, lista);
			}
			catch (Exception ex)
			{
				return this.Infra.TratarExcecaoJson(ex);
			}
		}
		/// <summary>Imprime os dados.</summary>
		/// <returns>Dados da impressão.</returns>
		[WebMethod()]
		public string Imprimir()
		{
			try
			{
				TOIdioma toIdioma = this.PopularTOIdiomaLista();
				Pxcsidxn.Idioma rnIdioma = this.Infra.InstanciarRN<Pxcsidxn.Idioma>();
				Retorno<string> retorno = rnIdioma.Imprimir(toIdioma);
				return this.Infra.RetornarJson(retorno);
			}
			catch (Exception ex)
			{
				return this.Infra.TratarExcecaoJson(ex);
			}
		}
		/// <summary>Inclui um registro.</summary>
		/// <returns>Status da operação</returns>
		[WebMethod()]
		public string Incluir()
		{
			try
			{
				TOIdioma toIdioma = this.PopularTOIdiomaCadastroIncluir();
				Pxcsidxn.Idioma rnIdioma = this.Infra.InstanciarRN<Pxcsidxn.Idioma>();
				Retorno<int> retorno = rnIdioma.Incluir(toIdioma);
				return this.Infra.RetornarJson(retorno);
			}
			catch (Exception ex)
			{
				return this.Infra.TratarExcecaoJson(ex);
			}
		}
		/// <summary>Salva alterações do item detalhado</summary>
		/// <returns>Status da operação.</returns>
		[WebMethod()]
		public string Alterar()
		{
			try
			{
				TOIdioma toIdioma = this.PopularTOIdiomaCadastro();
				Pxcsidxn.Idioma rnIdioma = this.Infra.InstanciarRN<Pxcsidxn.Idioma>();
				Retorno<int> retorno = rnIdioma.Alterar(toIdioma);
				return this.Infra.RetornarJson(retorno);
			}
			catch (Exception ex)
			{
				return this.Infra.TratarExcecaoJson(ex);
			}
		}
		/// <summary>Exclui o item detalhado.</summary>
		/// <returns>Status da operação.</returns>
		[WebMethod()]
		public string Excluir()
		{
			try
			{
				TOIdioma toIdioma = this.PopularTOIdiomaCadastro();
				Pxcsidxn.Idioma rnIdioma = this.Infra.InstanciarRN<Pxcsidxn.Idioma>();
				Retorno<int> retorno = rnIdioma.Excluir(toIdioma);
				return this.Infra.RetornarJson(retorno);
			}
			catch (Exception ex)
			{
				return this.Infra.TratarExcecaoJson(ex);
			}
		}
		/// <summary>Obtém o objeto pela chave.</summary>
		/// <returns>Objeto</returns>
		[WebMethod()]
		public string Obter()
		{
			try
			{
				TOIdioma toIdioma = this.PopularTOIdiomaLista();
				Pxcsidxn.Idioma rnIdioma = this.Infra.InstanciarRN<Pxcsidxn.Idioma>();
				Retorno<TOIdioma> retorno = rnIdioma.Obter(toIdioma);
				return this.Infra.RetornarJson(retorno);
			}
			catch (Exception ex)
			{
				return this.Infra.TratarExcecaoJson(ex);
			}
		}
		/// <summary>Popula um TOIdioma para ser utilizado nos métodos de Filtro.</summary>
		/// <returns>TO populado</returns>
		private TOIdioma PopularTOIdiomaFiltro()
		{
			TOIdioma toIdioma = new TOIdioma();
            if (this.LerValorCliente("cod_idioma") != null)
            {
                toIdioma.CodIdioma = Convert.ToInt32(this.LerValorCliente("cod_idioma"));
            }
            if (this.LerValorCliente("cod_iso_idioma") != null)
            {
                toIdioma.CodigoIsoCombinado = this.LerValorCliente("cod_iso_idioma");
            }
            return toIdioma;
		}
		/// <summary>Popula um TOIdioma para ser utilizado nos métodos de Lista.</summary>
		/// <returns>TO populado</returns>
		private TOIdioma PopularTOIdiomaLista()
		{
			TOIdioma toIdioma = new TOIdioma();
			if (this.LerValorCliente("cod_idioma") != null)
			{
			toIdioma.CodIdioma = Convert.ToInt32(this.LerValorCliente("cod_idioma"));
			}
			if (this.LerValorCliente("cod_iso_idioma") != null)
			{
				toIdioma.CodigoIsoCombinado = this.LerValorCliente("cod_iso_idioma");
			}
			return toIdioma;
		}
		/// <summary>Popula um TOIdioma para ser utilizado nos métodos de Cadastro.</summary>
		/// <returns>TO populado</returns>
		private TOIdioma PopularTOIdiomaCadastro()
		{
			TOIdioma toIdioma = new TOIdioma();
			if (this.LerValorCliente("cod_idioma") != null)
			{
				toIdioma.CodIdioma = Convert.ToInt32(this.LerValorCliente("cod_idioma"));
			}
			if (this.LerValorCliente("cod_iso_idioma") != null)
			{
				toIdioma.CodigoIsoCombinado = this.LerValorCliente("cod_iso_idioma");
			}
			if (this.LerValorCliente("desc_idioma") != null)
			{
				toIdioma.DescIdioma = this.LerValorCliente("desc_idioma");
			}
			if (this.LerValorCliente("cod_usuario") != null)
			{
				toIdioma.CodUsuario = this.LerValorCliente("cod_usuario");
			}
			else
			{
				toIdioma.CodUsuario = new CampoOpcional<string>(null);
			}
			if (this.LerValorCliente("dthr_ult_atu") != null)
			{
				toIdioma.DthrUltAtu = Convert.ToDateTime(this.LerValorCliente("dthr_ult_atu"));
			}
			else
			{
				toIdioma.DthrUltAtu = new CampoOpcional<DateTime>(null);
			}
			return toIdioma;
		}
        /// <summary>Popula um TOIdioma para ser utilizado nos métodos de Cadastro.</summary>
		/// <returns>TO populado</returns>
		private TOIdioma PopularTOIdiomaCadastroIncluir()
        {
            TOIdioma toIdioma = new TOIdioma();
            
            if (this.LerValorCliente("desc_idioma") != null)
            {
                toIdioma.DescIdioma = this.LerValorCliente("desc_idioma");
            }         
            else
            {
                toIdioma.DthrUltAtu = new CampoOpcional<DateTime>(null);
            }
            return toIdioma;
        }
    }
}

