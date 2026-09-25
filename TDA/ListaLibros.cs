using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
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

        public NodoListaLibro ObtenerPrimero()
        {
            return cabeza;
        }
    }
}