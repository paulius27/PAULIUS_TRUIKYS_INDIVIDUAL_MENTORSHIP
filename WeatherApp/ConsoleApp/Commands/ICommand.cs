namespace ConsoleApp.Commands;

public interface ICommand
{
    public Task Execute();
}