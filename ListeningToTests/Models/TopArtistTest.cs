﻿using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;
using ListeningTo.Models;
using ListeningToTests.TestObjects;
using NUnit.Framework;

namespace ListeningToTests.Models {
  [TestFixture]
  public class TopArtistTest {
    [Test]
    public void FromRepositoryObjects() {
      var lastfmTopArtists = TestTopArtist.CreateTopArtistCollection();

      var results = TopArtist.FromRepositoryObjects(lastfmTopArtists);
      Assert.That(results.Count(), Is.EqualTo(lastfmTopArtists.Count()));

      var expectedArtist = lastfmTopArtists.First();
      var actualArtist = results.First();
      Assert.That(actualArtist.Name, Is.EqualTo(expectedArtist.Name));
      Assert.That(actualArtist.ArtistImageLocation, Is.EqualTo(expectedArtist.LargeImageLocation));
      Assert.That(actualArtist.Rank, Is.EqualTo(expectedArtist.Rank));
      Assert.That(actualArtist.PlayCount, Is.EqualTo(expectedArtist.PlayCount));
    }

    [Test]
    public void FromRepositoryObjects_When_There_Are_No_TopArtists_Returns_Empty_List() {
      var results = TopArtist.FromRepositoryObjects(new List<LastfmUserTopArtist>());
      Assert.That(results, Is.Empty);
    }
  }
}