public interface State
{
    string id { get; set; }
    void Enter();
    void Execute();
    void Exit();
}
