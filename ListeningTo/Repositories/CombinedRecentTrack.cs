using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;

namespace ListeningTo.Repositories {
  public class CombinedRecentTrack : LastfmUserRecentTrack {
    public string MusicServiceName { get; set; }
    public string MusicServiceUrl { get; set; }

    public static CombinedRecentTrack FromLastFmObject(LastfmUserRecentTrack recentTrack) {
      return new CombinedRecentTrack {
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