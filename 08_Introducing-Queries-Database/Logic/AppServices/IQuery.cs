namespace Logic.AppServices;

public interface IQuery
{
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
{
    public TResult Handle(TQuery query);
}
