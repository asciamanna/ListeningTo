using System;
using System.Web.Http;
using LastfmClient;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class AlbumInfoController : ApiController {
    private readonly ILastfmRepository repository;

    public AlbumInfoController() : this(new LastfmRepository()) { }

    public AlbumInfoController(ILastfmRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetAlbumInfo([FromUri] string artist, [FromUri] string album) {
      try {
        return RetrieveAlbumInfo(artist, album);
      }
      catch (LastfmException e) {
        return HandleLastfmException(e);
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }

    private IHttpActionResult RetrieveAlbumInfo(string artist, string album) {
      var albumInfo = AlbumInfo.FromRepositoryObject(repository.FindAlbumInfo(artist, album));
      if (!string.IsNullOrWhiteSpace(albumInfo.Name)) {
        return Ok(albumInfo);
      }
      return NotFound();
    }

    private IHttpActionResult HandleLastfmException(LastfmException e) {
      if (e.ErrorCode == ErrorCodes.InvalidParameter && e.Message == "Album not found") {
        return NotFound();
      }
      return InternalServerError(e);
    }
  }
}
