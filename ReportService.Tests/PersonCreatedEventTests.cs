using MassTransit;
using MassTransit.Testing;
using ReportService.Infrastructure.EventHandlers;
using SharedKernel.Events;

namespace ReportService.Tests
{
    public class PersonCreatedEventTests : IAsyncLifetime
    {
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<PersonCreatedEventConsumer> _consumerHarness;

        public async Task InitializeAsync()
        {
            _harness = new InMemoryTestHarness();
            _consumerHarness = _harness.Consumer<PersonCreatedEventConsumer>();

            await _harness.Start();
        }

        public async Task DisposeAsync()
        {
            await _harness.Stop();
        }

        [Fact]
        public async Task PersonCreatedEvent_ShouldBeConsumed()
        {
            // Arrange
            var testEvent = new PersonCreatedEvent(
                Guid.NewGuid(),
                "Murat Bolulu",
                DateTime.UtcNow
            );

            // Act
            await _harness.InputQueueSendEndpoint.Send(testEvent);

            // Assert
            Assert.True(await _harness.Consumed.Any<PersonCreatedEvent>());
            Assert.True(await _consumerHarness.Consumed.Any<PersonCreatedEvent>());
        }
    }
}
