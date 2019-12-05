using System;

namespace Webapi.Exceptions
{
    public class PedidoException : Exception
    {
        public PedidoException(string mensagem): base(mensagem)
        {           
        }
    }
}