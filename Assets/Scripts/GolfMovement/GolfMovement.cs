using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golf
{
    public class GolfMovement : MonoBehaviour
    {
        private Stroke _currentStroke;
        public void HitBall(Stroke stroke)
        {
            
            //applie the forces to the ball
            stroke.Status = StrokeStatus.InMotion;
            
            //the stroke might be applied during aiming.
            _currentStroke = stroke;
        }

        private void Update()
        {
            if (_currentStroke.Status == StrokeStatus.InMotion)
            {
                //if ball stops moving
                //  //set the stroke to finished.
            }
        }
    }
}