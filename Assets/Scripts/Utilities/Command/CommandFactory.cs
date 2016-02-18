/*
    Author - Rohit Bhosle
*/

using System;
using System.Collections.Generic;

namespace BehaviourInject
{
    public class CommandFactory : ICommandFactory
    {
        public CommandFactory() { }
        
        public TCommand Build<TCommand>() where TCommand : ICommand, new()
        {
            return Build<TCommand>(StringConstants.DefaultContextName);
        }

        public TCommand Build<TCommand>(string contextName) where TCommand : ICommand, new()
        {
            TCommand command = _commandPool.CreateCommand<TCommand>(contextName);
            return command;
        }

        public void Execute<TCommand>(params object[] args) where TCommand : ICommand, new()
        {
            TCommand command = _commandPool.GetCommand<TCommand>();
            command.Execute(args);            
        }        

        CommandPool _commandPool = new CommandPool();

        private class CommandPool
        {
            Dictionary<Type, ICommand> _command = new Dictionary<Type, ICommand>();

            public TCommand CreateCommand<TCommand>(string contextName) where TCommand : ICommand, new()
            {
                Type type = typeof(TCommand);
                ICommand command = null;
                if (_command.TryGetValue(type, out command))
                {
                    return (TCommand)command;
                }

                command = new TCommand();
                _command[type] = command;
                command.SetContext(contextName);
                command.ResolveSelf();
                return (TCommand)command;
            }

            public TCommand GetCommand<TCommand>() where TCommand : ICommand, new()
            {
                Type type = typeof(TCommand);

                ICommand command = null;

                if (_command.TryGetValue(type, out command))
                {
                    return (TCommand)command;
                }
                else
                {
                    throw new BehaviourInjectException(type + " of command should first be built");
                }                
            }
        }
    }
}
