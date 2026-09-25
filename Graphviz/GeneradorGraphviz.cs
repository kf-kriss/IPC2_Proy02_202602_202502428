using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using IPC2_Proyecto2.TDA;

namespace IPC2_Proyecto2.Graphviz
{
    public class GeneradorGraphviz
    {
        private const string RutaEjecutableDot = @"C:\Program Files\Graphviz\bin\dot.exe";

        public string GenerarImagenEstructura(Catalogo catalogo, string nombreCategoriaInicio, string carpetaSalida, out string mensajeError)
        {
            mensajeError = null;

            string cuerpoDot = catalogo.Categorias.GenerarDotEstructura(nombreCategoriaInicio);

            if (cuerpoDot == null)
            {
                mensajeError = "La categoría indicada no existe.";
                return null;
            }

            if (string.IsNullOrWhiteSpace(cuerpoDot))
            {
                mensajeError = "Esa categoría no tiene subcategorías para graficar.";
                return null;
            }

            string textoDot = "digraph EstructuraCategorias {\n" +
                               "    rankdir=TB;\n" +
                               cuerpoDot +
                               "}\n";

            string nombreArchivo = "estructura_" + Guid.NewGuid().ToString("N") + ".png";
            return EjecutarGraphviz(textoDot, carpetaSalida, nombreArchivo, out mensajeError) ? nombreArchivo : null;
        }

        public string GenerarImagenLibrosCategoria(Catalogo catalogo, string nombreCategoria, string carpetaSalida, out string mensajeError)
        {
            mensajeError = null;

            ListaLibros libros = catalogo.ObtenerLibrosDeCategoria(nombreCategoria);

            if (libros.EstaVacia)
            {
                mensajeError = "Esa categoría no tiene libros para graficar.";
                return null;
            }

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph LibrosCategoria {");
            dot.AppendLine("    rankdir=LR;");

            string idCategoria = "\"cat_" + EscaparParaDot(nombreCategoria) + "\"";
            dot.AppendLine($"    {idCategoria} [label=\"{EscaparParaDot(nombreCategoria)}\", shape=folder, style=filled, fillcolor=khaki];");

            NodoListaLibro actual = libros.ObtenerPrimero();
            string idAnterior = idCategoria;

            while (actual != null)
            {
                string idLibro = "\"libro_" + actual.Libro.ISBN + "\"";
                string etiqueta = $"ISBN {actual.Libro.ISBN}\\n{EscaparParaDot(actual.Libro.Titulo)}";

                dot.AppendLine($"    {idLibro} [label=\"{etiqueta}\", shape=box, style=filled, fillcolor=lightgreen];");
                dot.AppendLine($"    {idAnterior} -> {idLibro};");

                idAnterior = idLibro;
                actual = actual.Siguiente;
            }

            dot.AppendLine("}");

            string nombreArchivo = "libros_" + Guid.NewGuid().ToString("N") + ".png";
            return EjecutarGraphviz(dot.ToString(), carpetaSalida, nombreArchivo, out mensajeError) ? nombreArchivo : null;
        }

        private bool EjecutarGraphviz(string textoDot, string carpetaSalida, string nombreArchivoPng, out string mensajeError)
        {
            mensajeError = null;

            try
            {
                if (!Directory.Exists(carpetaSalida))
                {
                    Directory.CreateDirectory(carpetaSalida);
                }

                string rutaDot = Path.Combine(carpetaSalida, Path.GetFileNameWithoutExtension(nombreArchivoPng) + ".dot");
                string rutaPng = Path.Combine(carpetaSalida, nombreArchivoPng);

                File.WriteAllText(rutaDot, textoDot);

                if (!File.Exists(RutaEjecutableDot))
                {
                    mensajeError = "No se encontró Graphviz en: " + RutaEjecutableDot +
                                    ". Verifica que esté instalado y ajusta la ruta en GeneradorGraphviz.cs.";
                    return false;
                }

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = RutaEjecutableDot,
                    Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaPng}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };

                using (Process proceso = Process.Start(info))
                {
                    string errores = proceso.StandardError.ReadToEnd();
                    proceso.WaitForExit();

                    if (proceso.ExitCode != 0)
                    {
                        mensajeError = "Graphviz reportó un error: " + errores;
                        return false;
                    }
                }

                return File.Exists(rutaPng);
            }
            catch (Exception ex)
            {
                mensajeError = "Ocurrió un error generando la imagen: " + ex.Message;
                return false;
            }
        }

        private string EscaparParaDot(string texto)
        {
            return texto.Replace("\"", "'");
        }
    }
}
