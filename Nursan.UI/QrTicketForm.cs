using System;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;
using XMLIslemi = Nursan.Core.Printing.XMLIslemi;

namespace Nursan.UI
{
    /// <summary>
    /// Форма за показване на QR код с адреса за проследяване на тикет
    /// </summary>
    public partial class QrTicketForm : Form
    {
        private System.Windows.Forms.Timer closeTimer;
        private PictureBox pictureBoxQr;
        private Label labelInfo;
        private Label labelUrl;
        
        /// <summary>
        /// Конструктор за QR Ticket форма
        /// </summary>
        /// <param name="ticketId">ID на тикета</param>
        /// <param name="serverIp">IP адрес на сървъра</param>
        public QrTicketForm(string ticketId, string serverIp)
        {
            try
            {
                InitializeComponent();
                GenerateQrCode(ticketId, serverIp);
                SetupAutoClose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при създаване на QrTicketForm: {ex.Message}");
                MessageBox.Show($"Грешка при отваряне на QR формата: {ex.Message}", 
                              "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Инициализира компонентите на формата
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxQr = new PictureBox();
            this.labelInfo = new Label();
            this.labelUrl = new Label();
            this.SuspendLayout();

            // 
            // pictureBoxQr
            // 
            this.pictureBoxQr.Location = new Point(60, 80);
            this.pictureBoxQr.Name = "pictureBoxQr";
            this.pictureBoxQr.Size = new Size(280, 280);
            this.pictureBoxQr.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxQr.TabIndex = 0;
            this.pictureBoxQr.TabStop = false;
            this.pictureBoxQr.BackColor = Color.White;
            // Добавяме border за по-хубав вид
            this.pictureBoxQr.BorderStyle = BorderStyle.FixedSingle;

            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = false;
            this.labelInfo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this.labelInfo.ForeColor = Color.Red; // Червен цвят като другите форми
            this.labelInfo.BackColor = Color.WhiteSmoke; // Прозрачен фон
            this.labelInfo.Location = new Point(30, 20);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new Size(340, 40);
            this.labelInfo.TabIndex = 1;
            this.labelInfo.Text = "✅ Тикетът е изпратен успешно!";
            this.labelInfo.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = false;
            this.labelUrl.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.labelUrl.ForeColor = Color.Red; // Червен цвят като другите форми
            this.labelUrl.BackColor = Color.WhiteSmoke; // Прозрачен фон
            this.labelUrl.Location = new Point(30, 380);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new Size(340, 80);
            this.labelUrl.TabIndex = 2;
            this.labelUrl.Text = "📱 Сканирайте QR кода за проследяване\n\n⏰ Формата се затваря след 10 секунди";
            this.labelUrl.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // QrTicketForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke; // Същия цвят като KlipV1 и ElTest
            this.TransparencyKey = Color.WhiteSmoke; // Прозрачност като другите форми
            this.ClientSize = new Size(400, 480);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.pictureBoxQr);
            this.FormBorderStyle = FormBorderStyle.None; // Без border като другите форми
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QrTicketForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "QR Код за Проследяване";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Генерира QR код с адреса за проследяване на тикета
        /// </summary>
        /// <param name="ticketId">ID на тикета</param>
        /// <param name="serverIp">IP адрес на сървъра</param>
        private void GenerateQrCode(string ticketId, string serverIp)
        {
            try
            {
                // Създаваме URL за проследяване на тикета използвайки XML конфигурацията
                string trackingUrl;
                try
                {
                    trackingUrl = XMLIslemi.GenerateTicketTrackingUrl(serverIp, ticketId);
                }
                catch (Exception xmlEx)
                {
                    // Ако има проблем с XML, използваме default URL
                    Console.WriteLine($"XML грешка: {xmlEx.Message}");
                    trackingUrl = $"http://{serverIp}/tickets/track/{ticketId}";
                }
                
                // Обновяваме label-а с URL-а
                labelUrl.Text = $"🔗 URL: {trackingUrl}\n\n📱 Сканирайте QR кода за проследяване\n\n⏰ Формата се затваря след 10 секунди";
                
                try
                {
                    // Генерираме QR код
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(trackingUrl, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    
                    // Създаваме изображението на QR кода
                    Bitmap qrCodeImage = qrCode.GetGraphic(10);
                    
                    // Показваме QR кода в PictureBox
                    pictureBoxQr.Image = qrCodeImage;
                }
                catch (Exception qrEx)
                {
                    // Ако QRCoder не работи, показваме само текста
                    Console.WriteLine($"QR генериране грешка: {qrEx.Message}");
                    pictureBoxQr.BackColor = Color.LightGray;
                    
                    // Създаваме просто текстово изображение
                    Bitmap textBitmap = new Bitmap(280, 280);
                    using (Graphics g = Graphics.FromImage(textBitmap))
                    {
                        g.Clear(Color.White);
                        g.DrawString("QR код\nне може\nда се генерира\n\n" + trackingUrl, 
                                   new Font("Arial", 10), Brushes.Black, new RectangleF(10, 10, 260, 260));
                    }
                    pictureBoxQr.Image = textBitmap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Обща грешка в GenerateQrCode: {ex.Message}");
                MessageBox.Show($"Грешка при генериране на QR код: {ex.Message}", 
                              "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Настройва автоматично затваряне на формата след 10 секунди
        /// </summary>
        private void SetupAutoClose()
        {
            closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 10000; // 10 секунди
            closeTimer.Tick += CloseTimer_Tick;
            closeTimer.Start();
        }

        /// <summary>
        /// Обработчик за затваряне на формата
        /// </summary>
        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            closeTimer?.Stop();
            closeTimer?.Dispose();
            this.Close();
        }


        /// <summary>
        /// Освобождава ресурсите
        /// </summary>
        /// <param name="disposing">Дали да освободи managed ресурсите</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                closeTimer?.Stop();
                closeTimer?.Dispose();
                pictureBoxQr?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
