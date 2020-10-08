//	Copyright (C) 2020  Aaron "Pyredrid" Bekker-Dulmage
//
// 	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//	GNU General Public License for more details.
//
//	You should have received a copy of the GNU General Public License
//	along with this program.If not, see<https://www.gnu.org/licenses/>.

using Godot;
using System;
using System.Collections.Generic;

public static class RandomExtensions {
	/// <summary>
	/// Generates a random coordinate within a sphere.  Should be
	/// close to uniformly distributed but I haven't fully
	/// tested that yet...
	/// </summary>
	/// <returns>The in sphere.</returns>
	/// <param name="rng">Rng.</param>
	public static Vector3 RandInSphere(this RandomNumberGenerator rng) {
		float x = rng.Randfn();
		float y = rng.Randfn();
		float z = rng.Randfn();
		float n = rng.Randf();

		float magnitude = Mathf.Sqrt(x * x + y * y + z * z);
		x /= magnitude; y /= magnitude; z /= magnitude;

		var c = Mathf.Pow(n, 1f / 3f);

		return new Vector3(x * c, y * c, z * c);
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

	/// <summary>
	/// Pick the a random element out of a list.
	/// </summary>
	/// <returns>The pick.</returns>
	/// <param name="rng">The RNG to use</param>
	/// <param name="list">The list of elements to pick from</param>
	public static T Pick<T>(this RandomNumberGenerator rng, IList<T> list) {
		int index = rng.RandiRange(0, list.Count - 1);
		return list[index];
	}

	/// <summary>
	/// Pick the a random element out of a list.
	/// </summary>
	/// <returns>The pick.</returns>
	/// <param name="rand">The RNG to use</param>
	/// <param name="list">The list of elements to pick from</param>
	public static T Pick<T>(this Random rand, IList<T> list) {
		int index = rand.Next(0, list.Count);
		return list[index];
	}
	
	/// <summary>
	/// Picks a random float from min to max.
	/// Can you believe .NET's random class only
	/// does double precision numbers?  This is
	/// annoying af...
	/// </summary>
	/// <returns>A random floating point number between min and max</returns>
	public static float RandfRange(this Random rand, float min, float max) {
		float r = (float)rand.NextDouble();
		r = Mathf.Lerp(min, max, r);
		return r;
	}
}