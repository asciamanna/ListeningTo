using System;
using LastfmClient.Responses;
using ListeningTo.Repositories;
using NUnit.Framework;

namespace ListeningToTests.Repositories {
  [TestFixture]
  public class CombinedRecentTrackTest {
    LastfmUserRecentTrack recentTrack;

    [SetUp]
    public void SetUp() {
      recentTrack = new LastfmUserRecentTrack {
        Artist = "Faith No More",
        Album = "The Real Thing",
        Name = "Zombie Eaters",
        IsNowPlaying = true,
        LastPlayed = DateTime.Now,
        SmallAlbumArtLocation = "small location",
        MediumAlbumArtLocation = "Medium location",
        LargeAlbumArtLocation = "large location",
        ExtraLargeAlbumArtLocation = "extra large location",
      };
    }

    [Test]
    public void FromLastFmObject_Copies_LastfmUserRecentTrack() {
      var combinedRecentTrack = CombinedRecentTrack.FromLastFmObject(recentTrack);

      Assert.That(combinedRecentTrack.Artist, Is.EqualTo(recentTrack.Artist));
      Assert.That(combinedRecentTrack.Album, Is.EqualTo(recentTrack.Album));
      Assert.That(combinedRecentTrack.Name, Is.EqualTo(recentTrack.Name));
      Assert.That(combinedRecentTrack.IsNowPlaying, Is.EqualTo(recentTrack.IsNowPlaying));
      Assert.That(combinedRecentTrack.LastPlayed, Is.EqualTo(recentTrack.LastPlayed));
      Assert.That(combinedRecentTrack.SmallAlbumArtLocation, Is.EqualTo(recentTrack.SmallAlbumArtLocation));
      Assert.That(combinedRecentTrack.MediumAlbumArtLocation, Is.EqualTo(recentTrack.MediumAlbumArtLocation));
      Assert.That(combinedRecentTrack.LargeAlbumArtLocation, Is.EqualTo(recentTrack.LargeAlbumArtLocation));
      Assert.That(combinedRecentTrack.ExtraLargeAlbumArtLocation, Is.EqualTo(recentTrack.ExtraLargeAlbumArtLocation));
    }

    [Test]
    public void FromLastFmObject_Returns_Empty_String_For_PlayingFrom_Fields() {
      var combinedRecentTrack = CombinedRecentTrack.FromLastFmObject(recentTrack);

      Assert.That(combinedRecentTrack.MusicServiceName, Is.Empty);
      Assert.That(combinedRecentTrack.MusicServiceUrl, Is.Empty);
    }
  }
}