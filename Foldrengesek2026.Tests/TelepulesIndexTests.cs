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

        // --------------------------------------------------------------------
        // LAPOZÁS TESZT: pageSize=10 a Controller-ben.
        // Ellenőrizzük:
        // - page=1 és page=2 nem fedik le egymást (stabil rendezés miatt)
        // - ViewData["CurrentPage"], ["TotalPages"], ["TotalCount"] adatai helyesek
        // --------------------------------------------------------------------
        [Fact]
        public async Task Index_Pagination_Page1_Page2_DoNotOverlap_AndViewDataAreCorrect()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(Index_Pagination_Page1_Page2_DoNotOverlap_AndViewDataAreCorrect));
            SeedTelepulesek(ctx);

            var controller = new TelepulesController(ctx);

            //// Act
            //var res1 = await controller.Index(nev: null, varmegye: null, page: 1, sort: "nev", dir: "asc");
            //var vr1 = Assert.IsType<ViewResult>(res1);
            //var page1 = Assert.IsAssignableFrom<List<Telepules>>(vr1.Model);

            //var res2 = await controller.Index(nev: null, varmegye: null, page: 2, sort: "nev", dir: "asc");
            //var vr2 = Assert.IsType<ViewResult>(res2);
            //var page2 = Assert.IsAssignableFrom<List<Telepules>>(vr2.Model);

            //// Assert: 25 elem van összesen, pageSize=10 -> totalPages=3
            //Assert.Equal(25, (int)vr1.ViewData["TotalCount"]!);
            //Assert.Equal(3, (int)vr1.ViewData["TotalPages"]!);
            //Assert.Equal(1, (int)vr1.ViewData["CurrentPage"]!);

            //Assert.Equal(25, (int)vr2.ViewData["TotalCount"]!);
            //Assert.Equal(3, (int)vr2.ViewData["TotalPages"]!);
            //Assert.Equal(2, (int)vr2.ViewData["CurrentPage"]!);

            // Act - Page 1 (külön controller)
            var controller1 = new TelepulesController(ctx);
            var res1 = await controller1.Index(nev: null, varmegye: null, page: 1, sort: "nev", dir: "asc");
            var vr1 = Assert.IsType<ViewResult>(res1);
            var page1 = Assert.IsAssignableFrom<List<Telepules>>(vr1.Model);

            // Assert ViewData page1 MIELŐTT jön a page2
            Assert.Equal(25, (int)vr1.ViewData["TotalCount"]!);
            Assert.Equal(3, (int)vr1.ViewData["TotalPages"]!);
            Assert.Equal(1, (int)vr1.ViewData["CurrentPage"]!);

            // Act - Page 2 (másik controller)
            var controller2 = new TelepulesController(ctx);
            var res2 = await controller2.Index(nev: null, varmegye: null, page: 2, sort: "nev", dir: "asc");
            var vr2 = Assert.IsType<ViewResult>(res2);
            var page2 = Assert.IsAssignableFrom<List<Telepules>>(vr2.Model);

            // Assert ViewData page2
            Assert.Equal(25, (int)vr2.ViewData["TotalCount"]!);
            Assert.Equal(3, (int)vr2.ViewData["TotalPages"]!);
            Assert.Equal(2, (int)vr2.ViewData["CurrentPage"]!);

            // Page méretek: 10 és 10 (a 3. oldal lesz 5)
            Assert.Equal(10, page1.Count);
            Assert.Equal(10, page2.Count);
            //Assert.Equal(5, (int)vr2.ViewData["TotalCount"]! - page1.Count - page2.Count);

            // Ne legyen átfedés (Skip/Take + determinisztikus rendezés miatt)
            var ids1 = page1.Select(x => x.ID).ToHashSet();
            var ids2 = page2.Select(x => x.ID).ToHashSet();
            Assert.Empty(ids1.Intersect(ids2));
        }

        // --------------------------------------------------------------------
        // RENDEZÉS TESZT: sort + dir váltás a Controller-ben.
        // Ellenőrizzünk két tipikus esetet:
        //  1. eset: név szerint csökkenő
        //  2. eset: vármegye szerint növekvő
        // --------------------------------------------------------------------
        [Fact]
        public async Task Index_Sorting_Works_ForNevDesc_AndVarmegyeAsc()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(Index_Sorting_Works_ForNevDesc_AndVarmegyeAsc));
            SeedTelepulesek(ctx);

            var controller = new TelepulesController(ctx);

            // 1. eset: Név DESC
            var resA = await controller.Index(nev: null, varmegye: null, page: 1, sort: "nev", dir: "desc");
            var vrA = Assert.IsType<ViewResult>(resA);
            var listA = Assert.IsAssignableFrom<List<Telepules>>(vrA.Model);

            Assert.Equal("nev", vrA.ViewData["CurrentSort"]?.ToString());
            Assert.Equal("desc", vrA.ViewData["CurrentDir"]?.ToString());

            for (int i = 0; i < listA.Count - 1; i++)
            {
                var a = listA[i].Nev!;
                var b = listA[i + 1].Nev!;
                Assert.True(string.Compare(a, b, StringComparison.CurrentCulture) >= 0, $"Hibás név DESC: '{a}' nem >= '{b}'");
            }

            // 2. eset: Vármegye ASC
            var resB = await controller.Index(nev: null, varmegye: null, page: 1, sort: "varmegye", dir: "asc");
            var vrB = Assert.IsType<ViewResult>(resB);
            var listB = Assert.IsAssignableFrom<List<Telepules>>(vrB.Model);

            Assert.Equal("varmegye", vrB.ViewData["CurrentSort"]?.ToString());
            Assert.Equal("asc", vrB.ViewData["CurrentDir"]?.ToString());

            for (int i = 0; i < listB.Count - 1; i++)
            {
                var a = listB[i].Varmegye!;
                var b = listB[i + 1].Varmegye!;
                Assert.True(string.Compare(a, b, StringComparison.CurrentCulture) <= 0, $"Hibás vármegye ASC: '{a}' nem <= '{b}'");
            }
        }
    }
}
