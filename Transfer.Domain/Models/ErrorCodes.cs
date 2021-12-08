using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class ErrorCodes
    {
        public ErrorCodes()
        {
            Errors = new HashSet<Errors>();
        }

        public int Id { get; set; }
        public int Value { get; set; }
        public string FaDescription { get; set; }
        public string EnDescription { get; set; }
        public string ExtraData { get; set; }

        public virtual ICollection<Errors> Errors { get; set; }
    }
}
