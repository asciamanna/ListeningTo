using System.Collections.Generic;
using System.Linq;
using LastfmClient;
using LastfmClient.Responses;

namespace ListeningTo.Repositories {
  public interface ILastfmRepository {
    IEnumerable<RecentTrackWithSource> FindRecentTracks(int count);
    IEnumerable<LastfmUserTopArtist> FindTopArtists(int count);
    LastfmArtistInfo FindArtistInfo(string artist);
    LastfmAlbumInfo FindAlbumInfo(string artist, string album);
  }

  public class LastfmRepository : ILastfmRepository {
    readonly ILastfmService service;
    readonly ILastfmCache cache;

    public LastfmRepository() : this(new LastfmService(Config.Instance.LastFmApiKey), new LastfmCache()) { }

    public LastfmRepository(ILastfmService service, ILastfmCache cache) {
      this.service = service;
      this.cache = cache;
    }

    public IEnumerable<RecentTrackWithSource> FindRecentTracks(int count) {
      List<RecentTrackWithSource> recentTracks;
      var cachedTracks = cache.Get(LastfmCache.RecentTracksCacheKey);

      if (cachedTracks == null) {
        recentTracks = RetrieveRecentTracksWithSource(count);
        cache.Insert(LastfmCache.RecentTracksCacheKey, recentTracks);
      }
      else {
        recentTracks = cachedTracks as List<RecentTrackWithSource>;
      }
      return recentTracks;
    }

    private List<RecentTrackWithSource> RetrieveRecentTracksWithSource(int count) {
      var lastfmRecentTracks = service.FindRecentTracks(Config.Instance.LastFmUser, count);
      var tracksWithSource = lastfmRecentTracks.Select(RecentTrackWithSource.FromLastFmObject).ToList();
      if (lastfmRecentTracks.Any(rt => rt.IsNowPlaying)) {
        ApplyMusicSource(tracksWithSource);
      }
      return tracksWithSource;
    }

    private void ApplyMusicSource(List<RecentTrackWithSource> recentTracks) {
      var playingFrom = service.FindMusicSource(Config.Instance.LastFmUser);
      var currentTrack = recentTracks.First(rt => rt.IsNowPlaying);
      currentTrack.MusicServiceName = playingFrom.MusicServiceName;
      currentTrack.MusicServiceUrl = playingFrom.MusicServiceUrl;
    }

    public IEnumerable<LastfmUserTopArtist> FindTopArtists(int count) {
      var topArtists = new List<LastfmUserTopArtist>();
      var cachedArtists = cache.Get(LastfmCache.TopArtistsCacheKey);

      if (cachedArtists == null) {
        topArtists = service.FindTopArtists(Config.Instance.LastFmUser, count);
        cache.Insert(LastfmCache.TopArtistsCacheKey, topArtists);
      }
      else {
        topArtists = cachedArtists as List<LastfmUserTopArtist>;
      }
      return topArtists;
    }

    public LastfmArtistInfo FindArtistInfo(string artist) {
      var artistInfo = cache.Get(BuildInfoCache(LastfmCache.ArtistInfoCacheKey, artist)) as LastfmArtistInfo;

      if (artistInfo == null) {
        artistInfo = service.FindArtistInfo(artist);
        cache.Insert(BuildInfoCache(LastfmCache.ArtistInfoCacheKey, artist), artistInfo);
      }
      return artistInfo;
    }

    public LastfmAlbumInfo FindAlbumInfo(string artist, string album) {
      var albumInfo = cache.Get(BuildInfoCache(LastfmCache.AlbumInfoCacheKey, artist + album)) as LastfmAlbumInfo;

      if (albumInfo == null) {
        albumInfo = service.FindAlbumInfo(artist, album);
        cache.Insert(BuildInfoCache(LastfmCache.AlbumInfoCacheKey, artist + album), albumInfo);
      }
      return albumInfo;
    }

    private string BuildInfoCache(string key, string value) {
      return string.Format("{0}:{1}", key, value);
    }
  }
}