using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LastfmClient.Responses;
using LastfmClient;
using ListeningTo.Repositories;

namespace ListeningTo.Controllers
{
    public class TopArtistsController : ApiController
    {
      public IEnumerable<LastfmUserTopArtist> GetTopArtists([FromUri] int count = 25) {
        return new LastfmUserRepository().FindTopArtists(count);
      }
    }
}
