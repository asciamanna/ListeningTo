using System;
using System.Web.Http;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class ArtistInfoController : ApiController {
    ILastfmRepository repository;

    public ArtistInfoController() : this(new LastfmRepository()) { }

    public ArtistInfoController(ILastfmRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetArtistInfo([FromUri] string artist) {
      try {
        var artistInfo = ArtistInfo.FromRepositoryObject(repository.FindArtistInfo(artist));
        if (!string.IsNullOrWhiteSpace(artistInfo.Name)) {
          return Ok(artistInfo);
        }
        return NotFound();
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }
  }
}
