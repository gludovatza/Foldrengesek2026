using Foldrengesek2026.ViewModels;

namespace Foldrengesek2026.Services
{
    public interface ILekerdezesiFeladatok
    {
        IQueryable<string> SomogyTelepulesNevek();
        IQueryable<Feladat3ViewModel> VarmegyeiRengesSzamok();
    }
}
