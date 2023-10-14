using System;
using UnityEngine;

namespace Utilities.LayerAttribute
{
	[AttributeUsage(AttributeTargets.Field,AllowMultiple = false,Inherited = true)]
	public class LayerAttribute : PropertyAttribute
	{
		
	}
}