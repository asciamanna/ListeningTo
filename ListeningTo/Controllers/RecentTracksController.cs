using System;
using System.Linq;
using System.Web.Http;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class RecentTracksController : ApiController {
    readonly ILastfmRepository repository;

    public RecentTracksController() : this(new LastfmRepository()) { }

    public RecentTracksController(ILastfmRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetRecentTracks([FromUri] int count = 25) {
      try {
        var recentTracks = RecentTrack.FromRepositoryObjects(repository.FindRecentTracks(count));
        if (recentTracks.Any()) {
          return Ok(recentTracks);
        }
        return NotFound();
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }
  }
}
