using System;
using System.Linq;
using System.Web.Http;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class TopArtistsController : ApiController {
    ILastfmRepository repository;

    public TopArtistsController() : this(new LastfmRepository()) { }

    public TopArtistsController(ILastfmRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetTopArtists([FromUri] int count = 25) {
      try {
        return RetrieveTopArtists(count);
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }

    private IHttpActionResult RetrieveTopArtists(int count) {
      var topArtists = TopArtist.FromRepositoryObjects(repository.FindTopArtists(count));
      if (topArtists.Any()) {
        return Ok(topArtists);
      }
      return NotFound();
    }
  }
}
