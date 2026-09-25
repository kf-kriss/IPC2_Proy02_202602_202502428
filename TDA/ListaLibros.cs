using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
    // Nodo simple para una lista enlazada de libros.
    // La usamos para "juntar" resultados (por ejemplo, todos los libros
    // de una categoría, o el recorrido ascendente del árbol) sin usar
    // List<T> de C#.
    public class NodoListaLibro
    {
        public Libro Libro { get; set; }
        public NodoListaLibro Siguiente { get; set; }

        public NodoListaLibro(Libro libro)
        {
            Libro = libro;
            Siguiente = null;
        }
    }

    // Lista enlazada simple de libros, construida por nosotros mismos.
    // Sirve como "contenedor de resultados": se puede agregar al final
    // y luego recorrer de principio a fin.
    public class ListaLibros
    {
        private NodoListaLibro cabeza;
        private NodoListaLibro cola;
        private int cantidad;

        public ListaLibros()
        {
            cabeza = null;
            cola = null;
            cantidad = 0;
        }

        public int Cantidad => cantidad;

        public bool EstaVacia => cantidad == 0;

        // Agrega un libro al final de la lista.
        public void Agregar(Libro libro)
        {
            NodoListaLibro nuevo = new NodoListaLibro(libro);

            if (cabeza == null)
            {
                cabeza = nuevo;
                cola = nuevo;
            }
            else
            {
                cola.Siguiente = nuevo;
                cola = nuevo;
            }

            cantidad++;
        }

        // Permite recorrer la lista desde afuera (por ejemplo, para
        // pintarla en una página web o generar el archivo .dot de Graphviz)
        // sin exponer los nodos internos directamente.
        public NodoListaLibro ObtenerPrimero()
        {
            return cabeza;
        }
    }
}