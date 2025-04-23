using Abc_Exchange_Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Abc_Exchange_Client.Interface
{
    public interface IStreamRepository
    {
        Task<List<AbxPacketDto>> StreamAllPackets();
        Task<AbxPacketDto?> RequestResendPacket(int sequenceNumber);
        Task<int> ReadExactlyAsync(NetworkStream stream, byte[] buffer, int count);
        AbxPacketDto ParsePacket(byte[] buffer);
    }
}
