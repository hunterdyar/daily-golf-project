using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Golf
{
	[CreateAssetMenu(fileName = "Club", menuName = "Golf/Club", order = 0)]
	public class Club : ScriptableObject
	{
		//icon, etc.
		public string displayName;
		[Min(0)]
		public float power = 1;
		[Range(0, 1)] public float minimumPowerPercentage;
		[Range(0,90)]
		public float angle = 0;
	}
}