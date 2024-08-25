using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotSkype
{
    public partial class AboutNotSkype : Form
    {
        public AboutNotSkype()
        {
            InitializeComponent();
        }

        private void AboutNotSkype_Resize(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(526, 410);
        }
    }
}
