using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class Player
    {
        public int playerIndex { get; set; }
        public NetConnection connectionID { get; set; }

        public Player(int index, NetConnection con)
        {
            playerIndex = index;
            connectionID = con;
        }
    }
}
