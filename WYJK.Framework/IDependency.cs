namespace WYJK.Framework
{
    /// <summary>
    /// 依赖注入基础接口
    /// </summary>
    public interface IDependency
    {
    }
    /// <summary>
    /// 单例依赖接口
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }
    /// <summary>
    /// 工作单元依赖接口
    /// </summary>
    public interface IUnitOfWorkDependency : IDependency
    {
    }
    /// <summary>
    /// 事务依赖接口
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }
}
