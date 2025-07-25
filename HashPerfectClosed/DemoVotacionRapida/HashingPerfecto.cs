using System;
using System.Collections.Generic;

class FKSHashing
{

    // Función de hash primaria (Hash1)
    // Suma los valores ASCII de los caracteres de la clave y toma el módulo del tamaño de la tabla
    static int Hash1(string key, int tamanio)
    {
        int sum = 0;
        foreach (char c in key)
            sum += c;
        return sum % tamanio;
    }

    // Función de hash secundaria (Hash2)
    // Aplica una función polinómica simple con un factor de dispersión para evitar colisiones
    static int Hash2(string key, int tamanio, int dispersion)
    {
        int hash = 0;
        foreach (char c in key)
            hash = (hash * dispersion + c) % tamanio;
        return hash;
    }

    class TablaSegundoNivel
    {
        public string[] tabla; // Subarray
        private int dispersion; // Factor de dispersión 

        //Contructor
        public TablaSegundoNivel(List<string> claves)
        {
            // Tamaño cuadrático (para la tabla) según la cantidad de claves para minimizar colisiones
            int tamanio = claves.Count * claves.Count;
            tabla = new string[tamanio];
            dispersion = 1;

            bool hayColisiones;
            do
            {
                hayColisiones = false;
                for (int i = 0; i < tabla.Length; i++)
                {
                    tabla[i] = null; // Limpiar la tabla
                }

                foreach (var clave in claves)
                {
                    int indice = Hash2(clave, tamanio, dispersion);
                    // Si hay colisión, aumentar dispersión y volver a intentar
                    if (tabla[indice] != null && tabla[indice] != clave)
                    {
                        dispersion++;
                        hayColisiones = true;
                        break;
                    }
                    tabla[indice] = clave;
                }
            } while (hayColisiones);
        }

        // Verifica si la clave está presente
        public bool Contiene(string clave)
        {
            int indice = Hash2(clave, tabla.Length, dispersion);
            return tabla[indice] == clave;
        }

        //Imprmir en consola los resultados
        public void Mostrar()
        {
            Console.WriteLine($"    h2(x) = (hash * {dispersion} + c) % {tabla.Length}");
            for (int i = 0; i < tabla.Length; i++)
                Console.WriteLine($"      [{i}] → {tabla[i] ?? ""}");
        }
    }

    // Clase principal para el primer nivel
    public class FKSHash
    {
        int tamanio;
        object[] tablaNivel1; // Arreglo que puede contener cadenas o tablas de segundo nivel

        public FKSHash(List<string> claves, int tamanio)
        {
            this.tamanio = tamanio;
            var buckets = new List<string>[tamanio];

            // Inicializar los buckets
            for (int i = 0; i < tamanio; i++)
                buckets[i] = new List<string>();

            // Distribuir claves en los buckets usando Hash1
            foreach (var clave in claves)
                buckets[Hash1(clave, tamanio)].Add(clave);

            // Crear tabla de primer nivel con claves o subtablas según la cantidad por bucket
            tablaNivel1 = new object[tamanio];
            for (int i = 0; i < tamanio; i++)
            {
                if (buckets[i].Count == 1)
                    tablaNivel1[i] = buckets[i][0];
                else if (buckets[i].Count > 1)
                    tablaNivel1[i] = new TablaSegundoNivel(buckets[i]);
            }
        }

        public bool Buscar(string clave)
        {
            int idx = Hash1(clave, tamanio);
            var entry = tablaNivel1[idx];

            if (entry == null) return false;
            if (entry is string s) return s == clave; // Coincide directamente
            if (entry is TablaSegundoNivel t) return t.Contiene(clave); // Buscar en subtabla
            return false;
        }

        public void MostrarEstructura()
        {
            Console.WriteLine("Estructura FKS:\n");
            for (int i = 0; i < tamanio; i++)
            {
                Console.WriteLine($"Bucket {i}:");
                var entry = tablaNivel1[i];
                if (entry == null)
                    Console.WriteLine("    ");
                else if (entry is string s)
                    Console.WriteLine($"    {s}");
                else if (entry is TablaSegundoNivel t)
                    t.Mostrar();
            }
        }
    }
}
