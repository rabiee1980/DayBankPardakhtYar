using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class TurnoverType
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Title { get; set; }
    }
}
