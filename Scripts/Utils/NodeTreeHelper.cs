using Godot;

namespace ULTRAmiami;

public static class NodeTreeHelper
{
	public static T GetChild<T>(this Node node) where T : Node
	{
		foreach (Node child in node.GetChildren())
			if (child is T t)
				return t;
		return null;
	}
	
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
	{
		if (node.GetParent() != null)
			node.Reparent(sibling.GetParent());
		else
			sibling.GetParent().AddChild(node);
	}
}
