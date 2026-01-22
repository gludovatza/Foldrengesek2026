using Foldrengesek2026.Data;
using Foldrengesek2026.ViewModels;

namespace Foldrengesek2026.Services
{
    public class LekerdezesiFeladatok : ILekerdezesiFeladatok
    {
        private readonly FoldrengesContext _context;
        public LekerdezesiFeladatok(FoldrengesContext context) => _context = context;

        public IQueryable<string> SomogyTelepulesNevek()
            => _context.Telepulesek
                .Where(t => t.Varmegye == "Somogy")
                .OrderBy(t => t.Nev)
                .Select(t => t.Nev);

        public IQueryable<Feladat3ViewModel> VarmegyeiRengesSzamok()
            => _context.Telepulesek
               .Join(_context.Naplok,
                   telepules => telepules.ID,
                   naplo => naplo.TelepulesID,
                   (telepules, naplo) => new { telepules.Varmegye })
               .GroupBy(x => x.Varmegye)
               .Select(g => new Feladat3ViewModel
               {
                   Varmegye = g.Key,
                   Count = g.Count()
               })
               .OrderByDescending(x => x.Count);

        public Feladat4ViewModel? LegnagyobbMagnitudo()
        {
            return _context.Telepulesek
                .Join(_context.Naplok,
                    telepules => telepules.ID,
                    naplo => naplo.TelepulesID,
                    (telepules, naplo) => new Feladat4ViewModel
                    {
                        Nev = telepules.Nev,
                        Datum = naplo.Datum,
                        Ido = naplo.Ido,
                        Magnitudo = naplo.Magnitudo
                    })
                .OrderByDescending(x => x.Magnitudo)
                .FirstOrDefault();
        }

        public IQueryable<Feladat5ViewModel> AligErzekelheto2022()
        {
            return _context.Naplok
                .Join(_context.Telepulesek,
                    naplo => naplo.TelepulesID,
                    telepules => telepules.ID,
                    (naplo, telepules) => new Feladat5ViewModel
                    {
                        Nev = telepules.Nev,
                        Datum = naplo.Datum,
                        Intenzitas = naplo.Intenzitas
                    })
                .Where(x =>
                    x.Datum.Year == 2022 &&
                    x.Intenzitas >= 2.0m &&
                    x.Intenzitas <= 3.0m)
                .OrderBy(x => x.Datum);
        }
    }
}
