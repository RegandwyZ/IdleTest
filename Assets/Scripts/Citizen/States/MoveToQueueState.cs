namespace Citizen.States
{
    public class MoveToQueueState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public MoveToQueueState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
        }

        public void OnUpdate()
        {
            if (_context.QueuePoint == null)
            {
                _stateMachine.SetState(CitizenState.Idle);
                return;
            }

            bool reached = _context.Controller.Movement.MoveTo(_context.QueuePoint.transform.position);
            if (reached)
            {
                if (_context.QueuePoint.IsTradePoint)
                {
                    _stateMachine.SetState(CitizenState.Trading);
                }
                else
                {
                    _stateMachine.SetState(CitizenState.Idle);
                }
            }
        }

        public void OnExit()
        {
        }
    }
}