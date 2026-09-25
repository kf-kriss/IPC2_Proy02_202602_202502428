using System;

namespace IPC2_Proyecto2.Excepciones
{
    public class IsbnDuplicadoException : Exception
    {
        public int Isbn { get; }

        public IsbnDuplicadoException(int isbn)
            : base($"Ya existe un libro con el ISBN {isbn}.")
        {
            Isbn = isbn;
        }
    }
}