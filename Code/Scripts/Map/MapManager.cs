using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapManager : Node {
	private static MapManager Instance;
	
	private List<Map> CurrentlyLoadedMaps = new List<Map>();
	private List<ResourceInteractiveLoader> UnfinishedLoaders = new List<ResourceInteractiveLoader>();
	
	private List<MapObject> AllMapObjects = new List<MapObject>();

	public override void _EnterTree() {
		base._EnterTree();
		Instance = this;
	}

	public override void _Process(float delta) {
		base._Process(delta);
		for(int i = UnfinishedLoaders.Count - 1; i >= 0; i--) {
			ResourceInteractiveLoader loader = UnfinishedLoaders[i];
			Error loadErr = loader.Poll();
			if(loadErr == Error.Ok) {
				continue;
			} else if(loadErr == Error.FileEof) {
				Resource resource = loader.GetResource();
				Map map = (Map)(((PackedScene)resource).Instance());
				CurrentlyLoadedMaps.Add(map);
				AddChild(map);
				UnfinishedLoaders.Remove(loader);
			} else {
				GD.PrintErr(loadErr);
			}
		}
	}

	public static MapNode GetClosestNode(Vector3 position) {
		if(Instance.CurrentlyLoadedMaps.Count == 0) {
			return null;
		}
		
		MapNode closestNode = Instance.CurrentlyLoadedMaps[0].GetMapNodes()[0];
		float closestDistanceSquared = position.DistanceSquaredTo(closestNode.GlobalTransform.origin);

		foreach(Map map in Instance.CurrentlyLoadedMaps) {
			foreach(MapNode node in map.GetMapNodes()) {
				float distanceSquared = position.DistanceSquaredTo(node.GlobalTransform.origin);
				if(distanceSquared < closestDistanceSquared) {
					closestDistanceSquared = distanceSquared;
					closestNode = node;
				}
			}
		}
		return closestNode;
	}

	public static void LoadMap(string mapPath) {
		ResourceInteractiveLoader loader = ResourceLoader.LoadInteractive(mapPath);
		Instance.UnfinishedLoaders.Add(loader);
	}

	public static void UnloadMap(Map map) {
		map.EmitSignal(nameof(Map.OnUnload));
		Instance.CurrentlyLoadedMaps.Remove(map);
		List<MapObject> loadedMapObjects = map.GetChildrenOfType<MapObject>();
		Instance.AllMapObjects = Instance.AllMapObjects.Except(loadedMapObjects).ToList();
		map.QueueFree();
	}

	public static void RegisterMapObject(MapObject mapObject) {
		Instance.AllMapObjects.Add(mapObject);
	}
	
	public static void UnregisterMapObject(MapObject mapObject) {
		Instance.AllMapObjects.Remove(mapObject);
	}

	public static List<MapObject> GetOccupants(MapNode node) {
		List<MapObject> occupants = new List<MapObject>();
		foreach(MapObject mapObject in Instance.AllMapObjects) {
			if(mapObject.GetCurrentNode() == node) {
				occupants.Add(mapObject);
			}
		}
		return occupants;
	}

	public static bool IsTraversable(MapNode node) {
		List<MapObject> occupants = GetOccupants(node);
		foreach(MapObject occupant in occupants) {
			if(occupant.IsSolid == true) {
				return false;
			}
		}
		return true;
	}
}
