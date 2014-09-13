using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;
using ListeningTo.Models;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class TopArtistTest {
    [Test]
    public void FromRepositoryObjects() {
      var lastfmTopArtists = new List<LastfmUserTopArtist> {
          new LastfmUserTopArtist { Name = "Miles Davis", PlayCount = 3500, Rank = 1, LargeArtistImageLocation = "here" },
          new LastfmUserTopArtist { Name = "Devo", PlayCount = 3000, Rank = 2, LargeArtistImageLocation = "there" }
        };

      var results = TopArtist.FromRepositoryObjects(lastfmTopArtists);
      Assert.That(results.Count(), Is.EqualTo(lastfmTopArtists.Count()));

      var expectedArtist = lastfmTopArtists.First();
      var actualArtist = results.First();
      Assert.That(actualArtist.Name, Is.EqualTo(expectedArtist.Name));
      Assert.That(actualArtist.ArtistImageLocation, Is.EqualTo(expectedArtist.LargeArtistImageLocation));
      Assert.That(actualArtist.Rank, Is.EqualTo(expectedArtist.Rank));
      Assert.That(actualArtist.PlayCount, Is.EqualTo(expectedArtist.PlayCount));
    }

    [Test]
    public void FromRepositoryObjects_When_There_Are_No_TopArtists_Returns_Empty_List() {
      var lastfmTopArtists = new List<LastfmUserTopArtist>();

      var results = TopArtist.FromRepositoryObjects(lastfmTopArtists);
      CollectionAssert.IsEmpty(results);
    }
  }
}