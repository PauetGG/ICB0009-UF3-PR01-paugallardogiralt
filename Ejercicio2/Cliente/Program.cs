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
                // Esperamos recibir la dirección desde el servidor
                string direccionRecibida = NetworkStreamClass.LeerMensajeNetworkStream(stream);
                Console.WriteLine($"[Cliente] Dirección recibida del servidor: {direccionRecibida}");
                // Creamos y enviarmos el vehiculo al servidor
                Vehiculo miVehiculo = new Vehiculo();
                miVehiculo.Id = int.Parse(idRecibido);
                miVehiculo.Direccion = direccionRecibida;
                Console.WriteLine($"[Cliente] Vehiculo creado. ID: {miVehiculo.Id}, Dirección: {miVehiculo.Direccion}");
                NetworkStreamClass.EscribirDatosVehiculoNS(stream, miVehiculo);
                Console.WriteLine("[Cliente] Vehiculo enviado al servidor.");
                // Creamos un hilo para llamar al método EscucharCarretera
                Thread hiloRecepcion = new Thread(() => EscucharCarretera(stream));
                hiloRecepcion.Start();
                // Simulamos el avance del vehiculo
                while (miVehiculo.Pos < 100)
                {
                    Thread.Sleep(miVehiculo.Velocidad); 
                    miVehiculo.Pos++; 
                    Console.WriteLine($"[Cliente] Vehiculo {miVehiculo.Id} avanzando. Nueva posición: {miVehiculo.Pos} km");

                    NetworkStreamClass.EscribirDatosVehiculoNS(stream, miVehiculo);
                    Console.WriteLine($"[Cliente] Vehiculo {miVehiculo.Id} actualizado enviado al servidor.");
                }
                Console.WriteLine($"[Cliente] Vehiculo {miVehiculo.Id} ha llegado al final de la carretera.");
                miVehiculo.Acabado = true;
                NetworkStreamClass.EscribirDatosVehiculoNS(stream, miVehiculo);
                Console.WriteLine($"[Cliente] Vehiculo {miVehiculo.Id} finalizado enviado al servidor.");
                // Esperamos medio segundo para asegurar que la información llega al servidor
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cliente] Error al conectar: {ex.Message}");
            }

            Console.ReadLine(); // Mantenemos el cliente abierto
        }
        static void EscucharCarretera(NetworkStream stream)
        {
            while (true)
            {
                try
                {
                    Carretera carreteraRecibida = NetworkStreamClass.LeerDatosCarreteraNS(stream);

                    Console.Clear(); // Vamos limpiando la pantalla para no acumular información
                    Console.WriteLine("[Cliente] Estado de la carretera actualizado:");

                    foreach (Vehiculo v in carreteraRecibida.VehiculosEnCarretera)
                    {
                        string estado = v.Acabado ? "🚩 Finalizado" : "⏩ Circulando";
                        Console.WriteLine($"[Cliente] Vehiculo {v.Id} - {v.Pos} km - Dirección {v.Direccion} - {estado}");
                    }

                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Cliente] Error al recibir datos del servidor: {ex.Message}");
                    break; 
                }
            }
        }
    }
}