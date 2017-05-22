using NetworkCommsDotNet;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    [ProtoContract]
    class TryConnectPackage
    {
        [ProtoMember(1)]
        public bool connected { get; private set; }

        [ProtoMember(2)]
        public string connectionInfo { get; private set; }

        private TryConnectPackage() { }

        public TryConnectPackage(bool connected, ConnectionInfo connectionInfo)
        {
            this.connected = connected;
            this.connectionInfo = connectionInfo.ToString();
        }

        [ProtoBeforeSerialization]
        private void Serialize()
        {
            
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            
        }
    }
}
