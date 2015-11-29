using System;
using LastfmClient.Responses;

namespace ListeningToTests.TestObjects {
  public class TestAlbumInfo {
    public static LastfmAlbumInfo Create() {
      return new LastfmAlbumInfo {
        Artist = "Bobby Hutcherson",
        Name = "Spiral",
        ReleaseDate = new DateTime(1968, 5, 20),
        WikiSummary = "Album summary goes here",
      };
    }
  }
}