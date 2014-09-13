﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;
using ListeningTo.Models;
using ListeningTo.Repositories;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class RecentTrackTest {
    [Test]
    public void FromRepositoryObjects() {
      var expectedUtcDate = new DateTime(2014, 5, 26, 6, 40, 0, DateTimeKind.Utc);

      var combinedRecentTracks = CreateRepositoryRecentTracks(expectedUtcDate);

      var results = RecentTrack.FromRepositoryObjects(combinedRecentTracks);

      Assert.That(results.Count(), Is.EqualTo(combinedRecentTracks.Count()));
      var result = results.First();
      var expected = combinedRecentTracks.First();

      Assert.That(result.Album, Is.EqualTo(expected.Album));
      Assert.That(result.AlbumArtLocation, Is.EqualTo(expected.LargeAlbumArtLocation));
      Assert.That(result.Name, Is.EqualTo(expected.Name));
      Assert.That(result.Artist, Is.EqualTo(expected.Artist));
      Assert.That(result.LastPlayed, Is.StringStarting("Monday, May 26, 2014"));
      Assert.That(result.MusicServiceName, Is.EqualTo(expected.MusicServiceName));
      Assert.That(result.MusicServiceUrl, Is.EqualTo(expected.MusicServiceUrl));
    }

    private static List<CombinedRecentTrack> CreateRepositoryRecentTracks(DateTime expectedUtcDate) {
      var combinedRecentTracks = new List<CombinedRecentTrack> {
          new CombinedRecentTrack { Album = "Milestones", LargeAlbumArtLocation = "here", Artist = "Miles Davis", LastPlayed = expectedUtcDate, Name = "Dr. Jackle", MusicServiceUrl = "http://www.spotify.com", MusicServiceName = "Spotify" },
          new CombinedRecentTrack { Album = "Freedom Of Choice", LargeAlbumArtLocation = "there", Artist = "Devo", LastPlayed = expectedUtcDate, Name = "Gates of Steel"}
        };
      return combinedRecentTracks;
    }

    [Test]
    public void FromRepositoryObjects_Converts_UTC_Time_To_Eastern_Local() {
      var utcDate = new DateTime(2014, 6, 7, 8, 55, 0, DateTimeKind.Utc);
      var expectedLocalDate = DetermineExpectedDate();
      
      var combinedRecentTracks = new List<CombinedRecentTrack> {
        new CombinedRecentTrack { LastPlayed = utcDate }
      };
      var convertedDate = RecentTrack.FromRepositoryObjects(combinedRecentTracks).First().LastPlayed;

      Assert.That(convertedDate, Is.EqualTo(expectedLocalDate.ToString("f")));
    }

    [Test]
    public void FromRepositoryObjects_Shows_Now_Playing_As_Last_Played_String() {
      var combinedRecentTracks = new List<CombinedRecentTrack> {
        new CombinedRecentTrack { IsNowPlaying = true }
      };
      var track = RecentTrack.FromRepositoryObjects(combinedRecentTracks).First();

      Assert.That(track.LastPlayed, Is.EqualTo("Now Playing"));
    }

    [Test]
    public void FromRepositoryObjects_When_There_Are_No_RecentTracks_Returns_Empty_List() {
      var combinedRecentTracks = new List<CombinedRecentTrack>();

      var results = RecentTrack.FromRepositoryObjects(combinedRecentTracks);
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