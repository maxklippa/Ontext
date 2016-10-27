namespace Ontext.DAL.Repositories.Contracts
{
    /// <summary>
    /// Base repository interface (used for DI)
    /// </summary>
    public interface ICustomRepository { }

    public interface ICustomRepository<T> : IEntityRepository<T>, ICustomRepository { }
}
