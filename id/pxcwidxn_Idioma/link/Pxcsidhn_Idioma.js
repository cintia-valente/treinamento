/// <reference path="c:\soft\pwx\link\pwxscohn_jquery_html5.js" />
'use strict';
function Pxcsidhn() { }

Pxcsidhn.prototype = {
    load: function () {

        $('#btnPesquisarFiltro').click(this.clicarPesquisar);
        $('#btnNovoFiltro').click(this.clicarNovo);
        $('#btnLimparFiltroFiltro').click( function () { oInfra.getTela().limparCamposAtivosFormulario('#boxDadosFiltro'); });
        $('#btnNovoLista').click(this.clicarNovo);
        $('#btnImprimirLista').click(this.clicarImprimir);
        $('#btnVoltarLista').click(function () { oInfra.getTela().clicarBotaoVoltar(); });
        $('#btnNovoCadastro').click(this.clicarNovo);
        $('#btnIncluirCadastro').click(this.clicarIncluir);
        $('#btnSalvarCadastro').click(this.clicarSalvar);
        $('#btnHabilitarCadastro').click(function () { oInfra.getTela().clicarHabilitarCadastro(); });
        $('#btnExcluirCadastro').click(this.clicarExcluir);
        $('#btnLimparCadastroCadastro').click( function () { oInfra.getTela().limparCamposAtivosFormulario('#boxDadosCadastro'); });
        $('#btnVoltarCadastro').click(function () { oInfra.getTela().clicarBotaoVoltar(); });
        oInfra.getUtil().carregarValidador();
    },
    formatarLinhaListaTabelalista: function (dados) {
        dados.cod_idioma_formatado = oInfra.getUtil().aplicarMascara(dados.cod_idioma, '');
        dados.dthr_ult_atu_formatado = oInfra.getUtil().aplicarMascara(dados.dthr_ult_atu, '00/00/0000 00:00:00');
        return dados;
    },
    clicarPesquisar: function () {
        if (!oInfra.getUtil().validarFormulario('#formFiltro')) {
            return false;
        }
        var cod_idioma = $('#txtFiltroCodIdioma').val();
        var cod_iso_idioma = $('#txtFiltroCodigoIsoCombinado').val().trim();

        oInfra.getTela().getParametros().limpar();
        var parms = oInfra.getTransacao().getParametros().oPares;
        parms.cod_idioma = cod_idioma;
        parms.cod_iso_idioma = cod_iso_idioma;
        oInfra.getTela().clicarBotaoPrimeiraPagina();
    },
    clicarNovo: function () {
        oInfra.getTela().limparCamposFormulario('#formCadastro');
        oInfra.getTela().mostrarTela('Cadastro', 'Inclusao');

    },
    clicarImprimir: function () {
        oInfra.getServidor().invocarServico('Pxcwidxn_Idioma.asmx/Imprimir', {
            parametros: oInfra.getTransacao().getParametros(),
            retorno: function(oJson) {
                if (oInfra.getJsonUtil().confirmarSucesso(oJson)) {
                    oInfra.getServidor().download(oJson.dados.registrousuario, {janela: 'window'});
                } else {
                    oInfra.getTela().mensagem(oInfra.getJsonUtil().lerMensagem(oJson));
                }
            }
        });
    },
    clicarIncluir: function () {
        if (!oInfra.getUtil().validarFormulario('#formCadastro')) {
            return false;
        }

        oInfra.getTela().getParametros().limpar();
        var parms = oInfra.getTela().getParametros().oPares;
        parms.cod_iso_idioma = $('#txtCadastroCodigoIsoCombinado').val().trim();
        parms.desc_idioma = $('#txtCadastroDescIdioma').val().trim();
        
        oInfra.getServidor().invocarServico('Pxcwidxn_Idioma.asmx/Incluir', {parametros: parms });
    },
    clicarSalvar: function () {
        if (!oInfra.getUtil().validarFormulario('#formCadastro')) {
            return false;
        }

        oInfra.getTela().getParametros().limpar();
        var parms = oInfra.getTela().getParametros().oPares;
        parms.cod_iso_idioma = $('#txtCadastroCodigoIsoCombinado').val().trim();
        parms.cod_idioma = $('#txtCadastroCodIdioma').val().trim();
        parms.desc_idioma = $('#txtCadastroDescIdioma').val().trim();
        parms.cod_usuario = $('#txtCadastroCodUsuario').val().trim();
        parms.dthr_ult_atu = $('#txtCadastroDthrUltAtu').val().trim();
        oInfra.getServidor().invocarServico('Pxcwidxn_Idioma.asmx/Alterar', {parametros: parms });
    },
    clicarExcluir: function () {
        oInfra.getTela().mensagem(
            'Confirma a exclusão do registro?',
            'Atenção',
            ['Sim', 'Não'],
            [
                function () {
                    oInfra.getTela().fecharMensagem();
                    var cod_idioma = $('#txtCadastroCodIdioma').val().trim();
                    oInfra.getTela().getParametros().limpar();
                    var parms = oInfra.getTela().getParametros().oPares;
                    parms.cod_idioma = cod_idioma;
                    oInfra.getServidor().invocarServico('Pxcwidxn_Idioma.asmx/Excluir', { parametros: parms});
                },
                function () {
                    oInfra.getTela().fecharMensagem();
                }
            ]
        );
    },
    tratarRetornoObterboxDadosCadastro: function (oJson) {
        if (oInfra.getJsonUtil().confirmarSucesso(oJson)) {
            if (oJson.dados) {
                oInfra.getTela().popularCamposFormulario('#formCadastro', {
                    cod_idioma: oJson.dados.registrousuario.cod_idioma,
                    cod_iso_idioma: oJson.dados.registrousuario.cod_iso_idioma,
                    desc_idioma: oJson.dados.registrousuario.desc_idioma,
                    cod_usuario: oJson.dados.registrousuario.cod_usuario,
                    dthr_ult_atu: oJson.dados.registrousuario.dthr_ult_atu,

                });
                oInfra.getTela().mostrarTela('Cadastro', 'Consulta');
            }
        } else {
            oInfra.getTela().mensagem(oInfra.getJsonUtil().lerMensagem(oJson));
        }
    },
    exibirIdioma: function (cod_idioma) {
        oInfra.getTela().getParametros().limpar();
        var parms = oInfra.getTela().getParametros().oPares;
        parms.cod_idioma = cod_idioma;
        //parms.cod_iso_idioma = cod_iso_idioma;
        oInfra.getServidor().invocarServico('Pxcwidxn_Idioma.asmx/Obter', {
            parametros: parms,
            retorno: oPxcsidhn.tratarRetornoObterboxDadosCadastro
        });
    },

};
