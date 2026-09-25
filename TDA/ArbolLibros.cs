using System;
using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
    public class ArbolLibros
    {
        private NodoLibro raiz;
        private int cantidadLibros;

        public ArbolLibros()
        {
            raiz = null;
            cantidadLibros = 0;
        }

        public int CantidadLibros => cantidadLibros;

        private int ObtenerAltura(NodoLibro nodo)
        {
            if (nodo == null) return 0;
            return nodo.Altura;
        }

        private int ObtenerFactorBalance(NodoLibro nodo)
        {
            if (nodo == null) return 0;
            return ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);
        }

        private void ActualizarAltura(NodoLibro nodo)
        {
            int alturaIzquierda = ObtenerAltura(nodo.Izquierdo);
            int alturaDerecha = ObtenerAltura(nodo.Derecho);
            nodo.Altura = 1 + (alturaIzquierda > alturaDerecha ? alturaIzquierda : alturaDerecha);
        }

        private NodoLibro RotarDerecha(NodoLibro y)
        {
            NodoLibro x = y.Izquierdo;
            NodoLibro t2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = t2;

            ActualizarAltura(y);
            ActualizarAltura(x);

            return x;
        }

        private NodoLibro RotarIzquierda(NodoLibro x)
        {
            NodoLibro y = x.Derecho;
            NodoLibro t2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = t2;

            ActualizarAltura(x);
            ActualizarAltura(y);

            return y;
        }

        private NodoLibro Balancear(NodoLibro nodo, int isbnReferencia)
        {
            ActualizarAltura(nodo);
            int balance = ObtenerFactorBalance(nodo);

            if (balance > 1 && isbnReferencia < nodo.Izquierdo.Libro.ISBN)
            {
                return RotarDerecha(nodo);
            }

            if (balance < -1 && isbnReferencia > nodo.Derecho.Libro.ISBN)
            {
                return RotarIzquierda(nodo);
            }

            if (balance > 1 && isbnReferencia > nodo.Izquierdo.Libro.ISBN)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            if (balance < -1 && isbnReferencia < nodo.Derecho.Libro.ISBN)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        public void Insertar(Libro libro)
        {
            raiz = InsertarRecursivo(raiz, libro);
        }

        private NodoLibro InsertarRecursivo(NodoLibro nodoActual, Libro libro)
        {
            if (nodoActual == null)
            {
                cantidadLibros++;
                return new NodoLibro(libro);
            }

            if (libro.ISBN < nodoActual.Libro.ISBN)
            {
                nodoActual.Izquierdo = InsertarRecursivo(nodoActual.Izquierdo, libro);
            }
            else if (libro.ISBN > nodoActual.Libro.ISBN)
            {
                nodoActual.Derecho = InsertarRecursivo(nodoActual.Derecho, libro);
            }
            else
            {
                return nodoActual;
            }

            return Balancear(nodoActual, libro.ISBN);
        }

        public Libro Buscar(int isbn)
        {
            NodoLibro actual = raiz;

            while (actual != null)
            {
                if (isbn == actual.Libro.ISBN)
                {
                    return actual.Libro;
                }
                else if (isbn < actual.Libro.ISBN)
                {
                    actual = actual.Izquierdo;
                }
                else
                {
                    actual = actual.Derecho;
                }
            }

            return null;
        }

        public Libro ObtenerMinimo()
        {
            if (raiz == null) return null;

            NodoLibro actual = raiz;
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }
            return actual.Libro;
        }

        public Libro ObtenerMaximo()
        {
            if (raiz == null) return null;

            NodoLibro actual = raiz;
            while (actual.Derecho != null)
            {
                actual = actual.Derecho;
            }
            return actual.Libro;
        }

        public ListaLibros RecorridoAscendente()
        {
            ListaLibros resultado = new ListaLibros();
            RecorridoAscendenteRecursivo(raiz, resultado);
            return resultado;
        }

        private void RecorridoAscendenteRecursivo(NodoLibro nodoActual, ListaLibros resultado)
        {
            if (nodoActual == null) return;

            RecorridoAscendenteRecursivo(nodoActual.Izquierdo, resultado);
            resultado.Agregar(nodoActual.Libro);
            RecorridoAscendenteRecursivo(nodoActual.Derecho, resultado);
        }

        public bool Eliminar(int isbn)
        {
            bool encontrado = Buscar(isbn) != null;
            if (encontrado)
            {
                raiz = EliminarRecursivo(raiz, isbn);
                cantidadLibros--;
            }
            return encontrado;
        }

        private NodoLibro EliminarRecursivo(NodoLibro nodoActual, int isbn)
        {
            if (nodoActual == null) return null;

            if (isbn < nodoActual.Libro.ISBN)
            {
                nodoActual.Izquierdo = EliminarRecursivo(nodoActual.Izquierdo, isbn);
            }
            else if (isbn > nodoActual.Libro.ISBN)
            {
                nodoActual.Derecho = EliminarRecursivo(nodoActual.Derecho, isbn);
            }
            else
            {
                if (nodoActual.Izquierdo == null) return nodoActual.Derecho;
                if (nodoActual.Derecho == null) return nodoActual.Izquierdo;

                NodoLibro sucesor = nodoActual.Derecho;
                while (sucesor.Izquierdo != null)
                {
                    sucesor = sucesor.Izquierdo;
                }

                nodoActual.Libro = sucesor.Libro;
                nodoActual.Derecho = EliminarRecursivo(nodoActual.Derecho, sucesor.Libro.ISBN);
            }

            ActualizarAltura(nodoActual);
            int balance = ObtenerFactorBalance(nodoActual);

            if (balance > 1 && ObtenerFactorBalance(nodoActual.Izquierdo) >= 0)
            {
                return RotarDerecha(nodoActual);
            }

            if (balance > 1 && ObtenerFactorBalance(nodoActual.Izquierdo) < 0)
            {
                nodoActual.Izquierdo = RotarIzquierda(nodoActual.Izquierdo);
                return RotarDerecha(nodoActual);
            }

            if (balance < -1 && ObtenerFactorBalance(nodoActual.Derecho) <= 0)
            {
                return RotarIzquierda(nodoActual);
            }

            if (balance < -1 && ObtenerFactorBalance(nodoActual.Derecho) > 0)
            {
                nodoActual.Derecho = RotarDerecha(nodoActual.Derecho);
                return RotarIzquierda(nodoActual);
            }

            return nodoActual;
        }

        public ListaLibros ObtenerLibrosPorCategoria(string nombreCategoria)
        {
            ListaLibros resultado = new ListaLibros();
            ObtenerLibrosPorCategoriaRecursivo(raiz, nombreCategoria, resultado);
            return resultado;
        }

        private void ObtenerLibrosPorCategoriaRecursivo(NodoLibro nodoActual, string nombreCategoria, ListaLibros resultado)
        {
            if (nodoActual == null) return;

            ObtenerLibrosPorCategoriaRecursivo(nodoActual.Izquierdo, nombreCategoria, resultado);

            if (string.Equals(nodoActual.Libro.NombreCategoria.Trim(), nombreCategoria.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                resultado.Agregar(nodoActual.Libro);
            }

            ObtenerLibrosPorCategoriaRecursivo(nodoActual.Derecho, nombreCategoria, resultado);
        }
    }
}