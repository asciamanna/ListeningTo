using ListeningTo.Models;
using ListeningToTests.TestObjects;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class AlbumInfoTest {
    [Test]
    public void FromRepositoryObject() {

      var lastfmAlbumInfo = TestAlbumInfo.Create();

      var result = AlbumInfo.FromRepositoryObject(lastfmAlbumInfo);
      Assert.That(result.Name, Is.EqualTo(lastfmAlbumInfo.Name));
      Assert.That(result.Artist, Is.EqualTo(lastfmAlbumInfo.Artist));
      Assert.That(result.ReleaseDate, Is.EqualTo(lastfmAlbumInfo.ReleaseDate));
      Assert.That(result.WikiSummary, Is.EqualTo(lastfmAlbumInfo.WikiSummary));
    }
  }
}