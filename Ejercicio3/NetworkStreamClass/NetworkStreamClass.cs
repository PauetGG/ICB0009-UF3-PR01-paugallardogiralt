using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using VehiculoClass;
using CarreteraClass;


namespace NetworkStreamNS
{
    public class NetworkStreamClass
    {
        
        //Método para escribir en un NetworkStream los datos de tipo Carretera
        public static void  EscribirDatosCarreteraNS(NetworkStream NS, Carretera C)
        {            
            byte[] bytesCarretera = C.CarreteraABytes(); 

            byte[] longitud = BitConverter.GetBytes(bytesCarretera.Length);
            NS.Write(longitud, 0, longitud.Length);

            NS.Write(bytesCarretera, 0, bytesCarretera.Length);               
        }

        //Metódo para leer de un NetworkStream los datos que de un objeto Carretera
        public static Carretera LeerDatosCarreteraNS (NetworkStream NS)
        {
            byte[] longitudBytes = new byte[4];
            NS.Read(longitudBytes, 0, 4);
            int longitud = BitConverter.ToInt32(longitudBytes, 0);

            byte[] datosCarretera = new byte[longitud];
            int totalLeidos = 0;

            while (totalLeidos < longitud)
            {
                int leidos = NS.Read(datosCarretera, totalLeidos, longitud - totalLeidos);
                if (leidos == 0) break;
                totalLeidos += leidos;
            }
            Carretera carretera = Carretera.BytesACarretera(datosCarretera);
            return carretera;
        }

        //Método para enviar datos de tipo Vehiculo en un NetworkStream
        public static void  EscribirDatosVehiculoNS(NetworkStream NS, Vehiculo V)
        {            
            byte[] bytesVehiculo = V.VehiculoaBytes(); 

            byte[] longitud = BitConverter.GetBytes(bytesVehiculo.Length);
            NS.Write(longitud, 0, longitud.Length);

            NS.Write(bytesVehiculo, 0, bytesVehiculo.Length);              
        }

        //Metódo para leer de un NetworkStream los datos que de un objeto Vehiculo
        public static Vehiculo LeerDatosVehiculoNS (NetworkStream NS)
        {
            byte[] longitudBytes = new byte[4];
            NS.Read(longitudBytes, 0, 4);
            int longitud = BitConverter.ToInt32(longitudBytes, 0);

            byte[] datosVehiculo = new byte[longitud];
            int totalLeidos = 0;

            while (totalLeidos < longitud)
            {
                int leidos = NS.Read(datosVehiculo, totalLeidos, longitud - totalLeidos);
                if (leidos == 0) break;
                totalLeidos += leidos;
            }

            Vehiculo vehiculo = Vehiculo.BytesAVehiculo(datosVehiculo);
            return vehiculo;
        }

        //Método que permite leer un mensaje de tipo texto (string) de un NetworkStream
        public static string LeerMensajeNetworkStream (NetworkStream NS)
        {
            byte[] bufferLectura = new byte[1024];

            //Lectura del mensaje
            int bytesLeidos = 0;
            var tmpStream = new MemoryStream();
            byte[] bytesTotales; 
            do
            {
                int bytesLectura = NS.Read(bufferLectura,0,bufferLectura.Length);
                tmpStream.Write(bufferLectura, 0, bytesLectura);
                bytesLeidos = bytesLeidos + bytesLectura;
            }while (NS.DataAvailable);

            bytesTotales = tmpStream.ToArray();            

            return Encoding.Unicode.GetString(bytesTotales, 0, bytesLeidos);                 
        }

        //Método que permite escribir un mensaje de tipo texto (string) al NetworkStream
        public static void  EscribirMensajeNetworkStream(NetworkStream NS, string Str)
        {            
            byte[] MensajeBytes = Encoding.Unicode.GetBytes(Str);
            NS.Write(MensajeBytes,0,MensajeBytes.Length);                        
        }                          

    }
}
