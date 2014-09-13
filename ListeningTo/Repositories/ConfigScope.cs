using System;

namespace ListeningTo.Repositories {
  public class ConfigScope : IDisposable {
    IConfig oldConfig;

    public ConfigScope(IConfig config) {
      oldConfig = Config.Instance;
      Config.Instance = config;
    }
    public void Dispose() {
      Config.Instance = oldConfig;
    }
  }
}