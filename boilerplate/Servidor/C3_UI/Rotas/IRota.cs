using PseudoFramework.SharedUtils;
using System;

namespace C3_UI.Rotas
{
    public interface IRota
    {
        RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json);
    }
}
