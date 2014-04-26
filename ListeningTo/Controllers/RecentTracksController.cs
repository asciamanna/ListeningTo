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

namespace ListeningTo.Controllers
{
    public class RecentTracksController : ApiController
    {
      public IEnumerable<LastfmUserRecentTrack> GetRecentTracks([FromUri] int count = 25) {
        
        var recentTracks = new LastfmUserRepository().FindRecentTracks(count);
        return recentTracks;
      }
    }
}
