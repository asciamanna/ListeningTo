using System.Net;
using System.Web.Http.Results;
using LastfmClient;
using LastfmClient.Responses;
using ListeningTo;
using ListeningTo.Controllers;
using ListeningTo.Models;
using ListeningTo.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace ListeningToTests.Controllers {
  [TestFixture]
  public class AlbumInfoControllerTest {
    private AlbumInfoController subject;
    private ILastfmRepository repository;

    [SetUp]
    public void Setup() {
      repository = MockRepository.GenerateStub<ILastfmRepository>();
      subject = new AlbumInfoController(repository);
    }

    [Test]
    public void GetAlbumInfo_Returns_Info_From_Repository() {
      var artist = CreateArtistName();
      var album = CreateAlbumName();
      var lastfmAlbumInfo = new LastfmAlbumInfo {
        Artist = artist,
        Name = album
      };
      repository.Stub(r => r.FindAlbumInfo(artist, album)).Return(lastfmAlbumInfo);

      var result = subject.GetAlbumInfo(artist, album) as OkNegotiatedContentResult<AlbumInfo>;
      
      Assert.That(result.Content.Name, Is.EqualTo(lastfmAlbumInfo.Name));
    }

    [Test]
    public void GetAlbumInfo_Returns_NotFound_When_No_AlbumInfo_Is_Found() {
      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(new LastfmAlbumInfo());

      var result = subject.GetAlbumInfo(CreateArtistName(), CreateAlbumName());
      
      Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetAlbumInfo_Returns_Error_When_Exception_Occurs() {
      var exceptionWhenLastfmIsDown = new WebException();
      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Throw(exceptionWhenLastfmIsDown);

      var result = subject.GetAlbumInfo(CreateArtistName(), CreateAlbumName());

      Assert.That(result, Is.InstanceOf<ExceptionResult>());
      Assert.That((result as ExceptionResult).Exception, Is.SameAs(exceptionWhenLastfmIsDown));
    }

    [Test]
    public void GetAlbumInfo_Returns_NotFound_When_LastfmException_Has_Invalid_Parameter_ErrorCode() {
      var lastfmException = new LastfmException("Album not found") { ErrorCode = ErrorCodes.InvalidParameter };
      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Throw(lastfmException);

      var result = subject.GetAlbumInfo(CreateArtistName(), "Not A Real Album");
      
      Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetAlbumInfo_Returns_InternalServerError_When_LastfmException_Is_Thrown_And_Its_Not_AlbumNotFound() {
      var lastfmException = new LastfmException("Invalid API key") { ErrorCode = ErrorCodes.InvalidApiKey };
      repository.Stub(r => r.FindAlbumInfo(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Throw(lastfmException);

      var result = subject.GetAlbumInfo(CreateArtistName(), CreateAlbumName());
     
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