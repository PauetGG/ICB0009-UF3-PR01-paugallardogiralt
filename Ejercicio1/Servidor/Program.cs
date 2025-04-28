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
        static int contadorVehiculos = 0;
        static readonly object lockObject = new object(); 

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

                // Creamos  un nuevo hilo para gestionar cada cliente
                Thread clientThread = new Thread(() => GestionarCliente(cliente));
                clientThread.Start();
            }
        }

        static void GestionarCliente(TcpClient cliente)
        {
            int id;
            string direccion;

            lock (lockObject)
            {
                contadorVehiculos++;
                id = contadorVehiculos;
            }

            // Asignamos una dirección aleatoria
            direccion = AsignarDireccionAleatoria();

            Console.WriteLine($"[Servidor] Vehículo ID: {id} Dirección: {direccion}");

            // Creamos el Vehiculo con su id y dirección 
            VehiculoClass.Vehiculo vehiculo = new VehiculoClass.Vehiculo();
            vehiculo.Id = id;
            vehiculo.Direccion = direccion;
        }


        static string AsignarDireccionAleatoria()
        {
            Random random = new Random();
            int valor = random.Next(0, 2);
            return valor == 0 ? "norte" : "sur";
        }
    }
}
