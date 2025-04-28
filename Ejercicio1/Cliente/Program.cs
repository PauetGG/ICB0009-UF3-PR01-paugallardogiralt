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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cliente] Error al conectar: {ex.Message}");
            }

            Console.ReadLine(); // Mantenemos el cliente abierto
        }
    }
}