using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapTilesetEditor))]
public class MapTilesetEditor : Editor
{
	// public override VisualElement CreateInspectorGUI()
	// {
	// 	var container = new VisualElement();
	// 	
	// 	var prefabProp = serializedObject.FindProperty("Prefab");
	// 	if (prefabProp != null)
	// 	{
	// 		container.Add(new PropertyField(prefabProp));
	// 	}
	//
	// 	return container;
	// }
}
