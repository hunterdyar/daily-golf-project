using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utilities;
using Utilities.LayerAttribute;

namespace Golf
{
	public class GolfHitPreviewLine : MonoBehaviour
	{
		//References
		private GolfMovement _golfMovement;
		private LineRenderer _lineRenderer;
		
		//line render/sim point cache
		private Vector3[] _previewPoints;
		
		//Simulation
		[Header("Simulation Settings")]
		[SerializeField]
		[Layer]
		private int simulationLayer;

		[SerializeField] private int _simulationTickCount = 50;
		//Physics Scene
		private Scene _predictionScene;
		private PhysicsScene _simulation;
		
		//sim objects reference
		private GameObject _simulatedBall;
		private Rigidbody _simulatedRB;
		
		//cache
		private Vector3 _previousSimulatedForce;
		private void Awake()
		{
			_lineRenderer = GetComponent<LineRenderer>();
			_golfMovement = GetComponentInParent<GolfMovement>();

			CreatePredictionScene();
		}

		private void CreatePredictionScene()
		{
			CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
			_predictionScene = SceneManager.CreateScene("Trajectory Simulation", csp);
			_simulation = _predictionScene.GetPhysicsScene();
			CreateSimulationObjects();
		}

		

		private void Start()
		{
			_lineRenderer.positionCount = _simulationTickCount;
			_previewPoints = new Vector3[_simulationTickCount];
			if (_golfMovement == null)
			{
				Debug.LogWarning("Golf Hit Preview Line must be on or child of GolfMovement.",gameObject);
				gameObject.SetActive(false);
			}
			
		}

		private void CreateSimulationObjects()
		{
			//Create a new ball in our new scene.
			_simulatedBall = new GameObject();
			SceneManager.MoveGameObjectToScene(_simulatedBall, _predictionScene);

			//copy the object with a collider to the simulated ball. 
			//We make the (inappropriate?) assumption that the collider is on it's own game object. That's a pretty standard setup I tend to use, and duplicating this object is easier than full settings copy.
			var col = Instantiate(_golfMovement.GetComponentInChildren<Collider>().gameObject);
			col.transform.SetParent(_simulatedBall.transform);
			col.transform.localPosition = Vector3.zero;
			col.gameObject.layer = simulationLayer;
			
			//Next we add a rigidbody and duplicate it. I assume a rb is not on the same object as the collider (ie: a parent)
			_simulatedRB = _simulatedBall.AddComponent<Rigidbody>();
			var rb = _golfMovement.Rigidbody;
			//hey wouldn't it be great if this function existed? It does now. see extensions.cs
			_simulatedRB.CopyPropertiesFrom(rb);
			
			//set layer to simulation layer
			_simulatedBall.layer = simulationLayer;
			_simulatedBall.name = _golfMovement.gameObject.name + " Simulation";
		}
		private void Update()
		{
			//to test if the sim is any good, comment the following line out. 
			//_lineRenderer.enabled = _golfMovement.IsAiming;

			if (_golfMovement.IsAiming)
			{
				UpdateTrajectory();
			}
		}

		private void UpdateTrajectory()
		{
			_simulatedBall.transform.position = _golfMovement.transform.position;
			_simulatedBall.transform.rotation = _golfMovement.transform.rotation;
			_simulatedRB.velocity = _golfMovement.Rigidbody.velocity;
			_simulatedRB.angularVelocity = _golfMovement.Rigidbody.angularVelocity;

			_previewPoints[0] = _simulatedBall.transform.position;

			Vector3 simulationForce = _golfMovement.CurrentStroke.GetForce();
			//don't sim unless something changes with the force, or if something in the environment moves... what are other edge cases to consider triggers to keep this accurate?
			bool simulationNeeded = simulationForce != _previousSimulatedForce;
			if (simulationNeeded)
			{
				_simulatedRB.AddForce(simulationForce, ForceMode.Impulse);

				for (int i = 1; i < _simulationTickCount; i++)
				{
					_simulation.Simulate(Time.fixedDeltaTime);
					_previewPoints[i] = _simulatedBall.transform.position;
				}

				_lineRenderer.SetPositions(_previewPoints);
				_previousSimulatedForce = simulationForce;
			}
		}
		private void UpdateTrajectoryStraight()
		{
			_lineRenderer.SetPosition(0, _golfMovement.CurrentStroke.startPosition);
			_lineRenderer.SetPosition(1,
				_golfMovement.CurrentStroke.startPosition + _golfMovement.CurrentStroke.RealAimDir);
			
			
		}
	}
}