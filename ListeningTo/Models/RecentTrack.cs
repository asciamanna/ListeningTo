using System;
using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;
using ListeningTo.Repositories;

namespace ListeningTo.Models {
  public class RecentTrack {
    private const string NowPlaying = "Now Playing";

    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string AlbumArtLocation { get; set; }
    public string LastPlayed { get; set; }
    public string MusicServiceName { get; set; }
    public string MusicServiceUrl { get; set; }

    public static IEnumerable<RecentTrack> FromRepositoryObjects(IEnumerable<RecentTrackWithSource> tracksWithSource) {
      return tracksWithSource.Select(CreateTrack);
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
      return track.IsNowPlaying ? NowPlaying : ConvertToLocalString(track.LastPlayed);
    }
    static string ConvertToLocalString(DateTime? date) {
      if (!date.HasValue) {
        return String.Empty;
      }
      return date.Value.Kind == DateTimeKind.Utc ? ConvertToEasternTime(date) : date.Value.ToString("f");
    }

    private static string ConvertToEasternTime(DateTime? date) {
      var utcTime = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
      return TimeZoneInfo.ConvertTimeFromUtc(utcTime, GetEasternTimeZone()).ToString("f");
    }

    static TimeZoneInfo easternTimeZone = null;
    static TimeZoneInfo GetEasternTimeZone() {
      return easternTimeZone ?? (easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
    }
  }
}