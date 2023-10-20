using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ReadOnlyAttribute;

namespace Golf
{
    public class GolfMovement : MonoBehaviour
    {
        public Action OnNewStroke;
        public Stroke CurrentStroke => _caddy.CurrentStroke;
        public Rigidbody Rigidbody => _rigidbody;
        private Rigidbody _rigidbody;
        public ActiveGolfConfiguration Caddy => _caddy;
        [SerializeField]
        private ActiveGolfConfiguration _caddy;

        //Convenience accessor for common pattern.
        public bool IsAiming => CurrentStroke is { Status: StrokeStatus.Aiming };
        //todo: use state change events instead of update listeners.
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            //We are just doing single player for now, but I prefer to go through an updatable reference instead of singleton pattern. Tutorials, cutscenes, and other edge cases.
            _caddy.SetCurrentPlayer(this);
        }

        private void OnEnable()
        {
            _caddy.OnSelectedClubChanged += OnSelectedClubChanged;
        }

        private void OnDisable()
        {
            _caddy.OnSelectedClubChanged -= OnSelectedClubChanged;
        }

        //instead, should we pass the caddy to the stroke? 
        //we don't, because the stroke serves as our historical data after the hit.
        private void OnSelectedClubChanged(Club club)
        {
            CurrentStroke.club = club;
        }

        private void Start()
        {
        }

        public void HitBall()
        { 
            //applies the forces to the ball
            CurrentStroke.Status = StrokeStatus.InMotion;
            _rigidbody.AddForce(CurrentStroke.GetForce(),ForceMode.Impulse);
        }
        

        private void FixedUpdate()
        {
            //A basic state machine using the enum property of current stroke. A more sophisticated method is uneccesary.
            //When a stroke is finished, we create a new stroke and add the old one to a list. Now it's the save/score data.
            
            if (CurrentStroke.Status == StrokeStatus.Aiming)
            {
                return;
            }
            if (CurrentStroke.Status == StrokeStatus.InMotion || CurrentStroke.Status == StrokeStatus.NotTaken)
            {
                CurrentStroke.Tick(Time.fixedDeltaTime);
                if (IsOutOfBounds())
                {
                    CurrentStroke.Failure();
                    CurrentStroke.ResetStrokeToStart();
                    _caddy.StartNewStrokeAndAim(_rigidbody);
                }
                if (CurrentStroke.IsStrokeComplete())
                {
                    //if status was inMotion, add to list.
                    //if status was NotTaken, then we are in debug or first shot testing.
                    CurrentStroke.Complete();
                    _caddy.StartNewStrokeAndAim(_rigidbody);
                }
            }
        }

        

        private bool IsOutOfBounds()
        {
            return transform.position.y < -5f;//todo: settings. other boundaries.
        }
    }
}