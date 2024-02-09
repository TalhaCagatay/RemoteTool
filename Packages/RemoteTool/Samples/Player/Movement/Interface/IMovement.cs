
using Samples.Player.Config;
using UnityEngine;

namespace Samples.Player.Movement.Interface
{
    public interface IMovement
    {
        void Init(PlayerMovementBehaviour playerMovementBehaviour);
        void Move();
        void OnCollisionEnter(Collision collision);
        void OnCollisionExit(Collision collision);
    }
}