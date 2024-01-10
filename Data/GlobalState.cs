using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCafe.Data
{
    internal class GlobalState
    {
        public User CurrentUser { get; set; }
        public Coffee UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsStaff { get; set; }
        public Role UserRole { get; set; }
        public string? AlertMessageType { get; set; }
        public string? AlertMessageText { get; set; }
    }
}
