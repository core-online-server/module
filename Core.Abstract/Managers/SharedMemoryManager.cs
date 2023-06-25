using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Core.Abstract.Managers;

public class SharedMemoryManager<T> : MemoryManager<T>
{
    private static readonly ConcurrentQueue<SharedMemoryManager<T>> Collection = new();

    public static SharedMemoryManager<T> Lease(Span<T> span)
    {
        var manager = Collection.TryDequeue(out var result) ? result : new SharedMemoryManager<T>();

        manager.Initialize(span);

        return manager;
    }

    private unsafe void* _pointer;

    private int _length;

    protected SharedMemoryManager()
    {
    }

    public unsafe void Initialize(Span<T> span)
    {
        _pointer = Unsafe.AsPointer(ref span.GetPinnableReference());

        _length = span.Length;
    }

    protected override void Dispose(bool disposing)
    {
        Collection.Enqueue(this);
    }

    public override unsafe Span<T> GetSpan()
    {
        return new Span<T>(_pointer, _length);
    }

    public override unsafe MemoryHandle Pin(int elementIndex = 0)
    {
        return new MemoryHandle(_pointer);
    }

    public override void Unpin()
    {
    }
}