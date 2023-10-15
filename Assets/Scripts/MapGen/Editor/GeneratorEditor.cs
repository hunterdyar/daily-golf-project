using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace MapGen
{
	[CustomEditor(typeof(Generator))]
	public class GeneratorEditor : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			Generator generator = (Generator)target;
			var container = new VisualElement();
			
			
			container.Add(new Label("Settings"));
			//null checks are because I keep getting editor errors in the console. IMHO i consider this a unity bug.
			
			var size = serializedObject.FindProperty("size");
			if(size != null)container.Add(new PropertyField(size));

			var numTees = serializedObject.FindProperty("numberTees");
			if (numTees != null)container.Add(new PropertyField(numTees));

			var initialCellularSteps = serializedObject.FindProperty("initialCellularSteps");
			if (initialCellularSteps != null) container.Add(new PropertyField(initialCellularSteps));

			var perlinScale = serializedObject.FindProperty("perlinScale");
			if (perlinScale != null) container.Add(new PropertyField(perlinScale));

			var heightCurve = serializedObject.FindProperty("heightCurve");
			if (heightCurve != null) container.Add(new PropertyField(heightCurve));
			
			var waterLevel = serializedObject.FindProperty("waterLevel");
			if (waterLevel != null) container.Add(new PropertyField(waterLevel));

			//
			container.Add(new Label("Generation"));

			var genbutton = new Button(generator.Generate);
			genbutton.name = "Generate";
			genbutton.text = "Generate";
			var texture = new VisualElement();
			texture.style.backgroundImage = new StyleBackground(generator.MapImage);
			texture.style.width = 300;
			texture.style.height = 300*generator.Size.y/generator.Size.x;

			var teePositions = serializedObject.FindProperty("teePositions");
			if (teePositions != null)
			{
				var teeList = new PropertyField(teePositions);
				teeList.SetEnabled(false);
				container.Add(teeList);
			}
			
			container.Add(genbutton);
			container.Add(texture);

			return container;
		}
		
	}
}