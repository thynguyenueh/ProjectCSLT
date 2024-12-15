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
        //PHẦN QUIZ
        int currentQuizQuestion = 1; // Biến đếm câu hỏi
        int quizScore = 0; // Điểm cho phần Quiz
        string[] questions = 
            { 
            "Câu hỏi 1: Bạn đang cần đóng hàng gửi chuyển phát qua đường bưu điện. Bạn nên chọn vật dụng nào để tái sử dụng và giảm phát thải nhất?", 
            "Câu hỏi 2: Đâu là những thói quen không nên làm, vì sẽ gây lãng phí điện?", 
            "Câu hỏi 3: Dùng để chế tạo túi nylon, lọ hóa chất. Không được dùng trong lò vi sóng, độ bền kém",
            "Câu hỏi 4: Đố bạn loại nhựa nào có các đặc điểm sau đây: rất độc hại, rẻ tiền, dùng để sản xuất vật dụng đựng hóa chất hay bình đựng nước",
            "Câu hỏi 5: Rác thải điện tử là một vấn đề nghiêm trọng hiện nay. Bạn có biết lượng rác thải điện tử mỗi năm bị thải ra trên toàn cầu là bao nhiêu không?",
            "Câu hỏi 6: Bạn đang ở siêu thị và mua các mặt hàng sau đây (rau, cà tím, nấm, cà rốt). Bạn hãy lựa chọn cách đựng các món hàng đã mua để giảm thiểu phát thải?",
            "Câu hỏi 7: iPhone 16 được ra mắt trong thời gian tới, bạn là người yêu thích công nghệ và có đủ tiền để mua, bạn sẽ làm gì?",
            "Câu hỏi 8: Bạn nên sử dụng thìa, dĩa nhựa dùng 1 lần để giảm phát thải trong các hoạt động tập thể nào?",
            "Câu hỏi 9: Bạn nghĩ đâu KHÔNG PHẢI là cách làm hữu hiệu nhất để giảm thiểu rác thải nhựa từ vỏ chai đựng các chất tẩy rửa?",
            "Câu hỏi 10: Bạn hãy cho biết, hành động nào làm pin sạc mau hư? ",
            };
        string[,] answers = {
            { "Mua thùng nhựa mới", "Mua thùng carton mới", "Tận dụng thùng nhựa hoặc carton cũ", "Không có đáp án đúng" },  // Đáp án cho câu 1
            { "Để tủ lạnh mở quá lâu", "Không rút sạc khi laptop và điện thoại đã được sạc đầy", "Bật quạt và đèn trong phòng trống", "Tất cả đáp án trên" }, // Đáp án cho câu 2
            { "PVC", "HDPE", "LDPE", "PET/PETE" }, // Đáp án cho câu 3
            {"Other" , "PET/PETE" , "LDPE", "HDPE"},
            {"54 triệu tấn", "24 triệu tấn", "44 triệu tấn","34 triệu tấn"},
            {"4 túi nilon, mỗi túi đựng 1 món", "1 túi nilon đựng 4 món hàng","2 túi nilon, mỗi túi đựng 2 món hàng","3 túi nilon, 1 túi đựng nắm, 1 túi đựng rau, 1 túi đựng cà tím và cà rốt"},
            {"Mua iPhone 16 và bán điện thoại cũ", "Mua iPhone 16 và dùng cả chiếc điện thoại cũ", "Tìm hiểu kỹ, nếu không có nhiều khác biệt và điện thoại hiện tại vẫn đáp ứng đủ nhu cầu của bản thân thì tiếp tục dùng điện thoại đang có","Mua iPhone 16 và cho người dân điện thoại cũ" },
            {"Không sử dụng trong bất kỳ hoạt động nào","Tiệc sinh nhật để đỡ phải dọn dẹp", "Buổi dễ ngoại ngoài trời để đỡ phải mang nặng và dọn dẹp","Câu B,C đúng" },
            {"Học cách tự làm nước tẩy rửa sinh học từ rác thải hữu cơ như từ vỏ dứa","Refill từ túi nhựa dùng 1 lần đựng hóa chất thể tích lớn","Lựa chọn các thương hiệu cho phép trả lại bao bì để sử dụng, refill" ,"Mua chai đựng hóa chất có thể tích lớn hơn" },
            {"Để pin trong máy quá lâu mà không sử dụng", "Để pin ở nơi có nhiệt độ không quá nóng","Sạc với thời gian vừa đủ","Sạc pin bằng bộ sạc tương thích" },
            { },
        };
        string[] correctAnswers = { "D", "D", "C","A","A","B","C","A","B","A" }; // Các đáp án đúng
        private string selectedAnswer = "";  // Biến lưu đáp án đã chọn



        //PHẦN PHÂN LOẠI RÁC
        //private Timer gameTimer;
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
            //PHẦN QUIZ
            StartQuiz(); // Bắt đầu quiz khi khởi tạo




            /*//PHẦN PHÂN LOẠI RÁC
            InitializeComponent();
            LoadHighScore();
            InitializeGame();

            gameTimer = new Timer();
            gameTimer.Interval = 1000; // Ví dụ: mỗi giây một lần
            gameTimer.Tick += GameTimer_Tick; // Gắn sự kiện cho timer

            random = new Random(); // Khởi tạo Random*/
        }

        //PHẦN QUIZ
        // HÀM BẮT ĐẦU QUIZ
        private void StartQuiz()
        {
            currentQuestionIndex = 0; // Đảm bảo bắt đầu từ câu hỏi đầu tiên
            ShowQuizQuestion(currentQuestionIndex); // Bắt đầu từ câu hỏi đầu tiên
        }


        // Thêm biến global để giữ Label câu hỏi và các nút trả lời
        // Khai báo các điều khiển toàn cục
        private Panel panel;
        private Label questionLabel;
        private Button answerA, answerB, answerC, answerD;

        // Khai báo biến toàn cục
        private int currentQuestionIndex = 0; // Dùng biến này để theo dõi câu hỏi hiện tại

        private void ShowQuizQuestion(int questionIndex)
        {
            // Tạo form quiz và các điều khiển chỉ một lần
            if (questionLabel == null)
            {
                // Tạo form quiz và các điều khiển nếu chưa có
                Form quizForm = this;
                quizForm.Width = 400;
                quizForm.Height = 500; // Điều chỉnh chiều cao form cho vừa với tất cả các điều khiển
                quizForm.Text = "Quiz";

                // Thêm nền vào form quiz
                panel = new Panel();  // Khai báo và tạo panel ở đây
                panel.Dock = DockStyle.Fill;
                panel.BackgroundImage = Properties.Resources.background_quiz; // Đặt hình nền từ resources
                panel.BackgroundImageLayout = ImageLayout.Stretch;
                quizForm.Controls.Add(panel);

                // Label câu hỏi
                questionLabel = new Label();
                questionLabel.Font = new Font("Arial", 16, FontStyle.Bold);  // Chữ với kích thước 16
                questionLabel.Location = new Point(20, 20); // Vị trí câu hỏi
                questionLabel.AutoSize = true; // Tự động điều chỉnh kích thước chữ theo nội dung
                questionLabel.ForeColor = Color.Black; // Màu chữ đen
                panel.Controls.Add(questionLabel);

                // Các nút đáp án A, B, C, D
                answerA = new Button();
                answerA.Left = 10;
                answerA.Top = 120;
                answerA.Width = 360;
                answerA.Height = 50;  // Đảm bảo chiều cao nút đủ lớn để chứa chữ
                answerA.Font = new Font("Arial", 16, FontStyle.Bold);  // Chữ lớn cho đáp án
                answerA.Click += (sender, e) => CheckAnswer("A", questionIndex);
                answerA.ForeColor = Color.Black;  // Màu chữ đen
                panel.Controls.Add(answerA);

                answerB = new Button();
                answerB.Left = 10;
                answerB.Top = 180;
                answerB.Width = 360;
                answerB.Height = 50;  // Đảm bảo chiều cao nút đủ lớn để chứa chữ
                answerB.Font = new Font("Arial", 16, FontStyle.Bold);  // Chữ lớn cho đáp án
                answerB.Click += (sender, e) => CheckAnswer("B", questionIndex);
                answerB.ForeColor = Color.Black;  // Màu chữ đen
                panel.Controls.Add(answerB);

                answerC = new Button();
                answerC.Left = 10;
                answerC.Top = 240;
                answerC.Width = 360;
                answerC.Height = 50;  // Đảm bảo chiều cao nút đủ lớn để chứa chữ
                answerC.Font = new Font("Arial", 16, FontStyle.Bold);  // Chữ lớn cho đáp án
                answerC.Click += (sender, e) => CheckAnswer("C", questionIndex);
                answerC.ForeColor = Color.Black;  // Màu chữ đen
                panel.Controls.Add(answerC);

                answerD = new Button();
                answerD.Left = 10;
                answerD.Top = 300;
                answerD.Width = 360;
                answerD.Height = 50;  // Đảm bảo chiều cao nút đủ lớn để chứa chữ
                answerD.Font = new Font("Arial", 16, FontStyle.Bold);  // Chữ lớn cho đáp án
                answerD.Click += (sender, e) => CheckAnswer("D", questionIndex);
                answerD.ForeColor = Color.Black;  // Màu chữ đen
                panel.Controls.Add(answerD);
            }

            // Cập nhật câu hỏi và các đáp án
            questionLabel.Text = questions[questionIndex]; // Cập nhật câu hỏi
            answerA.Text = answers[questionIndex, 0]; // Cập nhật đáp án A
            answerB.Text = answers[questionIndex, 1]; // Cập nhật đáp án B
            answerC.Text = answers[questionIndex, 2]; // Cập nhật đáp án C
            answerD.Text = answers[questionIndex, 3]; // Cập nhật đáp án D
        }

        private void CheckAnswer(string selectedAnswer, int questionIndex)
        {
            // Debug thông báo giá trị của selectedAnswer và correctAnswers
            Console.WriteLine($"Đáp án chọn: {selectedAnswer.Trim()}");
            Console.WriteLine($"Đáp án đúng: {correctAnswers[questionIndex].Trim()}");

            // So sánh đáp án đã chọn với đáp án đúng
            if (selectedAnswer.Trim().ToLower() == correctAnswers[questionIndex].Trim().ToLower())
            {
                quizScore++;
                MessageBox.Show($"Đúng rồi! Điểm của bạn: {quizScore}");
            }
            else
            {
                // Thông báo sai và hiển thị đáp án đúng
                MessageBox.Show($"Sai rồi! Đáp án đúng là: {correctAnswers[questionIndex]}\nĐiểm của bạn: {quizScore}");
            }

            // Kiểm tra nếu còn câu hỏi tiếp theo
            currentQuestionIndex++; // Tăng chỉ số câu hỏi
            if (currentQuestionIndex < questions.Length)
            {
                ShowQuizQuestion(currentQuestionIndex); // Hiển thị câu hỏi tiếp theo
            }
            else
            {
                // Sau khi quiz xong, chuyển qua phần phân loại rác
                MessageBox.Show("Bạn đã hoàn thành quiz! Điểm của bạn: " + quizScore);
                StartTrashSortingGame(); // Gọi game phân loại rác
            }
        }

        // HÀM BẮT ĐẦU PHÂN LOẠI RÁC
        private void StartTrashSortingGame()
        {
            // Chuyển sang game phân loại rác
            MessageBox.Show("Bây giờ chúng ta sẽ chuyển sang phần phân loại rác!");

            // Ẩn form quiz
            this.Hide();

            // Chạy lại game phân loại rác
            Form2 trashSortingForm = new Form2();
            trashSortingForm.Show();  // Hiển thị game phân loại rác
        }







        /*//PHẦN PHÂN LOẠI RÁC
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

        *//*private void GameTimer_Tick(object sender, EventArgs e)
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
        }*//*

        private bool isRoundCompleted = false;

        // Điểm cần đạt cho mỗi vòng
        private int[] requiredScores = { 10, 20, 30 };

        private void HandleRoundCompletion()
        {
            if (isRoundCompleted) return;  // Nếu vòng này đã được hoàn thành, thì không làm gì thêm.

            isRoundCompleted = true;  // Đánh dấu là vòng này đã hoàn thành.

            // Kiểm tra nếu điểm đạt yêu cầu để tiếp tục vòng tiếp theo
            if (score >= requiredScores[currentRound - 1])  // currentRound bắt đầu từ 1, nhưng mảng bắt đầu từ 0
            {
                // Kiểm tra nếu đây là vòng cuối cùng
                if (currentRound == 3)
                {
                    MessageBox.Show($"Chúc mừng! Bạn đã hoàn thành tất cả các vòng!");
                    //ResetGame();  // Reset trò chơi sau khi hoàn thành
                    base.Close();   //Đóng trò chơi
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
        }*/

    }
}
