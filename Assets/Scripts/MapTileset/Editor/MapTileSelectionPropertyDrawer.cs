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

			//Get references
			//abc
			//def
			//ghi
			var a = property.FindPropertyRelative("ForwardLeftCorner");
			var b = property.FindPropertyRelative("Forward");
			var c = property.FindPropertyRelative("ForwardRightCorner");

			var d = property.FindPropertyRelative("Left");
			//var e = property.FindPropertyRelative("");
			var f = property.FindPropertyRelative("Right");

			var g = property.FindPropertyRelative("BackLeftCorner");
			var h = property.FindPropertyRelative("Back");
			var i = property.FindPropertyRelative("BackRightCorner");
			
			var aboveProp = property.FindPropertyRelative("Above");
			var belowProp = property.FindPropertyRelative("Below");

			//exit early so no _NULL_ errors on weird reload edge cases.
			if (a == null || b == null || c == null || d == null || f == null || g == null || h == null || i == null)
			{
				return container;
			}

			if (aboveProp == null || belowProp == null)
			{
				return container;
			}

			float colWidth = 60;
			
			var neighbors = new VisualElement();
			neighbors.style.flexDirection = FlexDirection.Column;
			neighbors.style.alignItems = Align.Center;
			
			neighbors.Add(new Label("Above"));
			var abovef = new PropertyField(aboveProp);
			abovef.label = "";
			abovef.style.paddingBottom = 5;
			neighbors.Add(abovef);
			
			
			neighbors.Add(new Label("Surrounding"));
			var af = new PropertyField(a);
			af.label = "";
			af.style.width = colWidth;
			var bf = new PropertyField(b);
			bf.label = "";
			bf.style.width = colWidth;
			var cf = new PropertyField(c);
			cf.label = "";
			cf.style.width = colWidth;
			var df = new PropertyField(d);
			df.label = "";
			df.style.width = colWidth;
			var ff = new PropertyField(f);
			ff.label = "";
			ff.style.width = colWidth;
			var gf = new PropertyField(g);
			gf.label = "";
			gf.style.width = colWidth;
			var hf = new PropertyField(h);
			hf.label = "";
			hf.style.width = colWidth;
			var i_f = new PropertyField(i);
			i_f.label = "";
			i_f.style.width = colWidth;
			
			
			///
			var row = new VisualElement();
			row.style.flexDirection = FlexDirection.Row;

			row.Add(af);
			row.Add(bf);
			row.Add(cf);
			
			neighbors.Add(row);
		//////
			var row2 = new VisualElement();
			row2.style.flexDirection = FlexDirection.Row;

			row2.Add(df);
			var centerSpacer = new VisualElement();
			centerSpacer.style.width = colWidth;
			row2.Add(centerSpacer);
			row2.Add(ff);

			neighbors.Add(row2);
		//////////
			var row3 = new VisualElement();
			row3.style.flexDirection = FlexDirection.Row;

			row3.Add(gf);
			row3.Add(hf);
			row3.Add(i_f);

			neighbors.Add(row3);

			//below

			neighbors.Add(new Label("Below"));
			var belowf = new PropertyField(belowProp);
			belowf.label = "";
			belowf.style.paddingBottom = 5;
			neighbors.Add(belowf);
			
			//below
			
			container.Add(neighbors);
			return container;
		}
	}
}