using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using LastfmClient.Responses;
using ListeningTo.Controllers;
using ListeningTo.Models;
using ListeningTo.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace ListeningToTests.Controllers {
  [TestFixture]
  public class RecentTracksControllerTest {
    [Test]
    public void GetRecentTracks_Defaults_Number_Of_Tracks_Requested() {
      var repository = MockRepository.GenerateMock<ILastfmUserRepository>();
      var defaultCount = 25;
      repository.Expect(r => r.FindRecentTracks(defaultCount)).Return(new List<LastfmUserRecentTrack>());

      new RecentTracksController(repository).GetRecentTracks();
      repository.VerifyAllExpectations();
    }

    [Test]
    public void GetRecentTracks_Returns_Tracks_From_Repository() {
      var repository = MockRepository.GenerateStub<ILastfmUserRepository>();
      var count = 2;
      var tracks = new List<LastfmUserRecentTrack>() {
        new LastfmUserRecentTrack(),
        new LastfmUserRecentTrack(),
      };
      
      repository.Stub(r => r.FindRecentTracks(count)).Return(tracks);

      var results = new RecentTracksController(repository).GetRecentTracks(count) as OkNegotiatedContentResult<IEnumerable<RecentTrack>>;
      Assert.That(results.Content.Count(), Is.EqualTo(tracks.Count));
    }

    [Test]
    public void GetRecentTracks_Returns_NotFound_If_No_Tracks_Are_Found() {
      var repository = MockRepository.GenerateStub<ILastfmUserRepository>();
     
      repository.Stub(r => r.FindRecentTracks(Arg<int>.Is.Anything)).Return(new List<LastfmUserRecentTrack>());

     Assert.That(new RecentTracksController(repository).GetRecentTracks(), Is.InstanceOf<NotFoundResult>());
    }
  }
}