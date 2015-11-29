using System.Collections.Generic;
using LastfmClient.Responses;

namespace ListeningToTests.TestObjects {
  public static class TestTopArtist {
    public static List<LastfmUserTopArtist> CreateTopArtistCollection() {
      return new List<LastfmUserTopArtist> {
        new LastfmUserTopArtist { Name = "Miles Davis", PlayCount = 3500, Rank = 1, LargeImageLocation = "here" },
        new LastfmUserTopArtist { Name = "Devo", PlayCount = 3000, Rank = 2, LargeImageLocation = "there" }
      };
    }
  }
}