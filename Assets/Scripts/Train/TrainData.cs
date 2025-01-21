namespace Train
{
    public class TrainData
    {
        public TrainState CurrentState { get; private set; }

        public void ChangeState(TrainState newState)
        {
            CurrentState = newState;
        }
    }
}