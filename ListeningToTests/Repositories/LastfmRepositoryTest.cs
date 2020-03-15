using System.Collections.Generic;
using System.Linq;
using LastfmClient;
using LastfmClient.Responses;
using ListeningTo.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace ListeningToTests {
  [TestFixture]
  public class LastfmRepositoryTest {
    [Test]
    public void FindRecentTracks_Adds_Results_To_Cache_And_Returns_Them() {
      var service = MockRepository.GenerateStub<ILastfmService>();
      var cache = MockRepository.GenerateMock<ILastfmCache>();
      var config = MockRepository.GenerateMock<IConfig>();
      var expectedCount = 2;
     
      var lastfmUser = "me";
      var lastfmTracks = GenerateLastFmTracks(2);

      service.Stub(s => s.FindRecentTracks(lastfmUser, expectedCount)).Return(lastfmTracks);
      cache.Stub(c => c.Get(LastfmCache.RecentTracksCacheKey)).Return(null);
      cache.Expect(c => c.Insert(Arg<string>.Is.Equal(LastfmCache.RecentTracksCacheKey), 
                                 Arg<List<RecentTrackWithSource>>.List.Count(Rhino.Mocks.Constraints.Is.Equal(2))));
      config.Stub(c => c.LastFmUser).Return(lastfmUser);
      config.Stub(c => c.LastFmApiKey).Return("key");

      using (new ConfigScope(config)) {
        var repository = new LastfmRepository(service, cache);
        var recentTracks = repository.FindRecentTracks(expectedCount);

        cache.VerifyAllExpectations();
        Assert.That(recentTracks.Count(), Is.EqualTo(expectedCount));
        Assert.That(recentTracks.First(), Is.TypeOf<RecentTrackWithSource>());
      }
    }

    private List<LastfmUserRecentTrack> GenerateLastFmTracks(int count) {
      return Enumerable.Repeat(new LastfmUserRecentTrack(), count).ToList();
    }

    [Test]
    public void FindRecentTracks_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedTracks = new List<RecentTrackWithSource> {
        new RecentTrackWithSource(), 
      };
      var repository = new LastfmRepository(null, cache);
      cache.Stub(c => c.Get(LastfmCache.RecentTracksCacheKey)).Return(expectedTracks);

      var recentTracks = repository.FindRecentTracks(1);

      Assert.That(recentTracks, Is.EquivalentTo(expectedTracks));
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
      cache.Stub(c => c.Get(LastfmCache.TopArtistsCacheKey)).Return(null);
      cache.Expect(c => c.Insert(LastfmCache.TopArtistsCacheKey, expectedTopArtists));
      config.Stub(c => c.LastFmUser).Return(lastfmUser);
      config.Stub(c => c.LastFmApiKey).Return("key");

      using (new ConfigScope(config)) {
        var repository = new LastfmRepository(service, cache);
        var topArtists = repository.FindTopArtists(expectedCount);

        cache.VerifyAllExpectations();
        Assert.That(topArtists.Count(), Is.EqualTo(expectedCount));
        Assert.That(topArtists, Is.EqualTo(expectedTopArtists));
      }
    }

    [Test]
    public void FindTopArtists_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedArtists = new List<LastfmUserTopArtist> {
        new LastfmUserTopArtist(), 
      };
      var repository = new LastfmRepository(null, cache);
      cache.Stub(c => c.Get(LastfmCache.TopArtistsCacheKey)).Return(expectedArtists);

      var topArtists = repository.FindTopArtists(1);

      CollectionAssert.AreEqual(expectedArtists, topArtists);
    }

    [Test]
    public void FindArtistInfo_Adds_Result_To_Cache_And_Returns_It() {
      var service = MockRepository.GenerateStub<ILastfmService>();
      var cache = MockRepository.GenerateMock<ILastfmCache>();
      var config = MockRepository.GenerateStub<IConfig>();

      var expectedArtistInfo = new LastfmArtistInfo();
      var artist = "Bobby Hutcherson";

      service.Stub(s => s.FindArtistInfo(artist)).Return(expectedArtistInfo);
      cache.Stub(c => c.Get(LastfmCache.ArtistInfoCacheKey + ":" + artist)).Return(null);
      cache.Expect(c => c.Insert(LastfmCache.ArtistInfoCacheKey + ":" + artist, expectedArtistInfo));

      using (new ConfigScope(config)) {
        var repository = new LastfmRepository(service, cache);
        var artistInfo = repository.FindArtistInfo(artist);

        cache.VerifyAllExpectations();
        Assert.That(artistInfo, Is.SameAs(expectedArtistInfo));
      }
    }

    [Test]
    public void FindArtistInfo_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedArtistInfo = new LastfmArtistInfo();
      var artist = "Bobby Hutcherson";

      var repository = new LastfmRepository(null, cache);
      cache.Stub(c => c.Get(LastfmCache.ArtistInfoCacheKey + ":" + artist)).Return(expectedArtistInfo);

      var artistInfo = repository.FindArtistInfo(artist);

      Assert.That(artistInfo, Is.SameAs(expectedArtistInfo));
    }

    [Test]
    public void FindAlbumInfo_Adds_Result_To_Cache_And_Returns_It() {
      var service = MockRepository.GenerateStub<ILastfmService>();
      var cache = MockRepository.GenerateMock<ILastfmCache>();
      var config = MockRepository.GenerateStub<IConfig>();

      var expectedAlbumInfo = new LastfmAlbumInfo();
      var artist = "Bobby Hutcherson";
      var album = "Spiral";

      service.Stub(s => s.FindAlbumInfo(artist, album)).Return(expectedAlbumInfo);
      cache.Stub(c => c.Get(LastfmCache.AlbumInfoCacheKey + ":" + artist + album)).Return(null);
      cache.Expect(c => c.Insert(LastfmCache.AlbumInfoCacheKey + ":" + artist + album, expectedAlbumInfo));

      using (new ConfigScope(config)) {
        var repository = new LastfmRepository(service, cache);
        var albumInfo = repository.FindAlbumInfo(artist, album);

        cache.VerifyAllExpectations();
        Assert.That(albumInfo, Is.SameAs(expectedAlbumInfo));
      }
    }

    [Test]
    public void FindAlbumInfo_Gets_Data_From_Cache() {
      var cache = MockRepository.GenerateStub<ILastfmCache>();
      var expectedAlbumInfo = new LastfmAlbumInfo();
      const string artist = "Bobby Hutcherson";
      const string album = "Spiral";

      var repository = new LastfmRepository(null, cache);
      cache.Stub(c => c.Get(LastfmCache.AlbumInfoCacheKey + ":" + artist + album)).Return(expectedAlbumInfo);

      var albumInfo = repository.FindAlbumInfo(artist, album);

      Assert.That(albumInfo, Is.SameAs(expectedAlbumInfo));
    }
  }
}