using System;
using LastfmClient.Responses;

namespace ListeningToTests.TestObjects {
  public static class TestLastfmUserRecentTrack {
    public static LastfmUserRecentTrack Create() {
      return new LastfmUserRecentTrack {
        Artist = "Faith No More",
        Album = "The Real Thing",
        Name = "Zombie Eaters",
        IsNowPlaying = true,
        LastPlayed = DateTime.Now,
        SmallImageLocation = "small location",
        MediumImageLocation = "Medium location",
        LargeImageLocation = "large location",
        ExtraLargeImageLocation = "extra large location",
      };
    }
  }
}