namespace ConsoleApp.Commands;

public interface ICommand<T>
{
    Task<T> Execute();
}