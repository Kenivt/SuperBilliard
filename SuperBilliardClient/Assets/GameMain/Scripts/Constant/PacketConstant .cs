using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SuperBilliard.Constant
{
    public static class PacketConstant
    {
        public const int BufferSize = 1024 * 8;
        public const int SendMemoryStreamSize = 1024 * 8;
        public const int PacketFlagIndex = 0;
        public const int PacketIdIndex = 1;
        public const int PacketLengthIndex = 3;
        public const int PacketHeaderLength = 5;
    }
}
