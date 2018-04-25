using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApp.SignalRHub
{
    public class WolloHub : Hub
    {
        //for testing purpose
        public void Hello()
        {
            Clients.All.hello();
        }

        
        public void Buy(string security, string numberofshares, string price)
        {
            // Call the updateBid method to update Bid prices.
            Clients.All.updateBid(security, numberofshares, price);
        }
        public void Sell(string security, string numberofshares, string price)
        {
            // Call the updateAsk method to update Ask Prices.
            Clients.All.updateAsk(security, numberofshares, price);
        }

        //end testing purpose

    }
}