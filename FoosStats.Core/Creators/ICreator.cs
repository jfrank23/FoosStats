namespace FoosStats.Core.Creators
{
    public interface ICreator<T>
    {
        T Create(T element);
    }

}
