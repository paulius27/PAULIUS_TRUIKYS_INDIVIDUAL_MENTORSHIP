namespace ConsoleApp.Commands;

public class CloseApplicationCommand : ICommand
{
    public Task Execute()
    {
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}
