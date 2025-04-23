# ABX Exchange Client

> _Note: I'm not great at writing README files, but I'm giving it my best shot!_

This is a simple C# console application that connects to an ABX mock exchange TCP server, reads stock ticker packets, checks for missing packets by sequence number, and saves the cleaned and sorted output in JSON format.

---

## 📁 Folder Structure

Abc_Exchange_Client/ 
├── DTO/ # Data Transfer Objects (e.g., AbxPacketDto) 
├── Interface/ # Interfaces for repository abstraction 
├── Repository/ # Logic to connect to the TCP server and parse packets 
├── Utils/ # Constants like host, port, and packet size 
├── output/ # Folder where the output JSON file is saved


---

## ✅ Features

- Connects to a TCP stock exchange mock server (`127.0.0.1:3000`)
- Streams and parses binary packet data
- Detects and resends missing packets
- Outputs cleaned data to `output/output.json`
- Uses async/await for smooth networking

---

## 🚀 How to Run

1. Make sure the server is running at `127.0.0.1:3000`.
2. Clone or download this repo.
3. Build and run the project in Visual Studio or using the .NET CLI:

```bash
dotnet build
dotnet run

