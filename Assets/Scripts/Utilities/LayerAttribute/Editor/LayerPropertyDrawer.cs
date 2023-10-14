using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Utilities.LayerAttribute
{
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	public class LayerPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// One line of  oxygen free code.
			property.intValue = EditorGUI.LayerField(position, label, property.intValue);
		}

	}
}