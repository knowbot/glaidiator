using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarriorAnims
{
    public class PlayerSimulator : MonoBehaviour
    {
        [SerializeField] private Transform target;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void RunSim(Vector3 move)
        {
            // move
            // rotate
            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 100f);
            }
            
        }


    }
}

