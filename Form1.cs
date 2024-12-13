using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // For File operations
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form1
{
    public partial class Form1 : Form
    {
        private Timer gameTimer;
        private PictureBox trash;
        private int score = 0;
        private int highScore = 0;
        private int timeLeft = 10;
        private Random random;
        private string[] trashTypes = { "Organic", "Inorganic", "Non-recyclable" };
        private string correctBin;
        private int currentRound = 1; // Biến theo dõi vòng hiện tại

        public Form1()
        {
            InitializeComponent();
            LoadHighScore();
            InitializeGame();

            gameTimer = new Timer();
            gameTimer.Interval = 1000; // Ví dụ: mỗi giây một lần
            gameTimer.Tick += GameTimer_Tick; // Gắn sự kiện cho timer

            random = new Random(); // Khởi tạo Random
        }


        private Dictionary<string, string> trashCategoryMapping;
        private void InitializeGame()
        {
            this.Text = "Trash Sorting Game";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            random = new Random();

            // Labels for score, high score, and timer
            Label lblTime = new Label
            {
                Text = $"Time: {timeLeft}s",
                Font = new Font("Arial", 16),
                Location = new Point(20, 20),
                AutoSize = true
            };
            lblTime.Name = "lblTime";
            this.Controls.Add(lblTime);

            Label lblScore = new Label
            {
                Text = $"Score: {score}",
                Font = new Font("Arial", 16),
                Location = new Point(20, 60),
                AutoSize = true
            };
            lblScore.Name = "lblScore";
            this.Controls.Add(lblScore);

            Label lblHighScore = new Label
            {
                Text = $"High Score: {highScore}",
                Font = new Font("Arial", 16),
                Location = new Point(20, 100),
                AutoSize = true
            };
            this.Controls.Add(lblHighScore);

            // Trash bins with appropriate images
            //CreateTrashBin("chatlong", Properties.Resources.thungracchatlong, new Point(20, 250));
            CreateTrashBin("huuco", Properties.Resources.thungrachuuco, new Point(100, 250));
            CreateTrashBin("nhua", Properties.Resources.thungracnhua, new Point(300, 250));
            CreateTrashBin("racconlai", Properties.Resources.thungracconlai, new Point(500, 250));

            // Initialize the trash category mapping
            trashCategoryMapping = new Dictionary<string, string>
    {
            { "huuco_tao", "huuco" },
            { "huuco_xuongca", "huuco" },
            { "huuco_banhmi", "huuco" },
            //{ "giay_giaybao", "giay" },
            { "kimloai_coca", "kimloai" },
            { "nhua_chainuoc", "nhua" },
            { "racconlai_tuinilong", "racconlai" },
            { "racconlai_giayan", "racconlai" },
            { "racconlai_muongnia", "racconlai" },
            { "racconlai_hopnhua", "racconlai" },
            { "racconlai_khautrang", "racconlai" }
    };
            // Falling trash
            trash = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(random.Next(100, 600), 50),
            };
            this.Controls.Add(trash);

            // Game Timer
            gameTimer = new Timer
            {
                Interval = 1000
            };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            this.KeyDown += MainForm_KeyDown;
            AssignNewTrash();
        }

        private Image ResizeImageMaintainAspect(Image img, int maxWidth, int maxHeight)
        {
            int originalWidth = img.Width;
            int originalHeight = img.Height;

            // Calculate the new dimensions while maintaining the aspect ratio
            float ratioX = (float)maxWidth / originalWidth;
            float ratioY = (float)maxHeight / originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            Bitmap resizedImg = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImg))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                g.DrawImage(img, 0, 0, newWidth, newHeight);
            }
            return resizedImg;
        }


        private void CreateTrashBin(string name, Image image, Point location)
        {
            PictureBox bin = new PictureBox
            {
                Size = new Size(200, 300), // Increased size for bigger trash cans
                Location = location,
                Image = ResizeImageMaintainAspect(image, 200, 300), // Adjust image size proportionally
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = name,
            };

            this.Controls.Add(bin);
        }

        private void AssignNewTrash()
        {
            var randomTrash = trashCategoryMapping.ElementAt(random.Next(trashCategoryMapping.Count));
            string trashImageName = randomTrash.Key;
            correctBin = randomTrash.Value;

            Image trashImage = (Image)Properties.Resources.ResourceManager.GetObject(trashImageName);
            if (trashImage != null)
            {
                trash.Image = ResizeImageMaintainAspect(trashImage, 100, 100);
                trash.Size = new Size(100, 100);
                trash.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show($"Trash image not found for: {trashImageName}");
                return;
            }

            trash.Left = random.Next(100, this.Width - trash.Width);
            trash.Top = 50;
        }





        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && trash.Left > 0)
            {
                trash.Left -= 20;
            }
            else if (e.KeyCode == Keys.D && trash.Right < this.Width)
            {
                trash.Left += 20;
            }
            else if (e.KeyCode == Keys.S)
            {
                trash.Top += 20;
                if (trash.Bounds.Bottom >= this.Height - 200) // Near bins
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is PictureBox bin && bin.Tag != null && trash.Bounds.IntersectsWith(bin.Bounds))
                        {
                            CheckCorrectBin(bin.Tag.ToString());
                            break;
                        }
                    }

                    trash.Top = 50;
                    trash.Left = random.Next(100, 600);
                    AssignNewTrash();
                }
            }
        }

        private void CheckCorrectBin(string bin)
        {
            PictureBox binControl = this.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Tag?.ToString() == bin);
            if (binControl != null)
            {
                if (string.Equals(bin, correctBin, StringComparison.OrdinalIgnoreCase))
                {
                    score += 10;
                    this.Controls["lblScore"].Text = $"Score: {score}";
                    binControl.BackColor = Color.LightGreen;  // Hiển thị màu xanh khi đúng
                }
                else
                {
                    binControl.BackColor = Color.IndianRed;  // Hiển thị màu đỏ khi sai
                }

                // Đợi một thời gian ngắn rồi reset lại màu sắc (ví dụ 500ms)
                Task.Delay(500).ContinueWith(_ =>
                {
                    binControl.Invoke(new Action(() =>
                    {
                        binControl.BackColor = Color.Transparent; // Reset màu
                    }));
                });
            }
        }




        private void ResetGame()
        {
            score = 0; // Reset điểm số
            currentRound = 1; // Reset vòng chơi
            timeLeft = 10; // Reset thời gian
            InitializeRound(); // Gọi lại hàm khởi tạo vòng 1
            AssignNewTrash();  // Tạo lại rác mới
            gameTimer.Start();  // Khởi động lại game
        }





        private void LoadHighScore()
        {
            try
            {
                if (File.Exists("highscore.txt"))
                {
                    highScore = int.Parse(File.ReadAllText("highscore.txt"));
                }
            }
            catch
            {
                highScore = 0;
            }
        }

        private void SaveHighScore()
        {
            if (score > highScore)
            {
                File.WriteAllText("highscore.txt", score.ToString());
            }
        }

        /*private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            this.Controls["lblTime"].Text = $"Time: {timeLeft}s";

            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                MessageBox.Show($"Time's up! Final Score: {score}");
                SaveHighScore();
                this.Close();
            }
        }*/

        private bool isRoundCompleted = false;

        private void HandleRoundCompletion()
        {
            if (isRoundCompleted) return;  // Nếu vòng này đã được hoàn thành, thì không làm gì thêm.

            isRoundCompleted = true;  // Đánh dấu là vòng này đã hoàn thành.

            // Kiểm tra nếu điểm đạt yêu cầu để tiếp tục vòng tiếp theo
            if (score >= 10)
            {
                // Kiểm tra nếu đây là vòng cuối cùng
                if (currentRound == 3)
                {
                    MessageBox.Show($"Chúc mừng! Bạn đã hoàn thành tất cả các vòng!");
                    ResetGame();  // Reset trò chơi sau khi hoàn thành
                }
                else
                {
                    MessageBox.Show($"Bạn đã thắng vòng {currentRound}! Tiến đến vòng {currentRound + 1}.");
                    currentRound++;  // Tăng số vòng
                    InitializeRound();  // Khởi tạo vòng mới
                    gameTimer.Start();  // Tiếp tục đếm ngược thời gian cho vòng tiếp theo
                }
            }
            else
            {
                // Nếu điểm chưa đủ, quay lại vòng 1
                MessageBox.Show($"Điểm của bạn chưa đủ, chơi lại vòng 1.");
                currentRound = 1;  // Reset lại vòng 1
                timeLeft = 10;
                InitializeRound();  // Khởi tạo vòng 1
                gameTimer.Start();  // Bắt đầu lại thời gian cho vòng 1
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft <= 0)
            {
                gameTimer.Stop();  // Dừng đồng hồ khi hết thời gian
                HandleRoundCompletion();  // Gọi hàm để xử lý vòng hiện tại (hoàn thành)
            }
            else
            {
                timeLeft--;
                this.Controls["lblTime"].Text = $"Time: {timeLeft}s";  // Cập nhật thời gian
            }
        }







        private void InitializeRound()
        {
            // Đặt lại cờ khi bắt đầu vòng mới
            isRoundCompleted = false;  // Đánh dấu rằng vòng này chưa hoàn thành

            // Reset lại timeLeft cho từng vòng
            if (currentRound == 1)
            {
                timeLeft = 10;  // Đặt lại thời gian cho vòng 1 là 10s
            }
            else if (currentRound == 2)
            {
                timeLeft = 20;  // Đặt lại thời gian cho vòng 2 là 20s
            }
            else if (currentRound == 3)
            {
                timeLeft = 30;  // Đặt lại thời gian cho vòng 3 là 30s
            }

            // Tiếp theo là code để khởi tạo vòng chơi, ví dụ như tạo các thùng rác
            switch (currentRound)
            {
                case 1:
                    score = 0;
                    trashCategoryMapping = new Dictionary<string, string>
            {
                { "huuco_tao", "huuco" },
                { "huuco_xuongca", "huuco" },
                { "huuco_banhmi", "huuco" },
                { "nhua_chainuoc", "nhua" },
                { "racconlai_tuinilong", "racconlai" },
                { "racconlai_giayan", "racconlai" },
                { "racconlai_muongnia", "racconlai" },
                { "racconlai_hopnhua", "racconlai" },
                { "racconlai_khautrang", "racconlai" }
            };
                    CreateTrashBin("huuco", Properties.Resources.thungrachuuco, new Point(100, 250));
                    CreateTrashBin("nhua", Properties.Resources.thungracnhua, new Point(300, 250));
                    CreateTrashBin("racconlai", Properties.Resources.thungracconlai, new Point(500, 250));
                    break;

                case 2:
                    score = 0;
                    trashCategoryMapping = new Dictionary<string, string>
            {
                { "huuco_tao", "huuco" },
                { "huuco_xuongca", "huuco" },
                { "huuco_banhmi", "huuco" },
                { "kimloai_coca", "kimloai" },
                { "nhua_chainuoc", "nhua" },
                { "racconlai_tuinilong", "racconlai" },
                { "racconlai_giayan", "racconlai" },
                { "racconlai_muongnia", "racconlai" },
                { "racconlai_hopnhua", "racconlai" },
                { "racconlai_khautrang", "racconlai" }
            };
                    CreateTrashBin("chatlong", Properties.Resources.thungracchatlong, new Point(20, 250));
                    CreateTrashBin("huuco", Properties.Resources.thungrachuuco, new Point(100, 250));
                    CreateTrashBin("nhua", Properties.Resources.thungracnhua, new Point(300, 250));
                    CreateTrashBin("racconlai", Properties.Resources.thungracconlai, new Point(500, 250));
                    break;

                case 3:
                    score = 0;
                    trashCategoryMapping = new Dictionary<string, string>
            {
                { "huuco_tao", "huuco" },
                { "huuco_xuongca", "huuco" },
                { "huuco_banhmi", "huuco" },
                { "kimloai_coca", "kimloai" },
                { "nhua_chainuoc", "nhua" },
                { "racconlai_tuinilong", "racconlai" },
                { "racconlai_giayan", "racconlai" },
                { "racconlai_muongnia", "racconlai" },
                { "racconlai_hopnhua", "racconlai" },
                { "racconlai_khautrang", "racconlai" }
            };
                    CreateTrashBin("chatlong", Properties.Resources.thungracchatlong, new Point(20, 250));
                    CreateTrashBin("huuco", Properties.Resources.thungrachuuco, new Point(100, 250));
                    CreateTrashBin("nhua", Properties.Resources.thungracnhua, new Point(300, 250));
                    CreateTrashBin("racconlai", Properties.Resources.thungracconlai, new Point(500, 250));
                    break;
            }
        }



        private void UpdateUI()
        {
            this.Controls["lblTime"].Text = $"Time: {timeLeft}s";
            this.Controls["lblScore"].Text = $"Score: {score}";
            this.Controls["lblRound"].Text = $"Round: {currentRound}/3";  // Thêm phần hiển thị số vòng
        }

    }
}