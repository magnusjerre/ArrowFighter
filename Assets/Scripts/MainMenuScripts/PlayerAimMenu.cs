using UnityEngine;
using System.Collections;


namespace Jerre.MainMenu {
    public class PlayerAimMenu : MonoBehaviour
    {

        PlayerInputComponent playerInput;
        PlayerSettings settings;
        PlayerBoost boost;

        public RectTransform rectTransform;

        public float MaxLookRotationSpeedDegs;
        public int PlayerNumber;
        private Vector3 oldLookDirection;


        // Start is called before the first frame update
        void Start()
        {
            playerInput = GetComponent<PlayerInputComponent>();
            settings = GetComponent<PlayerSettings>();
            boost = GetComponent<PlayerBoost>();
            oldLookDirection = Vector3.forward;
        }

        // Update is called once per frame
        void Update()
        {
            var lookX = Input.GetAxis(PlayerInputTags.LOOK_X + PlayerNumber);
            var lookY = Input.GetAxis(PlayerInputTags.LOOK_Y + PlayerNumber);
            var targetLookDirection = new Vector3(lookX, 0, lookY).normalized;
            var angleRotation = Mathf.Min(Vector3.Angle(oldLookDirection, targetLookDirection), MaxLookRotationSpeedDegs * Time.deltaTime);
            transform.Rotate(angleRotation * Vector3.forward);
            var targetDirection = Vector3.RotateTowards(oldLookDirection, targetLookDirection, angleRotation * Mathf.Deg2Rad, 0f);
            transform.LookAt(transform.position + targetDirection);
        }
    }
}
