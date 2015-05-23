using System.Configuration;

namespace ListeningTo.Repositories {
  public interface IConfig {
    string LastFmApiKey { get; }
    string LastFmUser { get; }
  }

  public class Config : IConfig {
    private static IConfig config;

    private Config() { }
    
    public static IConfig Instance {
      get {
        return config ?? new Config();
      }
      set { config = value; }
    }

    public string LastFmApiKey {
      get {
        return GetConfigValue("lastFmApiKey");
      }
    }

    public string LastFmUser {
      get {
        return GetConfigValue("lastFmUser");
      }
    }

    private static string GetConfigValue(string key) {
      var value = ConfigurationManager.AppSettings[key];
      if (string.IsNullOrWhiteSpace(value)) {
        throw new ConfigurationErrorsException(string.Format("{0} must be specified in App.config", key));
      }
      return value;
    }
  }
}