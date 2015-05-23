using System.Net;
using System.Web.Http.Results;
using LastfmClient;
using LastfmClient.Responses;
using ListeningTo.Controllers;
using ListeningTo.Models;
using ListeningTo.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace ListeningToTests.Controllers {
  [TestFixture]
  public class ArtistInfoControllerTest {
    [Test]
    public void GetArtistInfo_Returns_Info_From_Repository() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      var artist = "Bobby Hutcherson";
      var lastfmArtistInfo = new LastfmArtistInfo {
        Name = artist
      };

      repository.Stub(r => r.FindArtistInfo(artist)).Return(lastfmArtistInfo);

      var result = new ArtistInfoController(repository).GetArtistInfo(artist) as OkNegotiatedContentResult<ArtistInfo>;
      Assert.That(result.Content.Name, Is.EqualTo(lastfmArtistInfo.Name));
    }

    [Test]
    public void GetArtistInfo_Returns_NotFound_If_No_ArtistInfo_Is_Found() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      repository.Stub(r => r.FindArtistInfo(Arg<string>.Is.Anything)).Return(new LastfmArtistInfo());

      Assert.That(new ArtistInfoController(repository).GetArtistInfo(CreateArtistName()), Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetAritsInfo_Returns_Error_If_Exception_Occurs() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new ArtistInfoController(repository);
      var exceptionWhenLastfmIsDown = new WebException();
      repository.Stub(r => r.FindArtistInfo(Arg<string>.Is.Anything)).Throw(exceptionWhenLastfmIsDown);

      var result = controller.GetArtistInfo(CreateArtistName());

      Assert.That(result, Is.InstanceOf<ExceptionResult>());
      Assert.That((result as ExceptionResult).Exception, Is.SameAs(exceptionWhenLastfmIsDown));
    }

    [Test]
    public void GetArtistInfo_Returns_NotFound_If_LastfmException_ArtistNotFound() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new ArtistInfoController(repository);
      var lastfmException = new LastfmException("The artist you supplied could not be found") { ErrorCode = 6 };

      repository.Stub(r => r.FindArtistInfo(Arg<string>.Is.Anything)).Throw(lastfmException);

      var result = controller.GetArtistInfo("Fake Band");
      Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetArtistInfo_Returns_InternalServerError_If_LastfmException_And_Not_ArtistNotFound() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new ArtistInfoController(repository);
      var lastfmException = new LastfmException("Invalid API key") { ErrorCode = 10 };

      repository.Stub(r => r.FindArtistInfo(Arg<string>.Is.Anything)).Throw(lastfmException);

      var result = controller.GetArtistInfo(CreateArtistName());
      Assert.That(result, Is.InstanceOf<ExceptionResult>());
    }

    private static string CreateArtistName() {
      return "Bobby Hutcherson";
    }
  }
}