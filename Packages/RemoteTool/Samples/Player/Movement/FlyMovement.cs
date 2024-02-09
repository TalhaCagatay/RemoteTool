using Samples.Bridge;
using Samples.Player.Config;
using Samples.Player.Movement.Interface;
using UnityEngine;

namespace Samples.Player
{
    public class FlyMovement : IMovement
    {
        private PlayerMovementBehaviour _playerMovementBehaviour;
        private MovementConfig _movementConfig;

        public void Init(PlayerMovementBehaviour playerMovementBehaviour)
        {
            _playerMovementBehaviour = playerMovementBehaviour;
            _movementConfig = ConfigHandler.GetMovementConfig<FlyMovementConfig>();
        }
        
        public void Move() => _playerMovementBehaviour.transform.Translate((Vector3.forward + Vector3.down) * (Time.deltaTime * _movementConfig.Speed));
        public void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Fly")) return;

            _playerMovementBehaviour.SwitchMovement(this);
            Debug.Log($"Switching to {GetType().Name}");
        }
        
        public void OnCollisionExit(Collision collision)
        {
            
        }
    }
}