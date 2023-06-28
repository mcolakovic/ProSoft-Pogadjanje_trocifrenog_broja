using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using Domain;
using Repository;

namespace Server
{
    public class ClientHandler
    {
        Socket socket;
        List<ClientHandler> clients;
        List<int?> polja;
        CommunicationHelper helper;
        public event EventHandler OdjavljenKorisnik;
        public ClientHandler(Socket socket, List<ClientHandler> clients, List<int?> polja)
        {
            this.socket = socket;
            this.clients = clients;
            this.polja = polja;
            helper = new CommunicationHelper(socket);
        }

        internal void Stop()
        {
            if(socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Dispose();
                socket = null;
            }
            OdjavljenKorisnik?.Invoke(this, EventArgs.Empty);
        }

        internal void HandleRequests()
        {
            try
            {
                Request request;
                while((request = helper.Receive<Request>()).Operations != Operations.EndCommunication)
                {
                    Response response;
                    try
                    {
                        response = CreateResponse(request);
                    }
                    catch (Exception ex)
                    {
                        response = new Response
                        {
                            IsSuccessful = false,
                            MessageText = ex.Message
                        };
                    }
                    helper.Send(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Stop();
            }
        }

        private Response CreateResponse(Request request)
        {
            Response response = new Response();
            switch (request.Operations)
            {
                case Operations.Igra:
                    response = VratiIgru((Igra)request.RequestObject);
                    break;
                case Operations.VratiRezultat:
                    response = VratiRezultat();
                    break;
            }
            return response;
        }

        private Response VratiRezultat()
        {
            return new Response
            {
                IsSuccessful = true,
                ResponseObject = new Igra
                {
                    Polja = polja
                }
            };
        }

        private Response VratiIgru(Igra igra)
        {
            return new Response
            {
                IsSuccessful = true,
                ResponseObject = new Igra
                {
                    Pozicija = igra.Pozicija,
                    Vrijednost = polja[igra.Pozicija] ?? -1
                }
            };
        }
    }
}
