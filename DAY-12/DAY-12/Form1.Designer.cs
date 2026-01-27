namespace DAY_12
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button8 = new Button();
            button7 = new Button();
            colorDialog1 = new ColorDialog();
            fontDialog1 = new FontDialog();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(33, 35);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(728, 335);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Font = new Font("Times New Roman", 9F);
            button1.Location = new Point(33, 376);
            button1.Name = "button1";
            button1.Size = new Size(80, 29);
            button1.TabIndex = 1;
            button1.Text = "COLOR";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.Font = new Font("Times New Roman", 9F);
            button2.Location = new Point(119, 376);
            button2.Name = "button2";
            button2.Size = new Size(80, 29);
            button2.TabIndex = 2;
            button2.Text = "BG COLOR";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Times New Roman", 9F);
            button3.Location = new Point(205, 376);
            button3.Name = "button3";
            button3.Size = new Size(80, 29);
            button3.TabIndex = 3;
            button3.Text = "FONT";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Font = new Font("Times New Roman", 9F);
            button4.Location = new Point(291, 376);
            button4.Name = "button4";
            button4.Size = new Size(80, 29);
            button4.TabIndex = 4;
            button4.Text = "OPEN";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Font = new Font("Times New Roman", 9F);
            button5.Location = new Point(423, 376);
            button5.Name = "button5";
            button5.Size = new Size(80, 29);
            button5.TabIndex = 5;
            button5.Text = "SAVE";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Font = new Font("Times New Roman", 9F);
            button6.Location = new Point(509, 376);
            button6.Name = "button6";
            button6.Size = new Size(80, 29);
            button6.TabIndex = 6;
            button6.Text = "APPEND";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button8
            // 
            button8.Font = new Font("Times New Roman", 9F);
            button8.Location = new Point(595, 376);
            button8.Name = "button8";
            button8.Size = new Size(80, 29);
            button8.TabIndex = 7;
            button8.Text = "COPY";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.Font = new Font("Times New Roman", 9F);
            button7.Location = new Point(681, 376);
            button7.Name = "button7";
            button7.Size = new Size(80, 29);
            button7.TabIndex = 8;
            button7.Text = "DELETE";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(button7);
            Controls.Add(button8);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button8;
        private Button button7;
        private ColorDialog colorDialog1;
        private FontDialog fontDialog1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
    }
}
