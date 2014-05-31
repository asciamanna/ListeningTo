using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;

namespace ListeningTo.Models {
  public class TopArtist {
    public string Name { get; set; }
    public int Rank { get; set; }
    public int PlayCount { get; set; }
    public string ArtistImageLocation { get; set; }

    public static IEnumerable<TopArtist> FromLastfmObjects(IEnumerable<LastfmUserTopArtist> lastfmTopArtists) {
      var topArtists = new List<TopArtist>();
      foreach (var artist in lastfmTopArtists) {
        topArtists.Add(
           new TopArtist {
             ArtistImageLocation = artist.LargeArtistImageLocation,
             Name = artist.Name,
             Rank = artist.Rank,
             PlayCount = artist.PlayCount,
           }
        );
      }
      return topArtists;
    }
  }
}