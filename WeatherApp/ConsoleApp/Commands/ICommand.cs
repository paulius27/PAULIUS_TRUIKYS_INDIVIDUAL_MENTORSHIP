namespace ConsoleApp.Commands;

public interface ICommand
{
    Task<string> Execute();
}