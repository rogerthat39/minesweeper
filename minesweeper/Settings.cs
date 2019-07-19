using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            wereSettingsChanged = false;
        }

        public static bool wereSettingsChanged;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(rdoSmall.Checked)
            {
                Form1.ChangeGridSize(9, 9, 10);
                wereSettingsChanged = true;
                this.Hide();
            }
            if(rdoMedium.Checked)
            {
                Form1.ChangeGridSize(16, 16, 40);
                wereSettingsChanged = true;
                this.Hide();
            }
            if(rdoLarge.Checked)
            {
                Form1.ChangeGridSize(30, 16, 99);
                wereSettingsChanged = true;
                this.Hide();
            }
        }
    }
}
