using Abc_Exchange_Client.DTO;
using Abc_Exchange_Client.Interface;
using Abc_Exchange_Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Abc_Exchange_Client.Repository
{
    public class StreamRepository : IStreamRepository
    {
        public async Task<List<AbxPacketDto>> StreamAllPackets()
        {
            var packets = new List<AbxPacketDto>();

            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(Constants.Host, Constants.Port);

                var stream = client.GetStream();
                await stream.WriteAsync(new byte[] { 1, 0 });

                var buffer = new byte[Constants.PacketSize];
                int bytesRead;
                while ((bytesRead = await ReadExactlyAsync(stream, buffer, Constants.PacketSize)) == Constants.PacketSize)
                {
                    var packet = ParsePacket(buffer);
                    packets.Add(packet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while streaming packets: {ex.Message}");
            }

            return packets;
        }

        public async Task<AbxPacketDto?> RequestResendPacket(int sequenceNumber)
        {
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(Constants.Host, Constants.Port);

                var stream = client.GetStream();
                await stream.WriteAsync(new byte[] { 2, (byte)sequenceNumber });

                var buffer = new byte[Constants.PacketSize];
                int bytesRead = await ReadExactlyAsync(stream, buffer, Constants.PacketSize);
                if (bytesRead == Constants.PacketSize)
                {
                    return ParsePacket(buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while requesting resend for packet {sequenceNumber}: {ex.Message}");
            }

            return null;
        }
        public async Task<int> ReadExactlyAsync(NetworkStream stream, byte[] buffer, int count)
        {
            int total = 0;
            try
            {
                while (total < count)
                {
                    int read = await stream.ReadAsync(buffer, total, count - total);
                    if (read == 0) break; // connection closed
                    total += read;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading from stream: {ex.Message}");
            }
            return total;
        }

        public AbxPacketDto ParsePacket(byte[] buffer)
        {
            var symbol = System.Text.Encoding.ASCII.GetString(buffer, 0, 4);
            var side = ((char)buffer[4]).ToString();
            var quantity = BitConverter.ToInt32(buffer[5..9].Reverse().ToArray(), 0);
            var price = BitConverter.ToInt32(buffer[9..13].Reverse().ToArray(), 0);
            var sequence = BitConverter.ToInt32(buffer[13..17].Reverse().ToArray(), 0);

            return new AbxPacketDto
            {
                Symbol = symbol,
                Side = side,
                Quantity = quantity,
                Price = price,
                Sequence = sequence
            };
        }
    }
}
