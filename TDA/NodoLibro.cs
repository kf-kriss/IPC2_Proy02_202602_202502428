using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
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
            Altura = 1;
        }
    }
}
