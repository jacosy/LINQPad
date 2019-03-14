<Query Kind="Program" />

void Main()
{
	var root = new TreeNode(1);
	root.left = null;
	root.right = new TreeNode(2);
	root.right.left = new TreeNode(3);
	var sln = new Solution();
	sln.PreorderTraversal(root).Dump();
}

// Define other methods and classes here

// Definition for a binary tree node.
public class TreeNode
{
	public int val;
	public TreeNode left;
	public TreeNode right;
	public TreeNode(int x) { val = x; }
}

public class Solution
{
	public IList<int> PreorderTraversal(TreeNode root)
	{
		var output = new List<int>();
		if (root != null)
		{
			output.Add(root.val);
			output.AddRange(LoopTreeNode(root));
		}
		return output;
	}

	private IList<int> LoopTreeNode(TreeNode root)
	{
		var result = new List<int>();		
		if (root.left != null)
		{
			result.Add(root.left.val);
			result.AddRange(LoopTreeNode(root.left));
		}
		if (root.right != null)
		{
			result.Add(root.right.val);
			result.AddRange(LoopTreeNode(root.right));
		}
		return result;
	}
}