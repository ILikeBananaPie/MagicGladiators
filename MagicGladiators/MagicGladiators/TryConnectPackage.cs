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
        public ConnectionInfo connectionInfo { get; set; }

        [ProtoMember(1)]
        public bool connected { get; private set; }

        [ProtoMember(2)]
        private MemoryStream connInfo;

        private TryConnectPackage() { }

        public TryConnectPackage(bool connected, ConnectionInfo connectionInfo)
        {
            this.connected = connected;
            this.connectionInfo = connectionInfo;
        }

        [ProtoBeforeSerialization]
        private void Serialize()
        {
            connectionInfo.Serialize(connInfo);
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            connectionInfo.Deserialize(connInfo);
        }
    }
}
