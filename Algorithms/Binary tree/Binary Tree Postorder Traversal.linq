<Query Kind="Program" />

void Main()
{
	var root = new TreeNode(1);
	root.left = null;
	root.right = new TreeNode(2);
	root.right.left = new TreeNode(3);
	var sln = new Solution();
	sln.PostorderTraversal(root).Dump();
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
	private List<int> elementList { get; set; } = new List<int>();

	public IList<int> PostorderTraversal(TreeNode node)
	{
		if (node != null)
		{
			if (node.left != null)
			{
				PostorderTraversal(node.left);
			}			

			if (node.right != null)
			{
				PostorderTraversal(node.right);
			}
			
			elementList.Add(node.val);
		}
		return elementList;
	}
}