using IPC2_Proyecto2;

namespace IPC2_Proyecto2.Tests;

public class CatalogoValidacionTests
{
    [Fact]
    public void AgregarCategoria_ConNombreVacio_NoDebePermitirlo()
    {
        var catalogo = new Catalogo();

        var resultado = catalogo.AgregarCategoria("   ", null);

        Assert.False(resultado);
        Assert.True(string.IsNullOrWhiteSpace(catalogo.ObtenerEstructuraCategorias()));
    }

    [Fact]
    public void RegistrarLibro_ConTituloVacio_DebeRechazarlo()
    {
        var catalogo = new Catalogo();
        catalogo.AgregarCategoria("Tecnologia", null);

        var resultado = catalogo.RegistrarLibro(1001, "   ", "Ana", "Tecnologia");

        Assert.Equal("El título no puede estar vacío.", resultado);
    }

    [Fact]
    public void RegistrarLibro_ConCategoriaVacia_DebeRechazarlo()
    {
        var catalogo = new Catalogo();

        var resultado = catalogo.RegistrarLibro(1002, "Libro", "Ana", "   ");

        Assert.Equal("La categoría del libro no puede estar vacía.", resultado);
    }
}
