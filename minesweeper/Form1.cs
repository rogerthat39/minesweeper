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
    public partial class Form1 : Form
    {
        public Form1()
        {
            //set up the initial game
            InitializeComponent();
            CreateButtons(FIELD_SIZE);
            bm.PlaceMines(MINE_NUM, mines, buttonsList);
        }

        //global variables
        Game game = new Game();
        ButtonManager bm = new ButtonManager();
        List<Button> buttonsList = new List<Button>();

        int FIELD_SIZE = 16; //creates a 16 by 16 grid
        static int MINE_NUM = 40;
        int[] mines = new int[MINE_NUM]; //stores the position (index) of mines in buttonsList

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

        //when the left mouse has been held down, display the "worried" expression on the face
        private void newButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && game.IsGameStillGoing)
            {
                btnNewGame.Image = Image.FromFile("face_worried.png");
            }
        }

        //when any button on the 'mine field' is clicked with a right or left click
        private void newButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (game.IsGameStillGoing)
            {
                //code from stackoverflow - https://stackoverflow.com/questions/14479143/what-is-the-use-of-object-sender-and-eventargs-e-parameters

                //object 'sender' is the clicked button, but since it is of type object (and not button) we can't apply button rules to it
                Button button = sender as Button; //changes sender to Button type                

                //if the right mouse button was clicked, toggle between flag icon, ?, and nothing (in that order)
                if (e.Button == MouseButtons.Right)
                {
                    //if flag icon already there, take away the image and put ? icon in its place
                    if (button.Image != null)
                    {
                        button.Image = null;
                        button.Text = "?";
                    }
                    //if there's a ?, remove it to show a blank button
                    else if(button.Text == "?")
                    {
                        button.Text = "";
                    }
                    //if there isn't an image, show the flag icon
                    else if (button.BackColor == Color.Gray) //only put flag on buttons that are un-clicked
                    {
                        button.Image = Image.FromFile("flag.png");
                    }
                }
                //if left mouse button clicked, display hidden value
                //if user has flagged button, ignore the left click
                else if(e.Button == MouseButtons.Left && button.Image == null)
                {
                    btnNewGame.Image = Image.FromFile("face_smile.png"); //change the smiley face back to the default
                    bm.ClickButton(button); //change button style and display hidden value

                    //additional tasks to complete if the button was a 0 or a mine
                    if (button.Name == "0")
                    {
                        bm.ClickZeros(button, buttonsList); //make surrounding 0's (and numbers) also clicked
                    }
                    else if (button.Name == "X") //if a mine was clicked
                    {
                        game.GameLost(buttonsList, mines, btnNewGame);
                    }

                    //invoke win state if win condition is met
                    if (game.CheckWinCondition(buttonsList))
                    {
                        game.GameWon(btnNewGame);
                    }
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            //clears and resets values on the buttons for the next game
            game.IsGameStillGoing = true;
            btnNewGame.Image = Image.FromFile("face_smile.png");
            foreach (Button button in buttonsList)
            {
                button.Name = "0";
                button.Text = "";
                button.Image = null;
                button.BackColor = Color.Gray; //resets color back to normal
            }
            bm.PlaceMines(MINE_NUM, mines, buttonsList);
        }
    }
}
