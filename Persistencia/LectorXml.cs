using System;
using System.Xml;
using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.Persistencia
{
    // Esta clase se encarga de leer un archivo entrada.xml y usar sus
    // datos para llenar el Catalogo (categorías y libros).
    //
    // Recuerda que, según el PDF, el archivo es incremental: listaCategorias
    // y listaLibros son ambos opcionales, y puede haber varios archivos
    // de entrada a lo largo del tiempo.
    public class LectorXml
    {
        // Devuelve un mensaje resumen para mostrarlo en la interfaz web.
        public string CargarArchivo(string rutaArchivo, Catalogo catalogo)
        {
            XmlDocument documento = new XmlDocument();

            try
            {
                documento.Load(rutaArchivo);
            }
            catch (Exception ex)
            {
                return "No se pudo leer el archivo XML: " + ex.Message;
            }

            int categoriasAgregadas = 0;
            int categoriasFallidas = 0;
            int librosAgregados = 0;
            int librosFallidos = 0;

            // --- 1. Procesar <listaCategorias> (si existe) ---
            XmlNode nodoListaCategorias = documento.SelectSingleNode("//listaCategorias");
            if (nodoListaCategorias != null)
            {
                XmlNodeList nodosCategoria = nodoListaCategorias.SelectNodes("categoria");

                // Como una categoría podría aparecer en el XML ANTES que su
                // padre (aunque no es lo normal), hacemos varias pasadas:
                // en cada pasada intentamos agregar las que falten, hasta
                // que ya no se pueda agregar ninguna más.
                bool seAgregoAlgunaEnEstaVuelta = true;
                bool[] yaProcesada = new bool[nodosCategoria.Count];

                while (seAgregoAlgunaEnEstaVuelta)
                {
                    seAgregoAlgunaEnEstaVuelta = false;

                    for (int i = 0; i < nodosCategoria.Count; i++)
                    {
                        if (yaProcesada[i]) continue;

                        XmlNode nodoCategoria = nodosCategoria[i];
                        string nombre = nodoCategoria.InnerText.Trim();
                        string nombrePadre = null;

                        if (nodoCategoria.Attributes["padre"] != null)
                        {
                            nombrePadre = nodoCategoria.Attributes["padre"].Value;
                        }

                        bool agregada = catalogo.AgregarCategoria(nombre, nombrePadre);

                        if (agregada)
                        {
                            categoriasAgregadas++;
                            yaProcesada[i] = true;
                            seAgregoAlgunaEnEstaVuelta = true;
                        }
                    }
                }

                // Cualquier categoría que quedó sin procesar es porque su
                // padre nunca apareció, o el nombre ya existía.
                for (int i = 0; i < nodosCategoria.Count; i++)
                {
                    if (!yaProcesada[i]) categoriasFallidas++;
                }
            }

            // --- 2. Procesar <listaLibros> (si existe) ---
            XmlNode nodoListaLibros = documento.SelectSingleNode("//listaLibros");
            if (nodoListaLibros != null)
            {
                XmlNodeList nodosLibro = nodoListaLibros.SelectNodes("libro");

                foreach (XmlNode nodoLibro in nodosLibro)
                {
                    string textoIsbn = nodoLibro.SelectSingleNode("ISBN")?.InnerText.Trim();
                    string titulo = nodoLibro.SelectSingleNode("titulo")?.InnerText.Trim();
                    string autor = nodoLibro.SelectSingleNode("autor")?.InnerText.Trim();
                    string categoria = nodoLibro.SelectSingleNode("categoria")?.InnerText.Trim();

                    int isbn;
                    bool isbnValido = int.TryParse(textoIsbn, out isbn);

                    if (!isbnValido || titulo == null || autor == null || categoria == null)
                    {
                        librosFallidos++;
                        continue;
                    }

                    string resultado = catalogo.RegistrarLibro(isbn, titulo, autor, categoria);

                    if (resultado == "Libro registrado correctamente.")
                    {
                        librosAgregados++;
                    }
                    else
                    {
                        librosFallidos++;
                    }
                }
            }

            return $"Categorías agregadas: {categoriasAgregadas} (fallidas: {categoriasFallidas}). " +
                   $"Libros agregados: {librosAgregados} (fallidos: {librosFallidos}).";
        }
    }
}
