using System;
using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.TDA
{
    // Árbol de categorías del catálogo.
    //
    // Usamos un nodo "raíz virtual" invisible que actúa como padre de
    // todas las categorías principales (las que no tienen padre en el
    // XML). Así todo el árbol cuelga de un solo punto de entrada, sin
    // necesidad de una lista de "categorías raíz".
    public class ArbolCategorias
    {
        private NodoCategoria raizVirtual;

        public ArbolCategorias()
        {
            raizVirtual = new NodoCategoria("CATALOGO_GENERAL", null);
        }

        // Primera categoría de nivel superior (para poder recorrer desde afuera).
        public NodoCategoria ObtenerCategoriasRaiz()
        {
            return raizVirtual.PrimerHijo;
        }

        // --- BUSCAR UNA CATEGORÍA POR NOMBRE (recorre todo el árbol) ---
        public NodoCategoria BuscarCategoria(string nombre)
        {
            return BuscarRecursivo(raizVirtual.PrimerHijo, nombre);
        }

        private NodoCategoria BuscarRecursivo(NodoCategoria actual, string nombre)
        {
            if (actual == null) return null;

            if (actual.Nombre == nombre) return actual;

            // Buscar primero entre los hijos de esta categoría...
            NodoCategoria enHijos = BuscarRecursivo(actual.PrimerHijo, nombre);
            if (enHijos != null) return enHijos;

            // ...si no está, seguir buscando entre los hermanos.
            return BuscarRecursivo(actual.HermanoSiguiente, nombre);
        }

        // --- AGREGAR UNA CATEGORÍA NUEVA ---
        // Si nombrePadre viene vacío o null, la categoría se agrega como
        // categoría principal (de primer nivel).
        public bool AgregarCategoria(string nombre, string nombrePadre)
        {
            // El nombre de categoría debe ser único en todo el árbol.
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
                    // El padre indicado no existe todavía.
                    return false;
                }
            }

            NodoCategoria nuevaCategoria = new NodoCategoria(nombre, padre == raizVirtual ? null : padre);
            InsertarOrdenadoAlfabeticamente(padre, nuevaCategoria);
            return true;
        }

        // Inserta "nuevoHijo" entre los hijos de "padre", manteniendo
        // el orden alfabético exigido por el enunciado.
        private void InsertarOrdenadoAlfabeticamente(NodoCategoria padre, NodoCategoria nuevoHijo)
        {
            if (padre.PrimerHijo == null)
            {
                padre.PrimerHijo = nuevoHijo;
                return;
            }

            // Caso especial: el nuevo hijo va antes que el primero.
            if (string.Compare(nuevoHijo.Nombre, padre.PrimerHijo.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                nuevoHijo.HermanoSiguiente = padre.PrimerHijo;
                padre.PrimerHijo = nuevoHijo;
                return;
            }

            // Caso general: buscamos el lugar correcto recorriendo los hermanos.
            NodoCategoria actual = padre.PrimerHijo;
            while (actual.HermanoSiguiente != null &&
                   string.Compare(actual.HermanoSiguiente.Nombre, nuevoHijo.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                actual = actual.HermanoSiguiente;
            }

            nuevoHijo.HermanoSiguiente = actual.HermanoSiguiente;
            actual.HermanoSiguiente = nuevoHijo;
        }

        // --- ASOCIAR UN LIBRO A SU CATEGORÍA ---
        public bool AgregarLibroACategoria(Libro libro)
        {
            NodoCategoria categoria = BuscarCategoria(libro.NombreCategoria);
            if (categoria == null)
            {
                return false; // La categoría indicada no existe.
            }

            categoria.Libros.Agregar(libro);
            return true;
        }

        // --- MOSTRAR ESTRUCTURA COMPLETA (o desde una subcategoría) ---
        // Genera un texto con sangrías que representa el árbol,
        // pensado para mostrarse en la interfaz web.
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

        // --- GENERAR TEXTO .DOT PARA GRAPHVIZ (estructura de categorías) ---
        // Devuelve solo el "cuerpo" del grafo (nodos y conexiones), para que
        // GeneradorGraphviz lo envuelva en "digraph { ... }".
        // Si la categoría indicada no existe, devuelve null.
        public string GenerarDotEstructura(string nombreCategoriaInicio = null)
        {
            System.Text.StringBuilder dot = new System.Text.StringBuilder();

            if (string.IsNullOrEmpty(nombreCategoriaInicio))
            {
                // Sin punto de inicio: dibujamos todas las categorías principales.
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
                    return null; // La categoría no existe.
                }
                VisitarCategoriaParaDot(inicio, dot);
            }

            return dot.ToString();
        }

        // Declara el nodo actual y lo conecta con cada uno de sus hijos,
        // visitando recursivamente a cada hijo (que a su vez se declara
        // y conecta con sus propios hijos, y así sucesivamente).
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

        // Genera un identificador único y válido para Graphviz a partir
        // del nombre de la categoría (entre comillas, para permitir espacios).
        private string ObtenerIdDot(NodoCategoria nodo)
        {
            return "\"cat_" + EscaparParaDot(nodo.Nombre) + "\"";
        }

        // Evita que comillas dentro del nombre rompan la sintaxis del .dot.
        private string EscaparParaDot(string texto)
        {
            return texto.Replace("\"", "'");
        }
    }
}