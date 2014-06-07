using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient;
using LastfmClient.Responses;
using ListeningTo.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace ListeningToTests {
  [TestFixture]
  public class LastfmUserRepositoryTest {
    [Test]
    public void FindRecentTracks_Adds_Results_To_Cache_And_Returns_Them() {
      var service = MockRepository.GenerateStub<ILastfmService>();
      var cache = MockRepository.GenerateMock<ILastfmCache>();
      var config = MockRepository.GenerateMock<IConfig>();
      var expectedCount = 2;
      var expectedTracks = new List<LastfmUserRecentTrack> {
        new LastfmUserRecentTrack(), 
        new LastfmUserRecentTrack()
      };
      var lastfmUser = "me";

      service.Stub(s => s.FindRecentTracks(lastfmUser, expectedCount)).Return(expectedTracks);
      cache.Stub(c => c.Get(LastfmUserRepository.RecentTracksCacheKey)).Return(null);
      cache.Expect(c => c.Insert(LastfmUserRepository.RecentTracksCacheKey, expectedTracks));
      config.Stub(c => c.LastFmUser).Return(lastfmUser);
      config.Stub(c => c.LastFmApiKey).Return("key");

      using (new ConfigScope(config)) {
        var repository = new LastfmUserRepository(service, cache);
        var recentTracks = repository.FindRecentTracks(expectedCount);

        cache.VerifyAllExpectations();
        Assert.That(recentTracks.Count(), Is.EqualTo(expectedCount));
        CollectionAssert.AreEqual(expectedTracks, recentTracks);
      }
    }

    [Test]
    public void FindRecentTracks_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedTracks = new List<LastfmUserRecentTrack> {
        new LastfmUserRecentTrack(), 
      };
      var repository = new LastfmUserRepository(null, cache);
      cache.Stub(c => c.Get(LastfmUserRepository.RecentTracksCacheKey)).Return(expectedTracks);

      var recentTracks = repository.FindRecentTracks(1);

      CollectionAssert.AreEqual(expectedTracks, recentTracks);
    }

    [Test]
    public void FindTopArtists_Adds_Results_To_Cache_And_Returns_Them() {
      var service = MockRepository.GenerateStub<ILastfmService>();
      var cache = MockRepository.GenerateMock<ILastfmCache>();
      var config = MockRepository.GenerateStub<IConfig>();

      var lastfmUser = "me";
      var expectedCount = 2;
      var expectedTopArtists = new List<LastfmUserTopArtist> {
        new LastfmUserTopArtist(), 
        new LastfmUserTopArtist()
      };

      service.Stub(s => s.FindTopArtists(lastfmUser, expectedCount)).Return(expectedTopArtists);
      cache.Stub(c => c.Get(LastfmUserRepository.TopArtistsCacheKey)).Return(null);
      cache.Expect(c => c.Insert(LastfmUserRepository.TopArtistsCacheKey, expectedTopArtists));
      config.Stub(c => c.LastFmUser).Return(lastfmUser);
      config.Stub(c => c.LastFmApiKey).Return("key");

      using (new ConfigScope(config)) {
        var repository = new LastfmUserRepository(service, cache);
        var topArtists = repository.FindTopArtists(expectedCount);

        cache.VerifyAllExpectations();
        Assert.That(topArtists.Count(), Is.EqualTo(expectedCount));
        CollectionAssert.AreEqual(expectedTopArtists, topArtists);
      }
    }

    [Test]
    public void FindTopArtists_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedArtists = new List<LastfmUserTopArtist> {
        new LastfmUserTopArtist(), 
      };
      var repository = new LastfmUserRepository(null, cache);
      cache.Stub(c => c.Get(LastfmUserRepository.TopArtistsCacheKey)).Return(expectedArtists);

      var topArtists = repository.FindTopArtists(1);

      CollectionAssert.AreEqual(expectedArtists, topArtists);
    }
  }
}