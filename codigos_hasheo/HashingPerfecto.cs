using System;
using System.Collections.Generic;

class FKSHashing
{
    //Función hash de primer nivel
    static int Hash1(string clave, int tamanio)
    {
        int suma = 0;
        foreach (char c in clave)
            suma += (int)c;
        return suma % tamanio;
    }

    // Función hash de segundo nivel final
    static int Hash2(string clave, int tamanio, int dispersion)
    {
        int suma = 0;
        foreach (char c in clave)
            suma += (int)c * dispersion;
        return suma % tamanio;
    }

    // Tabla de segundo nivel
    class TablaSegundoNivel
    {
        private string[] tabla;
        private int dispersion;

        public TablaSegundoNivel(List<string> claves)
        {
            int tamanio = claves.Count * claves.Count;
            tabla = new string[tamanio];
            dispersion = 1;

            bool hayColisiones;
            do
            {
                hayColisiones = false;
                Array.Fill(tabla, null);

                foreach (var clave in claves)
                {
                    int indice = Hash2(clave, tamanio, dispersion);
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

        public bool Contiene(string clave)
        {
            if (tabla == null) return false;
            int indice = Hash2(clave, tabla.Length, dispersion);
            return tabla[indice] == clave;
        }
    }

    //Clase final que realiza bien el hashing perfecto
    class FKSHash
    {
        int tamaño;
        TablaSegundoNivel[] tablasSegundoNivel;
        public FKSHash(List<string> claves, int tamaño)
        {
            this.tamaño = tamaño;
            tablasSegundoNivel = new TablaSegundoNivel[tamaño];

            // Primero agrupamos por el hash1 (igual que antes)
            List<string>[] bucketstemporales = new List<string>[tamaño];
            for (int i = 0; i < tamaño; i++)
                bucketstemporales[i] = new List<string>();

            foreach (var clave in claves)
            {
                int indice = Hash1(clave, tamaño);
                bucketstemporales[indice].Add(clave);
            }

            // Luego las tablas de segundo nivel
            for (int i = 0; i < tamaño; i++)
            {
                if (bucketstemporales[i].Count > 0)
                {
                    tablasSegundoNivel[i] = new TablaSegundoNivel(bucketstemporales[i]);
                }
            }
        }

        public bool BuscarEnTabla(string clave)
        {
            int indice = Hash1(clave, tamaño);
            return tablasSegundoNivel[indice]?.Contiene(clave) ?? false;
        }

        public void MostrarEstructura()
        {
            Console.WriteLine("Estructura FKS de dos niveles");
            for (int i = 0; i < tamaño; i++)
            {
                Console.WriteLine($"Bucket {i}: {(tablasSegundoNivel[i] != null ? "Tabla secundaria" : "No tiene ningún valor")}");
            }
        }
    }

    static void Main()
    {
        var claves = new List<string> {
            "ID01", "ID02", "ID03", "ID05", "ID50","ID06","ID07", "ID20"
        };

        int tamanioprimario = claves.Count;

        var fks = new FKSHash(claves, tamanioprimario);
        fks.MostrarEstructura();
    }
}