namespace LinkedList;

public class Node
{
    public int val;
    public Node next;

    public Node(int value, Node nextNode)
    {
        val = value;
        next = nextNode;
    }
}

class Program
{
    private Node head;
    private Node tail;
    private int size;

    public Program()
    {
        head = null;
        tail = null;
        size = 0;
    }

    public int Length()
    {
        return size;
    }

    public bool isEmpty()
    {
        return size == 0;
    }

    public void AddLast(int value)
    {
        Node newNode = new Node(value, null);
        if (isEmpty())
        {
            head = newNode;
        }
        else
        {
            tail.next = newNode;
        }
        tail = newNode;
        size++;
    }

    public void AddAny(int value, int pos)
    {
        if (pos < 0 || pos > size)
        {
            Console.WriteLine("Invalid position");
            return;
        }
        
        Node newNode = new Node(value, null);
        Node curr = head;

        int i = 1;
        while (i < pos - 1)
        {
            curr = curr.next;
            i += 1;
        }
        newNode.next = curr.next;
        curr.next = newNode;
        size++;
    }

    public void Display()
    {
        Node curr = head;
        while (curr != null)
        {
            Console.Write(curr.val+" ");
            curr = curr.next;
        }
        Console.WriteLine();
    }
    
    static void Main(string[] args)
    {
        Program LL = new Program();
        LL.AddLast(1);
        LL.AddLast(2);
        LL.AddLast(3);
        LL.AddLast(4);
        LL.AddLast(5);
        LL.Display();
        Console.WriteLine($"Size of Linked List: {LL.Length()}");
        LL.AddAny(7, 4);
        LL.Display();
    }
}