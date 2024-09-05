using System;
using System.Collections.Generic;
using System.Linq;


namespace MatchGame.Models
{
    public class MatchModel
    {
        public static int sizeField = 5;

        // 0 - пустое поле.
        static readonly int variantColor = 3;

        // Игровое поле.
        public static int[,] gameField;

        // Массив матрицы проверки соседних клеток.
        static readonly List<List<int[]>> matrixMatch = new List<List<int[]>>() {
            new List<int[]> () { new int[] { -1,0 }, new int[] { 0, 0 }, new int[] { 1, 0 } },
            new List<int[]> () { new int[] { 0,1 }, new int[] { 0, 0 }, new int[] { 0, -1 } },
            new List<int[]> () { new int[] { -1,1 }, new int[] { 0, 0 }, new int[] { 1, -1 } },
            new List<int[]> () { new int[] { -1,-1 }, new int[] { 0, 0 }, new int[] { 1, 1 } },

            new List<int[]> () { new int[] { -2,2 }, new int[] { -1, 1 }, new int[] { 0, 0 } },
            new List<int[]> () { new int[] { -2,0 }, new int[] { -1, 0 }, new int[] { 0, 0 } },
            new List<int[]> () { new int[] { -2,-2 }, new int[] { -1, -1 }, new int[] { 0, 0 } },
            new List<int[]> () { new int[] { -0,-2 }, new int[] { 0, -1 }, new int[] { 0, 0 } },
            new List<int[]> () { new int[] { -2,-2 }, new int[] { 1, -1 }, new int[] { 0, 0 } },

            new List<int[]> () { new int[] { 2,0 }, new int[] { 1, 0 }, new int[] { 0, 0 } },
            new List<int[]> () { new int[] { 2,2 }, new int[] { 1,1 }, new int[] { 0, 0 } },
            new List<int[]> () { new int[] { 0,2 }, new int[] { 0, 1 }, new int[] { 0, 0 } },
        };

        // Счет игрока.
        static public int scorePlayer = 0;

        public MatchModel()
        {

        }
        /// <summary>
        /// Инициализация - начало игры.
        /// </summary>
        public void initGame(int numberField)
        {
            sizeField = numberField;
            scorePlayer = 0;


            gameField = new int[sizeField, sizeField];

            Random random = new Random();

            fillNullFieldGameField();
            

        }
        /// <summary>
        /// Расчет результатов клика игрока.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void clickGameField(int x, int y)
        {
            List<List<List<int>>> victory_ar = new List<List<List<int>>>();
            foreach (List<int[]> match in matrixMatch)
            {
                List<int> gatherColor = new List<int>();
                List<List<int>> gatherColorCoordinate = new List<List<int>>();
                foreach (int[] col in match)
                {

                    if (x + col[0] >= 0 && x + col[0] < sizeField)
                    {
                        if (y + col[1] >= 0 && y + col[1] < sizeField)
                        {
                            System.Diagnostics.Debug.WriteLine((x + col[0]) + " ZZ " + (y + col[1]) + " xx ");
                            gatherColor.Add(gameField[x + col[0], y + col[1]]);
                            gatherColorCoordinate.Add(new List<int>() { x + col[0], y + col[1] });
                        }
                    }
                }
                if (matchColorGameField(gatherColor))
                {
                    victory_ar.Add(gatherColorCoordinate);
                }

            }
            victoryGatherGameField(victory_ar);


        }
        /// <summary>
        /// Есть ли совпадающие цвета в искомом ряду.
        /// </summary>
        /// <param name="gatherColor"></param>
        /// <returns></returns>
        private static bool matchColorGameField(List<int> gatherColor)
        {
            if (gatherColor.Count() < 3)
            {
                return false;
            }
            int startColor = gatherColor[0];
            for (int i = 1; i < gatherColor.Count(); i++)
            {
                if (gatherColor[i] != startColor)
                {
                    return false;
                }

            }
            return true;
        }
        /// <summary>
        /// Расчет победных очков, убирание правильных рядов, заполнение освободивших мест новыми карточками.
        /// </summary>
        /// <param name="victory_ar"></param>
        private void victoryGatherGameField(List<List<List<int>>> victory_ar)
        {
            // Преобразование в один общий массив.
            int[,] victoryGameField = new int[sizeField, sizeField];
            foreach (List<List<int>> vic in victory_ar)
            {
                foreach (var col in vic)
                {
                    victoryGameField[col[0], col[1]] = 1;
                }
            }
            // Убирание совпавших клеток и подсчет очков.
            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    if (victoryGameField[i, j] > 0)
                    {
                        scorePlayer++;
                        gameField[i, j] = 0;
                    }
                }
            }

            // Сдвиг полей под действием гравитации.
            offsetGameField();

            // Заполняем поля мусором. Отключать на время тестирования, непонятно что происходит.
            fillNullFieldGameField();
        }

        /// <summary>
        /// Сдвиг полей под действием гравитации.
        /// </summary>
        private void offsetGameField()
        {
            // К черту рекурсию.
            for (int x = 0; x < sizeField; x++)
            {
                // Смещение поля при уничтожение нижестоящих клеток.
                for (int i = 0; i < sizeField; i++)
                {

                    offsetFieldVertical(i);
                }
            }
        }
        /// <summary>
        /// Сдвиг полей под действием гравитации. Подчиненный метод.
        /// </summary>
        /// <param name="ii"></param>
        private void offsetFieldVertical(int ii)
        {
            int color = gameField[0, ii];
            for (int j = 1; j < sizeField; j++)
            {

                    if (gameField[j, ii] == 0)
                    {
                        gameField[j-1, ii] = 0;
                        gameField[j, ii] = color;
                        //offsetFieldVertical(ii);
                    }
                    color = gameField[j, ii];

            }
        }
        /// <summary>
        /// Заполнить все пустые поля. Мусором.
        /// </summary>
        private void fillNullFieldGameField()
        {
            Random random = new Random();

            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    if (gameField[i, j]==0)
                    {
                            gameField[i, j] = random.Next(1, variantColor + 1);
                    }
                   
              

                }
            }
        }
    }
}