using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Domain.Models;

namespace Transfer.Services.Contracts
{
    public interface IErrorsService
    {
        Errors Find(int? id);
    }
}
