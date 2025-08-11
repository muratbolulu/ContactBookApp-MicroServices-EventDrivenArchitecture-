using MassTransit;
using MassTransit.Testing;
using SharedKernel.Events;
using ReportService.Infrastructure.EventHandlers;

namespace ReportService.Tests;

public class PersonCreatedEventTests
{
    [Fact]
    public async Task PersonCreatedEvent_ShouldBeConsumed()
    {
        // Arrange - InMemory broker başlat  
        using var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer<PersonCreatedEventConsumer>();

        await harness.Start();
        try
        {
            // Act - Event gönder  
            await harness.InputQueueSendEndpoint.Send(new PersonCreatedEvent(
                Guid.NewGuid(),
                "Murat Bolulu",
                DateTime.UtcNow
            ));

            // Asserts
            Assert.True(await harness.Consumed.Any<PersonCreatedEvent>(), "Event publish edilmedi!");
            Assert.True(await consumerHarness.Consumed.Any<PersonCreatedEvent>(), "Event consumer tarafından alınmadı!");
        }
        finally
        {
            await harness.Stop();
        }
    }
}
