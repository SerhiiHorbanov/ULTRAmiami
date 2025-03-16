using Godot;
using System;

public static class NodeTreeHelper
{
	public static T GetAncestor<T>(this Node node) where T : Node
	{
		while (node != null)
		{
			if (node is T t)
				return t;
			
			node = node.GetParent();
		}
		
		return null;
	}

	public static void MakeSiblingOf(this Node node, Node sibling)
		=> node.Reparent(sibling.GetParent());
}
