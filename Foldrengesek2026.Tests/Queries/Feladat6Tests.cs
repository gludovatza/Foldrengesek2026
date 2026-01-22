using Foldrengesek2026.Models;
using Foldrengesek2026.Services;
using Foldrengesek2026.ViewModels;

namespace Foldrengesek2026.Tests.Queries
{
    public class Feladat6Tests
    {
        [Fact]
        public void Top3Evk_3nalNagyobbIntenzitas_ReturnsTop3Years_ByCountDesc()
        {
            // Arrange
            var ctx = TestDbFactory.CreateContext(nameof(Top3Evk_3nalNagyobbIntenzitas_ReturnsTop3Years_ByCountDesc));

            // Kell legalább 1 település, hogy a FK rendben legyen
            ctx.Telepulesek.Add(new Telepules { ID = 1, Nev = "Kaposvár", Varmegye = "Somogy" });

            ctx.Naplok.AddRange(
                // 2020: 3 db > 3.0
                new Naplo { ID = 1, TelepulesID = 1, Datum = new DateTime(2020, 1, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.1m },
                new Naplo { ID = 2, TelepulesID = 1, Datum = new DateTime(2020, 2, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.5m },
                new Naplo { ID = 3, TelepulesID = 1, Datum = new DateTime(2020, 3, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 4.0m },

                // 2021: 1 db > 3.0 + 1 db pont 3.0 (nem számít, mert > 3.0 kell!)
                new Naplo { ID = 4, TelepulesID = 1, Datum = new DateTime(2021, 1, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.2m },
                new Naplo { ID = 5, TelepulesID = 1, Datum = new DateTime(2021, 2, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.0m }, // kiesik

                // 2022: 2 db > 3.0
                new Naplo { ID = 6, TelepulesID = 1, Datum = new DateTime(2022, 1, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.1m },
                new Naplo { ID = 7, TelepulesID = 1, Datum = new DateTime(2022, 2, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.9m },

                // 2019: 4 db > 3.0  (ez lesz az 1. hely)
                new Naplo { ID = 8, TelepulesID = 1, Datum = new DateTime(2019, 1, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.1m },
                new Naplo { ID = 9, TelepulesID = 1, Datum = new DateTime(2019, 2, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.2m },
                new Naplo { ID = 10, TelepulesID = 1, Datum = new DateTime(2019, 3, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.3m },
                new Naplo { ID = 11, TelepulesID = 1, Datum = new DateTime(2019, 4, 1), Ido = new TimeSpan(10, 0, 0), Magnitudo = 1.0m, Intenzitas = 3.4m }
            );

            ctx.SaveChanges();

            var service = new LekerdezesiFeladatok(ctx);

            // Act
            var list = service.Top3Ev_3nalNagyobbIntenzitassal().ToList();

            // Assert
            Assert.Equal(3, list.Count);

            // sorrend: 2019 (4), 2020 (3), 2022 (2)
            Assert.Equal(new Feladat6ViewModel { Year = 2019, Count = 4 }.Year, list[0].Year);
            Assert.Equal(4, list[0].Count);

            Assert.Equal(2020, list[1].Year);
            Assert.Equal(3, list[1].Count);

            Assert.Equal(2022, list[2].Year);
            Assert.Equal(2, list[2].Count);

            // 2021 csak 1 db > 3.0, ezért nem fér be a top3-ba
            Assert.DoesNotContain(list, x => x.Year == 2021);
        }
    }
}
