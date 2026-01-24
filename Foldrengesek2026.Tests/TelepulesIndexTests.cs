using Foldrengesek2026.Controllers;
using Foldrengesek2026.Data;
using Foldrengesek2026.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foldrengesek2026.Tests
{
    public class TelepulesIndexTests
    {
        private static void SeedTelepulesek(FoldrengesContext ctx)
        {
            // 25 db: biztosan lesz 3 oldal is pageSize=10 mellett.
            // Direkt vegyes sorrendben adjuk hozzá.
            var data = new List<Telepules>
        {
            new() { ID = 1,  Nev = "Kaposvár", Varmegye = "Somogy" },
            new() { ID = 2,  Nev = "Siófok", Varmegye = "Somogy" },
            new() { ID = 3,  Nev = "Barcs", Varmegye = "Somogy" },
            new() { ID = 4,  Nev = "Kaposfő", Varmegye = "Somogy" },

            new() { ID = 5,  Nev = "Szekszárd", Varmegye = "Tolna" },
            new() { ID = 6,  Nev = "Bonyhád", Varmegye = "Tolna" },

            new() { ID = 7,  Nev = "Zalaegerszeg", Varmegye = "Zala" },
            new() { ID = 8,  Nev = "Kecskemét", Varmegye = "Bács-Kiskun" },
            new() { ID = 9,  Nev = "Kazincbarcika", Varmegye = "Borsod-Abaúj-Zemplén" },
            new() { ID = 10, Nev = "Kisbér", Varmegye = "Komárom-Esztergom" },
            new() { ID = 11, Nev = "Kőszeg", Varmegye = "Vas" },
            new() { ID = 12, Nev = "Körmend", Varmegye = "Vas" },

            // plusz elemek, hogy biztosan legyen page 2 is
            new() { ID = 13, Nev = "Győr", Varmegye = "Győr-Moson-Sopron" },
            new() { ID = 14, Nev = "Szeged", Varmegye = "Csongrád-Csanád" },
            new() { ID = 15, Nev = "Pécs", Varmegye = "Baranya" },
            new() { ID = 16, Nev = "Eger", Varmegye = "Heves" },
            new() { ID = 17, Nev = "Veszprém", Varmegye = "Veszprém" },
            new() { ID = 18, Nev = "Kaposkeresztúr", Varmegye = "Somogy" },
            new() { ID = 19, Nev = "Kaposmérő", Varmegye = "Somogy" },
            new() { ID = 20, Nev = "Kaposújlak", Varmegye = "Somogy" },

            new() { ID = 21, Nev = "Budapest", Varmegye = "Pest" },
            new() { ID = 22, Nev = "Debrecen", Varmegye = "Hajdú-Bihar" },
            new() { ID = 23, Nev = "Nyíregyháza", Varmegye = "Szabolcs-Szatmár-Bereg" },
            new() { ID = 24, Nev = "Miskolc", Varmegye = "Borsod-Abaúj-Zemplén" },
            new() { ID = 25, Nev = "Sopron", Varmegye = "Győr-Moson-Sopron" },
        };

            ctx.Telepulesek.AddRange(data);
            ctx.SaveChanges();
        }

        private static List<Telepules> ExtractModelList(IActionResult actionResult)
        {
            var vr = Assert.IsType<ViewResult>(actionResult);
            var model = Assert.IsAssignableFrom<List<Telepules>>(vr.Model);
            return model;
        }

        // --------------------------------------------------------------------
        // SZŰRÉS TESZT: nev + varmegye egyszerre
        // TelepulesController-ben: Where(p => p.Nev!.ToLower().Contains(nev.ToLower()))
        // és ugyanez Varmegye-re.
        // --------------------------------------------------------------------
        [Fact]
        public async Task Index_Filter_NevAndVarmegye_ReturnsOnlyMatchingItems()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(Index_Filter_NevAndVarmegye_ReturnsOnlyMatchingItems));
            SeedTelepulesek(ctx);

            var controller = new TelepulesController(ctx);

            // Act
            // nev: "kapos" -> Kaposvár, Kaposfő, Kaposkeresztúr, Kaposmérő, Kaposújlak
            // varmegye: "Somogy"
            var result = await controller.Index(nev: "kapos", varmegye: "somogy", page: 1, sort: "nev", dir: "asc");
            var list = ExtractModelList(result);

            // Assert
            Assert.NotEmpty(list);

            // Mind Somogy
            Assert.All(list, t => Assert.Equal("Somogy", t.Varmegye));

            // Mind tartalmazza a "kapos" részt (case-insensitive)
            Assert.All(list, t => Assert.Contains("kapos", t.Nev!, StringComparison.OrdinalIgnoreCase));

            // Negatív ellenőrzés: ne csússzon be pl. Siófok
            Assert.DoesNotContain(list, t => t.Nev == "Siófok");
        }
    }
}
