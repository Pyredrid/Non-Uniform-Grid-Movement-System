using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class NodeExtensions {
	public static T GetChildOfType<T>(this Node node) {
		if(Godot.Object.IsInstanceValid(node) == false) {
			return default(T);
		}
		foreach(object child in node.GetChildren()) {
			if(child is T && Godot.Object.IsInstanceValid((Godot.Object)child) == true) {
				return (T)child;
			}
		}
		return default(T);
	}

	public static List<T> GetChildrenOfType<T>(this Node node, bool isRecursive = true) {
		if(Godot.Object.IsInstanceValid(node) == false) {
			return null;
		}
		List<T> children = new List<T>();
		foreach(object child in node.GetChildren()) {
			if(child is T && Godot.Object.IsInstanceValid((Godot.Object)child) == true) {
				children.Add((T)child);
			}
			if(isRecursive == true) {
				children.AddRange(((Node)child).GetChildrenOfType<T>(true));
			}
		}
		return children;
	}

	public static T GetParentOfTypeRecursive<T>(this Node node) where T : Node {
		if(Godot.Object.IsInstanceValid(node) == false) {
			return null;
		}
		Node parent = node.GetParent();
		while(parent != null && Godot.Object.IsInstanceValid(parent) == true) {
			if(parent is T) {
				return (T)parent;
			}
			parent = parent.GetParent();
		}
		return null;
	}
}
