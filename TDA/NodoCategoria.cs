namespace IPC2_Proyecto2.TDA
{
    // Nodo para el árbol de categorías.
    //
    // En vez de que cada categoría tenga una "lista de hijos" (lo cual
    // nos obligaría a usar List<T>), usamos la técnica clásica
    // "primer hijo / hermano siguiente":
    //   - PrimerHijo apunta a la primera subcategoría.
    //   - HermanoSiguiente apunta a la "siguiente subcategoría" al mismo nivel.
    // Así representamos un árbol con cualquier número de hijos usando
    // solamente dos punteros por nodo, igual que una lista enlazada.
    public class NodoCategoria
    {
        public string Nombre { get; set; }
        public NodoCategoria Padre { get; set; }
        public NodoCategoria PrimerHijo { get; set; }
        public NodoCategoria HermanoSiguiente { get; set; }

        // Cada categoría tiene su propia lista enlazada de libros asociados.
        public ListaLibros Libros { get; set; }

        public NodoCategoria(string nombre, NodoCategoria padre)
        {
            Nombre = nombre;
            Padre = padre;
            PrimerHijo = null;
            HermanoSiguiente = null;
            Libros = new ListaLibros();
        }
    }
}