using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using LastfmClient;
using LastfmClient.Responses;
using ListeningTo.Models;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers {
  public class RecentTracksController : ApiController {
    readonly ILastfmUserRepository repository;

    public RecentTracksController() : this(new LastfmUserRepository()) { }

    public RecentTracksController(ILastfmUserRepository repository) {
      this.repository = repository;
    }

    public IHttpActionResult GetRecentTracks([FromUri] int count = 25) {
      try {
        var recentTracks = RecentTrack.FromLastfmObjects(repository.FindRecentTracks(count));
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
