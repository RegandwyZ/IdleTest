namespace Character.States
{
    public interface ICharacterState
    {
        void Enter();
        void Update();
        void Exit();
    }
}