using System;
using System.Web;
using System.Web.Caching;

namespace ListeningTo.Repositories {
  public interface ILastfmCache {
    void Insert(string key, object obj);
    object Get(string cacheKey);
  }

  public class LastfmCache : ILastfmCache {
    public const string RecentTracksCacheKey = "lastfmuser-recenttracks";
    public const string TopArtistsCacheKey = "lastfmuser-topartists";
    public const string ArtistInfoCacheKey = "lastfmartist-artistinfo";
    public const string AlbumInfoCacheKey = "lastfmalbum-albuminfo";

    public void Insert(string key, object obj) {
      HttpRuntime.Cache.Insert(key, obj, null, DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration);
    }

    public object Get(string cacheKey) {
      return HttpRuntime.Cache.Get(cacheKey);
    }
  }
}
