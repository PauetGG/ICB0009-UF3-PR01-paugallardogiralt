using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipServidor = "127.0.0.1"; 
            int puerto = 5000;

            try
            {
                Console.WriteLine("[Cliente] Intentando conectar al servidor...");
                TcpClient cliente = new TcpClient();
                cliente.Connect(ipServidor, puerto);
                Console.WriteLine("[Cliente] Conectado al servidor correctamente.");
                NetworkStream stream = cliente.GetStream();
                Console.WriteLine("[Cliente] NetworkStream obtenido para la conexión.");
                // Handshake
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, "INICIO");
                Console.WriteLine("[Cliente] Enviado INICIO al servidor.");
                // Esperamos recibir el ID del servidor
                string idRecibido = NetworkStreamClass.LeerMensajeNetworkStream(stream);
                Console.WriteLine($"[Cliente] ID recibido del servidor: {idRecibido}");
                // Enviamos de vuelta el mismo ID como confirmación
                NetworkStreamClass.EscribirMensajeNetworkStream(stream, idRecibido);
                Console.WriteLine("[Cliente] ID reenviado al servidor como confirmación.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cliente] Error al conectar: {ex.Message}");
            }

            Console.ReadLine(); // Mantenemos el cliente abierto
        }
    }
}