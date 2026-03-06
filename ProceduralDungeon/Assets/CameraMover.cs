using UnityEngine;

namespace ProceduralDungeon
{
    public class CameraMover : MonoBehaviour
    {
        private Blackboard blackboard;
        private float cameraSpeed;

        public void Setup(Blackboard blackboard) => this.blackboard = blackboard;
        public void Pull() => cameraSpeed = blackboard.GetValue<float>(nameof(cameraSpeed));

        private void Update()
        {
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            movement.Normalize();

            transform.Translate(cameraSpeed * Time.deltaTime * movement);
        }
    }
}
