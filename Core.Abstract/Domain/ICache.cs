namespace Core.Abstract.Domain;

public interface ICache<TValue> : IStore<TValue>
{
    void Hold(TValue value);

    void Release(TValue value);
}