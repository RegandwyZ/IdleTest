﻿using Character;
using UnityEngine;
using CharacterController = Character.CharacterController;

namespace Queue
{
    public class QueuePoint : MonoBehaviour
    {
        public bool IsTradePoint { get; private set; }
        public QueueState CurrentState { get; private set; }

        public CharacterController Occupant { get; private set; }
        
        public void SetTradePoint()
        {
            IsTradePoint = true;
        }
        
        public void ChangeState(QueueState newState)
        {
            CurrentState = newState;
        }
        
        public void AssignOccupant(CharacterController occupant)
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