using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount
{
    public class BankAccount
    {
        public string owner { get; set; }
        public Guid accountNumber { get; set; }
        public decimal balance { get; set; }

    }
}