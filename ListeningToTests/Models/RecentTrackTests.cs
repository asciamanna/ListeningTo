using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;
using ListeningTo.Models;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class RecentTrackTests {
    [Test]
    public void FromLastfmObject_Converts_UTCTime_To_Eastern_As_A_String() {
      var expectedUtcDate = new DateTime(2014, 5, 26, 6, 40, 0, DateTimeKind.Utc);

      var lastfmRecentTracks = new List<LastfmUserRecentTrack> {
          new LastfmUserRecentTrack { Album = "Milestones", LargeAlbumArtLocation = "here", Artist = "Miles Davis", LastPlayed = expectedUtcDate, Name = "Dr. Jackle"},
          new LastfmUserRecentTrack { Album = "Freedom Of Choice", LargeAlbumArtLocation = "there", Artist = "Devo", LastPlayed = expectedUtcDate, Name = "Gates of Steel"}
        };

      var results = RecentTrack.FromLastfmObjects(lastfmRecentTracks);

      Assert.That(results.Count(), Is.EqualTo(lastfmRecentTracks.Count()));
      var result = results.First();
      var expected = lastfmRecentTracks.First();

      Assert.That(result.Album, Is.EqualTo(expected.Album));
      Assert.That(result.AlbumArtLocation, Is.EqualTo(expected.LargeAlbumArtLocation));
      Assert.That(result.Name, Is.EqualTo(expected.Name));
      Assert.That(result.Artist, Is.EqualTo(expected.Artist));
      Assert.That(result.LastPlayed.StartsWith("Monday, May 26, 2014"));

    }
  }
}