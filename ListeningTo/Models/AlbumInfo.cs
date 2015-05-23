using System;
using LastfmClient.Responses;

namespace ListeningTo.Models {
  public class AlbumInfo {
    public string Artist { get; set; }
    public string Name { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string WikiSummary { get; set; }

    public static AlbumInfo FromRepositoryObject(LastfmAlbumInfo repoObject) {
      return new AlbumInfo {
        Name = repoObject.Name,
        Artist = repoObject.Artist,
        ReleaseDate = repoObject.ReleaseDate,
        WikiSummary = repoObject.WikiSummary,
      };
    }
  }
}