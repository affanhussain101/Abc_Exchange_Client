using Abc_Exchange_Client;
using Abc_Exchange_Client.DTO;
using Abc_Exchange_Client.Interface;
using Abc_Exchange_Client.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var streamRepository = new StreamRepository();
        var program = new Program(streamRepository);
        await program.Run();
    }
    private readonly IStreamRepository _streamRepository;

    public Program(IStreamRepository streamRepository)
    {
        _streamRepository = streamRepository;
    }
    public async Task Run()
    {

        var packets = await _streamRepository.StreamAllPackets();

        // Find missing sequences
        var sequenceSet = new HashSet<int>();
        int maxSeq = int.MinValue;
        foreach (var packet in packets)
        {
            sequenceSet.Add(packet.Sequence);
            if (packet.Sequence > maxSeq)
                maxSeq = packet.Sequence;
        }

        for (int i = 1; i <= maxSeq; i++)
        {
            if (!sequenceSet.Contains(i))
            {
                Console.WriteLine($"Missing packet: {i}, requesting...");
                var resendPacket = await _streamRepository.RequestResendPacket(i);
                if (resendPacket != null)
                    packets.Add(resendPacket);
            }
        }

        packets.Sort((a, b) => a.Sequence.CompareTo(b.Sequence));

        // Output JSON
        var json = JsonSerializer.Serialize(packets, new JsonSerializerOptions { WriteIndented = true });
        var path = Path.Combine(Directory.GetCurrentDirectory(), "output.json");
        await File.WriteAllTextAsync(path, json);
        Console.WriteLine($"Output written to: {path}");

        Console.WriteLine("Done. Output written to output.json");
    }

}
