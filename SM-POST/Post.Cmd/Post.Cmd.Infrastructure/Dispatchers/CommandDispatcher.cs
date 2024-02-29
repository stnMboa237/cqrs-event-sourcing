using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;
using CQRS.Core.Iinfrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        /// <summary>
        /// _handlers will hold all of our command handling methods as function delegates
        /// </summary>
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new Dictionary<Type, Func<BaseCommand, Task>>();
        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            //we throw an exception if the handler is already registered into the _handlers dictionnary
            if(_handlers.ContainsKey(typeof(T))) 
            {
                throw new IndexOutOfRangeException("You cannot register the same command handler twice!");
            }

            //if the handler is not already registered into the _handlers dictionnary, we add it into the _handlers
            // key = typeof(T) <=> concrete command Object type as (NewPostCommand, AddCommentCommand...)
            // value = handler(x) where 'x' is a cast of baseCommand to specific concrete command object
            _handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task SendAsync(BaseCommand command)
        {
            if(_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task> handler)) 
            {
                await handler(command);
            }
            else
            {
                throw new ArgumentNullException(nameof(handler), "No command handler was registered!");
            }
        }
    }
}