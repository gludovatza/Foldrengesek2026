using Foldrengesek2026.Models;
using Foldrengesek2026.Services;

namespace Foldrengesek2026.Tests.Queries
{
    public class Feladat3Tests
    {
        [Fact]
        public void VarmegyeiRengesSzamok_GroupByAndCountAndOrder_Works()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(VarmegyeiRengesSzamok_GroupByAndCountAndOrder_Works));

            ctx.Telepulesek.AddRange(
                new Telepules { ID = 1, Nev = "Kaposvár", Varmegye = "Somogy" },
                new Telepules { ID = 2, Nev = "Siófok", Varmegye = "Somogy" },
                new Telepules { ID = 3, Nev = "Szekszárd", Varmegye = "Tolna" }
            );

            // Somogyhoz 3 naplóbejegyzés, Tolnához 1 naplóbejegyzés
            ctx.Naplok.AddRange(
                new Naplo { ID = 1, TelepulesID = 1, Datum = new DateTime(2022, 1, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.1m, Intenzitas = 2.2m },
                new Naplo { ID = 2, TelepulesID = 1, Datum = new DateTime(2022, 1, 2), Ido = new TimeSpan(11, 0, 0), Magnitudo = 1.2m, Intenzitas = 2.3m },
                new Naplo { ID = 3, TelepulesID = 2, Datum = new DateTime(2022, 1, 3), Ido = new TimeSpan(12, 0, 0), Magnitudo = 1.3m, Intenzitas = 2.4m },
                new Naplo { ID = 4, TelepulesID = 3, Datum = new DateTime(2022, 1, 4), Ido = new TimeSpan(13, 0, 0), Magnitudo = 1.4m, Intenzitas = 2.5m }
            );

            ctx.SaveChanges();

            var service = new LekerdezesiFeladatok(ctx);

            // Act
            var list = service.VarmegyeiRengesSzamok().ToList();

            // Assert
            Assert.Equal(2, list.Count);

            // OrderByDescending miatt Somogy legyen az első (3 db)
            Assert.Equal("Somogy", list[0].Varmegye);
            Assert.Equal(3, list[0].Count);

            Assert.Equal("Tolna", list[1].Varmegye);
            Assert.Equal(1, list[1].Count);
        }
    }
}
