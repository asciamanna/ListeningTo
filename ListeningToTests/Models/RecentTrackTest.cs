using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;
using ListeningTo.Models;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class RecentTrackTest {
    [Test]
    public void FromRepositoryObjects() {
      var expectedUtcDate = new DateTime(2014, 5, 26, 6, 40, 0, DateTimeKind.Utc);

      var lastfmRecentTracks = new List<LastfmUserRecentTrack> {
          new LastfmUserRecentTrack { Album = "Milestones", LargeAlbumArtLocation = "here", Artist = "Miles Davis", LastPlayed = expectedUtcDate, Name = "Dr. Jackle"},
          new LastfmUserRecentTrack { Album = "Freedom Of Choice", LargeAlbumArtLocation = "there", Artist = "Devo", LastPlayed = expectedUtcDate, Name = "Gates of Steel"}
        };

      var results = RecentTrack.FromRepositoryObjects(lastfmRecentTracks);

      Assert.That(results.Count(), Is.EqualTo(lastfmRecentTracks.Count()));
      var result = results.First();
      var expected = lastfmRecentTracks.First();

      Assert.That(result.Album, Is.EqualTo(expected.Album));
      Assert.That(result.AlbumArtLocation, Is.EqualTo(expected.LargeAlbumArtLocation));
      Assert.That(result.Name, Is.EqualTo(expected.Name));
      Assert.That(result.Artist, Is.EqualTo(expected.Artist));
      Assert.That(result.LastPlayed, Is.StringStarting("Monday, May 26, 2014"));
    }

    [Test]
    public void FromRepositoryObjects_Converts_UTC_Time_To_Eastern_Local() {
      var utcDate = new DateTime(2014, 6, 7, 8, 55, 0, DateTimeKind.Utc);
      var expectedLocalDate = DetermineExpectedDate();
      
      var lastfmRecentTracks = new List<LastfmUserRecentTrack> {
        new LastfmUserRecentTrack { LastPlayed = utcDate }
      };
      var convertedDate = RecentTrack.FromRepositoryObjects(lastfmRecentTracks).First().LastPlayed;

      Assert.That(convertedDate, Is.EqualTo(expectedLocalDate.ToString("f")));
    }

    [Test]
    public void FromRepositoryObjects_Shows_Now_Playing_As_Last_Played_String() {
      var lastfmRecentTracks = new List<LastfmUserRecentTrack> {
        new LastfmUserRecentTrack { IsNowPlaying = true }
      };
      var track = RecentTrack.FromRepositoryObjects(lastfmRecentTracks).First();

      Assert.That(track.LastPlayed, Is.EqualTo("Now Playing"));
    }

    [Test]
    public void FromRepositoryObjects_When_There_Are_No_RecentTracks_Returns_Empty_List() {
      var lastfmRecentTracks = new List<LastfmUserRecentTrack>();

      var results = RecentTrack.FromRepositoryObjects(lastfmRecentTracks);
      CollectionAssert.IsEmpty(results);
    }

    private static DateTime DetermineExpectedDate() {
      var expectedLocalDateDaylightSavingsTime = new DateTime(2014, 6, 7, 4, 55, 0, DateTimeKind.Local);
      var expectedLocalDateStandardTime = expectedLocalDateDaylightSavingsTime.AddHours(1);
      var isDaylightSavingsTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").IsDaylightSavingTime(DateTime.Now);
      return isDaylightSavingsTime ? expectedLocalDateDaylightSavingsTime : expectedLocalDateStandardTime;
    }
  }
}