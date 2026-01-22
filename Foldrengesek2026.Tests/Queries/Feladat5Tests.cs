using Foldrengesek2026.Models;
using Foldrengesek2026.Services;

namespace Foldrengesek2026.Tests.Queries
{
    public class Feladat5Tests
    {
        [Fact]
        public void AligErzekelheto2022_FiltersByYearAndIntensityInclusive_AndSortsByDateAsc()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(AligErzekelheto2022_FiltersByYearAndIntensityInclusive_AndSortsByDateAsc));

            ctx.Telepulesek.AddRange(
                new Telepules { ID = 1, Nev = "Kaposvár", Varmegye = "Somogy" },
                new Telepules { ID = 2, Nev = "Szekszárd", Varmegye = "Tolna" }
            );

            ctx.Naplok.AddRange(
                // 2022, benne van (alsó határ)
                new Naplo { ID = 1, TelepulesID = 1, Datum = new DateTime(2022, 1, 10), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 2.0m },

                // 2022, benne van (felső határ)
                new Naplo { ID = 2, TelepulesID = 2, Datum = new DateTime(2022, 1, 5), Ido = new TimeSpan(11, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.0m },

                // 2022, KIESIK (túl kicsi)
                new Naplo { ID = 3, TelepulesID = 1, Datum = new DateTime(2022, 2, 1), Ido = new TimeSpan(12, 0, 0), Magnitudo = 1.0m, Intenzitas = 1.9m },

                // 2022, KIESIK (túl nagy)
                new Naplo { ID = 4, TelepulesID = 2, Datum = new DateTime(2022, 3, 1), Ido = new TimeSpan(9, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.1m },

                // Más év, KIESIK
                new Naplo { ID = 5, TelepulesID = 2, Datum = new DateTime(2021, 12, 31), Ido = new TimeSpan(9, 0, 0), Magnitudo = 1.0m, Intenzitas = 2.5m }
            );

            ctx.SaveChanges();

            var service = new LekerdezesiFeladatok(ctx);

            // Act
            var list = service.AligErzekelheto2022().ToList();

            // Assert: csak 2 rekord marad (ID 1 és 2)
            Assert.Equal(2, list.Count);

            // dátum szerint növekvő: 2022-01-05, majd 2022-01-10
            Assert.Equal(new DateTime(2022, 1, 5), list[0].Datum);
            Assert.Equal("Szekszárd", list[0].Nev);
            Assert.Equal(3.0m, list[0].Intenzitas);

            Assert.Equal(new DateTime(2022, 1, 10), list[1].Datum);
            Assert.Equal("Kaposvár", list[1].Nev);
            Assert.Equal(2.0m, list[1].Intenzitas);
        }
    }
}
