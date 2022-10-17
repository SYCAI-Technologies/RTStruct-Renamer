using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTLabelRenamer
{
    
    public partial class Form3 : Form
    {
        private TextBox TBox = new TextBox();
        public Form3()
        {
            InitializeComponent();
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            int vertPos = 10;

            
                TBox.Location = new Point(85, vertPos);
                TBox.Size = new Size(150, 50);
                TBox.TabIndex = 1;
                this.Controls.Add(TBox);

                vertPos += 20;
            
           
        }
    }
}
