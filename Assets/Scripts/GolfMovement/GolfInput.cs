using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Manages Input state and applies swings to the player.

namespace Golf
{
    [RequireComponent(typeof(GolfMovement))]
    public class GolfInput : MonoBehaviour
    {
        private GolfMovement _golfMovement;
        private Camera _camera;

        private Vector3 _aimDir = Vector3.forward;
        [SerializeField] private InputReader _inputReader;
        private void Awake()
        {
            _golfMovement = GetComponent<GolfMovement>();
            _camera = Camera.main;//while Unity fixed this field doing a FindObjectWithTag call, it's still considered good practice to cache camera references.
        }

        private void OnEnable()
        {
            _inputReader.Swing += Swing;
            _inputReader.CycleClub += CycleClub;
            _golfMovement.OnNewStroke += OnNewStroke;
        }
        
        private void OnDisable()
        {
            _inputReader.Swing -= Swing;
            _inputReader.CycleClub -= CycleClub;
            _golfMovement.OnNewStroke -= OnNewStroke;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_golfMovement.IsAiming)
            {
                return;
            }
            
            _inputReader.AdjustAimVectorTick(ref _golfMovement.CurrentStroke.aimDir);
            
            _golfMovement.CurrentStroke.inputPower = Mathf.Clamp(_golfMovement.CurrentStroke.inputPower + _inputReader.PowerDelta * Time.deltaTime,_golfMovement.CurrentStroke.club.minimumPowerPercentage,1);
            //Vector3 hitDir = Vector3.ProjectOnPlane(_camera.transform.forward.normalized, Vector3.up).normalized;
           // if (_camera != null) _golfMovement.CurrentStroke.aimDir = hitDir;
            //this doesn't need constant updating, ideally, but lets us drag the ball around in the scene for testing.
            
            _golfMovement.CurrentStroke.startPosition = transform.position;
        }

        public void Swing()
        {
            if (_golfMovement.IsAiming)
            {
                _golfMovement.HitBall();
            }
        }
        
        //Because the caddy is directly controlled by the input reader, we COULD directly reference it!
        //except we are using this object for input state. So we disable the ability to change clubs unless we are aiming.
        //also so we aren't cycling clubs while in the... like, menu....
        private void CycleClub(int delta)
        {
            if (_golfMovement.IsAiming)
            {
                _golfMovement.Caddy.CycleClub(delta);
            }
        }
        

        private void OnNewStroke()
        {
            _golfMovement.CurrentStroke.inputPower = 0.5f; //save last value and reapply?
        }
    }
}
