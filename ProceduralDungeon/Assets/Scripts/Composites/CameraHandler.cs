using UnityEngine;

namespace ProceduralDungeon
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private Camera cam = default;
        [SerializeField] private float interval = default;
        
        private Blackboard blackboard;
        private float cameraSpeed;
        private float fov;

        public void Setup(Blackboard blackboard) => this.blackboard = blackboard;
        private float nextSync;

        private void Update()
        {
            if (Time.time >= nextSync)
            {
                cameraSpeed = blackboard.GetValue<float>(nameof(cameraSpeed));
                fov = blackboard.GetValue<float>(nameof(fov));
                nextSync = Time.time + interval;
            }
            
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            movement.Normalize();

            transform.Translate(cameraSpeed * Time.deltaTime * movement);
            cam.orthographicSize = fov;
        }
    }
}
