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
        public Stroke CurrentStroke => _currentStroke;

        [SerializeField]
        [ReadOnly]
        private Stroke _currentStroke;

        public Rigidbody Rigidbody => _rigidbody;
        private Rigidbody _rigidbody;

        public ActiveGolfConfiguration Caddy => _caddy;
        [SerializeField]
        private ActiveGolfConfiguration _caddy;

        //Convenience accessor for common pattern.
        public bool IsAiming => _currentStroke is { Status: StrokeStatus.Aiming };
        //todo: use state change events instead of update listeners.
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            _currentStroke.club = club;
        }

        private void Start()
        {
            //I save awake for configuring scriptableobjects, (like loading from save files) so I try not to read data until start.
            _currentStroke = new Stroke(_rigidbody, _caddy.SelectedClub);
            OnNewStroke?.Invoke();
        }

        public void HitBall()
        {
            //applies the forces to the ball
            _currentStroke.Status = StrokeStatus.InMotion;
            _rigidbody.AddForce(_currentStroke.GetForce(),ForceMode.Impulse);
        }
        

        private void FixedUpdate()
        {
            //A basic state machine using the enum property of current stroke. A more sophisticated method is uneccesary.
            //When a stroke is finished, we create a new stroke and add the old one to a list. Now it's the save/score data.
            
            if (_currentStroke.Status == StrokeStatus.Aiming)
            {
                return;
            }
            if (_currentStroke.Status == StrokeStatus.InMotion || _currentStroke.Status == StrokeStatus.NotTaken)
            {
                _currentStroke.Tick(Time.fixedDeltaTime);
                if (_currentStroke.hitTimer > 0.75f && _rigidbody.velocity.sqrMagnitude < 0.01f)
                {
                    //if status was inMotion, add to list.
                    //if status was NotTaken, then we are in debug or first shot testing.
                    _currentStroke.Status = StrokeStatus.Taken;
                    _currentStroke = new Stroke(_rigidbody, _caddy.SelectedClub);
                    _currentStroke.Status = StrokeStatus.Aiming;
                    //Update
                    OnNewStroke?.Invoke();
                }
            }
        }
    }
}