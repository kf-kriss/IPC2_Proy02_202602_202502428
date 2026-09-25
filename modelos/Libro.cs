using System;

namespace IPC2_Proyecto2.Modelos
{
    public class Libro
    {
        public int ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string NombreCategoria { get; set; }

        public Libro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            NombreCategoria = nombreCategoria;
        }

        public override string ToString()
        {
            return $"ISBN: {ISBN} | Título: {Titulo} | Autor: {Autor} | Categoría: {NombreCategoria}";
        }
    }
}