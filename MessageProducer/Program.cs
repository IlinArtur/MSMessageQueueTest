using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Contracts;

namespace MessageProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string QUEUE_NAME = ".\\SimpleTestQueue";
            MessageQueue msgQ;
            if (!MessageQueue.Exists(QUEUE_NAME))
            {
                msgQ = MessageQueue.Create(QUEUE_NAME);
            }
            else
            {
                msgQ = new MessageQueue(QUEUE_NAME);
            }

            string input = Console.ReadLine();
            while (!string.IsNullOrEmpty(input))
            {
                Market market = new Market
                {
                    Id = 1,
                    Name = "Market",
                    TradingStatus = MarketStatus.Trading,
                    MarketType = new MarketType {Id = 2, Name = "Market Type"},
                    Selections = new List<Selection>(),
                };

                market.Selections.Add(new Selection { Id = 3, Name = "Home", Kef = 1.9m});

                CreateMarketCommand command = new CreateMarketCommand
                {
                    Market = market,
                    MatchId = 4,
                };

                MessageQueueTransaction tran = new MessageQueueTransaction();
                tran.Begin();
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        msgQ.Send(command, tran);
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Abort();
                    throw;
                }
                finally
                {
                    tran?.Dispose();
                }
                input = Console.ReadLine();
            }
        }
    }
}
