namespace Tasks
{
    public interface IPlayableTask
    {
        bool IsCompleted { get; protected set; }
    }
}