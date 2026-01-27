
namespace FileHandlingUsingConsole
{
    internal class Program
    {
        public static void readData() {
            FileStream fs = null;
            StreamReader sr = null;

            fs = new FileStream(@"C:\Users\shivc\Desktop\Training\DAY-12\FileHandlingUsingConsole\sample.txt", FileMode.Open, FileAccess.Read);
            sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = sr.ReadLine();
            while (str != null) {
                Console.WriteLine($"{str}");
                str = sr.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            readData();
        }
    }
}
