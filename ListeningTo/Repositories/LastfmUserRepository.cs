using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient;
using LastfmClient.Responses;

namespace ListeningTo.Repositories {
  public interface ILastfmUserRepository {
    IEnumerable<LastfmUserRecentTrack> FindRecentTracks(int count);
    IEnumerable<LastfmUserTopArtist> FindTopArtists(int count);
  }

  public class LastfmUserRepository : ILastfmUserRepository {
    readonly ILastfmService service;
    readonly ILastfmCache cache;
    public static string RecentTracksCacheKey = "lastfmuser-recenttracks";
    public static string TopArtistsCacheKey = "lastfmuser-topartists";

    public LastfmUserRepository() : this(new LastfmService(Config.Instance.LastFmApiKey), new LastfmCache()) { }

    public LastfmUserRepository(ILastfmService service, ILastfmCache cache) {
      this.service = service;
      this.cache = cache;
    }
    public IEnumerable<LastfmUserRecentTrack> FindRecentTracks(int count) {
      var cachedTracks = cache.Get(RecentTracksCacheKey);
      var recentTracks = new List<LastfmUserRecentTrack>();

      if (cachedTracks == null) {
        recentTracks = service.FindRecentTracks(Config.Instance.LastFmUser, count);
        cache.Insert(RecentTracksCacheKey, recentTracks);
      }
      else {
        recentTracks = cachedTracks as List<LastfmUserRecentTrack>;
      }
      return recentTracks;
    }

    public IEnumerable<LastfmUserTopArtist> FindTopArtists(int count) {
      var cachedArtists = cache.Get(TopArtistsCacheKey);
      var topArtists = new List<LastfmUserTopArtist>();

      if (cachedArtists == null) {
        topArtists = service.FindTopArtists(Config.Instance.LastFmUser, count);
        cache.Insert(TopArtistsCacheKey, topArtists);
      }
      else {
        topArtists = cachedArtists as List<LastfmUserTopArtist>;
      }
      return topArtists;
    }
  }
}