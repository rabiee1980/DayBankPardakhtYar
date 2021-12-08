using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
