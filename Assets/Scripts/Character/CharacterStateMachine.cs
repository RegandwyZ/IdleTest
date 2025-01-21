using Character.States;

namespace Character
{
    public class CharacterStateMachine
    {
        private ICharacterState _currentState;

        public void SetState(ICharacterState newState)
        {
            _currentState?.Exit();
            
            _currentState = newState;
            
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}