using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Servidor
{
    class Program
    {
        static void Main(string[] args)
        {
            int puerto = 5000;
            TcpListener servidor = new TcpListener(IPAddress.Any, puerto);

            servidor.Start();
            Console.WriteLine($"[Servidor] Servidor iniciado en el puerto {puerto}");

            while (true)
            {
                Console.WriteLine("[Servidor] Esperando conexión de un cliente...");
                TcpClient cliente = servidor.AcceptTcpClient();
                Console.WriteLine($"[Servidor] Cliente conectado desde {cliente.Client.RemoteEndPoint}");

                // Creamos un nuevo hilo para gestionar cada cliente
                Thread clientThread = new Thread(() => GestionarCliente(cliente));
                clientThread.Start();
            }
        }

        static void GestionarCliente(TcpClient cliente)
        {
            Console.WriteLine("[Servidor] Gestionando nuevo vehículo...");

        }
    }
}