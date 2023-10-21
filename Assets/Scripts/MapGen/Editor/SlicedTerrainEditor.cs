using UnityEditor;
using UnityEngine.UIElements;

namespace MapGen
{
	[CustomEditor(typeof(SlicedTerrain))]
	public class SlicedTerrainEditor : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			var container = new VisualElement();
			//gameobject a, b, c
			//d, e, f

			return container;
		}
	}
}