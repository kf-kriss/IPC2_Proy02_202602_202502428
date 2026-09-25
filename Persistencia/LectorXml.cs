using System;
using System.Xml;
using IPC2_Proyecto2.Modelos;

namespace IPC2_Proyecto2.Persistencia
{
    public class LectorXml
    {
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

            XmlNode nodoListaCategorias = documento.SelectSingleNode("//listaCategorias");
            if (nodoListaCategorias != null)
            {
                XmlNodeList nodosCategoria = nodoListaCategorias.SelectNodes("categoria");

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

                for (int i = 0; i < nodosCategoria.Count; i++)
                {
                    if (!yaProcesada[i]) categoriasFallidas++;
                }
            }

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
