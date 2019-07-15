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
            CreateButtons(FIELD_SIZE);
            newGame();
        }

        //global variables
        ButtonManager bm = new ButtonManager();

        List<Button> buttonsList = new List<Button>();
        int minesToFind;
        int totalTime; //stores how long the player takes to solve the puzzle (in seconds)

        bool isGameStillGoing = true; //true if game is running, false if game has been won or lost
        int FIELD_SIZE = 16; //creates a 16 by 16 grid
        int[] mines = new int[40]; //stores the position (index) of mines in buttonsList

        /// <summary>
        /// Create the grid of buttons based on the width/height given
        /// </summary>
        /// <param name="field_size">The width and height of the grid</param>
        public void CreateButtons(int field_size)
        {
            //loops for each row
            for (int r = 0; r < field_size; r++)
            {
                //loops for each column
                for (int c = 0; c < field_size; c++)
                {
                    Button newButton = new Button
                    {
                        Location = new Point(9 + 22 * c, 15 + 22 * r), //puts buttons in the row and column next to the previous button
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
                bm.ClickZeros(clickedButton, buttonsList); //make surrounding 0's (and numbers) also clicked
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
            //clears and resets values on the buttons for the new game
            foreach (Button button in buttonsList)
            {
                button.Name = "0";
                button.Text = "";
                button.Image = null;
                button.BackColor = Color.Gray; //resets color back to normal
            }

            newGame();
        }

        private void newGame()
        {
            //set values for new game
            isGameStillGoing = true;
            btnNewGame.Image = Image.FromFile("face_smile.png");
            minesToFind = mines.Length;
            totalTime = 0;
            timer1.Start();

            bm.PlaceMines(mines, buttonsList);
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
    }
}