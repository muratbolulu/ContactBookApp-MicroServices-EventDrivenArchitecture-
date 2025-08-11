using MassTransit;
using MassTransit.Testing;
using SharedKernel.Events; // event sınıfının namespace'i
using ReportService.Application.EventHandlers; // consumer'ın namespace'i
using Xunit;
using ReportService.Infrastructure.EventHandlers;

namespace ReportService.Tests;

public class PersonCreatedEventTests
{
    [Fact]
    public async Task PersonCreatedEvent_ShouldBeConsumed()
    {
        // Arrange - InMemory broker başlat
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer<PersonCreatedEventConsumer>();

        await harness.Start();
        try
        {
            // Act - Event gönder
            await harness.InputQueueSendEndpoint.Send(new PersonCreatedEvent(
                Guid.NewGuid(),
                "Ali Veli",
                DateTime.UtcNow
            ));

            // Assert - Event publish ve consume edilmiş mi?
            Assert.True(await harness.Consumed.Any<PersonCreatedEvent>(), "Event publish edilmedi!");
            Assert.True(await consumerHarness.Consumed.Any<PersonCreatedEvent>(), "Event consumer tarafından alınmadı!");
        }
        finally
        {
            await harness.Stop();
        }
    }
}
