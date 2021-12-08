using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class IbanInfo
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Cif { get; set; }
        public string DspositNumber { get; set; }
        public string Code { get; set; }
        public string ForeignName { get; set; }
        public string Gender { get; set; }
    }
}
