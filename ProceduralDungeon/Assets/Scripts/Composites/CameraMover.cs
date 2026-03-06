using UnityEngine;

namespace ProceduralDungeon
{
    public class CameraMover : MonoBehaviour
    {
        private Blackboard blackboard;
        private float cameraSpeed;

        public void Setup(Blackboard blackboard) => this.blackboard = blackboard;
        private float nextSync;

        private void Update()
        {
            if (Time.time >= nextSync)
            {
                cameraSpeed = blackboard.GetValue<float>(nameof(cameraSpeed));
                nextSync = Time.time + 0.5f;
            }
            
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            movement.Normalize();

            transform.Translate(cameraSpeed * Time.deltaTime * movement);
        }
    }
}
