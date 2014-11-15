using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;
using ListeningTo.Models;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class ArtistInfoTest {
    [Test]
    public void FromRepositoryObject() {

      var lastfmArtistInfo = new LastfmArtistInfo {
        Name = "Bobby Hutcherson",
        YearFormed = 1963,
        PlaceFormed = "New York City, New York, United States",
        BioSummary = "Shorter form of artist biography will be included here."
      };

      var result = ArtistInfo.FromRepositoryObject(lastfmArtistInfo);
      Assert.That(result.Name, Is.EqualTo(lastfmArtistInfo.Name));
      Assert.That(result.YearFormed, Is.EqualTo(lastfmArtistInfo.YearFormed));
      Assert.That(result.PlaceFormed, Is.EqualTo(lastfmArtistInfo.PlaceFormed));
      Assert.That(result.BioSummary, Is.EqualTo(lastfmArtistInfo.BioSummary));
    }
  }
}