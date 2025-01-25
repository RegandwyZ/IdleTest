using Citizen;
using UnityEngine;

namespace Queue
{
    public class QueuePoint : MonoBehaviour
    {
        public bool IsTradePoint { get; private set; }
        public QueueState CurrentState { get; private set; }

        public CitizenController Occupant { get; private set; }
        
        public void SetTradePoint()
        {
            IsTradePoint = true;
        }
        
        private void ChangeState(QueueState newState)
        {
            CurrentState = newState;
        }
        
        public void AssignOccupant(CitizenController occupant)
        {
            Occupant = occupant;
            ChangeState(QueueState.Engaged);
        }
        
        public void ClearOccupant()
        {
            Occupant = null;
            ChangeState(QueueState.Empty);
        }
    }
}