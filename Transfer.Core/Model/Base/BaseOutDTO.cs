using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Core.Model.Base
{
    public abstract class BaseOutDTO<T>
    {
        public T Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDatePersian { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string LastModificationDatePersian { get; set; }
        public virtual bool Disabled { get; set; }
    }
}
