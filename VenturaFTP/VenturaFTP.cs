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
        public static readonly byte[] terminalBoy = { 4, 8, 16, 32, 64, 128, 47, 47, 47 };
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
                    TcpClient client = this.tcpListener.AcceptTcpClient();
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

                ASCIIEncoding encoder = new ASCIIEncoding();
                string filename = "empty";

                byte[] message = new byte[tcpClient.ReceiveBufferSize];
                int bytesRead;
                {   bytesRead = 0;
               
                {
                   
                    bytesRead = clientStream.Read(message, 0, tcpClient.ReceiveBufferSize);
                    string headerstring = encoder.GetString(message, 0, bytesRead);
                        int filepathlength = message[0];
                    filename = headerstring.Substring(1,filepathlength-1);
                }
                    Console.WriteLine("filename=" + filename);
            }
               
                while (true)
                {
                    using (System.IO.StreamWriter file =
             new System.IO.StreamWriter("C:\\Users\\Yoloswag\\Desktop\\stevenftp\\received\\" + filename, true))
                    {
                        


                        try
                        {
                            //blocks until a client sends a message
                            bytesRead = clientStream.Read(message, 0, tcpClient.ReceiveBufferSize);
                            file.Write(message);
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


                        if (message.Equals(StevenFileSendTelemetry.terminalBoy))
                        {
                            Console.WriteLine("Terminal string received -- Done reading file");
                            break;
                        }

                        //Console.WriteLine(encoder.GetString(message, 0, bytesRead));
                        if (bytesRead != 0)
                        {
                            break;
                        }
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
                    clientStream.Write(Encoding.ASCII.GetBytes(filePath), 0, filePath.Length);
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    
                    using (Stream f = new FileStream(filePath, FileMode.Open))
                    {
                        int offset = 0;
                        long len = f.Length;
                        

                        int readLen = client.SendBufferSize; // using chunks of 100 for default
                        byte[] buffer = new byte[readLen];
                        while (offset < len)
                        {
                            Console.WriteLine("offset= " + offset + ", len= " + len);
                            /*if (offset + readLen > len)
                            {
                                readLen = (int)len - offset;
                            }*/
                            offset += f.Read(buffer, 0, readLen);
                            clientStream.Write(buffer, 0, buffer.Length);
                        }


                    }
                    clientStream.Write(StevenFileSendTelemetry.terminalBoy, 0, StevenFileSendTelemetry.terminalBoy.Length);

                    
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
