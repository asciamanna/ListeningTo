using System;
using LastfmClient.Responses;

namespace ListeningTo.Repositories {
  public class RecentTrackWithSource : LastfmUserRecentTrack {
    public string MusicServiceName { get; set; }
    public string MusicServiceUrl { get; set; }

    public static RecentTrackWithSource FromLastFmObject(LastfmUserRecentTrack recentTrack) {
      return new RecentTrackWithSource {
        Album = recentTrack.Album,
        Artist = recentTrack.Artist,
        Name = recentTrack.Name,
        IsNowPlaying = recentTrack.IsNowPlaying,
        LastPlayed = recentTrack.LastPlayed,
        SmallImageLocation = recentTrack.SmallImageLocation,
        MediumImageLocation = recentTrack.MediumImageLocation,
        LargeImageLocation = recentTrack.LargeImageLocation,
        ExtraLargeImageLocation = recentTrack.ExtraLargeImageLocation,
        MusicServiceName = String.Empty,
        MusicServiceUrl = String.Empty,
      };
    }
  }
}