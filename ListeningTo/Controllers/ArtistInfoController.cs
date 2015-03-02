using System;
using System.Web.Http;
using LastfmClient;
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
        return RetrieveArtistInfo(artist);
      }
      catch (LastfmException e) {
        return HandleLastfmException(e);
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }

    private IHttpActionResult RetrieveArtistInfo(string artist) {
      var artistInfo = ArtistInfo.FromRepositoryObject(repository.FindArtistInfo(artist));
      if (!string.IsNullOrWhiteSpace(artistInfo.Name)) {
        return Ok(artistInfo);
      }
      return NotFound();
    }

    private IHttpActionResult HandleLastfmException(LastfmException e) {
      if (e.ErrorCode == 6 && e.Message == "The artist you supplied could not be found") {
        return NotFound();
      }
      return InternalServerError(e);
    }

  }
}
