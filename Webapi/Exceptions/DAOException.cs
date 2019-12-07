using System;

namespace Webapi.Exceptions
{
    public class DAOException: Exception
    {
        public DAOException(string mensagem): base(mensagem)
        {           
        }
    }
}