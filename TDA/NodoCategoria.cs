namespace IPC2_Proyecto2.TDA
{
    public class NodoCategoria
    {
        public string Nombre { get; set; }
        public NodoCategoria Padre { get; set; }
        public NodoCategoria PrimerHijo { get; set; }
        public NodoCategoria HermanoSiguiente { get; set; }

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