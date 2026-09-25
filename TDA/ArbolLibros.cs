using System;
using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
    // Árbol AVL de libros, ordenado por ISBN.
    //
    // Es un Árbol Binario de Búsqueda que se AUTOBALANCEA en cada
    // inserción y eliminación, garantizando que la altura del árbol
    // siempre se mantenga cercana a log2(n). Esto asegura que, sin
    // importar el orden en que lleguen los ISBN, las operaciones sigan
    // siendo rápidas incluso con miles de libros (requisito del PDF).
    //
    // Resuelve directamente:
    //   - Buscar un libro por ISBN            -> Buscar()
    //   - Libro con el ISBN más pequeño        -> ObtenerMinimo()
    //   - Libro con el ISBN más grande         -> ObtenerMaximo()
    //   - Listado ascendente de libros         -> RecorridoAscendente()
    //   - Eliminar un libro                    -> Eliminar()
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

        // ================= UTILIDADES DE BALANCEO =================

        // Altura de un nodo (0 si el nodo es null).
        private int ObtenerAltura(NodoLibro nodo)
        {
            if (nodo == null) return 0;
            return nodo.Altura;
        }

        // Factor de balance: diferencia entre la altura del subárbol
        // izquierdo y el derecho. Si es mayor a 1 o menor a -1, el
        // árbol está desbalanceado en ese nodo y hay que rotar.
        private int ObtenerFactorBalance(NodoLibro nodo)
        {
            if (nodo == null) return 0;
            return ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);
        }

        // Recalcula la altura de un nodo a partir de sus hijos.
        private void ActualizarAltura(NodoLibro nodo)
        {
            int alturaIzquierda = ObtenerAltura(nodo.Izquierdo);
            int alturaDerecha = ObtenerAltura(nodo.Derecho);
            nodo.Altura = 1 + (alturaIzquierda > alturaDerecha ? alturaIzquierda : alturaDerecha);
        }

        // Rotación simple hacia la derecha (para el caso Izquierda-Izquierda).
        //        y                              x
        //       / \                            / \
        //      x   T3     -- rota a la der -->  T1  y
        //     / \                                  / \
        //    T1 T2                                T2 T3
        private NodoLibro RotarDerecha(NodoLibro y)
        {
            NodoLibro x = y.Izquierdo;
            NodoLibro t2 = x.Derecho;

            // Hacemos la rotación.
            x.Derecho = y;
            y.Izquierdo = t2;

            // Actualizamos alturas (primero "y" que ahora está más abajo).
            ActualizarAltura(y);
            ActualizarAltura(x);

            return x; // "x" es la nueva raíz de este subárbol.
        }

        // Rotación simple hacia la izquierda (para el caso Derecha-Derecha).
        private NodoLibro RotarIzquierda(NodoLibro x)
        {
            NodoLibro y = x.Derecho;
            NodoLibro t2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = t2;

            ActualizarAltura(x);
            ActualizarAltura(y);

            return y; // "y" es la nueva raíz de este subárbol.
        }

        // Revisa el balance de un nodo y aplica la rotación necesaria
        // (simple o doble) para dejarlo balanceado.
        private NodoLibro Balancear(NodoLibro nodo, int isbnReferencia)
        {
            ActualizarAltura(nodo);
            int balance = ObtenerFactorBalance(nodo);

            // Caso Izquierda-Izquierda
            if (balance > 1 && isbnReferencia < nodo.Izquierdo.Libro.ISBN)
            {
                return RotarDerecha(nodo);
            }

            // Caso Derecha-Derecha
            if (balance < -1 && isbnReferencia > nodo.Derecho.Libro.ISBN)
            {
                return RotarIzquierda(nodo);
            }

            // Caso Izquierda-Derecha (doble rotación)
            if (balance > 1 && isbnReferencia > nodo.Izquierdo.Libro.ISBN)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            // Caso Derecha-Izquierda (doble rotación)
            if (balance < -1 && isbnReferencia < nodo.Derecho.Libro.ISBN)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo; // Ya estaba balanceado.
        }

        // ================= INSERTAR =================
        public void Insertar(Libro libro)
        {
            raiz = InsertarRecursivo(raiz, libro);
        }

        private NodoLibro InsertarRecursivo(NodoLibro nodoActual, Libro libro)
        {
            // 1. Inserción normal de ABB.
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
                // ISBN ya existe (deben ser únicos): no se inserta duplicado.
                return nodoActual;
            }

            // 2. Balancear este nodo si hizo falta.
            return Balancear(nodoActual, libro.ISBN);
        }

        // ================= BUSCAR POR ISBN =================
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

            return null; // No se encontró.
        }

        // ================= MÍNIMO Y MÁXIMO ISBN =================
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

        // ================= RECORRIDO ASCENDENTE (in-order) =================
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

        // ================= ELIMINAR POR ISBN =================
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
            // 1. Eliminación normal de ABB.
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
                // Este es el nodo a eliminar.
                if (nodoActual.Izquierdo == null) return nodoActual.Derecho;
                if (nodoActual.Derecho == null) return nodoActual.Izquierdo;

                // Dos hijos: buscamos el sucesor (menor del subárbol derecho).
                NodoLibro sucesor = nodoActual.Derecho;
                while (sucesor.Izquierdo != null)
                {
                    sucesor = sucesor.Izquierdo;
                }

                nodoActual.Libro = sucesor.Libro;
                nodoActual.Derecho = EliminarRecursivo(nodoActual.Derecho, sucesor.Libro.ISBN);
            }

            // 2. Actualizar altura y rebalancear en el camino de regreso.
            ActualizarAltura(nodoActual);
            int balance = ObtenerFactorBalance(nodoActual);

            // Caso Izquierda-Izquierda
            if (balance > 1 && ObtenerFactorBalance(nodoActual.Izquierdo) >= 0)
            {
                return RotarDerecha(nodoActual);
            }

            // Caso Izquierda-Derecha
            if (balance > 1 && ObtenerFactorBalance(nodoActual.Izquierdo) < 0)
            {
                nodoActual.Izquierdo = RotarIzquierda(nodoActual.Izquierdo);
                return RotarDerecha(nodoActual);
            }

            // Caso Derecha-Derecha
            if (balance < -1 && ObtenerFactorBalance(nodoActual.Derecho) <= 0)
            {
                return RotarIzquierda(nodoActual);
            }

            // Caso Derecha-Izquierda
            if (balance < -1 && ObtenerFactorBalance(nodoActual.Derecho) > 0)
            {
                nodoActual.Derecho = RotarDerecha(nodoActual.Derecho);
                return RotarIzquierda(nodoActual);
            }

            return nodoActual;
        }

        // ================= LIBROS DE UNA CATEGORÍA =================
        // (en orden ascendente por ISBN)
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