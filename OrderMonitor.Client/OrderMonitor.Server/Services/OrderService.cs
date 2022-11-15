using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMonitor.Server
{
    public class OrderService : OrderMonitor.OrderMonitorBase
    {
        private readonly ILogger<OrderService> _logger;
        private List<string> symbols;
        private List<string> clients;
        private Dictionary<string, double> livePrice;
        private List<string> side;
        private readonly static Random rand = new Random();
        private static List<Client> lstClient = new List<Client>();
        private static Dictionary<Guid, IServerStreamWriter<OrderModel>> responsStreams = new Dictionary<Guid, IServerStreamWriter<OrderModel>>();
        private static long orderId = 1;
        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;           
            symbols = new List<string>();
            for(int i = 1; i < 11; i++)
            {
                StringBuilder symbol = new StringBuilder(i.ToString() + ".HK");
                symbols.Add(symbol.ToString());                
            }
            clients = new List<string>();
            for(int i = 1; i < 11; i++)
            {
                StringBuilder client = new StringBuilder("Client" + i.ToString());
                clients.Add(client.ToString());
            }        
            side = new List<string>()
            {
                "Buy",
                "Sell"
            };
            livePrice = new Dictionary<string, double>();
            var enumerator = symbols.GetEnumerator();
            while(enumerator.MoveNext())
            {
                livePrice.Add(enumerator.Current, rand.Next(100, 200000) / 100);
            }
          
        }
        public override async Task GetOrderStream(ClientInfo info, IServerStreamWriter<OrderModel> responseStream, ServerCallContext context)
        {
            Client client = new Client();

            client.UserName = info.UserName;

            // save the responsestream of each client and respond back to the client
            responsStreams.Add(client.CID, responseStream);
            
            lstClient.Add(client);
            int updateLivePrice = 0;
            for (int i = 0; i < 30; i++)
            {        
                if(updateLivePrice == 5)
                {
                    UpdateLivePrice();
                    updateLivePrice = 0;
                }
                var order = CreateOrder();
                Guid clientConnected = client.CID;
                
                if (!responsStreams.ContainsKey(clientConnected))
                {
                    lstClient.Add(client);
                }
                foreach (var connectedClient in lstClient)
                {
                    IServerStreamWriter<OrderModel> rStream = responsStreams[connectedClient.CID];
                    await rStream.WriteAsync(order);
                }
                updateLivePrice++;
                await Task.Delay(1000);                
            }           
        }       

        private OrderModel CreateOrder()
        {            
            Random r = new Random();
            OrderModel model = new OrderModel();
            model.OrderId = "Ord" + orderId.ToString();
            model.Symbol = symbols[r.Next(0, 10)];
            model.Side = side[r.Next(0, 2)];
            model.Client = clients[r.Next(0, 10)];
            model.LivePrice = livePrice[model.Symbol];
            model.Qty = r.Next(1, 2001);
            model.Total = model.LivePrice * model.Qty;

            orderId++;
            return model;
        }
        /// <summary>
        /// Updates live price for a symbol.
        /// </summary>
        /// <returns></returns>
        private void UpdateLivePrice()
        {
            string symbol = symbols[rand.Next(0, symbols.Count)];
            livePrice[symbol] = rand.Next(100, 200000) / 100;           
        }
       
    }
}
