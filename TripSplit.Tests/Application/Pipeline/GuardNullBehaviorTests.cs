using FluentAssertions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TripSplit.Application.Pipeline;
using Xunit;

namespace TripSplit.Tests.Application.Pipeline
{
    public class GuardNullBehaviorTests
    {
        private sealed record Ping(string Message) : IRequest<string>;

        [Fact]
        public async Task Throws_On_Null_Request()
        {
            var behavior = new GuardNullBehavior<Ping, string>();

            // Twoja wersja MediatR wymaga tokena w delegacie:
            RequestHandlerDelegate<string> next = (CancellationToken _) => Task.FromResult("ok");

            Func<Task> act = async () =>
                await behavior.Handle(null!, next, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Passes_Through_When_Not_Null()
        {
            var behavior = new GuardNullBehavior<Ping, string>();

            RequestHandlerDelegate<string> next = (CancellationToken _) => Task.FromResult("ok");

            var result = await behavior.Handle(new Ping("hi"), next, CancellationToken.None);
            result.Should().Be("ok");
        }
    }
}
