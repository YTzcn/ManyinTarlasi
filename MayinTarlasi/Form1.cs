using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MayinTarlasi
{
    public partial class Form1 : Form
    {
        private int gridSize = 20; // Varsayılan grid boyutu
        private int cellSize = 30; // Hücre boyutu
        private int mineCount; // Mayın sayısı
        private Button[,] buttons;
        private bool[,] mines;

        public Form1()
        {
            InitializeComponent();
            ShowSettings();
        }

        // Ayar ekranını oluştur
        private void ShowSettings()
        {
            this.Controls.Clear();
            this.Size = new Size(300, 200);
            this.Text = "Mayın Tarlası - Ayarlar";

            Label label = new Label
            {
                Text = "Boyut (Varsayılan 20):",
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(label);

            TextBox txtGridSize = new TextBox
            {
                Name = "txtGridSize",
                Text = gridSize.ToString(),
                Location = new Point(150, 20),
                Width = 100
            };
            this.Controls.Add(txtGridSize);

            Label labelMine = new Label
            {
                Text = "Mayın Sayısı (Varsayılan: 20):",
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(labelMine);

            TextBox txtMineCount = new TextBox
            {
                Name = "txtMineCount",
                Text = "20",
                Location = new Point(200, 60),
                Width = 100
            };
            this.Controls.Add(txtMineCount);

            Button btnStart = new Button
            {
                Text = "Başla",
                Location = new Point(100, 100),
                Width = 100
            };
            btnStart.Click += (s, e) =>
            {
                if (int.TryParse(txtGridSize.Text, out int size))
                    gridSize = size;

                if (int.TryParse(txtMineCount.Text, out int count))
                    mineCount = count;

                StartGame();
            };
            this.Controls.Add(btnStart);
        }

        // Oyunu başlat
        private void StartGame()
        {
            this.Controls.Clear();
            this.Text = "Mayın Tarlası";
            buttons = new Button[gridSize, gridSize];
            mines = new bool[gridSize, gridSize];

            Random random = new Random();

            // Mayınları rastgele yerleştir
            for (int i = 0; i < mineCount; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(gridSize);
                    y = random.Next(gridSize);
                } while (mines[x, y]);

                mines[x, y] = true;
            }

            // Dinamik buton oluştur
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(i * cellSize, j * cellSize),
                        Tag = new Point(i, j)
                    };
                    buttons[i, j].Click += ButtonClick;
                    this.Controls.Add(buttons[i, j]);
                }
            }

            // Form boyutunu ayarla
            this.Size = new Size(gridSize * cellSize + 20, gridSize * cellSize + 40);
        }

        // Butona tıklanınca çalışacak olay
        private void ButtonClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Point location = (Point)btn.Tag;
            int x = location.X;
            int y = location.Y;

            if (mines[x, y])
            {
                btn.BackColor = Color.Red;
                MessageBox.Show("Mayına bastınız! Oyun bitti.");
                ShowSettings();
            }
            else
            {
                btn.BackColor = Color.Green;
                btn.Enabled = false;

                int adjacentMines = CountAdjacentMines(x, y);
                if (adjacentMines > 0)
                    btn.Text = adjacentMines.ToString();
            }
        }

        // Komşu mayınları say
        private int CountAdjacentMines(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newY >= 0 && newX < gridSize && newY < gridSize && mines[newX, newY])
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
