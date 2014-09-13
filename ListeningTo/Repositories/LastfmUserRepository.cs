using System.Collections.Generic;
using System.Linq;
using LastfmClient;
using LastfmClient.Responses;

namespace ListeningTo.Repositories {
  public interface ILastfmUserRepository {
    IEnumerable<CombinedRecentTrack> FindRecentTracks(int count);
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

    public IEnumerable<CombinedRecentTrack> FindRecentTracks(int count) {
      var recentTracks = new List<CombinedRecentTrack>();
      var cachedTracks = cache.Get(RecentTracksCacheKey);

      if (cachedTracks == null) {
        var lastfmRecentTracks = service.FindRecentTracks(Config.Instance.LastFmUser, count);
        recentTracks = lastfmRecentTracks.Select(rt => CombinedRecentTrack.FromLastFmObject(rt)).ToList();

        if (lastfmRecentTracks.Any(rt => rt.IsNowPlaying)) {
          ApplyPlayingFromInformation(recentTracks);
        }
        cache.Insert(RecentTracksCacheKey, recentTracks);
      }
      else {
        recentTracks = cachedTracks as List<CombinedRecentTrack>;
      }
      return recentTracks;
    }

    private void ApplyPlayingFromInformation(List<CombinedRecentTrack> recentTracks) {
      var playingFrom = service.FindCurrentlyPlayingFrom(Config.Instance.LastFmUser);
      var currentTrack = recentTracks.First(rt => rt.IsNowPlaying);
      currentTrack.MusicServiceName = playingFrom.MusicServiceName;
      currentTrack.MusicServiceUrl = playingFrom.MusicServiceUrl;
    }

    public IEnumerable<LastfmUserTopArtist> FindTopArtists(int count) {
      var topArtists = new List<LastfmUserTopArtist>();
      var cachedArtists = cache.Get(TopArtistsCacheKey);

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