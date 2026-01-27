using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace PseudoFramework.ServidorUtils
{
    public class CampoArquivoJsonMapper
    {
        public static Dictionary<string, object> ToDicionario(object objeto)
        {
            var dicionario = new Dictionary<string, object>();

            var tipo = objeto.GetType();

            foreach (var propriedade in tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var anotacao = propriedade.GetCustomAttribute<CampoArquivoJsonAttribute>();

                var nomeJson = anotacao?.NomeCampo ?? propriedade.Name;

                var valor = propriedade.GetValue(objeto);

                dicionario[nomeJson] = ConverterValor(valor);
            }

            return dicionario;
        }

        public static T FromDicionario<T>(Dictionary<string, object> dicionario) where T : new()
        {
            var objeto = new T();

            var tipo = typeof(T);

            foreach (var propriedade in tipo.GetProperties())
            {
                var anotacao = propriedade.GetCustomAttribute<CampoArquivoJsonAttribute>();

                var nomeJson = anotacao?.NomeCampo ?? propriedade.Name;

                if (!dicionario.ContainsKey(nomeJson))
                    continue;

                var valor = dicionario[nomeJson];

                if (valor == null)
                {
                    propriedade.SetValue(objeto, null);

                    continue;
                }

                propriedade.SetValue(objeto, ConverterValor(valor, propriedade.PropertyType));
            }

            return objeto;
        }

        private static object ConverterValor(object valor, Type destino)
        {
            if (valor == null)
                return null;

            if (destino == typeof(string))
                return valor.ToString();

            if (destino.IsEnum)
                return Enum.Parse(destino, valor.ToString());

            if (destino.IsPrimitive || destino == typeof(decimal))
                return Convert.ChangeType(valor, destino);

            if (destino == typeof(DateTime))
                return Convert.ToDateTime(valor);

            if (typeof(IEnumerable).IsAssignableFrom(destino) && destino.IsGenericType)
            {
                var tipoItem = destino.GetGenericArguments()[0];

                if (valor is IEnumerable enumerable)
                {
                    var novaLista = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(tipoItem));

                    foreach (var valorItem in enumerable)
                        novaLista.Add(ConverterValor(valorItem, tipoItem));

                    return novaLista;
                }
            }

            if (valor is Dictionary<string, object> subDicionario)
            {
                if (destino.IsPrimitive || destino == typeof(decimal) || destino == typeof(string))
                    return valor;

                var metodo = typeof(CampoArquivoJsonMapper)
                    .GetMethod(nameof(FromDicionario))
                    .MakeGenericMethod(destino);

                return metodo.Invoke(null, new object[] { subDicionario });
            }

            return valor;
        }

        private static object ConverterValor(object valor)
        {
            if (valor == null)
                return null;

            if (valor is IEnumerable lista && !(valor is string))
            {
                var novaLista = new List<object>();

                foreach (var item in lista)
                    novaLista.Add(ConverterValor(item));

                return novaLista;
            }

            var tipo = valor.GetType();

            if (tipo.IsClass && tipo != typeof(string))
                return ToDicionario(valor);

            return valor;
        }
    }
}
