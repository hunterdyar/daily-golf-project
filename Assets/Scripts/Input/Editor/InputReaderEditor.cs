using System;
using Cinemachine;
using Golf;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Input.Editor
{
	[CustomEditor(typeof(InputReader))]
	public class InputReaderEditor : UnityEditor.Editor
	{
		private InputReader _reader;

		public override VisualElement CreateInspectorGUI()
		{
			_reader = (InputReader)target;

			var container = new VisualElement();
			
			//Settings
			
			//settings label
			var settingsLabel = new Label("Settings");
			SetLabelStyle(settingsLabel);
			container.Add(settingsLabel);
			
			//aim speed prperty
			var aimSpeed = serializedObject.FindProperty("_aimSpeed");
			if (aimSpeed != null) container.Add(new PropertyField(aimSpeed));

			//Debug View
			
			//debug view label
			var debugLabel = new Label("Debug View");
			SetLabelStyle(debugLabel);
			container.Add(debugLabel);

			//debug properties.
			
			//Don't actually need to FindProperty, we can just craft property fields with the public data.
			var look = new Vector2Field
			{
				label = "Look: ",
			};
			look.bindingPath = "_look";
			look.SetEnabled(false);
			container.Add(look);


			var prop = new FloatField();
			prop.label = "Aim Delta: ";
			prop.bindingPath = "_aim";
			prop.SetEnabled(false);
			container.Add(prop);

			var power = new FloatField
			{
				label = "Power Delta: ",
				value = _reader.PowerDelta,
			};
			power.bindingPath = "_powerDelta";
			power.SetEnabled(false);
			container.Add(power);

			var testingLabel = new Label("Testing");
			SetLabelStyle(testingLabel);
			container.Add(testingLabel);

			var swingButton = new Button(_reader.DoSwing);
			swingButton.text = "Swing!";
			var buttons = new VisualElement();
			buttons.style.flexDirection = FlexDirection.Row;
			var CLButton = new Button(_reader.DoCycleClubLeft);
			CLButton.style.flexGrow = 1;
			CLButton.text = "Prev Club";
			var CRButton = new Button(_reader.DoCycleClubRight);
			CRButton.style.flexGrow = 1;
			CRButton.text = "Next Club";
			
			
			container.Add(swingButton);
			buttons.Add(CLButton);
			buttons.Add(CRButton);
			container.Add(buttons);
			return container;
		}

		//this should applied with USS (css) but, like, who can be bothered.
		private void SetLabelStyle(VisualElement element)
		{
			element.style.unityFontStyleAndWeight = FontStyle.Bold;
			element.style.borderBottomWidth = 1;
			element.style.borderBottomColor = new Color(0, 0, 0, 0.5f);
			element.style.marginBottom = 10;
			element.style.marginTop = 20;
		}
	}
}