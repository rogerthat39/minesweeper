using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //set up the initial game
            InitializeComponent();
            ChangeGridSize(16, 16, 40);
            CreateButtons();
            NewGame();
        }

        //global variables
        ButtonManager bm = new ButtonManager();
        
        List<Button> buttonsList = new List<Button>();
        int minesToFind;
        int totalTime; //stores how long the player takes to solve the puzzle (in seconds)
        bool isGameStillGoing = true; //true if game is running, false if game has been won or lost

        static int gridWidth;
        static int gridHeight;
        static int[] mines;

        /// <summary>
        /// Set the height, width, and number of mines in the grid
        /// </summary>
        /// <param name="width">The width of the grid</param>
        /// <param name="height">The height of the grid</param>
        /// <param name="mineNum">The number of mines</param>
        public static void ChangeGridSize(int width, int height, int mineNum)
        {
            gridWidth = width;
            gridHeight = height;
            mines = new int[mineNum];
        }

        /// <summary>
        /// Change the size of the form (and groupbox) based on how large the grid is
        /// </summary>
        public void EditFormSize()
        {
            gbxButtons.Width = 22 * gridWidth + 20;
            gbxButtons.Height = 22 * gridHeight + 25;

            //change form width and height
            this.Size = new System.Drawing.Size(22 * gridWidth + 60, 22 * gridHeight + 150);

            //center the butotn
            btnNewGame.Location = new Point(this.Width / 2 - 30, 33);

            //center the labels depending on the grid size
            if (gridWidth <= 10)
            {
                lblTimer.Location = new Point(12, 43);
                pbxFlagLabel.Location = new Point(this.Width - lblFlagCounter.Width - pbxFlagLabel.Width - 28, 43);
                lblFlagCounter.Location = new Point(this.Width - lblFlagCounter.Width - 28, 43);
            }
            else if(gridWidth < 16)
            {
                lblTimer.Location = new Point(this.Width / 2 - 110, 43);
                pbxFlagLabel.Location = new Point(this.Width / 2 + 50, 43);
                lblFlagCounter.Location = new Point(this.Width / 2 + 73, 43);
            }
            else if(gridWidth < 20)
            {
                lblTimer.Location = new Point(this.Width / 2 - 130, 43);
                pbxFlagLabel.Location = new Point(this.Width / 2 + 70, 43);
                lblFlagCounter.Location = new Point(this.Width / 2 + 93, 43);
            }
            else
            {
                lblTimer.Location = new Point(this.Width / 2 - 160, 43);
                pbxFlagLabel.Location = new Point(this.Width / 2 + 100, 43);
                lblFlagCounter.Location = new Point(this.Width / 2 + 123, 43);
            }
        }

        /// <summary>
        /// Dispose of the physical buttons in the grid, and clear the buttonsList
        /// </summary>
        public void DeleteButtons()
        {
            foreach(Button b in buttonsList)
            {
                //https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-add-to-or-remove-from-a-collection-of-controls-at-run-time
                this.Controls.Remove(b);
                b.Dispose();
            }
            buttonsList.Clear();
        }

        /// <summary>
        /// Create the grid of buttons based on the width/height of the grid
        /// </summary>
        public void CreateButtons()
        {
            //loops for each row
            for (int r = 0; r < gridHeight; r++)
            {
                //loops for each column
                for (int c = 0; c < gridWidth; c++)
                {
                    Button newButton = new Button
                    {
                        Location = new Point(22 * c + 9, 22 * r + 15), //puts buttons in the row and column next to the previous button
                        Size = new Size(23, 23),
                        Name = "0",
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.Gray
                    };

                    buttonsList.Add(newButton); //will add buttons to list in the order they are created

                    gbxButtons.Controls.Add(newButton); //if you don't add controls to gbx, they won't show up on the screen

                    //creates click events for the button
                    newButton.MouseDown += newButton_MouseDown; //when the mouse is pressed down
                    newButton.MouseUp += newButton_MouseUp; //when the mouse is released
                }
            }
        }

        private void ButtonLeftClick(Button clickedButton)
        {
            bm.ClickButton(clickedButton);

            //additional tasks to complete if the button was a 0 or a mine
            if (clickedButton.Name == "0")
            {
                //make surrounding 0's (and numbers) also clicked
                bm.ClickZeros(clickedButton, buttonsList, gridWidth, gridHeight); 
            }
            else if (clickedButton.Name == "X")
            {
                timer1.Stop();
                Game.GameLost(buttonsList, mines);
                btnNewGame.Image = Image.FromFile("face_lose.png");
                isGameStillGoing = false; //so that you can't click any other mines after the game is finished
            }

            //after each button click, check if the game has now been won
            if (Game.CheckWinCondition(buttonsList))
            {
                timer1.Stop();
                Game.GameWon();
                btnNewGame.Image = Image.FromFile("face_win.png");
                isGameStillGoing = false; //so that you can't click any other mines after the game is finished
            }
        }

        //when the left mouse has been held down, display the "worried" expression on the face
        private void newButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isGameStillGoing)
            {
                btnNewGame.Image = Image.FromFile("face_worried.png");
            }
        }

        //when any button on the 'mine field' is clicked with a right or left click
        private void newButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (isGameStillGoing)
            {
                //from https://stackoverflow.com/questions/14479143/what-is-the-use-of-object-sender-and-eventargs-e-parameters
                //object 'sender' is the clicked button, but since it is of type object (and not button) we can't apply button rules to it
                Button button = sender as Button; //changes sender to Button type       

                btnNewGame.Image = Image.FromFile("face_smile.png"); //change the face back to the default smile

                if (e.Button == MouseButtons.Right)
                {
                    int indicator = bm.ToggleImage(button);

                    //increase or decrease minesToFind counter and update label
                    minesToFind += indicator;
                    lblFlagCounter.Text = minesToFind.ToString();
                }
                //if user has flagged button, ignore the left click
                else if (e.Button == MouseButtons.Left && button.Image == null)
                {
                    ButtonLeftClick(button);
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            ResetButtons();
            NewGame();
        }

        private void ResetButtons()
        {
            //clears and resets values on the buttons for the new game
            foreach (Button button in buttonsList)
            {
                button.Name = "0";
                button.Text = "";
                button.Image = null;
                button.BackColor = Color.Gray; //resets color back to normal
            }
        }

        private void NewGame()
        {
            //set values for new game
            isGameStillGoing = true;
            btnNewGame.Image = Image.FromFile("face_smile.png");
            minesToFind = mines.Length;
            lblFlagCounter.Text = minesToFind.ToString();
            totalTime = 0;
            lblTimer.Text = "0:00";
            timer1.Start();

            bm.PlaceMines(mines, buttonsList, gridWidth, gridHeight);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            totalTime++;
            double minutes = totalTime / 60;
            int seconds = totalTime % 60;

            //if there is only 1 digit in the seconds column
            if (seconds.ToString().Length < 2)
            {
                //add an extra '0' to the seconds when displaying to label
                lblTimer.Text = Math.Round(minutes).ToString() + ":0" + seconds.ToString();
            }
            else
            {
                lblTimer.Text = Math.Round(minutes).ToString() + ":" + seconds.ToString();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetButtons();
            NewGame();
        }

        private void boardSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop(); //pause the timer while the settings form is open

            Settings settingsPanel = new Settings();
            settingsPanel.ShowDialog();

            timer1.Start();

            if (Settings.wereSettingsChanged)
            {
                DeleteButtons();
                CreateButtons();
                EditFormSize();
                NewGame();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}