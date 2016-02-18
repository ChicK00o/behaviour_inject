/*
    Author - Rohit Bhosle
*/

namespace BehaviourInject
{
    internal interface ICommandFactory
    {
        TCommand Build<TCommand>() where TCommand : ICommand, new();
        TCommand Build<TCommand>(string contextName) where TCommand : ICommand, new();
        void Execute<TCommand>(params object[] args) where TCommand : ICommand, new();
    }
}
