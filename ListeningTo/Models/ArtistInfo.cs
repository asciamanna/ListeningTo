using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LastfmClient.Responses;

namespace ListeningTo.Models {
  public class ArtistInfo {
    public string Name { get; set; }
    public int? YearFormed { get; set; }
    public string PlaceFormed { get; set; }
    public string BioSummary { get; set; }

    public static ArtistInfo FromRepositoryObject(LastfmArtistInfo repoObject) {
      return new ArtistInfo {
        Name = repoObject.Name,
        YearFormed = repoObject.YearFormed,
        PlaceFormed = repoObject.PlaceFormed,
        BioSummary = repoObject.BioSummary,
      };
    }
  }
}