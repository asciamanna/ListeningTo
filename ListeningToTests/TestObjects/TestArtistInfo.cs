using LastfmClient.Responses;

namespace ListeningToTests.TestObjects {
  public class TestArtistInfo {
    public static LastfmArtistInfo Create() {
      return new LastfmArtistInfo {
        Name = "Bobby Hutcherson",
        YearFormed = 1963,
        PlaceFormed = "New York City, New York, United States",
        BioSummary = "Shorter form of artist biography will be included here."
      };
    }
  }
}