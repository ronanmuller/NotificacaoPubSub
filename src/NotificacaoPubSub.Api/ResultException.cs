using System;

namespace NotificacaoPubSub.Api
{
    public class ResultException : Exception
    {
        public int CodigoErro { get; }

        public ResultException(int codigoErro, string mensagem)
            : base(mensagem)
        {
            CodigoErro = codigoErro;
        }
    }
}
