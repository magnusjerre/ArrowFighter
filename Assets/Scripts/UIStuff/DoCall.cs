using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.UIStuff
{
    public class DoCall : MonoBehaviour
    {
        public float Delay = 2f;
        // Start is called before the first frame update
        void Start()
        {
            Invoke("Delayed", Delay);   
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void Delayed() {
            var doCallers = GetComponents<IDo>();
            for (var i = 0; i < doCallers.Length; i++) {
                doCallers[i].Do();
            }
        }
    }
}
