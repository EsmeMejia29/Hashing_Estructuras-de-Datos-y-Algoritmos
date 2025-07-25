using System;

namespace DemoVotacionRapida
{
    // Clase que tiene la estructura del voto (ID y opcion escogida)
    class Voto
    {
        public int ID { get; set; }
        public string Opcion { get; set; }

        // Constructor de la clase Voto
        public Voto(int id, string opcion)
        {
            ID = id;
            Opcion = opcion;
        }
    }

    // Clase tiene toda la logica  del sistema. Usa tabla hash, con resolucion de colisiones nediante el hashing cerrado y su metodo de hashing doble
    class SistemaVotos
    {
        private Voto[] tabla; //Tabla hash
        private int tamanio;
        private Voto eliminado = new Voto(-1, "ELIMINADO"); // Marca para espacios donde se eliminan votos

        // Constructor que inicializa la tabla 
        public SistemaVotos(int capacidad)
        {
            tamanio = capacidad;
            tabla = new Voto[tamanio];
        }

        // Primera funcion hash: calcula una posición en la tabla hash a partir del ID
        private int funcion_hash_primaria(int llave) => llave % tamanio;

        // Segunda funcion hash: se usa para el desplazamiento por la tabla hash por si hay colisiones entre hashes
        private int funcion_hash_secundaria(int llave) => 1 + (llave % (tamanio - 1));

        public bool InsertarVoto(int idVotante, string opcion)
        {
            // Método para insertar votos

            int h1 = funcion_hash_primaria(idVotante);
            int h2 = funcion_hash_secundaria(idVotante);
            int indice = h1;

            // Insertar usando doble hashing
            for (int i = 0; i < tamanio; i++)
            {
                if (tabla[indice] == null || tabla[indice] == eliminado)
                {
                    tabla[indice] = new Voto(idVotante, opcion);
                    Console.WriteLine($"Insertado en índice {indice} usando (i={i})");
                    return true;
                }

                if (tabla[indice].ID == idVotante)
                {
                    Console.WriteLine("Este ID ya ha votado.");
                    return false;
                }

                // Calcula nuevo índice usando doble hashing
                indice = (h1 + i * h2) % tamanio;
            }

            Console.WriteLine("No se puede insertar: tabla llena.");
            return false;
        }

        // Metodo para obtener el voto registrado por un ID dado
        public string ObtenerVoto(int idVotante)
        {
            int h1 = funcion_hash_primaria(idVotante);
            int h2 = funcion_hash_secundaria(idVotante);
            int index = h1;

            for (int i = 0; i < tamanio; i++)
            {
                if (tabla[index] == null) // Si encuentra un espacio vacío, el ID no está
                    return null;
                if (tabla[index].ID == idVotante)
                    return tabla[index].Opcion;

                // Continua el doble hashing
                index = (h1 + i * h2) % tamanio;
            }
            return null;
        }

        public bool EliminarVoto(int idVotante)
        {
            // Método para eliminar un voto dado su ID

            int h1 = funcion_hash_primaria(idVotante);
            int h2 = funcion_hash_secundaria(idVotante);
            int index = h1;

            for (int i = 0; i < tamanio; i++)
            {
                if (tabla[index] == null) 
                    return false; // No se encontró el ID
                if (tabla[index].ID == idVotante)
                {
                    tabla[index] = eliminado; // Marca como eliminado
                    Console.WriteLine($"Eliminado en índice {index}");
                    return true;
                }

                index = (h1 + i * h2) % tamanio;
            }

            return false;
        }

        public void ImprimirTabla()
        {
            Console.WriteLine("\nEstado de la tabla hash:");
            for (int i = 0; i < tamanio; i++)
            {
                Console.Write($"[{i}]: ");
                if (tabla[i] == null)
                    Console.WriteLine("VACÍO");
                else if (tabla[i] == eliminado)
                    Console.WriteLine("ELIMINADO");
                else
                    Console.WriteLine($"ID={tabla[i].ID}, Voto='{tabla[i].Opcion}'");
            }
            Console.WriteLine();
        }
    }
}
