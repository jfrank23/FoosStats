namespace FoosStats.Core.Updaters
{
    public interface IUpdater<T>
    {
        T Update(T foosElement);
    }
}
