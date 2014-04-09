using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AcctrueTerminal.ManageBox
{
    public partial class Frm_Groupbox : Form
    {
        public Frm_Groupbox()
        {
            InitializeComponent();
        }

        private void Frm_Groupbox_Closed(object sender, EventArgs e)
        {
            this.Close();
        }       
    }
}