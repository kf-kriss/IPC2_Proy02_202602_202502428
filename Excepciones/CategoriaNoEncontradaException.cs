using System;

namespace IPC2_Proyecto2.Excepciones
{
    // Otra excepción propia que también hereda de Exception.
    // Se lanza cuando se busca una categoría por nombre y no existe.
    public class CategoriaNoEncontradaException : Exception
    {
        public string NombreCategoria { get; }

        public CategoriaNoEncontradaException(string nombreCategoria)
            : base($"No se encontró la categoría \"{nombreCategoria}\". Debe crearla primero.")
        {
            NombreCategoria = nombreCategoria;
        }
    }
}