using System;
using System.Collections.Generic;
using MapGen;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Utilities.LayerAttribute;

namespace Golf
{
	[RequireComponent(typeof(LineRenderer))]
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
		private Vector3 lastRefactoredPosition;
		private List<Collider> _simulatedEnvironmentObjects = new List<Collider>();

		private Vector3 _previewFirstHitLocation;
		private int _simCount;
		
		//cache
		private Vector3 _previousSimulatedForce;
		private void Awake()
		{
			_lineRenderer = GetComponent<LineRenderer>();
			_golfMovement = GetComponentInParent<GolfMovement>();

			CreatePredictionScene();
		}

		private void OnEnable()
		{
			_golfMovement.OnNewStroke += ForceUpdateTrajectoryLine;
			MapGenerator.OnGenerationComplete += OnGenerationComplete;
		}

	

		private void OnDisable()
		{
			_golfMovement.OnNewStroke -= ForceUpdateTrajectoryLine;
			MapGenerator.OnGenerationComplete -= OnGenerationComplete;

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

		//create the environment
		private void OnGenerationComplete(MapGenerator mapGen)
		{
			//todo: avoid race condition with check if the predictionScene is null, create it.

			foreach (var collider in _simulatedEnvironmentObjects)
			{
				Destroy(collider.gameObject);
			}
			foreach (var collider in mapGen.EnvironmentColliders)
			{
				var col = Instantiate(collider);
				col.gameObject.name = collider.gameObject.name + " Simulation";
				col.gameObject.layer = simulationLayer;
				SceneManager.MoveGameObjectToScene(col.gameObject, _predictionScene);
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
			var rb = _golfMovement.GetComponent<Rigidbody>();//golfMovement has a public Rigidbody, but this is being called in awake (too) and it may not have gotten that reference yet.
			//hey wouldn't it be great if this function existed? It does now. see extensions.cs
			_simulatedRB.CopyPropertiesFrom(rb);
			
			//set layer to simulation layer
			_simulatedBall.layer = simulationLayer;
			_simulatedBall.name = _golfMovement.gameObject.name + " Simulation";
		}
		private void Update()
		{
			//disable display and when to update display are two separate problems.
			//we should disable (draw) and never update when not aiming.
			//we should update when the ball or the force changes.

			if (ShouldDrawDisplay())
			{
				_lineRenderer.enabled = true;
				UpdateTrajectory();
			}
			else
			{
				_lineRenderer.enabled = false;
			}
		}

		private bool ShouldDrawDisplay()
		{
			//we can check the camera system to make sure it's not wrong either.
			return (_golfMovement.CurrentStroke.Status != StrokeStatus.InMotion);//also check aiming so we don't draw during ball drop.
		}

		[ContextMenu("Force update Trajectory Line")]
		public void ForceUpdateTrajectoryLine()
		{
			UpdateTrajectory();
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
			bool simulationNeeded = simulationForce != _previousSimulatedForce || lastRefactoredPosition != _golfMovement.transform.position;
			//

			if (simulationNeeded)
			{
				_simCount = 0;
				_simulatedRB.AddForce(simulationForce, ForceMode.Impulse);

				for (int i = 1; i < _simulationTickCount; i++)
				{
					_simulation.Simulate(Time.fixedDeltaTime*2);
					_previewPoints[i] = _simulatedBall.transform.position;
				}

				_lineRenderer.SetPositions(_previewPoints);
				_previousSimulatedForce = simulationForce;
				_simCount++;
			}

			lastRefactoredPosition = _golfMovement.transform.position;
		}

		private void OnDestroy()
		{
			//clean up after ourselves. If the game reloads, we don't want a thousand scenes lying around, or hanging out on menu's or what-have-you.
			SceneManager.UnloadSceneAsync(_predictionScene);
		}
	}
}