using System.Collections.Generic;
using System.Linq;
using LastfmClient;
using LastfmClient.Responses;

namespace ListeningTo.Repositories {
  public interface ILastfmRepository {
    IEnumerable<CombinedRecentTrack> FindRecentTracks(int count);
    IEnumerable<LastfmUserTopArtist> FindTopArtists(int count);
    LastfmArtistInfo FindArtistInfo(string artist);
    LastfmAlbumInfo FindAlbumInfo(string artist, string album);
  }

  public class LastfmRepository : ILastfmRepository {
    readonly ILastfmService service;
    readonly ILastfmCache cache;
    public const string RecentTracksCacheKey = "lastfmuser-recenttracks";
    public const string TopArtistsCacheKey = "lastfmuser-topartists";
    public const string ArtistInfoCacheKey = "lastfmartist-artistinfo";
    public const string AlbumInfoCacheKey = "lastfmalbum-albuminfo";

    public LastfmRepository() : this(new LastfmService(Config.Instance.LastFmApiKey), new LastfmCache()) { }

    public LastfmRepository(ILastfmService service, ILastfmCache cache) {
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

    public LastfmArtistInfo FindArtistInfo(string artist) {
      var artistInfo = cache.Get(BuildInfoCache(ArtistInfoCacheKey, artist)) as LastfmArtistInfo;

      if (artistInfo == null) {
        artistInfo = service.FindArtistInfo(artist);
        cache.Insert(BuildInfoCache(ArtistInfoCacheKey, artist), artistInfo);
      }
      return artistInfo;
    }

    public LastfmAlbumInfo FindAlbumInfo(string artist, string album) {
      var albumInfo = cache.Get(BuildInfoCache(AlbumInfoCacheKey, artist + album)) as LastfmAlbumInfo;

      if (albumInfo == null) {
        albumInfo = service.FindAlbumInfo(artist, album);
        cache.Insert(BuildInfoCache(AlbumInfoCacheKey, artist + album), albumInfo);
      }
      return albumInfo;
    }

    private string BuildInfoCache(string key, string value) {
      return string.Format("{0}:{1}", key, value);
    }
  }
}