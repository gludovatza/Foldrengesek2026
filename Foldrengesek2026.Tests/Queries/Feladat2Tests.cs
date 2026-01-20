using Foldrengesek2026.Models;
using Foldrengesek2026.Services;

namespace Foldrengesek2026.Tests.Queries;

public class Feladat2Tests
{
    [Fact]
    public void Feladat2_Returns_Somogy_Telepulesek_Alphabetical_Order()
    {
        // Arrange
        var ctx = TestDbFactory.CreateContext(nameof(Feladat2_Returns_Somogy_Telepulesek_Alphabetical_Order));

        ctx.Telepulesek.AddRange(
            new Telepules { ID = 1, Nev = "Kaposvár", Varmegye = "Somogy" },
            new Telepules { ID = 2, Nev = "Barcs", Varmegye = "Somogy" },
            new Telepules { ID = 3, Nev = "Siófok", Varmegye = "Somogy" },
            new Telepules { ID = 4, Nev = "Szekszárd", Varmegye = "Tolna" }
        );
        ctx.SaveChanges();

        var service = new LekerdezesiFeladatok(ctx);

        // Act
        var list = service.SomogyTelepulesNevek().ToList();
        
        // Assert
        // 1️. Csak Somogy
        Assert.DoesNotContain("Szekszárd", list);

        // 2️. Darabszám
        Assert.Equal(3, list.Count);

        // 3️. ABC sorrend
        Assert.Equal(
            new[] { "Barcs", "Kaposvár", "Siófok" },
            list
        );
    }
}
