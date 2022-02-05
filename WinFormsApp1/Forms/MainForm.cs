namespace tiktaktoe
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //сброс текста на всех кнопках и начало новой игры
            lbl_WinLose.Text = ""; 
            for (int i = 0; i < tbl_Game.Controls.Count; i++)
            {
                tbl_Game.Controls[i].Text = "";
            }
            EnableAllButton(true);

        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnableAllButton(bool enable)
        {
            //включение или выключение всех кнопок
            for(int i = 0; i < tbl_Game.Controls.Count; i++)
            {
                tbl_Game.Controls[i].Enabled = enable;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            //Перевод колекции кнопок в двумерный массив
            string[,] arr = new string[(int)Math.Sqrt(tbl_Game.Controls.Count), (int)Math.Sqrt(tbl_Game.Controls.Count)];
            for(int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                for(int j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    arr[i, j] = tbl_Game.GetControlFromPosition(i, j).Text;
                }
            }

            TikTakToe t = new TikTakToe(arr);

            //Получение координат кнопки на которую мы нажали
            var coord = tbl_Game.GetPositionFromControl(sender as Button);
            //если движение успешно мы продолжаем работу
            if (t.Move(coord.Row, coord.Column,"X") == true)
            {
                //Меняем текст кнопок на текст полученый в результате игры
                for (int i = 0; i <= arr.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= arr.GetUpperBound(1); j++)
                    {
                        tbl_Game.GetControlFromPosition(i, j).Text = t.Matrix[i, j];
                    }
                }
                //Проверяем на победу
                if (t.CheckWin("X") == true)
                {
                    lbl_WinLose.Text = "Win";
                    EnableAllButton(false);
                }
                else if (t.CheckWin("O") == true)
                {
                    lbl_WinLose.Text = "Lose";
                    EnableAllButton(false);
                }
                else if (t.CheckFull())
                {
                    lbl_WinLose.Text = "Draw";
                    EnableAllButton(false);
                }
            }
        }


    }
}