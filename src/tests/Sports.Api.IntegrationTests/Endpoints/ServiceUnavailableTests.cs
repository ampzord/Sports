using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Sports.Api.IntegrationTests.Infrastructure;

namespace Sports.Api.IntegrationTests.Endpoints;

[Collection("Unavailable")]
public class ServiceUnavailableTests(UnavailableServicesFactory factory)
{
    [Fact]
    public async Task GivenDatabaseDown_WhenGetLeagues_ThenReturns503WithAllProblemDetailsFields()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/leagues");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(503);
        problem.Title.Should().Be("Database Unavailable");
        problem.Detail.Should().NotBeNullOrEmpty();
        problem.Instance.Should().Be("GET /api/leagues");
        problem.Type.Should().Be("https://tools.ietf.org/html/rfc9110#section-15.6.4");
        problem.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task GivenRabbitMqDown_WhenSimulateMatches_ThenReturns503WithAllProblemDetailsFields()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/matches/simulate", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(503);
        problem.Title.Should().Be("Message Broker Unavailable");
        problem.Detail.Should().NotBeNullOrEmpty();
        problem.Instance.Should().Be("POST /api/matches/simulate");
        problem.Type.Should().Be("https://tools.ietf.org/html/rfc9110#section-15.6.4");
        problem.Extensions.Should().ContainKey("traceId");
    }
}
