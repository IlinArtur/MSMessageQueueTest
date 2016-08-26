using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts;

namespace MSMessageQueueReciever
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

            msgQ.ReceiveCompleted += MsgQ_ReceiveCompleted;
            msgQ.BeginReceive();

            //string input = Console.ReadLine();
            //while (!string.IsNullOrEmpty(input))
            //{
            //    Market market = new Market
            //    {
            //        Id = 1,
            //        Name = "Market",
            //        TradingStatus = MarketStatus.Trading,
            //        MarketType = new MarketType { Id = 2, Name = "Market Type" },
            //        Selections = new List<Selection>(),
            //    };

            //    market.Selections.Add(new Selection { Id = 3, Name = "Home", Kef = 1.9m });

            //    ICommand command = new CreateMarketCommand
            //    {
            //        Market = market,
            //        MatchId = 4,
            //    };


            //    using(new TransactionScope())
            //        for (int i = 0; i < 4; i++)
            //        {
            //            if (i == 3) throw new NotImplementedException();
            //            msgQ.Send(command);
            //        }

            //    input = Console.ReadLine();
            //}
            Console.Read();
        }

        private static void MsgQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            MessageQueue mq = (MessageQueue) sender;
            Message message = mq.EndReceive(e.AsyncResult);
            message.Formatter = new XmlMessageFormatter(new [] {typeof(CreateMarketCommand)});

            CreateMarketCommand command = (CreateMarketCommand) message.Body;

            Debug.Assert(command.MatchId == 4, $"Match Id expected 4 but was {command.MatchId}");
            Debug.Assert(command.Market != null, "Market expected but was null");

            Market market = command.Market;
            Console.WriteLine($"Message: {market.Id} {market.Name} {market.TradingStatus} || {market.MarketType.Id} {market.MarketType.Name} || {market.Selections[0].Kef} {market.Selections[0].Name}");
            Thread.Sleep(1500);
            mq.BeginReceive();
        }

    }
}
