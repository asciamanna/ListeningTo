﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using LastfmClient.Responses;
using ListeningTo.Controllers;
using ListeningTo.Models;
using ListeningTo.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace ListeningToTests.Controllers {
  [TestFixture]
  public class TopArtistsControllerTest {
    [Test]
    public void GetTopArtists_Defaults_Number_Of_Artists_Requested() {
      var repository = MockRepository.GenerateMock<ILastfmRepository>();
      var defaultCount = 25;
      repository.Expect(r => r.FindTopArtists(defaultCount)).Return(new List<LastfmUserTopArtist>());

      new TopArtistsController(repository).GetTopArtists();
      repository.VerifyAllExpectations();
    }

    [Test]
    public void GetTopArtists_Returns_Artists_From_Repository() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      var count = 2;
      var artists = new List<LastfmUserTopArtist>() {
        new LastfmUserTopArtist(),
        new LastfmUserTopArtist(),
      };

      repository.Stub(r => r.FindTopArtists(count)).Return(artists);

      var results = new TopArtistsController(repository).GetTopArtists(count) as OkNegotiatedContentResult<IEnumerable<TopArtist>>;
      Assert.That(results.Content.Count(), Is.EqualTo(artists.Count));
    }

    [Test]
    public void GetTopArtists_Returns_NotFound_If_No_Artists_Are_Found() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();
      repository.Stub(r => r.FindTopArtists(Arg<int>.Is.Anything)).Return(new List<LastfmUserTopArtist>());

      Assert.That(new TopArtistsController(repository).GetTopArtists(), Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetTopArtists_Returns_Error_If_Exception_Occurs() {
      var repository = MockRepository.GenerateStub<ILastfmRepository>();

      var controller = new TopArtistsController(repository);
      var exceptionWhenLastfmIsDown = new WebException();
      repository.Stub(r => r.FindTopArtists(Arg<int>.Is.Anything)).Throw(exceptionWhenLastfmIsDown);

      var result = controller.GetTopArtists();

      Assert.That(result, Is.InstanceOf<ExceptionResult>());
      Assert.That((result as ExceptionResult).Exception, Is.SameAs(exceptionWhenLastfmIsDown));
    }
  }
}