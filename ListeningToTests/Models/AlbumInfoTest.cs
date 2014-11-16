using System;
using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;
using ListeningTo.Models;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class AlbumInfoTest {
    [Test]
    public void FromRepositoryObject() {

      var lastfmAlbumInfo = new LastfmAlbumInfo {
        Artist = "Bobby Hutcherson",
        Name = "Spiral",
        ReleaseDate = new DateTime(1968, 5, 20),
        WikiSummary = "Album summary goes here",
      };

      var result = AlbumInfo.FromRepositoryObject(lastfmAlbumInfo);
      Assert.That(result.Name, Is.EqualTo(lastfmAlbumInfo.Name));
      Assert.That(result.Artist, Is.EqualTo(lastfmAlbumInfo.Artist));
      Assert.That(result.ReleaseDate, Is.EqualTo(lastfmAlbumInfo.ReleaseDate));
      Assert.That(result.WikiSummary, Is.EqualTo(lastfmAlbumInfo.WikiSummary));
    }
  }
}