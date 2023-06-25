namespace Core.Abstract.Domain;

public interface IPool<TValue> : IStore<TValue>
{
    TValue Lease();

    void Release(TValue entity);

    void Flush();
}