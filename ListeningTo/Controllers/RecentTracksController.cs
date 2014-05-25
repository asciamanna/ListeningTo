using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using LastfmClient;
using LastfmClient.Responses;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class RecentTracksController : ApiController {
    readonly ILastfmUserRepository repository;

    public RecentTracksController() : this(new LastfmUserRepository()) { }

    public RecentTracksController(ILastfmUserRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetRecentTracks([FromUri] int count = 25) {
      var recentTracks = repository.FindRecentTracks(count);
      foreach (var track in recentTracks) {
        track.LastPlayed = ConvertToLocal(track.LastPlayed);
      }
      if (recentTracks.Any()) {
        return Ok(recentTracks);
      }
      return NotFound();
    }

    static DateTime? ConvertToLocal(DateTime? date) {
      if (date == null) {
        return null;
      }
      if (date.Value.Kind == DateTimeKind.Utc) {
        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        return DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(date.Value, DateTimeKind.Utc), easternZone), DateTimeKind.Unspecified);
      }
      else return date;
    }
  }
}
