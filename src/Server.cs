using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Logs from your program will appear here!");

TcpListener server = new (IPAddress.Any, 4221);
try
{
    server.Start();
    var socket = server.AcceptSocket();
    var responseBuffer = new byte[1024];
    var received = socket.ReceiveAsync(responseBuffer);
    var response = Encoding.UTF8.GetString(responseBuffer, 0, await received);
    var parseResponse = response.Split("\r\n");
    var str = string.Empty;
    if (parseResponse[0] == "GET / HTTP/1.1")
    {
        socket.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));
    } else if (parseResponse[0] == $"GET /echo/{str} HTTP/1.1"){
        socket.Send(Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: 3\r\n\r\n{str}"));
    }
    else {
        socket.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
    }
}
finally
{
    server.Stop();
}