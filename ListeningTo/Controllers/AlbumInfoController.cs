using System;
using System.Web.Http;
using LastfmClient;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class AlbumInfoController : ApiController {
    ILastfmRepository repository;

    public AlbumInfoController() : this(new LastfmRepository()) { }

    public AlbumInfoController(ILastfmRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetAlbumInfo([FromUri] string artist, [FromUri] string album) {
      try {
        var albumInfo = AlbumInfo.FromRepositoryObject(repository.FindAlbumInfo(artist, album));
        if (!string.IsNullOrWhiteSpace(albumInfo.Name)) {
          return Ok(albumInfo);
        }
        return NotFound();
      }
      catch (LastfmException e) {
        if (e.ErrorCode == 6 && e.Message == "Album not found") {
          return NotFound();
        }
        return InternalServerError(e);
      }
      catch (Exception e) {
        return InternalServerError(e);
      }
    }
  }
}
