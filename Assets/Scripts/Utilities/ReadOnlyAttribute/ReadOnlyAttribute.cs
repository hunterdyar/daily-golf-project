using System;
using UnityEngine;

namespace Utilities.ReadOnlyAttribute
{
	[AttributeUsage(AttributeTargets.Field,AllowMultiple = false,Inherited = true)]
	public class ReadOnlyAttribute : PropertyAttribute
	{
		
	}
}