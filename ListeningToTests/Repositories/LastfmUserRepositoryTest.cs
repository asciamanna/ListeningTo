﻿using System.Collections.Generic;
using System.Linq;
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
     
      var expectedTracks = new List<CombinedRecentTrack> {
        new CombinedRecentTrack(), new CombinedRecentTrack()
      };
      var lastfmUser = "me";
      var lastfmTracks = GenerateLastFmTracks(2);

      service.Stub(s => s.FindRecentTracks(lastfmUser, expectedCount)).Return(lastfmTracks);
      cache.Stub(c => c.Get(LastfmUserRepository.RecentTracksCacheKey)).Return(null);
      cache.Expect(c => c.Insert(Arg<string>.Is.Equal(LastfmUserRepository.RecentTracksCacheKey), 
                                 Arg<List<CombinedRecentTrack>>.List.Count(Rhino.Mocks.Constraints.Is.Equal(2))));
      config.Stub(c => c.LastFmUser).Return(lastfmUser);
      config.Stub(c => c.LastFmApiKey).Return("key");

      using (new ConfigScope(config)) {
        var repository = new LastfmUserRepository(service, cache);
        var recentTracks = repository.FindRecentTracks(expectedCount);

        cache.VerifyAllExpectations();
        Assert.That(recentTracks.Count(), Is.EqualTo(expectedCount));
        Assert.That(recentTracks.First(), Is.TypeOf<CombinedRecentTrack>());
      }
    }

    private List<LastfmUserRecentTrack> GenerateLastFmTracks(int count) {
      return Enumerable.Repeat(new LastfmUserRecentTrack(), count).ToList();
    }

    [Test]
    public void FindRecentTracks_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedTracks = new List<CombinedRecentTrack> {
        new CombinedRecentTrack(), 
      };
      var repository = new LastfmUserRepository(null, cache);
      cache.Stub(c => c.Get(LastfmUserRepository.RecentTracksCacheKey)).Return(expectedTracks);

      var recentTracks = repository.FindRecentTracks(1);

      CollectionAssert.AreEqual(expectedTracks, recentTracks);
    }

    [Test]
    public void FindRecentTracks_Gets_MusicService_Info_If_Track_Is_Currently_Playing() {
      var service = MockRepository.GenerateStub<ILastfmService>();
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var config = MockRepository.GenerateStub<IConfig>();
      var expectedTracks = new List<LastfmUserRecentTrack> {
        new LastfmUserRecentTrack { IsNowPlaying = true },
      };
      var lastfmUser = "me";
      var playingFrom = new LastfmPlayingFrom { MusicServiceName = "Spotify", MusicServiceUrl = "http://www.spotify.com"};

      config.Stub(c => c.LastFmUser).Return(lastfmUser);
      service.Stub(s => s.FindRecentTracks(lastfmUser, 1)).Return(expectedTracks);
      service.Stub(s => s.FindCurrentlyPlayingFrom(lastfmUser)).Return(playingFrom);
      
      using (new ConfigScope(config)) {
        var repository = new LastfmUserRepository(service, cache);
        var recentTrack = repository.FindRecentTracks(1).First();

        Assert.That(recentTrack.MusicServiceName, Is.EqualTo(playingFrom.MusicServiceName));
        Assert.That(recentTrack.MusicServiceUrl, Is.EqualTo(playingFrom.MusicServiceUrl));
      }
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