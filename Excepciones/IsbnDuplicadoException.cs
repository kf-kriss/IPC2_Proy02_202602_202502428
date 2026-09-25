using System;

namespace IPC2_Proyecto2.Excepciones
{
    // Excepción propia que HEREDA de la clase base Exception de C#.
    // Se lanza cuando se intenta registrar un libro con un ISBN que
    // ya existe en el catálogo.
    public class IsbnDuplicadoException : Exception
    {
        // Guardamos el ISBN que causó el problema, para poder
        // usarlo después si hace falta (por ejemplo, en un log).
        public int Isbn { get; }

        // El constructor llama al constructor de la clase base (Exception)
        // usando "base(...)", pasándole un mensaje ya armado y específico
        // para este tipo de error. Esto es herencia en acción: reutilizamos
        // todo el comportamiento de Exception (pila de llamadas, Message,
        // etc.) y solo personalizamos el mensaje.
        public IsbnDuplicadoException(int isbn)
            : base($"Ya existe un libro con el ISBN {isbn}.")
        {
            Isbn = isbn;
        }
    }
}