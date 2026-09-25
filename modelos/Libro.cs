using System;

namespace IPC2_Proyecto2.Modelos
{
    // Esta clase representa un libro dentro del catálogo.
    // Es una clase de dominio simple: solo guarda datos y los expone
    // con propiedades. No usa ninguna estructura prohibida de C#.
    public class Libro
    {
        public int ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }

        // Guardamos el nombre de la categoría a la que pertenece el libro.
        // (La relación "real" -el objeto Categoria- se maneja aparte,
        // dentro del árbol de categorías).
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