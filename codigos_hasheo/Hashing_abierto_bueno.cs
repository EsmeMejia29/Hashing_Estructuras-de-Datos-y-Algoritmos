using System;
using System.Collections.Generic;


// En este código, se aprecia el hashing abierto. ><

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

    // La magia del hashing ><, decide en qué cubeta va cada pedido
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

        Console.WriteLine($"Anotado: {pedidoCompleto} (en bucket #{hashMesa})");
    }

    // Muestra cómo están organizados los pedidos dentro del bucket o, en todo caso, mesa
    public void MostrarOrganizacion()
    {
        Console.WriteLine("Así estamos organizando los pedidos");

        for (int i = 0; i < TOTAL_MESAS; i++)
        {
            Console.Write($"Bucket {i}: ");

            if (mesas[i].Count == 0)
            {
                Console.WriteLine("No hay pedidos");
                continue;
            }

            // Mostramos los pedidos como una lista enlazada
            Console.Write(string.Join("--->", mesas[i]));
            Console.WriteLine();
        }

        Console.WriteLine("Las colisiones van al mismo bucket pero se mantienen separadas, podemos saber cual es cual");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("¡Pedidos en mesa del resturante ThebestTeam");

        var restaurante = new HashingAbiertoRestaurante();

        // Vamos a agregar algunos pedidos de ejemplo >D

        Console.WriteLine("----Tomando algunos pedidos-----");


        // Aquí podemos ver colisiones con la tercera y quinta mesa
        restaurante.AgregarPedido(3, "Hamburguesa");
        restaurante.AgregarPedido(13, "Pizza");
        restaurante.AgregarPedido(5, "Ensalada");
        restaurante.AgregarPedido(3, "Batido");
        restaurante.AgregarPedido(7, "Pasta");
        restaurante.AgregarPedido(25, "Sopa");

        // Mostramos cómo quedó organizado todo
        restaurante.MostrarOrganizacion();

        Console.WriteLine("Nota: Mesa 3 y 13 comparten bucket pero son diferentes, devoró el code");
    }
}