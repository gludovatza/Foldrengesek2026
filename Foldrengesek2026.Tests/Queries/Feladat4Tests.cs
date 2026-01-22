using Foldrengesek2026.Models;
using Foldrengesek2026.Services;

namespace Foldrengesek2026.Tests.Queries
{
    public class Feladat4QueriesTests
    {
        [Fact]
        public void LegnagyobbMagnitudo_ReturnsMaxMagnitudeTelepulesName()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(LegnagyobbMagnitudo_ReturnsMaxMagnitudeTelepulesName));

            ctx.Telepulesek.AddRange(
                new Telepules { ID = 1, Nev = "Kaposvár", Varmegye = "Somogy" },
                new Telepules { ID = 2, Nev = "Szekszárd", Varmegye = "Tolna" }
            );

            ctx.Naplok.AddRange(
                new Naplo { ID = 1, TelepulesID = 1, Datum = new DateTime(2022, 1, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.10m, Intenzitas = 2.0m },
                new Naplo { ID = 2, TelepulesID = 2, Datum = new DateTime(2022, 1, 2), Ido = new TimeSpan(11, 0, 0), Magnitudo = 3.50m, Intenzitas = 4.0m },
                new Naplo { ID = 3, TelepulesID = 1, Datum = new DateTime(2022, 1, 3), Ido = new TimeSpan(12, 0, 0), Magnitudo = 2.20m, Intenzitas = 3.0m }
            );

            ctx.SaveChanges();

            var service = new LekerdezesiFeladatok(ctx);

            // Act
            var result = service.LegnagyobbMagnitudo();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Szekszárd", result!.Nev);
            Assert.Equal(3.50m, result.Magnitudo);
        }

        [Fact]
        public void LegnagyobbMagnitudo_WhenNoData_ReturnsNull()
        {
            var ctx = TestDbFactory.CreateContext(nameof(LegnagyobbMagnitudo_WhenNoData_ReturnsNull));
            var service = new LekerdezesiFeladatok(ctx);

            var result = service.LegnagyobbMagnitudo();

            Assert.Null(result);
        }
    }
}