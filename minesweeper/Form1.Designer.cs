﻿namespace minesweeper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbxButtons = new System.Windows.Forms.GroupBox();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gbxButtons
            // 
            this.gbxButtons.Location = new System.Drawing.Point(12, 12);
            this.gbxButtons.Name = "gbxButtons";
            this.gbxButtons.Size = new System.Drawing.Size(372, 386);
            this.gbxButtons.TabIndex = 0;
            this.gbxButtons.TabStop = false;
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(12, 404);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(84, 23);
            this.btnNewGame.TabIndex = 0;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 435);
            this.Controls.Add(this.gbxButtons);
            this.Controls.Add(this.btnNewGame);
            this.Name = "Form1";
            this.Text = "Minesweeper";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxButtons;
        private System.Windows.Forms.Button btnNewGame;
    }
}

