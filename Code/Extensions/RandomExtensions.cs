
using Godot;
using System;
using System.Collections.Generic;

public static class RandomExtensions {
	public static Vector3 RandInSphere(this RandomNumberGenerator rng) {
		float u = rng.Randf();
		float x1 = rng.Randfn();
		float x2 = rng.Randfn();
		float x3 = rng.Randfn();

		float mag = Mathf.Sqrt(x1 * x1 + x2 * x2 + x3 * x3);
		x1 /= mag; x2 /= mag; x3 /= mag;

		//This is close enough to a cube root
		//Note:  This would fail if u was ever negative
		var c = Mathf.Pow(u, 1f / 3f);

		return new Vector3(x1 * c, x2 * c, x3 * c);
	}

	/// <summary>
	/// Rearranges all the items in this list into a random order
	/// </summary>
	/// <param name="rng">The RNG to use for random ordering</param>
	public static void Shuffle<T>(this IList<T> list, RandomNumberGenerator rng) {
		int n = list.Count;
		while(n > 1) {
			n--;
			int k = rng.RandiRange(0, n);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	/// <summary>
	/// Rearranges all the items in this list into a random order
	/// </summary>
	public static void Shuffle<T>(this IList<T> list) {
		RandomNumberGenerator rng = new RandomNumberGenerator();
		list.Shuffle(rng);
	}

	public static T Pick<T>(this RandomNumberGenerator rng, IList<T> list) {
		int index = rng.RandiRange(0, list.Count - 1);
		return list[index];
	}


	public static T Pick<T>(this Random rand, IList<T> list) {
		int index = rand.Next(0, list.Count - 1);
		return list[index];
	}
	
	public static float RandfRange(this Random rand, float min, float max) {
		float r = (float)rand.NextDouble();
		r = Mathf.Lerp(min, max, r);
		return r;
	}
}