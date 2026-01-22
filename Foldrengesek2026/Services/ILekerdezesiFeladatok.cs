using Foldrengesek2026.ViewModels;

namespace Foldrengesek2026.Services
{
    public interface ILekerdezesiFeladatok
    {
        IQueryable<string> SomogyTelepulesNevek(); // Feladat2
        IQueryable<Feladat3ViewModel> VarmegyeiRengesSzamok(); // Feladat3
        Feladat4ViewModel? LegnagyobbMagnitudo(); // Feladat4
        IQueryable<Feladat5ViewModel> AligErzekelheto2022(); // Feladat5
        IQueryable<Feladat6ViewModel> Top3Ev_3nalNagyobbIntenzitassal(); // Feladat6
    }
}
