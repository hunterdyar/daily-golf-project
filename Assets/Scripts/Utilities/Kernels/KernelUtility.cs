using UnityEngine;

namespace Utilities.Kernels
{
	public static class KernelUtility
	{
		public static float[,] BoxKernal = new[,]
		{
			{ 1 / 9f, 1 / 9f, 1 / 9f },
			{ 1 / 9f, 1 / 9f, 1 / 9f },
			{ 1 / 9f, 1 / 9f, 1 / 9f }
		};
		
		public static float[,] GetGuassianKernel(int length, float weight)
		{
			float[,] kernel = new float [length, length];
			float runningTotal = 0;//used to normalize.
			int radius = length / 2;

			//do this once
			float euler = 1f / (2f * Mathf.PI * Mathf.Pow(weight, 2));
			
			//it's basically just creating a gradient, but in a grid of floats.
			for (int filterY = -radius; filterY <= radius; filterY++)
			{
				for (int filterX = -radius; filterX <= radius; filterX++)
				{
					float distance = (Mathf.Pow(filterX,2)) + (Mathf.Pow(filterY,2)) / (2 * Mathf.Pow(weight,2));
					kernel[filterY + radius, filterX + radius] = euler * Mathf.Exp(-distance);
					runningTotal += kernel[filterY + radius, filterX + radius];
				}
			}

			//Scale so that the entire kernel sums to 1.
			for (var y = 0; y < length; y++)
			{
				for (var x = 0; x < length; x++)
				{
					kernel[y, x] = kernel[y, x] * (1f / runningTotal);
				}
			}
			
			return kernel;
		}
	}
}