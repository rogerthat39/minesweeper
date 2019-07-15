namespace minesweeper
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gbxButtons = new System.Windows.Forms.GroupBox();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.lblFlagCounter = new System.Windows.Forms.Label();
            this.pbxFlagLabel = new System.Windows.Forms.PictureBox();
            this.lblTimer = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbxFlagLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxButtons
            // 
            this.gbxButtons.Location = new System.Drawing.Point(12, 50);
            this.gbxButtons.Name = "gbxButtons";
            this.gbxButtons.Size = new System.Drawing.Size(372, 376);
            this.gbxButtons.TabIndex = 0;
            this.gbxButtons.TabStop = false;
            // 
            // btnNewGame
            // 
            this.btnNewGame.Image = ((System.Drawing.Image)(resources.GetObject("btnNewGame.Image")));
            this.btnNewGame.Location = new System.Drawing.Point(170, 7);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(42, 42);
            this.btnNewGame.TabIndex = 0;
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // lblFlagCounter
            // 
            this.lblFlagCounter.AutoSize = true;
            this.lblFlagCounter.BackColor = System.Drawing.Color.White;
            this.lblFlagCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblFlagCounter.Location = new System.Drawing.Point(303, 17);
            this.lblFlagCounter.Name = "lblFlagCounter";
            this.lblFlagCounter.Size = new System.Drawing.Size(30, 24);
            this.lblFlagCounter.TabIndex = 1;
            this.lblFlagCounter.Text = "40";
            // 
            // pbxFlagLabel
            // 
            this.pbxFlagLabel.Image = ((System.Drawing.Image)(resources.GetObject("pbxFlagLabel.Image")));
            this.pbxFlagLabel.Location = new System.Drawing.Point(281, 17);
            this.pbxFlagLabel.Name = "pbxFlagLabel";
            this.pbxFlagLabel.Size = new System.Drawing.Size(23, 23);
            this.pbxFlagLabel.TabIndex = 2;
            this.pbxFlagLabel.TabStop = false;
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.BackColor = System.Drawing.Color.White;
            this.lblTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblTimer.Location = new System.Drawing.Point(61, 17);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(55, 24);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "00:00";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 438);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.pbxFlagLabel);
            this.Controls.Add(this.lblFlagCounter);
            this.Controls.Add(this.gbxButtons);
            this.Controls.Add(this.btnNewGame);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Minesweeper";
            ((System.ComponentModel.ISupportInitialize)(this.pbxFlagLabel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxButtons;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Label lblFlagCounter;
        private System.Windows.Forms.PictureBox pbxFlagLabel;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Timer timer1;
    }
}

