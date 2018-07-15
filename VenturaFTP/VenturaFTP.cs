using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * [done]make 1 file work
 * []make bigger files work//https://stackoverflow.com/questions/11597295/how-do-i-use-file-readallbytes-in-chunks
 * []make directories work
 * */

namespace VenturaFTP
{
    class StevenFileSendTelemetry
    {
        public static string StringToSendFromFile(string filepath)
        {



            /*
            using (Stream f = new FileStream(path, FileMode.Open))
            {
                int offset = 0;
                long len = f.Length;
                byte[] buffer = new byte[len];

                int readLen = 100; // using chunks of 100 for default

                while (offset != len)
                {
                    if (offset + readLen > len)
                    {
                        readLen = (int)len - offset;
                    }
                    offset += f.Read(buffer, offset, readLen);
                }
            }*/



            string alllines = File.ReadAllText(filepath);
            return filepath + "ø" + alllines;
        }


    }
    class VenturaFTPClassName
    {
        const string receiveDirectory = "C:\\Users\\Yoloswag\\Desktop\\stevenftp\\received";
        public VenturaFTPClassName()
        {

        }
        public void SendStringToGuy()
        {



            /*Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("localhost");
            System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 8303);
            soc.Connect(remoteEP);
            //Start sending stuff..
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(please);
            soc.Send(byData);
            */

            Client c = new Client();
            c.DoClientThings("localhost", "C:\\Users\\Yoloswag\\Desktop\\stevenftp\\sendme.txt");
            


        }
        public void GetStringFromGuy()
        {
            Console.WriteLine("gettin da dstrings");
            Server s = new Server();
        }
        class Server
        {
            private TcpListener tcpListener;

            public Server()
            {
                try
                {
                    this.tcpListener = new TcpListener(IPAddress.Any, 80);
                    
                    ListenForClients();
                    
                }catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            public void ListenForClients()
            {
                Console.WriteLine("doublebeforeboi");
                this.tcpListener.Start();
                
                while (true)
                {
                    //blocks until a client has connected to the server
                    Console.WriteLine("beforeboii");
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    Console.WriteLine("found a client!");
                    //create a thread to handle communication 
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
            }
            private void HandleClientComm(object client)
            {
                TcpClient tcpClient = (TcpClient)client;
                NetworkStream clientStream = tcpClient.GetStream();

                byte[] message = new byte[4096];
                int bytesRead;

                while (true)
                {
                    bytesRead = 0;

                    try
                    {
                        //blocks until a client sends a message
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        //a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        break;
                    }

                    //message has successfully been received
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    Console.WriteLine(encoder.GetString(message, 0, bytesRead));
                    if (bytesRead != 0)
                    {
                        break;
                    }
                }
                TalkBackToClient(tcpClient);

                tcpClient.Close();
            }
            public void TalkBackToClient(TcpClient tcpClient)
            {
                //https://web.archive.org/web/20090720052829/http://www.switchonthecode.com/tutorials/csharp-tutorial-simple-threaded-tcp-server
                NetworkStream clientStream = tcpClient.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes("henlo");

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }
        }
        class Client
        {
            public Client()
            {

            }
            public void DoClientThings(string ipToConnectTo,string filePath)
            {
                try
                {
                    TcpClient client = new TcpClient();

                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.204"), 80);

                    client.Connect(serverEndPoint);
                    //https://stackoverflow.com/questions/2972600/no-connection-could-be-made-because-the-target-machine-actively-refused-it

                    NetworkStream clientStream = client.GetStream();

                    ASCIIEncoding encoder = new ASCIIEncoding();
                    string please = StevenFileSendTelemetry.StringToSendFromFile(filePath);
                    byte[] buffer = encoder.GetBytes(please);


                    clientStream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("it should have sent");
                    byte[] receiveBuffer = new byte[4096];
                    int bytesRead = clientStream.Read(receiveBuffer,0,4096);
                    Console.WriteLine("Recieved text: " + encoder.GetString(receiveBuffer, 0, bytesRead));

                    clientStream.Flush();
                    clientStream.Close();
                }catch(Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }
            }

        }

        /*
         * 
         * connect to the client
         * send the file over the socket
         * 
         * listen for connections on a socket
         * download the file
         * 
         * */



    }
}
