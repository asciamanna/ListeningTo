using System;
using System.Linq;
using System.Web.Http;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class RecentTracksController : ApiController {
    private readonly ILastfmRepository repository;

    public RecentTracksController() : this(new LastfmRepository()) { }

    public RecentTracksController(ILastfmRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetRecentTracks([FromUri] int count = 25) {
      try {
        return RetrieveRecentTracks(count);
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }

    private IHttpActionResult RetrieveRecentTracks(int count) {
      var recentTracks = RecentTrack.FromRepositoryObjects(repository.FindRecentTracks(count));
      if (recentTracks.Any()) {
        return Ok(recentTracks);
      }
      return NotFound();
    }
  }
}
