namespace CHAPTER_4;

class SortingMethods
{
    // * Bubble Sort
    public int[] BubbleSort(int[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                }
            }
        }
        return array;
    }

    // * Selection Sort
    public int[] SelectionSort(int[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (array[j] < array[minIndex])
                {
                    minIndex = j;
                }
            }
            if (minIndex != i)
            {
                (array[i], array[minIndex]) = (array[minIndex], array[i]);
            }
        }
        return array;
    }

    // * Merge Sort
    int[] MergeSort(int[] array)
    {
        if (array.Length <= 1)
            return array;

        int n = array.Length;
        int mid = n / 2;

        int[] left = new int[mid];
        int[] right = new int[n - mid];

        Array.Copy(array, 0, left, 0, mid);
        Array.Copy(array, mid, right, 0, n - mid);

        left = MergeSort(left);
        right = MergeSort(right);

        return Merge(left, right);
    }

    static int[] Merge(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0, j = 0, k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
                result[k++] = left[i++];
            else
                result[k++] = right[j++];
        }

        while (i < left.Length)
            result[k++] = left[i++];

        while (j < right.Length)
            result[k++] = right[j++];

        return result;
    }
    
    // * Quick Sort
    void QuickSort(int[] array, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(array, low, high);

            QuickSort(array, low, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, high);
        }
    }

    static int Partition(int[] array, int low, int high)
    {
        int pivot = array[high];
        int i = low - 1;

        for (var j = low; j < high; j++)
        {
            if (array[j] >= pivot) continue;
            i++;
            (array[i], array[j]) = (array[j], array[i]);
        }

        (array[i + 1], array[high]) = (array[high], array[i + 1]);

        return i + 1;
    }
    
    // * Insertion Sort
    int[] InsertionSort(int[] array)
    {
        int n = array.Length;

        for (int i = 1; i < n; i++)
        {
            int key = array[i];
            int j = i - 1;

            // Shift elements to the right
            while (j >= 0 && array[j] > key)
            {
                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = key;
        }

        return array;
    }
}

class SearchingMethods
{
    public int LinearSearch(int[] array, int target)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
            {
                return i;
            }
        }
        return -1;
    }

    public int BinarySearc(int[] array, int target)
    {
        int left  = 0;
        int right = array.Length - 1;
        while (left <= right)
        {
            var mid = (left + right) / 2;
            if (array[mid] == target) return mid;
            if  (array[mid] > target)  right = mid - 1;
            else left = mid + 1;
        }
        return -1;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}