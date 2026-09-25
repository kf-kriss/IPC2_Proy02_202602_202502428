using System;

namespace IPC2_Proyecto2.Excepciones
{
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