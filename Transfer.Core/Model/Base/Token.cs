using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Core.Model.Base
{
    public class Token
    {
        public string AccessToken { get; set; }
        public Token(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
