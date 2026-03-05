namespace TREES;

class TreeNode
{
    public int Value;
    public TreeNode Left;
    public TreeNode Right;

    public TreeNode(int x)
    {
        Value = x;
        Left = null;
        Right = null;
    }
}

class BinarySearchTree
{
    public TreeNode root;

    public BinarySearchTree()
    {
        root = null;
    }

    public void Insert(int val)
    {
        root = InsertNode(root, val);
    }

    TreeNode InsertNode(TreeNode? Node, int val)
    {
        if (Node == null) return new TreeNode(val);
        
        if (val < Node.Value) {
            Node.Left = InsertNode(Node.Left, val);
        } else if (val > Node.Value) {
            Node.Right = InsertNode(Node.Right, val);
        }

        return Node;
    }
}

class Program
{
    // ! Types of traversals in trees
    void PreOrder(TreeNode? root)
    {
        if (root == null)
        {
            return;
        }
        Console.WriteLine(root.Value);
        PreOrder(root.Left);
        PreOrder(root.Right);
    }

    void InOrder(TreeNode? root)
    {
        if (root == null) return;
        InOrder(root.Left);
        Console.WriteLine(root.Value);
        InOrder(root.Right);
    }

    void PostOrder(TreeNode? root)
    {
        if (root == null) return;
        PostOrder(root.Left);
        PostOrder(root.Right);
        Console.WriteLine(root.Value);
    }
    
    // ! Searching in Binary Search Tree
    
    // * Depth First Search - Inorder, Preorder, Postorder
    // * Breadth First Search
    
    // * Basic BFS / Printing Binary Search Tree
    static void LevelOrder(TreeNode root)
    {
        if (root == null) return;
        
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            TreeNode current = queue.Dequeue();
            Console.Write(current.Value+" ");
            
            if (current.Left != null) queue.Enqueue(current.Left);
            if (current.Right != null) queue.Enqueue(current.Right);
        }
    }

    static void PrintBinarySearchTree(BinarySearchTree tree)
    {
        LevelByLevel(tree.root);
    }
    
    // * Grouped Levels
    static void LevelByLevel(TreeNode root)
    {
        if (root == null) return;
        
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                TreeNode curr =  queue.Dequeue();
                Console.Write(curr.Value+" ");
                
                if  (curr.Left != null) queue.Enqueue(curr.Left);
                if  (curr.Right != null) queue.Enqueue(curr.Right);
            }
            
            Console.WriteLine();
        }
    }
    
    
    // * Height of binary search tree

    int Height(TreeNode root)
    {
        if (root == null) return 0;
        return 1 + Math.Max(Height(root.Left), Height(root.Right));
    }
    
    
    
    static void Main(string[] args)
    {
        // ? Trees
        // * --> Non linear data structure 
        // * --> Contains parents, child nodes and relationships
        // * --> Number of nodes in tree : n, Number of edges : n - 1
        // * --> Tree can be empty
        // * --> Root Node : no parent node, Sibling Node(s) : Same parent node
        // * --> Leaf/External Node : No children nodes
        // * --> Edge(u,v) -> 'u' is the parent of 'v'
        // * --> Collection of disjoint nodes is known as 'Forest'
        // * --> Height of tree : DFS, Diameter of tree : BFS
        // * --> Degree of a node : Number of children nodes
        // * --> Maximum number of nodes of binary tree of height 'h' : 2^h - 1
        // * --> Left skewed binary tree : only left side children nodes exist
        // * --> Right skewed binary tree : only right side children nodes exist

        // ? Implementation of tree using array
        // * --> left child = 2*i+1, right child = 2*(i+1)

        // int[] treeArray = { 1, 2, 3, 4, 5, 6, 7 };
        // int treeHeight = (int)Math.Floor(Math.Log2(treeArray.Length)) + 1;
        //
        // for (int i = 0; i < treeHeight; i++)
        // {
        //     Console.WriteLine(
        //         $"Parent Node : {treeArray[i]}\t Left Child Node : {treeArray[2 * i + 1]}\tRight Child Node : {treeArray[2 * (i + 1)]}");
        // }

        // ? Traversing a tree
        // * --> Preorder : N L R
        // * --> Inorder : L N R
        // * --> Postorder : L R N
        
        // ? Binary Search Tree 
        // * --> A binary tree where left element should be less than parent node and right element should be greater than parent node
        
        BinarySearchTree bst  = new BinarySearchTree();
        for (int i=1;i<8;i++) bst.Insert(i);
        PrintBinarySearchTree(bst);
    }
}