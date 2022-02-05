namespace tiktaktoe
{
    class TikTakToe
    {
        string[,] game;
        public string [,] Matrix { get { return game; } }

        MoveCoord Choice;

        public TikTakToe(string [,] game)
        {
            this.game = game;
        }

        public bool Move(int x, int y, string sym, string[,]? arr = null)
        {
            if (arr == null)
                arr = game;

            if (arr[y,x] == "")
            {
                arr[y, x] = sym;

                if (arr == game)
                {
                    MoveBot(game);
                    if(arr[Choice.Y, Choice.X] == "")
                        arr[Choice.Y, Choice.X] = "O";
                }
                return true;
            }
            return false;
        }

        public bool CheckFull(string[,]? arr = null)
        {
            if (arr == null)
                arr = game;

            bool isFull = true;
            for(int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                for(int j=0; j <= arr.GetUpperBound(1); j++)
                {
                    if(arr[i,j]=="")
                        isFull = false;
                }
            }
            return isFull;
        }

        public int MoveBot(string[,]? arr = null, bool Turn = false)
        {
            //для хода бота я использовал алгоритм minmax
            //который рекурсивно предсказывает будущие ходы и выбирает самый лучший для себя 
            //правда он не очень любит защищаться
            string[,]? tmp = arr.Clone() as string[,];

            if (CheckWin("X", tmp))//оценка ходов
                return 10;
            else if (CheckWin("O", tmp))
                return -10;
            else if (CheckFull(tmp))
                return 0;

            List<MoveCoord> scores = new List<MoveCoord>();
            for(int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                for(int j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    if (tmp[i, j] == "")
                    {
                        Move(j, i, Turn ? "X" : "O", tmp);
                        scores.Add(new MoveCoord(MoveBot(tmp, !Turn), j, i));
                        tmp[i, j] = "";
                    }
                }
            }

            if (Turn)
            {
                Choice=new MoveCoord(scores.Max().Score, scores.Max().X, scores.Max().Y);
                return scores.Max((coord) => coord.Score);
            }
            Choice = new MoveCoord(scores.Min().Score, scores.Min().X, scores.Min().Y);
            return scores.Min((coord) => coord.Score);
        }

        public bool CheckGorizOrVert(string sym, string[,]? arr = null)
        {
            //проверка на победу в горизонталях и вертикалях 
            if (arr == null)
                arr = game;

            bool top, left;
            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                top = true;
                left = true;
                for (int j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    top &= (arr[i, j] == sym);
                    left &= (arr[j, i] == sym);
                }

                if (top || left) return true;
            }
            return false;
        }

        public bool CheckDiagonal(string sym, string[,]? arr = null)
        {
            //проверка диагоналей 
            if (arr == null)
                arr = game;

            bool toright = true;
            bool toleft = true;
            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                toright &= (arr[i, i] == sym);
                toleft &= (arr[3 - i - 1, i] == sym);
            }
            if (toright || toleft)
                return true;
            return false;
        }

        public bool CheckWin(string sym, string[,]? arr = null)
        {
            if (arr == null)
                arr = game;

            return CheckDiagonal(sym, arr) || CheckGorizOrVert(sym, arr);
        }

    }

    struct MoveCoord: IComparable
    {
        //структура которая хранит в себе координаты точки и очки хода
        public int Score { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public MoveCoord(int score,int x,int y)
        {
            Score = score;
            X = x;
            Y = y;
        }

        public int CompareTo(object? obj)
        {
            return this.Score;
        }
    }
}
