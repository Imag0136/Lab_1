using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab0
{
    class Neuron
    {
        byte t = 0; //номер эпохи
        byte error = 0;
        int limit = 0;
        public int[] imgArray; // матрица входов
        public double[,] weight = new double[10, 10000]; //матрица весовых коэффициентов
        double alpha = 0.4; //Скорость обучения
        double delta;
        int y; //фактический результат
        int yk; //ожидаемый результат
        Bitmap img;

        public Neuron()
        {
            //Установление случайных весов
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    weight[i, j] = Convert.ToDouble(rand.Next(-3, 4) / 10.0);
                }
            }
        }

        public void Learn()
        {
            t += 1;
            error = 0;
            for (int k = 0; k < 100; k++)
            {
                img = (Bitmap)Image.FromFile($"../../Resources/{k}.jpg");
                for (int n = 0; n < 10; n++)
                {
                    y = Output(n, img) > limit ? 1 : 0;
                    yk = n == (k / 10) ? 1 : 0;
                    if (y != yk)
                    {
                        delta = yk - y;
                        for (int i = 0; i < 10000; i++)
                        {
                            if (imgArray[i] == 1) weight[n, i] += alpha * delta;
                        }
                        error++;
                    }   
                }
            }
            Console.WriteLine($"t = {t}");
            Console.WriteLine($"error = {error}");
            if (error > 0) Learn();
            else Console.WriteLine($"t = {t}");
        }

        public double Output(int n, Bitmap img)
        {
            double sum = 0; //сумма
            int t = 0;
            imgArray = new int[10000];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j) == Color.FromArgb(255, 0, 0, 0))
                    {
                        imgArray[t] = 1;
                        sum += imgArray[t] * weight[n, t];
                    }
                    t++;
                }
            }
            return sum;
        }

        // Процедура обрезание рисунка по краям и преобразование в массив.
        public static int[,] CutImage(Bitmap b, Point max)
        {
            var x1 = 0;
            var y1 = 0;
            var x2 = max.X;
            var y2 = max.Y;

            for (var y = 0; y < b.Height && y1 == 0; y++)
                for (var x = 0; x < b.Width && y1 == 0; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0) y1 = y;
            for (var y = b.Height - 1; y >= 0 && y2 == max.Y; y--)
                for (var x = 0; x < b.Width && y2 == max.Y; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0) y2 = y;
            for (var x = 0; x < b.Width && x1 == 0; x++)
                for (var y = 0; y < b.Height && x1 == 0; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0) x1 = x;
            for (var x = b.Width - 1; x >= 0 && x2 == max.X; x--)
                for (var y = 0; y < b.Height && x2 == max.X; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0) x2 = x;

            if (x1 == 0 && y1 == 0 && x2 == max.X && y2 == max.Y) return null;

            var size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1;
            var dx = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0;
            var dy = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0;

            var res = new int[size, size];
            for (var x = 0; x < res.GetLength(0); x++)
                for (var y = 0; y < res.GetLength(1); y++)
                {
                    var pX = x + x1 - dx;
                    var pY = y + y1 - dy;
                    if (pX < 0 || pX >= max.X || pY < 0 || pY >= max.Y)
                        res[x, y] = 0;
                    else
                        res[x, y] = b.GetPixel(x + x1 - dx, y + y1 - dy).ToArgb() == 0 ? 0 : 1;
                }
            return res;
        }

        // Пересчёт массива source в массив res, для приведения произвольного массива данных к массиву стандартных размеров.
        public int[,] LeadArray(int[,] source, int[,] res)
        {
            for (var n = 0; n < res.GetLength(0); n++)
                for (var m = 0; m < res.GetLength(1); m++) res[n, m] = 0;

            var pX = (double)res.GetLength(0) / (double)source.GetLength(0);
            var pY = (double)res.GetLength(1) / (double)source.GetLength(1);

            for (var n = 0; n < source.GetLength(0); n++)
                for (var m = 0; m < source.GetLength(1); m++)
                {
                    var posX = (int)(n * pX);
                    var posY = (int)(m * pY);
                    if (res[posX, posY] == 0) res[posX, posY] = source[n, m];
                }
            return res;
        }

        public void Recognize(Bitmap img)
        {
            for (int n = 0; n < 10; n++)
            {
                if (Output(n, img) > limit) MessageBox.Show($"Это {n}");
            }
        }
    }
}
