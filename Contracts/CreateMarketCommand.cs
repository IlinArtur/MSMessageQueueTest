using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CreateMarketCommand : ICommand
    {
        public int MatchId { get; set; }

        public Market Market { get; set; }
    }
}
