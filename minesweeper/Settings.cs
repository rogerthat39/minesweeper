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

        /// <summary>
        /// Used by Form1 to decide whether to create a new grid or not
        /// True if settings were changed, false by default
        /// </summary>
        public static bool wereSettingsChanged;

        /// <summary>
        /// Close the Settings panel without changing any settings
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Change the grid size to what was selected and close the Settings form
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            wereSettingsChanged = true;

            if (rdoSmall.Checked)
            {
                Form1.ChangeGridSize(9, 9, 10);
                this.Close();
            }
            else if(rdoMedium.Checked)
            {
                Form1.ChangeGridSize(16, 16, 40);
                this.Close();
            }
            else if(rdoLarge.Checked)
            {
                Form1.ChangeGridSize(30, 16, 99);
                this.Close();
            }
            else if(rdoCustom.Checked)
            {
                //rounding each number (because numericUpDown control rounds the value displayed)
                int width = (int)Math.Round(numericWidth.Value, 0, MidpointRounding.AwayFromZero);
                int height = (int)Math.Round(numericHeight.Value, 0, MidpointRounding.AwayFromZero);
                int mines = (int)Math.Round(numericMines.Value, 0, MidpointRounding.AwayFromZero);

                //checking the values are within the acceptable range
                if (height < 6)
                {
                    MessageBox.Show("The height cannot be less than 6");
                }
                else if(width < 6)
                {
                    MessageBox.Show("The width cannot be less than 6");
                }
                else if(mines < 1)
                {
                    MessageBox.Show("Please enter at least one mine");
                }
                else if(height*width <= mines)
                {
                    MessageBox.Show("Cannot have all tiles be mines!");
                }
                else //if values are ok
                {
                    Form1.ChangeGridSize(width, height, mines);
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Selects the "custom" radio button if the height numericUpDown control has its value changed
        /// </summary>
        private void numericHeight_ValueChanged(object sender, EventArgs e)
        {
            rdoCustom.Checked = true;
        }

        /// <summary>
        /// Selects the "custom" radio button if the width numericUpDown control has its value changed
        /// </summary>
        private void numericWidth_ValueChanged(object sender, EventArgs e)
        {
            rdoCustom.Checked = true;
        }

        /// <summary>
        /// Selects the "custom" radio button if the mines numericUpDown control has its value changed
        /// </summary>
        private void numericMines_ValueChanged(object sender, EventArgs e)
        {
            rdoCustom.Checked = true;
        }
    }
}
