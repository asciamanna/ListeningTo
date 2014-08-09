using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;

namespace ListeningTo.Models {
  public class RecentTrack {
    
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string AlbumArtLocation { get; set; }
    public string LastPlayed { get; set; }

    public static IEnumerable<RecentTrack> FromRepositoryObjects(IEnumerable<LastfmUserRecentTrack> lastfmRecentTracks) {
      var recentTracks = new List<RecentTrack>();
      foreach (var track in lastfmRecentTracks) {
        recentTracks.Add(
           new RecentTrack {
             Album = track.Album,
             Artist = track.Artist,
             AlbumArtLocation = track.LargeAlbumArtLocation,
             Name = track.Name,
             LastPlayed = PopulateLastPlayed(track)
           }
        );
      }
      return recentTracks;
    }

    private static string PopulateLastPlayed(LastfmUserRecentTrack track) {
      return track.IsNowPlaying ? "Now Playing" : ConvertToLocalString(track.LastPlayed);
    }
    static string ConvertToLocalString(DateTime? date) {
      if (!date.HasValue) {
        return String.Empty;
      }
      if (date.Value.Kind == DateTimeKind.Utc) {
        var easternZone = GetEasternTimeZone();
        var utcTime = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternZone).ToString("f");
      }
      return date.Value.ToString("f");
    }

    static TimeZoneInfo easternTimeZone = null;
    static TimeZoneInfo GetEasternTimeZone() {
      if (easternTimeZone == null) {
        easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
      }
      return easternTimeZone;
    }
  }
}