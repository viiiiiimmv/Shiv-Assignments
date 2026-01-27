using System.Collections;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialisation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        ArrayList obj = new ArrayList();

        private void button1_Click(object sender, EventArgs e)
        {
            obj.Add(textBox1.Text);
            textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SoapFormatter ss = new SoapFormatter();
            string filename = @"C:\Users\shivc\Desktop\sampledemo.xml";
            using (FileStream fs = new FileStream
                (filename, FileMode.Create))
            {
                ss.Serialize(fs, obj);
            }

        }
        [Serializable]
        class Customer
        {
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string City { get; set; }
        }

        private const string filename2 = "data.dat";
        FileStream fs;
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter;

        private void button2_Click(object sender, EventArgs e)
        {
            Customer c1 = new Customer();
            c1.CustomerID = Convert.ToInt32(textBox2.Text);
            c1.CustomerName = textBox3.Text;
            c1.City = textBox4.Text;
            fs = File.Create(filename2);
            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(fs, c1);
            fs.Close();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fs = File.OpenRead(filename2);
            Customer c2 = (Customer)formatter.Deserialize(fs);
            textBox2.Text = c2.CustomerID.ToString();
            textBox3.Text = c2.CustomerName;
            textBox4.Text = c2.City;
            fs.Close();
        }
    }
}
