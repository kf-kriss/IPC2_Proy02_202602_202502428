using System;
using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
    public class ArbolCategorias
    {
        private NodoCategoria raizVirtual;

        public ArbolCategorias()
        {
            raizVirtual = new NodoCategoria("CATALOGO_GENERAL", null);
        }

        public NodoCategoria ObtenerCategoriasRaiz()
        {
            return raizVirtual.PrimerHijo;
        }

        public NodoCategoria BuscarCategoria(string nombre)
        {
            return BuscarRecursivo(raizVirtual.PrimerHijo, nombre);
        }

        private NodoCategoria BuscarRecursivo(NodoCategoria actual, string nombre)
        {
            if (actual == null) return null;

            if (actual.Nombre == nombre) return actual;

            NodoCategoria enHijos = BuscarRecursivo(actual.PrimerHijo, nombre);
            if (enHijos != null) return enHijos;

            return BuscarRecursivo(actual.HermanoSiguiente, nombre);
        }

        public bool AgregarCategoria(string nombre, string nombrePadre)
        {
            if (BuscarCategoria(nombre) != null)
            {
                return false;
            }

            NodoCategoria padre;

            if (string.IsNullOrEmpty(nombrePadre))
            {
                padre = raizVirtual;
            }
            else
            {
                padre = BuscarCategoria(nombrePadre);
                if (padre == null)
                {
                    return false;
                }
            }

            NodoCategoria nuevaCategoria = new NodoCategoria(nombre, padre == raizVirtual ? null : padre);
            InsertarOrdenadoAlfabeticamente(padre, nuevaCategoria);
            return true;
        }

        private void InsertarOrdenadoAlfabeticamente(NodoCategoria padre, NodoCategoria nuevoHijo)
        {
            if (padre.PrimerHijo == null)
            {
                padre.PrimerHijo = nuevoHijo;
                return;
            }

            if (string.Compare(nuevoHijo.Nombre, padre.PrimerHijo.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                nuevoHijo.HermanoSiguiente = padre.PrimerHijo;
                padre.PrimerHijo = nuevoHijo;
                return;
            }

            NodoCategoria actual = padre.PrimerHijo;
            while (actual.HermanoSiguiente != null &&
                   string.Compare(actual.HermanoSiguiente.Nombre, nuevoHijo.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                actual = actual.HermanoSiguiente;
            }

            nuevoHijo.HermanoSiguiente = actual.HermanoSiguiente;
            actual.HermanoSiguiente = nuevoHijo;
        }

        public bool AgregarLibroACategoria(Libro libro)
        {
            NodoCategoria categoria = BuscarCategoria(libro.NombreCategoria);
            if (categoria == null)
            {
                return false;
            }

            categoria.Libros.Agregar(libro);
            return true;
        }

        public string MostrarEstructura(string nombreCategoriaInicio = null)
        {
            NodoCategoria inicio;

            if (string.IsNullOrEmpty(nombreCategoriaInicio))
            {
                inicio = raizVirtual.PrimerHijo;
            }
            else
            {
                inicio = BuscarCategoria(nombreCategoriaInicio);
                if (inicio == null)
                {
                    return "La categoría indicada no existe.";
                }
            }

            System.Text.StringBuilder texto = new System.Text.StringBuilder();
            ConstruirTextoEstructura(inicio, 0, texto);
            return texto.ToString();
        }

        private void ConstruirTextoEstructura(NodoCategoria actual, int nivel, System.Text.StringBuilder texto)
        {
            if (actual == null) return;

            texto.Append(new string(' ', nivel * 4));
            texto.Append("- ");
            texto.AppendLine(actual.Nombre);

            ConstruirTextoEstructura(actual.PrimerHijo, nivel + 1, texto);
            ConstruirTextoEstructura(actual.HermanoSiguiente, nivel, texto);
        }

        public string GenerarDotEstructura(string nombreCategoriaInicio = null)
        {
            System.Text.StringBuilder dot = new System.Text.StringBuilder();

            if (string.IsNullOrEmpty(nombreCategoriaInicio))
            {
                NodoCategoria actual = raizVirtual.PrimerHijo;
                while (actual != null)
                {
                    VisitarCategoriaParaDot(actual, dot);
                    actual = actual.HermanoSiguiente;
                }
            }
            else
            {
                NodoCategoria inicio = BuscarCategoria(nombreCategoriaInicio);
                if (inicio == null)
                {
                    return null;
                }
                VisitarCategoriaParaDot(inicio, dot);
            }

            return dot.ToString();
        }

        private void VisitarCategoriaParaDot(NodoCategoria nodo, System.Text.StringBuilder dot)
        {
            string idNodo = ObtenerIdDot(nodo);
            dot.AppendLine($"    {idNodo} [label=\"{EscaparParaDot(nodo.Nombre)}\", shape=box, style=filled, fillcolor=lightblue];");

            NodoCategoria hijo = nodo.PrimerHijo;
            while (hijo != null)
            {
                string idHijo = ObtenerIdDot(hijo);
                dot.AppendLine($"    {idNodo} -> {idHijo};");
                VisitarCategoriaParaDot(hijo, dot);
                hijo = hijo.HermanoSiguiente;
            }
        }

        private string ObtenerIdDot(NodoCategoria nodo)
        {
            return "\"cat_" + EscaparParaDot(nodo.Nombre) + "\"";
        }

        private string EscaparParaDot(string texto)
        {
            return texto.Replace("\"", "'");
        }
    }
}