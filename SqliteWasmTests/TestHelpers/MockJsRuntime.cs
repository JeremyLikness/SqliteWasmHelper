using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqliteWasmTests.TestHelpers
{
    public class MockJsRuntime : IJSRuntime
    {
        public IJSObjectReference JSObjectReference { get; set; } = default!;

        public ValueTask<TValue> InvokeAsync<
            [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(
            string identifier,
            object?[]? args) => InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public ValueTask<TValue> InvokeAsync<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(
            string identifier,
            CancellationToken cancellationToken,
            object?[]? args)
        {
            if (typeof(TValue) == typeof(IJSObjectReference)
                && JSObjectReference is MockJsModule module)
            {
                module.ModuleIdentifier = identifier;
                if (args != null)
                {
                    module.ModuleLocation =
                        args
                        .First()!
                        .ToString();
                }

                return ValueTask.FromResult((TValue)JSObjectReference);
            }

            return default!;
        }
    }
}
