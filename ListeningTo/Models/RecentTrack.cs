using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;
using ListeningTo.Repositories;

namespace ListeningTo.Models {
  public class RecentTrack {
    private const string nowPlaying = "Now Playing";

    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string AlbumArtLocation { get; set; }
    public string LastPlayed { get; set; }
    public string MusicServiceName { get; set; }
    public string MusicServiceUrl { get; set; }

    public static IEnumerable<RecentTrack> FromRepositoryObjects(IEnumerable<RecentTrackWithSource> combinedRecentTracks) {
      return combinedRecentTracks.Select(CreateTrack);
    }

    private static RecentTrack CreateTrack(RecentTrackWithSource track) {
      return new RecentTrack {
        Album = track.Album,
        Artist = track.Artist,
        AlbumArtLocation = track.LargeImageLocation,
        Name = track.Name,
        LastPlayed = PopulateLastPlayed(track),
        MusicServiceName = track.MusicServiceName,
        MusicServiceUrl = track.MusicServiceUrl,
      };
    }

    private static string PopulateLastPlayed(LastfmUserRecentTrack track) {
      return track.IsNowPlaying ? nowPlaying : ConvertToLocalString(track.LastPlayed);
    }
    static string ConvertToLocalString(DateTime? date) {
      if (!date.HasValue) {
        return String.Empty;
      }
      if (date.Value.Kind == DateTimeKind.Utc) {
        return ConvertToEasternTime(date);
      }
      return date.Value.ToString("f");
    }

    private static string ConvertToEasternTime(DateTime? date) {
      var utcTime = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
      return TimeZoneInfo.ConvertTimeFromUtc(utcTime, GetEasternTimeZone()).ToString("f");
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