using System;
using System.Collections.Generic;

class FKSHashing
{
    // ---------- h1: función hash simple de primer nivel ----------
    static int Hash1(string key, int tamanio)
    {
        int sum = 0;
        foreach (char c in key)
            sum += (int)c;
        return sum % tamanio;
    }

    // ---------- Clase principal: tabla hash FKS nivel 1 ----------
    class FKSHash
    {
        int tamanio;
        List<string>[] buckets;

        public FKSHash(List<string> clave, int tamanio)
        {
            this.tamanio = tamanio;
            buckets = new List<string>[tamanio];
            for (int i = 0; i < tamanio; i++)
                buckets[i] = new List<string>();

            foreach (var key in clave)
            {
                int index = Hash1(key, tamanio);
                buckets[index].Add(key);
            }
        }

        public bool BuscarEnTabla(string key)
        {
            int index = Hash1(key, tamanio);
            return buckets[index].Contains(key);
        }

        public void MostrarEstructura()
        {
            Console.WriteLine("Estructura FKS (aqui solo esta el h1)\n");
            for (int i = 0; i < tamanio; i++)
            {
                Console.WriteLine($"Bucket {i}: [{string.Join(", ", buckets[i])}]");
            }
        }
    }

    static void Main()
    {
        var claves = new List<string> {
            "ID01", "ID02", "ID03", "ID05", "ID50", "ID20"
        };

        int tamañoPrimario = claves.Count;

        var fks = new FKSHash(claves, tamañoPrimario);
        fks.MostrarEstructura();

        Console.WriteLine("\nPruebas de búsqueda:");
        Console.WriteLine("¿Contiene 'ID01'? " + fks.BuscarEnTabla("ID01"));  
        Console.WriteLine("¿Contiene 'ID07'? " + fks.BuscarEnTabla("ID07"));  
        Console.WriteLine("¿Contiene 'ID02'? " + fks.BuscarEnTabla("ID02"));    
    }
}
