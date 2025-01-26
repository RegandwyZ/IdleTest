namespace Citizen.States
{
    public class MoveToMarketPlaceState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public MoveToMarketPlaceState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            _context.Controller.Animator.SetMoveAnimation();
        }

        public void OnUpdate()
        {
            var pointsToTown = _context.PointsToTown;
            var index = _context.CurrentPointToTownIndex;
            
            bool reached = _context.Controller.Movement.MoveTo(pointsToTown[index].position);
            if (reached)
            {
                _context.CurrentPointToTownIndex++;
                
                if (_context.CurrentPointToTownIndex >= pointsToTown.Length)
                {
                    if (_context.CurrentShopIndex < _context.Shops.Length)
                    {
                        SetNextShop();
                        _stateMachine.SetState(CitizenState.ToShop);
                    }
                    else
                    {
                        _stateMachine.SetState(CitizenState.MoveToTrain);
                    }
                }
            }
        }

        public void OnExit()
        {
            
        }

        private void SetNextShop()
        {
            if (_context.CurrentShopIndex < _context.Shops.Length)
            {
                _context.CurrentShop = _context.Shops[_context.CurrentShopIndex];
                _context.CurrentShopIndex++;
            }
            else
            {
                _context.IsVisitedAllShops = true;
            }
        }
    }
}