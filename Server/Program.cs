using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using Nore.Server;
using Norr.CommonLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {

        /// <summary>
        /// This object is used to host Chat Service on a SCS server.
        /// </summary>
        private static IScsServiceApplication _serviceApplication;

        /// <summary>
        /// Chat Service object that serves clients.
        /// </summary>
        private static MsgService _msgService;

        static void Main(string[] args)
        {
            int port;
            try {
                port = 9000; // Hard code test
                if (port <= 0 || port > 65536) {
                    throw new Exception(port + " is not a valid TCP port number.");
                }
            } catch (Exception ex) {
                Console.WriteLine("TCP port must be a positive number. Exception detail: " + ex.Message);
                return;
            }

            try {
                _serviceApplication = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(port));
                _msgService = new MsgService();
                _serviceApplication.AddService<IMsgService, MsgService>(_msgService);
                _msgService.UserListChanged += msgService_UserListChanged;
                _serviceApplication.Start();
            } catch (Exception ex) {
                Console.WriteLine("Service can not be started. Exception detail: " + ex.Message);
                return;
            }

        }

        private static void msgService_UserListChanged(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}
