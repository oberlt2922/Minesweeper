using System;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //change if statements so they are more readable
                    //add variable for i + yNeighborArray[z] etc
        //add method to set button background immage to number pictures


        /////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////Global Variables
        bool gameOver;
        int gridSize;
        int mineLimit;
        int flagLimit;
        MineSweeperButton[,] btnArray;
        int[] yNeighborArray = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };
        int[] xNeighborArray = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 };
        ///////////////////////////////////////Global Variables
        /////////////////////////////////////////////////////////////////////////////////////////




        ////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////New Game
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            switch (menuItem.Text)
            {
                case "Easy":
                    {
                        if (MessageBox.Show("Do you want to start a new game in" + menuItem.Text + " mode.",
                        "New Game", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ClearForm();
                            gameOver = false;
                            gridSize = 10;
                            mineLimit = 15;
                            flagLimit = 15;
                            SetUpGrid();
                            FillMines();
                            FillNumbers();
                        }
                        break;
                    }
                case "Medium":
                    {
                        if (MessageBox.Show("Do you want to start a new game in" + menuItem.Text + " mode.",
                        "New Game", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ClearForm();
                            gameOver = false;
                            gridSize = 15;
                            mineLimit = 40;
                            flagLimit = 40;
                            SetUpGrid();
                            FillMines();
                            FillNumbers();
                        }
                        break;
                    }
                case "Hard":
                    {
                        if (MessageBox.Show("Do you want to start a new game in" + menuItem.Text + " mode.",
                        "New Game", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ClearForm();
                            gameOver = false;
                            gridSize = 20;
                            mineLimit = 80;
                            flagLimit = 80;
                            SetUpGrid();
                            FillMines();
                            FillNumbers();
                        }
                        break;
                    }
            }
            textBoxFlags.Text = Convert.ToString(flagLimit);
        }
        ///////////////////////////////////////////New Game
        ////////////////////////////////////////////////////////////////////////////////////////
        




        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////Clear form
        private void ClearForm()
        {
            labelGameOver.Visible = false;
            if(btnArray != null)
            {
                foreach (MineSweeperButton btn in btnArray)
                {
                    this.Controls.Remove(btn);
                }
            }
            btnArray = null;
        }
        //////////////////////////////////Clear Form
        ////////////////////////////////////////////////////////////////////////////////////////





        ////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////Fill form with buttons
        private void SetUpGrid()
        {
            btnArray = new MineSweeperButton[gridSize, gridSize];
            int btnHeight = 20,
                btnWidth = 20;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    MineSweeperButton newBtn = new MineSweeperButton();
                    btnArray[i, j] = newBtn;
                    btnArray[i, j].Height = btnHeight;
                    btnArray[i, j].Width = btnWidth;
                    btnArray[i, j].Location = new Point(20 * i + 30, 20 * j + 30);
                    this.Controls.Add(btnArray[i, j]);
                    newBtn.NeighborNumber = 0;
                    newBtn.Mine = false;
                    newBtn.Revealed = false;
                    newBtn.Flagged = false;
                    newBtn.Y = i;
                    newBtn.X = j;
                    newBtn.Click += button_Click;
                    newBtn.MouseDown += button_Right_Click;
                }
            }
        }
        ////////////////////////////////////Fill form with buttons
        ///////////////////////////////////////////////////////////////////////////////////////




        ///////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////Fill Mines
        private void FillMines()
        {
            Random rnd = new Random();
            int mineCount = 0;
            while (mineCount < mineLimit)
            {
                int y = rnd.Next(gridSize);
                int x = rnd.Next(gridSize);
                if (btnArray[y, x].Mine == true)
                {
                    continue;
                }
                else
                {
                    btnArray[y, x].Mine = true;
                    mineCount++;
                }
            }
        }
        ////////////////////////////////////Fill Mines
        ///////////////////////////////////////////////////////////////////////////////////////




        //////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////Fill Numbers
        private void FillNumbers()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int neighborCount = 0;
                    for (int z = 0; z < xNeighborArray.Length; z++)
                    {
                        if (i + yNeighborArray[z] < 0 || i + yNeighborArray[z] >= gridSize || j + xNeighborArray[z] < 0 || j + xNeighborArray[z] >= gridSize)
                        {
                            continue;
                        }
                        if (btnArray[i + yNeighborArray[z], j + xNeighborArray[z]].Mine == true)
                        {
                            neighborCount++;
                        }
                    }
                    btnArray[i, j].NeighborNumber = neighborCount;
                }
            }
        }
        ////////////////////////////////////Fill Numbers
        /////////////////////////////////////////////////////////////////////////////////////





        ///////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////Button left click
        private void button_Click(object sender, EventArgs e)
        {
            MineSweeperButton btn = sender as MineSweeperButton;
            if(btn.Revealed == false && btn.Flagged == false)
            {
                if(btn.Mine == true)
                {
                    gameOver = true;
                    labelGameOver.Text = "You Lost!";
                    labelGameOver.ForeColor = Color.Red;
                    labelGameOver.Visible = true;
                    for(int i = 0; i < gridSize; i++)
                    {
                        for(int j = 0; j < gridSize; j++)
                        {
                            btnArray[i, j].Enabled = false;
                            btnArray[i, j].BackgroundImage = null;
                            if (btnArray[i, j].Mine == true)
                            {
                                btnArray[i, j].Text = "*";
                                btnArray[i, j].BackColor = Color.Red;
                            }
                            else if (btnArray[i, j].NeighborNumber == 0 && btnArray[i, j].Mine == false)
                            {
                                continue;
                            }
                            else
                            {
                                btnArray[i, j].Text = Convert.ToString(btnArray[i, j].NeighborNumber);
                            }
                        }
                    }
                }
                else if(btn.NeighborNumber != 0)
                {
                    btn.Enabled = false;
                    btn.Revealed = true;
                    btn.Text = Convert.ToString(btn.NeighborNumber);
                    CheckWin();
                }
                else
                {
                    int y = btn.Y;
                    int x = btn.X;
                    RevealButtons(y, x);
                    CheckWin();
                }
            }
        }
        /////////////////////////////////////Button left click
        ///////////////////////////////////////////////////////////////////////////////////////
        



        ///////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////Reveal Buttons
        private void RevealButtons(int y, int x)
        {
            btnArray[y, x].Enabled = false;
            btnArray[y, x].Revealed = true;
            for (int z = 0; z < xNeighborArray.Length; z++)
            {
                if (y + yNeighborArray[z] < 0 || y + yNeighborArray[z] >= gridSize || x + xNeighborArray[z] < 0 || x + xNeighborArray[z] >= gridSize)
                {
                    continue;
                }
                else if(btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].Revealed == true || btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].Flagged == true)
                {
                    continue;
                }

                else
                {
                    btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].Enabled = false;
                    btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].Revealed = true;
                    if (btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].NeighborNumber != 0)
                    {
                        btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].Text = Convert.ToString(btnArray[y + yNeighborArray[z], x + xNeighborArray[z]].NeighborNumber);
                    }
                    else
                    {
                        RevealButtons(y + yNeighborArray[z], x + xNeighborArray[z]);
                    }
                }
            }
        }
        //////////////////////////////////////////Reveal Buttons
        ////////////////////////////////////////////////////////////////////////////////////////




        //////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////Button right click
        private void button_Right_Click(object sender, MouseEventArgs e)
        {
            int flagCount;
            MineSweeperButton btn = (MineSweeperButton)sender;
            if (e.Button == MouseButtons.Right)
            {
                if(btn.Flagged == false)
                {
                    flagCount = int.Parse(textBoxFlags.Text);
                    if (flagCount > 0)
                    {
                        btn.Flagged = true;
                        btn.BackgroundImage = System.Drawing.Image.FromFile("Minesweeper_flag3.png");
                        flagCount--;
                        textBoxFlags.Text = Convert.ToString(flagCount);
                        CheckFlagWin();
                    }
                }
                else
                {
                    btn.Flagged = false;
                    btn.BackgroundImage = null;
                    flagCount = int.Parse(textBoxFlags.Text);
                    flagCount++;
                    textBoxFlags.Text = Convert.ToString(flagCount);
                }
            }
        }
        /////////////////////////////////////Button right click
        //////////////////////////////////////////////////////////////////////////////////////
        


        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////Check for reveal win
        private void CheckWin()
        {
            int remaining = 0;
            for(int i = 0; i < gridSize; i++)
            {
                for(int j = 0; j < gridSize; j++)
                {
                    if(btnArray[i,j].Revealed == false)
                    {
                        remaining++;
                    }
                }
            }
            if(remaining == flagLimit)
            {
                gameOver = true;
                labelGameOver.Text = "You Won!";
                labelGameOver.ForeColor = Color.Blue;
                labelGameOver.Visible = true;
                for(int i = 0; i < gridSize; i++)
                {
                    for(int j = 0; j < gridSize; j++)
                    {
                        btnArray[i, j].Enabled = false;
                        if(btnArray[i,j].Mine == true)
                        {
                            btnArray[i, j].Text = "*";
                            btnArray[i, j].BackColor = Color.Green;
                        }
                    }
                }
            }

        }
        //////////////////////////////////////Check for reveal win
        //////////////////////////////////////////////////////////////////////////////////////
        

        //////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////Check for flag win
        private void CheckFlagWin()
        {
            bool win = true;
            int flagCount = int.Parse(textBoxFlags.Text);
            if(flagCount == 0)
            {
                for(int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (btnArray[i, j].Flagged == true)
                        {
                            if(btnArray[i,j].Mine == false)
                            {
                                win = false;
                            }
                        }
                    }
                }
                if(win == true)
                {
                    gameOver = true;
                    labelGameOver.Text = "You Won!";
                    labelGameOver.ForeColor = Color.Blue;
                    labelGameOver.Visible = true;
                    for(int i = 0; i < gridSize; i++)
                    {
                        for(int j = 0; j < gridSize; j++)
                        {
                            btnArray[i, j].Enabled = false;
                        }
                    }
                }
            }
        }
        /////////////////////////Check for flag win
        ///////////////////////////////////////////////////////////////////////////////////////




        /////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////Quit
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /////////////////////////////////////Quit
        /////////////////////////////////////////////////////////////////////////////////////
    }
}