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
  public class AlbumInfoControllerTest {
    [Test]
    public void GetAlbumInfo_Returns_Info_From_Repository() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      var artist = CreateArtistName();
      var album = CreateAlbumName();
      var lastfmAlbumInfo = new LastfmAlbumInfo {
        Artist = artist,
        Name = album
      };

      repository.Stub(r => r.FindAlbumInfo(artist, album)).Return(lastfmAlbumInfo);

      var result = new AlbumInfoController(repository).GetAlbumInfo(artist, album) as OkNegotiatedContentResult<AlbumInfo>;
      Assert.That(result.Content.Name, Is.EqualTo(lastfmAlbumInfo.Name));
    }

    [Test]
    public void GetAlbumInfo_Returns_NotFound_If_No_AlbumInfo_Is_Found() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(new LastfmAlbumInfo());

      Assert.That(new AlbumInfoController(repository).GetAlbumInfo(CreateArtistName(), CreateAlbumName()), Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetAlbumInfo_Returns_Error_If_Exception_Occurs() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new AlbumInfoController(repository);
      var exceptionWhenLastfmIsDown = new WebException();
      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Throw(exceptionWhenLastfmIsDown);

      var result = controller.GetAlbumInfo(CreateArtistName(), CreateAlbumName());

      Assert.That(result, Is.InstanceOf<ExceptionResult>());
      Assert.That((result as ExceptionResult).Exception, Is.SameAs(exceptionWhenLastfmIsDown));
    }

    [Test]
    public void GetAlbumInfo_Returns_NotFound_If_LastfmException_AlbumNotFound() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new AlbumInfoController(repository);
      var lastfmException = new LastfmException("Album not found") { ErrorCode = 6 };

      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Throw(lastfmException);

      var result = controller.GetAlbumInfo(CreateArtistName(), "Not A Real Album");
      Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetAlbumInfo_Returns_InternalServerError_If_LastfmException_And_Not_AlbumNotFound() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new AlbumInfoController(repository);
      var lastfmException = new LastfmException("Invalid API key") { ErrorCode = 10 };

      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Throw(lastfmException);

      var result = controller.GetAlbumInfo(CreateArtistName(), CreateAlbumName());
      Assert.That(result, Is.InstanceOf<ExceptionResult>());
    }

    private static string CreateArtistName() {
      return "Bobby Hutcherson";
    }

    private static string CreateAlbumName() {
      return "Spiral";
    }
  }
}