using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqliteWasmTests.TestHelpers
{
    public sealed class MockJsModule : IJSObjectReference
    {
        public string? ModuleIdentifier { get; set; }
        public string? ModuleLocation { get; set; }

        public string? Identifier { get; private set; }
        public object?[]? Args { get; private set; }       

        public bool Disposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            Disposed = true;
            return ValueTask.CompletedTask;
        }

        public ValueTask<TValue> InvokeAsync<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(
            string identifier,
            object?[]? args) => InvokeAsync<TValue>(
                identifier,
                CancellationToken.None,
                args);

        public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            Identifier = identifier;
            Args = args;
            return ValueTask.FromResult(default(TValue)!);
        }
    }
}
