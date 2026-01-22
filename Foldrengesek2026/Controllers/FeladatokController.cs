using Foldrengesek2026.Data;
using Foldrengesek2026.Services;
using Foldrengesek2026.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Foldrengesek2026.Controllers
{
    public class FeladatokController : Controller
    {
        private readonly FoldrengesContext _context;
        private readonly ILekerdezesiFeladatok _queries;

        public FeladatokController(FoldrengesContext context, ILekerdezesiFeladatok queries)
        {
            _context = context;
            _queries = queries;
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
            var results = _queries.SomogyTelepulesNevek();

            return View(results);
        }

        // SELECT varmegye, COUNT(*)
        // FROM telepulesek
        // INNER JOIN naplok ON naplok.telepulesid = telepulesek.id
        // GROUP BY varmegye
        // ORDER BY COUNT(*) DESC
        public IActionResult Feladat3()
        {
            var results = _queries.VarmegyeiRengesSzamok();

            return View(results);
        }

        // SELECT nev, datum, ido, magnitudo
        // FROM naplok
        // INNER JOIN telepulesek ON telepulesek.id = naplok.telepulesid
        // ORDER BY magnitudo DESC
        // LIMIT 1
        public IActionResult Feladat4()
        {
            var result = _queries.LegnagyobbMagnitudo();

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

        // SELECT nev, datum, intenzitas
        // FROM telepulesek
        // INNER JOIN naplok ON naplok.telepulesid = telepulesek.id
        // WHERE YEAR(datum) = 2022 AND intenzitas BETWEEN 2.0 AND 3.0
        // ORDER BY datum
        public IActionResult Feladat5()
        {
            var results = _queries.AligErzekelheto2022();

            return View(results);
        }

        // SELECT YEAR(datum), COUNT(*)
        // FROM naplok
        // WHERE intenzitas > 3.0
        // GROUP BY YEAR(datum)
        // ORDER BY COUNT(*) DESC
        // LIMIT 3
        public IActionResult Feladat6()
        {
            var results = _queries.Top3Ev_3nalNagyobbIntenzitassal();

            return View(results);
        }
    }
}
