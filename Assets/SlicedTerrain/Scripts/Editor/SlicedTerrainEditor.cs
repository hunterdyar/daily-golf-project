using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace MapGen
{
	[CustomEditor(typeof(SlicedTerrain))]
	public class SlicedTerrainEditor : Editor
	{
		private static readonly string[] Labels = new[] { "Top Layer", "Middle Layer", "Bottom Layer" };

		public override VisualElement CreateInspectorGUI()
		{
			var container = new VisualElement();
			container.styleSheets.Add(Resources.Load<StyleSheet>("slicedTerrain/slicedTerrain"));
			//gameobject a, b, c
			//d, e, f
			container.Add(GetPrefabNineGrid(0));
			container.Add(GetPrefabNineGrid(1));
			container.Add(GetPrefabNineGrid(2));
			return container;
		}

		public VisualElement GetPrefabNineGrid(int layer)
		{
			var grid = new VisualElement();	

			var label = new Label(Labels[layer]);
			label.style.alignContent = Align.Center;
			grid.Add(label);
			for(int y = 0;y<3;y++)
			{
				grid.Add(GetPrefabRow(layer,y));
			}

			grid.style.paddingBottom = 20;

			return grid;
		}

		
	
		public VisualElement GetPrefabRow(int layer, int y)
		{
			var terrain = (SlicedTerrain)(target);


			var row = new VisualElement();
			row.style.flexDirection = FlexDirection.Row;
			row.style.alignContent = Align.Center;
			for (int i = 0; i < 3; i++)
			{
				int x = i;
				var field = new ObjectField("gameObject");
				field.objectType = typeof(GameObject);
				field.allowSceneObjects = false;
				field.value = terrain.GetPrefab(layer, y, x);
				//field.style.flexGrow = 0.3f;
				field.Remove(field.labelElement);
				field.RegisterValueChangedCallback(evt =>
				{
					terrain.SetObject(layer, y, x, (GameObject)evt.newValue);
				});
				row.Add(field);
			}
			
			return row;
		}
	}
}