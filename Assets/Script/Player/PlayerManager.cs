using Project.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player{
    public class PlayerManager : MonoBehaviour{

        [Header("Data")]
        [SerializeField]
        private float speed = 4;

        [Header("Class References")]
        [SerializeField]
        private NetworkIdentity NetworkIdentity;


        

        
        public CharacterController controller;

        

        // Start is called before the first frame update

        // Update is called once per frame
        public void Update()
        {
            
            if (NetworkIdentity.IsControlling())
            {
                checkMovement();
            }
        }

        private void checkMovement()
        {
            

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

        }
    }
}
