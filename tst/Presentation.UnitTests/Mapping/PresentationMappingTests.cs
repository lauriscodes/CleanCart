using Application.Operations.UseCases.ProcessPayment;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.BSA.Models.ViewModels;
using Presentation.Functions;
using Xunit;

namespace Presentation.UnitTests.Mapping;

public class PresentationMappingTests
{
    protected readonly IFixture AutoFixture;
    protected readonly IMapper Mapper;

    public PresentationMappingTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();
        var startup = new PresentationServiceComposition();
        startup.ConfigureServices(services, configuration);
        // Get the real mapper from the service provider
        // Testing the profile itself will not catch runtime errors
        Mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        AutoFixture = new Fixture();
    }

    [Fact]
    public void CheckoutViewModel_MapsTo_ProcessPaymentCommand()
    {
        var checkoutViewModel = AutoFixture.Create<CheckoutViewModel>();

        var processPaymentCommand = Mapper.Map<ProcessPaymentCommand>(checkoutViewModel);

        processPaymentCommand.Should().BeEquivalentTo(checkoutViewModel);
    }
}