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

    public static IEnumerable<TopArtist> FromRepositoryObjects(IEnumerable<LastfmUserTopArtist> lastfmTopArtists) {
      return lastfmTopArtists.Select(CreateTopArtist);
    }

    private static TopArtist CreateTopArtist(LastfmUserTopArtist artist) {
      return new TopArtist {
        ArtistImageLocation = artist.LargeImageLocation,
        Name = artist.Name,
        Rank = artist.Rank,
        PlayCount = artist.PlayCount,
      };
    }
  }
}