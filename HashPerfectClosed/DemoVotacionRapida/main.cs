using System;
using System.Collections.Generic;
using static FKSHashing;

namespace DemoVotacionRapida
{
    public static class Programa
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("----Programa Principal----");
            Console.WriteLine("1 - Ejecutar Hashing Cerrado con el ejemplo de Sistema de Votación");
            Console.WriteLine("2 - Ejecutar Hashing perfecto con prueba FKS Hash con ejemplo de una base de datos");
            Console.WriteLine("3 - Ejecutar Hashing abierto con el ejemplo de un restaurante");
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
                    MenuRestaurante();
                    break;
                default:
                    Console.WriteLine("Saliendo");
                    break;
            }
        }

        static void MenuVotacion()
        {
            SistemaVotos votar = new SistemaVotos(11); // Tamaño primo tabla

            while (true)
            {
                Console.WriteLine("--- Menú de Votación ---");
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

        static void MenuRestaurante()
        {
            var restaurante = new HashingAbiertoRestaurante();
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("Pedidos del Restaurante");
                Console.WriteLine("1. Agregar pedido");
                Console.WriteLine("2. Mostrar organización de pedidos");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida.Clickee enter para continuar");
                    Console.ReadKey();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        try
                        {
                            Console.Write("Ingrese número de mesa: ");
                            int numeroMesa = int.Parse(Console.ReadLine());

                            Console.Write("Ingrese el pedido: ");
                            string pedido = Console.ReadLine();

                            restaurante.AgregarPedido(numeroMesa, pedido);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }

                        Console.WriteLine("Presione una enter para continuar");
                        Console.ReadKey();
                        break;

                    case 2:
                        restaurante.MostrarOrganizacion();
                        Console.WriteLine("Presione una enter para continuar");
                        Console.ReadKey();
                        break;

                    case 3:
                        Console.WriteLine("Saliendo del menú de pedidos del restaurante");
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Presione una enter para continuar");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 3);
        }
    }
}


