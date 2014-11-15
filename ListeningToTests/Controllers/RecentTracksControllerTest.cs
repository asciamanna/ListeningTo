using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
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
      var repository = MockRepository.GenerateMock<ILastfmRepository>();
      var defaultCount = 25;
      repository.Expect(r => r.FindRecentTracks(defaultCount)).Return(new List<CombinedRecentTrack>());

      new RecentTracksController(repository).GetRecentTracks();
      repository.VerifyAllExpectations();
    }

    [Test]
    public void GetRecentTracks_Returns_Tracks_From_Repository() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      var count = 2;
      var tracks = new List<CombinedRecentTrack>() { new CombinedRecentTrack(), new CombinedRecentTrack(), };
      
      repository.Stub(r => r.FindRecentTracks(count)).Return(tracks);

      var results = new RecentTracksController(repository).GetRecentTracks(count) as OkNegotiatedContentResult<IEnumerable<RecentTrack>>;
      Assert.That(results.Content.Count(), Is.EqualTo(tracks.Count));
    }

    [Test]
    public void GetRecentTracks_Returns_NotFound_If_No_Tracks_Are_Found() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
     
      repository.Stub(r => r.FindRecentTracks(Arg<int>.Is.Anything)).Return(new List<CombinedRecentTrack>());

     Assert.That(new RecentTracksController(repository).GetRecentTracks(), Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetRecentTracks_Returns_Error_If_Exception_Occurs() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new RecentTracksController(repository);
      var lastfmException = new WebException();
      repository.Stub(r => r.FindRecentTracks(Arg<int>.Is.Anything)).Throw(lastfmException);

      var result = controller.GetRecentTracks();

      Assert.That(result, Is.InstanceOf<ExceptionResult>());
      Assert.That((result as ExceptionResult).Exception, Is.SameAs(lastfmException));
    }
  }
}