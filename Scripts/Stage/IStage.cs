namespace Frame.Stage
{
    public interface IStage
    {
        void OnEnter();

        void OnExit();

        void OnProcess();
    }
}