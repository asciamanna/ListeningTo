using System;
using System.Collections.Generic;
using System.Linq;
using ListeningTo.Models;
using ListeningTo.Repositories;
using ListeningToTests.TestObjects;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class RecentTrackTest {
    [Test]
    public void FromRepositoryObjects() {
      var expectedUtcDate = new DateTime(2014, 5, 26, 6, 40, 0, DateTimeKind.Utc);

      var combinedRecentTracks = TestRecentTrackWithSource.CreateRepositoryRecentTracks(expectedUtcDate);

      var results = RecentTrack.FromRepositoryObjects(combinedRecentTracks);

      Assert.That(results.Count(), Is.EqualTo(combinedRecentTracks.Count()));
      var result = results.First();
      var expected = combinedRecentTracks.First();

      Assert.That(result.Album, Is.EqualTo(expected.Album));
      Assert.That(result.AlbumArtLocation, Is.EqualTo(expected.LargeImageLocation));
      Assert.That(result.Name, Is.EqualTo(expected.Name));
      Assert.That(result.Artist, Is.EqualTo(expected.Artist));
      Assert.That(result.LastPlayed, Is.StringStarting("Monday, May 26, 2014"));
      Assert.That(result.MusicServiceName, Is.EqualTo(expected.MusicServiceName));
      Assert.That(result.MusicServiceUrl, Is.EqualTo(expected.MusicServiceUrl));
    }

    [Test]
    public void FromRepositoryObjects_Converts_UTC_Time_To_Eastern_Local() {
      var utcDate = new DateTime(2014, 6, 7, 8, 55, 0, DateTimeKind.Utc);
      var expectedLocalDate = new DateTime(2014, 6, 7, 4, 55, 0, DateTimeKind.Local);
      
      var combinedRecentTracks = new List<RecentTrackWithSource> {
        new RecentTrackWithSource { LastPlayed = utcDate }
      };
      var convertedDate = RecentTrack.FromRepositoryObjects(combinedRecentTracks).First().LastPlayed;

      Assert.That(convertedDate, Is.EqualTo(expectedLocalDate.ToString("f")));
    }

    [Test]
    public void FromRepositoryObjects_Shows_Now_Playing_As_Last_Played_String() {
      var combinedRecentTracks = new List<RecentTrackWithSource> {
        new RecentTrackWithSource { IsNowPlaying = true }
      };
      var track = RecentTrack.FromRepositoryObjects(combinedRecentTracks).First();

      Assert.That(track.LastPlayed, Is.EqualTo("Now Playing"));
    }

    [Test]
    public void FromRepositoryObjects_When_There_Are_No_RecentTracks_Returns_Empty_List() {
      var combinedRecentTracks = new List<RecentTrackWithSource>();

      var results = RecentTrack.FromRepositoryObjects(combinedRecentTracks);
      Assert.That(results, Is.Empty);
    }
  }
}