using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Common;
using System.Net.Sockets;
using System.Diagnostics;

namespace KorisnickiInterfejs
{
    public class Communication
    {
        Socket socket;
        CommunicationHelper helper;
        private static Communication instance;
        private Communication()
        {
        }
        public static Communication Instance
        {
            get
            {
                if (instance == null)
                    instance = new Communication();
                return instance;
            }
        }

        public void Connect()
        {
            try
            {
                if (socket == null || !socket.Connected)
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect("127.0.0.1", 9999);
                    helper = new CommunicationHelper(socket);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void CloseConnectio()
        {
            try
            {
                Request request = new Request
                {
                    Operations = Operations.EndCommunication
                };
                helper.Send(request);
                socket.Shutdown(SocketShutdown.Both);
                socket.Dispose();
                socket = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public T SendRequests<T>(Request request) where T : class
        {
            try
            {
                helper.Send(request);
                Response response = helper.Receive<Response>();
                if (response.IsSuccessful)
                {
                    return (T)response.ResponseObject;
                }
                else
                {
                    throw new Exception(response.MessageText);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
