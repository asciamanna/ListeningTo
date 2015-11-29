using LastfmClient.Responses;
using ListeningTo.Models;
using ListeningToTests.TestObjects;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class ArtistInfoTest {
    [Test]
    public void FromRepositoryObject() {

      var lastfmArtistInfo = TestArtistInfo.Create();

      var result = ArtistInfo.FromRepositoryObject(lastfmArtistInfo);
      Assert.That(result.Name, Is.EqualTo(lastfmArtistInfo.Name));
      Assert.That(result.YearFormed, Is.EqualTo(lastfmArtistInfo.YearFormed));
      Assert.That(result.PlaceFormed, Is.EqualTo(lastfmArtistInfo.PlaceFormed));
      Assert.That(result.BioSummary, Is.EqualTo(lastfmArtistInfo.BioSummary));
    }
  }
}