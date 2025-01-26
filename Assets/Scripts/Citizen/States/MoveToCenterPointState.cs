namespace Citizen.States
{
    public class MoveToCenterPointState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public MoveToCenterPointState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            // Можно сбросить/установить нужную анимацию (если нужно)
            // _context.Controller.Animator.SetMoveAnimation();
        }

        public void OnUpdate()
        {
            if (_context.CurrentPointToShopIndex <= 0)
            {
                var reached = _context.Controller.Movement.MoveTo(_context.CenterPoint);
                if (reached)
                {
                    if (_context.IsVisitedAllShops)
                    {
                        _stateMachine.SetState(CitizenState.MoveToTrain);
                    }
                    else
                    {
                        _stateMachine.SetState(CitizenState.ToShop);
                    }
                }
            }
            else
            {
                var shopPoints = _context.PointsToShop;
                var prevIndex = _context.CurrentPointToShopIndex - 1;

                var reached = _context.Controller.Movement.MoveTo(shopPoints[prevIndex].position);
                if (reached)
                {
                    _context.CurrentPointToShopIndex--;
                }
            }
        }

        public void OnExit()
        {
            
        }
    }
}