using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using OrderMonitor.Server;
using GrpcClient;

namespace GrpcClient
{
    public class Class1
    {
        private static GrpcChannel channel;
        private static OrderMonitor.OrderMonitorClient client;        
        public static async IAsyncEnumerable<OrderModel> GetStreamOrders()
        {
            channel = GrpcChannel.ForAddress("https://localhost:5001");
           
            ClientInfo info = new ClientInfo()
            {
                UserName = "Rohit Kumar"
            };
        
            client = new OrderMonitor.OrderMonitorClient(channel);
            using (var call = client.GetOrderStream(info))
            {
                while (await call.ResponseStream.MoveNext().ConfigureAwait(false))
                {
                    var order = call.ResponseStream.Current;

                    yield return order;
                }
            }

            
        }
    }
}