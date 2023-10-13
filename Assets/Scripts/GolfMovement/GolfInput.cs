using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages Input state and applies swings to the player.

namespace Golf
{
    [RequireComponent(typeof(GolfMovement))]
    public class GolfInput : MonoBehaviour
    {
        private GolfMovement _golfMovement;
        private Camera _camera;
        
        private void Awake()
        {
            _golfMovement = GetComponent<GolfMovement>();
            _camera = Camera.main;//while Unity fixed this field doing a FindObjectWithTag call, it's still considered good practice to cache camera references.
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!_golfMovement.IsAiming)
            {
                return;
            }

            Vector3 hitDir = Vector3.ProjectOnPlane(_camera.transform.forward.normalized, Vector3.up).normalized;
            if (_camera != null) _golfMovement.CurrentStroke.aimDir = hitDir;
            //this doesn't need constant updating, ideally, but lets us drag the ball around in the scene for testing.
            
            _golfMovement.CurrentStroke.startPosition = transform.position;
            _golfMovement.CurrentStroke.inputPower = 1;//drag and set this.
        }
    }
}
