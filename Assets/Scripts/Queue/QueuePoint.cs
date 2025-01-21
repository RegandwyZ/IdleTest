using UnityEngine;

namespace Queue
{
    public class QueuePoint : MonoBehaviour
    {
        public bool IsTradePoint { get; private set; }
        public QueueState CurrentState { get; private set; }

        
        public void ChangeState(QueueState newState)
        {
            CurrentState = newState;
        }

        public void SetTradePoint()
        {
            IsTradePoint = true;
        }
    }
}