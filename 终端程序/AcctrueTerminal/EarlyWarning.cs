using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AcctrueTerminal
{
    public partial class EarlyWarning : Form
    {
        public EarlyWarning()
        {
            InitializeComponent();
        }

        private void EarlyWarning_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}