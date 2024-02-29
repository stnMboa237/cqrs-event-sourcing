using CQRS.Core.Commands;

namespace CQRS.Core.Iinfrastructure
{
    public interface ICommandDispatcher
    {
        /// <summary>
        /// RegisterHandler uses Generics. It takes a Func<T> delegate(with input parameter generic type T) as parameter. The output parameter is a Task<T> where <T> implements BaseCommand class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void RegisterHandler<T>(Func<T, Task> handler) where T: BaseCommand;
        /// <summary>
        /// Seand Async takes as input a BaseCommand object, whih will allow us to pass any of our concrete command object.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task SendAsync(BaseCommand command);

    }
}