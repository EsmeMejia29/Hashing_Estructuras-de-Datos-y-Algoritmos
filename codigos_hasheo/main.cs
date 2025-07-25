using System;
using System.Collections.Generic;

namespace DemoVotacionRapida
{
    class Programa
    {
        static void Main()
        {
            Console.WriteLine("-----Menú pruebas-----");
            Console.WriteLine("1 - Hashing Cerrado - Sistema de Votación");
            Console.WriteLine("2 - Hashing perfecto - con prueba FKS Hash");
            Console.WriteLine("3 - Hashing Abierto - Restaurante)");
            Console.WriteLine("4 - Salir");
            Console.Write("Selecciona una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    MenuVotacion();
                    break;
                case "2":
                    PruebaFKSHash();
                    break;
                case "3":
                    EjecutarDemoRestaurante();
                    break;
                default:
                    Console.WriteLine("bye, bye");
                    break;
            }
        }

        static void EjecutarDemoRestaurante()
        {
            Console.WriteLine("Bienvenido al Sistema de Pedidos en mesa del Restaurante");
            var restaurante = new HashingAbiertoRestaurante();

            Console.WriteLine("Tomando pedidos :D");

            // Pedidos normales
            restaurante.AgregarPedido(1, "Ceviche");
            restaurante.AgregarPedido(2, "Lomo Saltado");

            // Pedidos con colisiones
            Console.WriteLine("Pedidos que generan colisiones: ");
            restaurante.AgregarPedido(3, "Hamburguesa XXL");
            restaurante.AgregarPedido(13, "Pizza Familiar");
            restaurante.AgregarPedido(5, "Ensalada Fresca");
            restaurante.AgregarPedido(25, "Sopa del Día");
            restaurante.AgregarPedido(3, "Refresco Grande");

            Console.WriteLine("Organización interna de pedidos");
            restaurante.MostrarOrganizacion();

            Console.WriteLine("¿Cómo funciona?");
            Console.WriteLine("- Mesas 3 y 13 comparten bucket (3 % 10 = 3, 13 % 10 = 3)");
            Console.WriteLine("- Pero sus pedidos se mantienen separados en la misma lista");
            Console.WriteLine("-Así es el hashing abierto en acción");
        }

        static void MenuVotacion()
        {
            SistemaVotos votar = new SistemaVotos(11); // Tamaño primo tabla

            while (true)
            {
                Console.WriteLine("=== Menú de Votación ===");
                Console.WriteLine("1 - Insertar voto");
                Console.WriteLine("2 - Ver voto por ID");
                Console.WriteLine("3 - Eliminar voto");
                Console.WriteLine("4 - Mostrar tabla hash");
                Console.WriteLine("5 - Salir");
                Console.Write("Elige una opción: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                if (input == "5")
                    break;

                int id;
                switch (input)
                {
                    case "1":
                        Console.Write("Ingrese ID del votante: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.Write("Ingrese opción de voto (Sí / No / Ninguno): ");
                            string voto = Console.ReadLine();
                            votar.InsertarVoto(id, voto);
                            votar.ImprimirTabla();
                        }
                        else Console.WriteLine("ID no válido.\n");
                        break;

                    case "2":
                        Console.Write("Ingrese ID a consultar: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            string resultado = votar.ObtenerVoto(id);
                            if (resultado != null)
                                Console.WriteLine($"El ID {id} votó: '{resultado}'");
                            else
                                Console.WriteLine("No se encontró ese ID.");
                            votar.ImprimirTabla();
                        }
                        else Console.WriteLine("ID no válido.\n");
                        break;

                    case "3":
                        Console.Write("Ingrese ID a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            votar.EliminarVoto(id);
                            votar.ImprimirTabla();
                        }
                        else Console.WriteLine("ID no válido.\n");
                        break;

                    case "4":
                        votar.ImprimirTabla();
                        break;

                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }

            Console.WriteLine("¡Adiós!");
        }

        static void PruebaFKSHash()
        {
            // Claves de ejemplo
            var claves = new List<string> { "ID01", "ID02", "ID03", "ID05", "ID50", "ID20" };

            var fks = new FKSHash(claves, claves.Count);
            fks.MostrarEstructura();

            // Búsquedas de prueba
            Console.WriteLine("\nPruebas de búsqueda:");
            Console.WriteLine("¿Contiene 'ID01'? " + fks.Buscar("ID01"));
            Console.WriteLine("¿Contiene 'ID07'? " + fks.Buscar("ID07"));
            Console.WriteLine("¿Contiene 'ID50'? " + fks.Buscar("ID50"));
        }
    }

    // Clase para Hashing Abierto en Restaurante 
    public class HashingAbiertoRestaurante
    {
        // Aquí guardamos los pedidos, por mesa
        private List<string>[] mesas;
        private const int TOTAL_MESAS = 10;

        public HashingAbiertoRestaurante()
        {
            // Inicializamos las mesas, cada una con su lista vacía
            mesas = new List<string>[TOTAL_MESAS];
            for (int i = 0; i < TOTAL_MESAS; i++)
            {
                mesas[i] = new List<string>();
            }
        }

        // La magia del hashing ><, decide en qué bucket va cada pedido
        private int ObtenerHashMesa(int numeroMesa)
        {
            return numeroMesa % TOTAL_MESAS;
        }

        // Así agregamos un nuevo pedido a la mesa
        public void AgregarPedido(int numeroMesa, string pedido)
        {
            // Validación básica porque mesas negativas no existen, wtf?
            if (numeroMesa < 0) throw new ArgumentException("Oye, las mesas no pueden ser negativas, ¿ok?");

            int hashMesa = ObtenerHashMesa(numeroMesa);
            string pedidoCompleto = $"Mesa {numeroMesa} pidió: {pedido} a las {DateTime.Now:HH:mm}";

            mesas[hashMesa].Add(pedidoCompleto);

            Console.WriteLine($"{pedidoCompleto} (bucket #{hashMesa})");
        }

        // Muestra cómo están organizados los pedidos dentro del bucket
        public void MostrarOrganizacion()
        {
            for (int i = 0; i < TOTAL_MESAS; i++)
            {
                Console.Write($"Bucket {i}: ");

                if (mesas[i].Count == 0)
                {
                    Console.WriteLine("Vacío");
                    continue;
                }

                // Mostramos los pedidos como una lista enlazada
                Console.WriteLine(string.Join("--> ", mesas[i]));
            }
        }
    }

    // Clases necesarias para las otras funcionalidades
    public class SistemaVotos
    {
        private string[] tabla;
        private int tamanio;

        public SistemaVotos(int size)
        {
            this.tamanio = size;
            tabla = new string[size];
        }

        private int Hash(int id) => id % tamanio;

        public void InsertarVoto(int id, string voto)
        {
            int indice = Hash(id);
            tabla[indice] = voto;
            Console.WriteLine($"Voto agregado para ID {id} en posición {indice}");
        }

        public string ObtenerVoto(int id)
        {
            int indice = Hash(id);
            return tabla[indice];
        }

        public void EliminarVoto(int id)
        {
            int indice = Hash(id);
            tabla[indice] = null;
            Console.WriteLine($"Voto eliminado para ID {id} en posición {indice}");
        }

        public void ImprimirTabla()
        {
            Console.WriteLine("Tabla de votos");
            for (int i = 0; i < tamanio; i++)
            {
                Console.WriteLine($"Posición {i}: {(tabla[i] ?? "no tiene votos")}");
            }
            Console.WriteLine();
        }
    }

    public class FKSHash
    {
        private int tamanio;
        private List<string>[] buckets;

        public FKSHash(List<string> claves, int tamanio)
        {
            this.tamanio = tamanio;
            buckets = new List<string>[tamanio];
            for (int i = 0; i < tamanio; i++)
                buckets[i] = new List<string>();

            foreach (var clave in claves)
            {
                int indice = Hash(clave, tamanio);
                buckets[indice].Add(clave);
            }
        }

        private int Hash(string clave, int tamanio)
        {
            int sum = 0;
            foreach (char c in clave)
                sum += (int)c;
            return sum % tamanio;
        }

        public bool Buscar(string clave)
        {
            int indice = Hash(clave, tamanio);
            return buckets[indice].Contains(clave);
        }

        public void MostrarEstructura()
        {
            Console.WriteLine("Estructura FKS");
            for (int i = 0; i < tamanio; i++)
            {
                Console.WriteLine($"Bucket {i}: [{string.Join(", ", buckets[i])}]");
            }
        }
    }
}


