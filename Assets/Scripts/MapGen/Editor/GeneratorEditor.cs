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

			//
			container.Add(new Label("Settings"));
			container.Add(new PropertyField(serializedObject.FindProperty("size")));
			container.Add(new PropertyField(serializedObject.FindProperty("initialCellularSteps")));
			container.Add(new PropertyField(serializedObject.FindProperty("perlinScale")));
			container.Add(new PropertyField(serializedObject.FindProperty("heightCurve")));

			//
			container.Add(new Label("Generation"));

			var genbutton = new Button(generator.Generate);
			genbutton.name = "Generate";
			genbutton.text = "Generate";
			var texture = new VisualElement();
			texture.style.backgroundImage = new StyleBackground(generator.MapImage);
			texture.style.width = 300;
			texture.style.height = 300*generator.Size.y/generator.Size.x;
			
			container.Add(genbutton);
			container.Add(texture);

			return container;
		}
		
	}
}