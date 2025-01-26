using System.Collections.Generic;
using Citizen.States;
using UnityEngine;

namespace Citizen
{
    public class CitizenStateMachine : MonoBehaviour
    {
        private ICitizenState _currentState;
        private Dictionary<CitizenState, ICitizenState> _states;

        public CitizenContext Context { get; private set; }


        public void Init(CitizenController controller)
        {
            Context = new CitizenContext { Controller = controller };

            _states = new Dictionary<CitizenState, ICitizenState>
            {
                { CitizenState.Idle, new IdleState(Context, this) },
                { CitizenState.MoveToMarketPlace, new MoveToMarketPlaceState(Context, this) },
                { CitizenState.ToShop, new MoveToShopState(Context, this) },
                { CitizenState.ToQueue, new MoveToQueueState(Context, this) },
                { CitizenState.Trading, new TradingState(Context, this) },
                { CitizenState.MoveToCenterPoint, new MoveToCenterPointState(Context, this) },
                { CitizenState.MoveToTrain, new MoveToTrainState(Context, this) },
                { CitizenState.MoveToTrainPlacePoint, new MoveToTrainPlaceState(Context, this) }
            };

            SetState(CitizenState.Idle);
        }

        public void SetState(CitizenState newState)
        {
            _currentState?.OnExit();
            _currentState = _states[newState];
            _currentState.OnEnter();
        }

        public void UpdateState()
        {
            _currentState?.OnUpdate();
        }
    }
}