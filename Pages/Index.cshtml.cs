using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Catalogo catalogo;

        public IndexModel(Catalogo catalogo)
        {
            this.catalogo = catalogo;
        }

        public string MensajeReinicio { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostReiniciar()
        {
            catalogo.Reiniciar();
            MensajeReinicio = "El catálogo se reinició correctamente. Ya no tiene categorías ni libros.";
            return Page();
        }
    }
}