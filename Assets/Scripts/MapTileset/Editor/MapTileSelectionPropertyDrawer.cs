using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MapTileset.Editor
{
	[CustomPropertyDrawer(typeof(MapTileSelection))]
	public class MapTileSelectionPropertyDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			//normal fields
			var container = new VisualElement();
			var prefabProp = property.FindPropertyRelative("Prefab");
			if (prefabProp != null)
			{
				container.Add(new PropertyField(prefabProp));
			}

			var faceProp = property.FindPropertyRelative("Face");
			if (faceProp != null)
			{
				container.Add(new PropertyField(faceProp));
			}
			
			//tileNeighborFields
			
			//above
			
			//around
			//var row = new VisualElement();
			
			
			//below
			return container;
		}
	}
}