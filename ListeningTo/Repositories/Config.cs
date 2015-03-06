using System.Configuration;

namespace ListeningTo.Repositories {
  public interface IConfig {
    string LastFmApiKey { get; }
    string LastFmUser { get; }
  }

  public class Config : IConfig {
    static IConfig config;

    private Config() { }
    public static IConfig Instance {
      get {
        return config ?? new Config();
      }
      set { config = value; }
    }

    public string LastFmApiKey {
      get {
        var lastFmApiKey = ConfigurationManager.AppSettings["lastFmApiKey"];
        if (string.IsNullOrWhiteSpace(lastFmApiKey)) {
          throw new ConfigurationErrorsException("lastFmApiKey must be specified in App.config");
        }
        return lastFmApiKey;
      }
    }

    public string LastFmUser {
      get {
        var lastFmUser = ConfigurationManager.AppSettings["lastFmUser"];
        if (string.IsNullOrWhiteSpace(lastFmUser)) {
          throw new ConfigurationErrorsException("lastFmUser must be specified in App.config");
        }
        return lastFmUser;
      }
    }
  }
}