using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Nursan.UI
{
    /// <summary>
    /// Клас за конвертиране на Bitmap към Pix (Leptonica image format)
    /// </summary>
    public static class PixConverter
    {
        /// <summary>
        /// Конвертира Bitmap към Pix формат чрез записване във временен файл
        /// </summary>
        public static Pix ToPix(Bitmap bitmap)
        {
            try
            {
                // Създаваме временен файл за съхранение на изображението
                string tempPath = Path.Combine(Path.GetTempPath(), $"ocr_temp_{Guid.NewGuid()}.png");
                
                // Запазваме bitmap като PNG
                bitmap.Save(tempPath, ImageFormat.Png);
                
                // Създаваме Pix от файла
                var pix = Pix.LoadFromFile(tempPath);
                
                // Изтриваме временния файл
                try
                {
                    File.Delete(tempPath);
                }
                catch
                {
                    // Игнорираме грешките при изтриване на временния файл
                }
                
                return pix;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при конвертиране на Bitmap към Pix: {ex.Message}");
                throw;
            }
        }
    }

    public class ScreenMonitor : IDisposable
    {
        private readonly System.Windows.Forms.Timer _captureTimer;
        private readonly TesseractEngine _ocrEngine;
        private readonly List<string> _textToWatch = new List<string>();
        private readonly Dictionary<string, Action> _actionsOnDetection = new Dictionary<string, Action>();
        
        // Събитие, което се задейства, когато се намери наблюдаван текст
        public event EventHandler<TextDetectedEventArgs> TextDetected;
        
        public bool IsMonitoring { get; private set; }
        public int CaptureIntervalMs { get; set; } = 1000; // По подразбиране 1 секунда
        
        public ScreenMonitor()
        {
            // Инициализираме Tesseract OCR Engine
            string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            if (!Directory.Exists(tessDataPath))
            {
                Directory.CreateDirectory(tessDataPath);
            }
            
            // Проверяваме дали съществуват нужните файлове за Tesseract
            if (!File.Exists(Path.Combine(tessDataPath, "eng.traineddata")))
            {
                throw new FileNotFoundException("Tesseract OCR data файловете не са намерени. Моля, инсталирайте Tesseract и tessdata.");
            }
            
            _ocrEngine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
            
            // Инициализираме таймера, но още не го стартираме
            _captureTimer = new System.Windows.Forms.Timer
            {
                Interval = CaptureIntervalMs
            };
            _captureTimer.Tick += CaptureTimer_Tick;
        }
        
        // Добавя текст, който трябва да се наблюдава на екрана
        public void AddTextToWatch(string text, Action actionOnDetection = null)
        {
            if (string.IsNullOrEmpty(text))
                return;
                
            if (!_textToWatch.Contains(text))
            {
                _textToWatch.Add(text);
                
                if (actionOnDetection != null)
                {
                    _actionsOnDetection[text] = actionOnDetection;
                }
            }
        }
        
        // Премахва текст от наблюдаваните
        public void RemoveTextToWatch(string text)
        {
            if (_textToWatch.Contains(text))
            {
                _textToWatch.Remove(text);
                
                if (_actionsOnDetection.ContainsKey(text))
                {
                    _actionsOnDetection.Remove(text);
                }
            }
        }
        
        // Стартира наблюдението
        public void StartMonitoring()
        {
            if (IsMonitoring)
                return;
                
            _captureTimer.Interval = CaptureIntervalMs;
            _captureTimer.Start();
            IsMonitoring = true;
        }
        
        // Спира наблюдението
        public void StopMonitoring()
        {
            if (!IsMonitoring)
                return;
                
            _captureTimer.Stop();
            IsMonitoring = false;
        }
        
        // Прави скрийншот на целия екран
        private Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);
            
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            
            return screenshot;
        }
        
        // Разпознава текст в изображение
        private string RecognizeText(Bitmap image)
        {
            try
            {
                // Преобразуваме Bitmap към Pix формат, който Tesseract може да обработи
                using (var pix = PixConverter.ToPix(image))
                {
                    using (var page = _ocrEngine.Process(pix))
                    {
                        return page.GetText();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при разпознаване на текст: {ex.Message}");
                return string.Empty;
            }
        }
        
        // Събития на таймера - прави скрийншот и разпознава текст
        private async void CaptureTimer_Tick(object sender, EventArgs e)
        {
            // Спираме таймера временно, докато обработваме текущия скрийншот
            _captureTimer.Stop();
            
            try
            {
                // Правим скрийншот асинхронно
                Bitmap screenshot = await Task.Run(() => CaptureScreen());
                
                // Разпознаваме текст асинхронно
                string recognizedText = await Task.Run(() => RecognizeText(screenshot));
                
                // Проверяваме дали някой от наблюдаваните текстове е намерен
                foreach (string textToDetect in _textToWatch)
                {
                    if (recognizedText.Contains(textToDetect))
                    {
                        // Изпълняваме регистрираното действие, ако има такова
                        if (_actionsOnDetection.ContainsKey(textToDetect))
                        {
                            _actionsOnDetection[textToDetect]?.Invoke();
                        }
                        
                        // Задействаме събитието
                        TextDetected?.Invoke(this, new TextDetectedEventArgs(textToDetect, recognizedText));
                    }
                }
                
                // Освобождаваме ресурса
                screenshot.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при обработка на скрийншот: {ex.Message}");
            }
            finally
            {
                // Стартираме таймера отново, ако наблюдението все още е активно
                if (IsMonitoring)
                {
                    _captureTimer.Start();
                }
            }
        }
        
        // Dispose метод за освобождаване на ресурси
        public void Dispose()
        {
            StopMonitoring();
            _captureTimer?.Dispose();
            _ocrEngine?.Dispose();
        }
    }
    
    // Клас за параметрите на събитието TextDetected
    public class TextDetectedEventArgs : EventArgs
    {
        public string DetectedText { get; }
        public string FullRecognizedText { get; }
        
        public TextDetectedEventArgs(string detectedText, string fullRecognizedText)
        {
            DetectedText = detectedText;
            FullRecognizedText = fullRecognizedText;
        }
    }
} 