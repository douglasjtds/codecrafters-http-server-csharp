using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = new TcpListener(IPAddress.Any, 4221);
server.Start();
while (true)
{
    TcpClient client = server.AcceptTcpClient();
    NetworkStream stream = client.GetStream();
    byte[] buffer = new byte[1024];
    int bytesRead = stream.Read(buffer, 0, buffer.Length);
    string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    string[] lines = request.Split(new string[] { "\r\n" }, StringSplitOptions.None);
    string[] startLineParts = lines[0].Split(' ');
    string response;
    if (startLineParts[1] == "/")
    {
        response = "HTTP/1.1 200 OK\r\n\r\n";
    }
    else if (startLineParts[1].StartsWith("/echo/"))
    {
        string message = startLineParts[1].Substring(6);
        response =
            $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: {message.Length}\r\n\r\n{message}";
    }
    else
    {
        response = "HTTP/1.1 404 Not Found\r\n\r\n";
    }
    byte[] responseBytes = Encoding.ASCII.GetBytes(response);
    stream.Write(responseBytes, 0, responseBytes.Length);
    client.Close();
}