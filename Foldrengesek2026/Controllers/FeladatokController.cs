using Foldrengesek2026.Data;
using Foldrengesek2026.Models;
using Foldrengesek2026.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;

namespace Foldrengesek2026.Controllers
{
    public class FeladatokController : Controller
    {
        private readonly FoldrengesContext _context;

        public FeladatokController(FoldrengesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // SELECT nev
        // FROM telepulesek
        // WHERE varmegye = "Somogy"
        // ORDER BY nev
        public IActionResult Feladat2()
        {
            var results = _context.Telepulesek
                .Where(t => t.Varmegye == "Somogy")
                .OrderBy(t => t.Nev)
                .Select(t => t.Nev);

            return View(results);
        }

        // SELECT varmegye, COUNT(*)
        // FROM telepulesek
        // INNER JOIN naplok ON naplok.telepulesid = telepulesek.id
        // GROUP BY varmegye
        // ORDER BY COUNT(*) DESC
        public IActionResult Feladat3()
        {
            var results = _context.Telepulesek
                .Join(_context.Naplok,
                        telepules => telepules.ID,
                        naplo => naplo.TelepulesID,
                        (telepules, naplo) => new
                        {
                            telepules.Varmegye
                        })
                .GroupBy(t => t.Varmegye)
                .Select(g => new Feladat3ViewModel
                {
                    Varmegye = g.Key, // a mező, ami szerint csoportosítva van: Varmegye
                    Count = g.Count()
                })
                .OrderByDescending(t => t.Count);

            return View(results);
        }



        // SELECT nev, datum, ido, magnitudo
        // FROM naplok
        // INNER JOIN telepulesek ON telepulesek.id = naplok.telepulesid
        // ORDER BY magnitudo DESC
        // LIMIT 1
        public IActionResult Feladat4()
        {
            var result = _context.Telepulesek
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
                .OrderByDescending(j => j.Magnitudo)
                .FirstOrDefault();

            //// Alternatíva: Mivel a két Model osztály között van kapcsolat, ezért a Join helyett használható Include is
            //var result = _context.Naplok
            //    .Include(n => n.Telepules)
            //    .OrderByDescending(j => j.Magnitudo)
            //    .Select(n => new Feladat4ViewModel
            //    {
            //        Nev = n.Telepules!.Nev,
            //        Datum = n.Datum,
            //        Ido = n.Ido,
            //        Magnitudo = n.Magnitudo
            //    })
            //    .FirstOrDefault();

            return View(result);
        }
    }
}
