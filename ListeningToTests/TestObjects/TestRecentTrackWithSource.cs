using System;
using System.Collections.Generic;
using ListeningTo.Repositories;

namespace ListeningToTests.TestObjects {
  public class TestRecentTrackWithSource {
    public static List<RecentTrackWithSource> CreateRepositoryRecentTracks(DateTime expectedUtcDate) {
      var combinedRecentTracks = new List<RecentTrackWithSource> {
        new RecentTrackWithSource { Album = "Milestones", LargeImageLocation = "here", Artist = "Miles Davis", LastPlayed = expectedUtcDate, Name = "Dr. Jackle", MusicServiceUrl = "http://www.spotify.com", MusicServiceName = "Spotify" },
        new RecentTrackWithSource { Album = "Freedom Of Choice", LargeImageLocation = "there", Artist = "Devo", LastPlayed = expectedUtcDate, Name = "Gates of Steel"}
      };
      return combinedRecentTracks;
    }
  }
}