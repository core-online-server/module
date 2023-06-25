using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Core.Abstract.Extensions;

public static class SpanExtensions
{
    public static unsafe ReadOnlySpan<char> AsText(this Span<char> span)
    {
        var p = (char*)Unsafe.AsPointer(ref span.GetPinnableReference());

        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(p);
    }
}