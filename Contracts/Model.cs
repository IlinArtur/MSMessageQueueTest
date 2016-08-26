using System.Collections.Generic;

namespace Contracts
{
    public class Market : Entity
    {
        public MarketStatus TradingStatus{ get; set; }

        public MarketType MarketType { get; set; }

        public List<Selection> Selections { get; set; }
    }

    public enum MarketStatus
    {
        Trading,
        Closed,
        Suspended
    }

    public class Selection : Entity
    {
        public decimal Kef { get; set; }
    }

    public class MarketType : Entity
    {

    }

    public class Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
