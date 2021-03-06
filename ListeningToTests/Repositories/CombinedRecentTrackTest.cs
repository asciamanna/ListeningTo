﻿using LastfmClient.Responses;
using ListeningTo.Repositories;
using ListeningToTests.TestObjects;
using NUnit.Framework;

namespace ListeningToTests.Repositories {
  [TestFixture]
  public class CombinedRecentTrackTest {
    LastfmUserRecentTrack recentTrack;

    [SetUp]
    public void SetUp() {
     recentTrack = TestLastfmUserRecentTrack.Create();
    }

    [Test]
    public void FromLastFmObject_Copies_LastfmUserRecentTrack() {
      var combinedRecentTrack = RecentTrackWithSource.FromLastFmObject(recentTrack);

      Assert.That(combinedRecentTrack.Artist, Is.EqualTo(recentTrack.Artist));
      Assert.That(combinedRecentTrack.Album, Is.EqualTo(recentTrack.Album));
      Assert.That(combinedRecentTrack.Name, Is.EqualTo(recentTrack.Name));
      Assert.That(combinedRecentTrack.IsNowPlaying, Is.EqualTo(recentTrack.IsNowPlaying));
      Assert.That(combinedRecentTrack.LastPlayed, Is.EqualTo(recentTrack.LastPlayed));
      Assert.That(combinedRecentTrack.SmallImageLocation, Is.EqualTo(recentTrack.SmallImageLocation));
      Assert.That(combinedRecentTrack.MediumImageLocation, Is.EqualTo(recentTrack.MediumImageLocation));
      Assert.That(combinedRecentTrack.LargeImageLocation, Is.EqualTo(recentTrack.LargeImageLocation));
      Assert.That(combinedRecentTrack.ExtraLargeImageLocation, Is.EqualTo(recentTrack.ExtraLargeImageLocation));
    }

    [Test]
    public void FromLastFmObject_Returns_Empty_String_For_PlayingFrom_Fields() {
      var combinedRecentTrack = RecentTrackWithSource.FromLastFmObject(recentTrack);

      Assert.That(combinedRecentTrack.MusicServiceName, Is.Empty);
      Assert.That(combinedRecentTrack.MusicServiceUrl, Is.Empty);
    }
  }
}