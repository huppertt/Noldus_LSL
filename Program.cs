using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

using LSL;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        bool result;
        var mutex = new System.Threading.Mutex(true, "UniqueAppId", out result);

        if (!result)
        {
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"),11000); // endpoint where server is listening
            client.Connect(ep);

            // send data
            if (args.Length > 0)
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(args[0]);
                client.Send(sendBytes, sendBytes.Length);
            }
            else
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes("1");
                client.Send(sendBytes, sendBytes.Length);
            }
             
            Console.WriteLine("Sending UDP message to other instance.");
            return;
        }
        
        Console.WriteLine("New instance");
        GC.KeepAlive(mutex);                // mutex shouldn't be released - important line
        UdpClient udpServer = new UdpClient(11000);

        StreamInfo streamInfo = new StreamInfo("Trigger", "Markers");
        StreamOutlet streamOutlet = new StreamOutlet(streamInfo);

        while (true)
        {
            var remoteEP = new IPEndPoint(IPAddress.Any, 11000);
            Byte[] data = udpServer.Receive(ref remoteEP); // listen on port 11000
            string res = Encoding.UTF8.GetString(data);
            Console.Write("receive data from " + remoteEP.ToString());
            Console.Write("\t" + res +"\n");

            if (res == "stop")
            {
                streamOutlet.Close();
                Console.Write("Exiting\n");
                break;
            }else
            {
                string[] sample = new string[1];
                sample[0] = res;
                streamOutlet.push_sample(sample);
            }

        }
    }
}
