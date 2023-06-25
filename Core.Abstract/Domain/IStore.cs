namespace Core.Abstract.Domain;

public interface IStore<TValue>
{
    int Get(TValue value);

    TValue Get(int id);
}