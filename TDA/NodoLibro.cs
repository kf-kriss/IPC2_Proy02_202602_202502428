using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
    // Nodo propio para el Árbol AVL de libros (un ABB que se autobalancea).
    // Cada nodo guarda un Libro, sus hijos izquierdo/derecho, y su Altura
    // (necesaria para calcular el balance y decidir si hay que rotar).
    public class NodoLibro
    {
        public Libro Libro { get; set; }
        public NodoLibro Izquierdo { get; set; }
        public NodoLibro Derecho { get; set; }
        public int Altura { get; set; }

        public NodoLibro(Libro libro)
        {
            Libro = libro;
            Izquierdo = null;
            Derecho = null;
            Altura = 1; // Un nodo recién creado es una hoja: altura 1.
        }
    }
}
