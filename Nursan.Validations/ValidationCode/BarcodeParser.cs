using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Validations.SortedList;
using System.Text.RegularExpressions;

namespace Nursan.Validations.ValidationCode
{
    public class BarcodeParser
    {
        private readonly Messaglama _messaglama;

        public BarcodeParser()
        {
            _messaglama = new Messaglama();
        }

        public SyBarcodeOut ParseBarcodeContent(string barcodeContent)
        {
            try
            {
                var barcode = new SyBarcodeOut();
    
                // 1. Разделяме основните части (харнес_ид_станция)
                var mainParts = StringSpanConverter.SplitWithoutAllocationReturnArray(barcodeContent.AsSpan(), '_');
                if (mainParts.Length != 3)
                {
                    _messaglama.messanger($"Невалиден формат на баркод: {barcodeContent}");
                    throw new FormatException("Невалиден формат на баркод");
                }
        
                // 2. Разделяме харнес модела (prefix-family-suffix)
                var harnessParts = StringSpanConverter.SplitWithoutAllocationReturnArray(mainParts[0].AsSpan(), '-');
                if (harnessParts.Length != 3)
                {
                    _messaglama.messanger($"Невалиден формат на харнес модел: {mainParts[0]}");
                    throw new FormatException("Невалиден формат на харнес модел");
                }
        
                // 3. Извличаме и валидираме компонентите
                barcode.prefix = harnessParts[0].ToString();
                barcode.family = harnessParts[1].ToString();
                barcode.suffix = ValidateSuffix(harnessParts[2].ToString());
                barcode.IdDonanim = ValidateId(mainParts[1].ToString());
                barcode.Name = mainParts[2].ToString();
                barcode.BarcodeIcerik = barcodeContent;
    
                return barcode;
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при парсване на баркод: {ex.Message}");
                throw;
            }
        }

        private string ValidateSuffix(string suffix)
        {
            // Премахваме всички символи без букви и специални символи
            return Regex.Replace(suffix, "[^a-zA-Z@^/]", "");
        }

        private string ValidateId(string id)
        {
            // Проверяваме дали ID-то е валидно число
            if (!int.TryParse(id, out _))
            {
                _messaglama.messanger($"Невалиден формат на ID: {id}");
                throw new FormatException("Невалиден формат на ID");
            }
            return id;
        }

        public (string prefix, string family, string suffix) ParseHarnessModel(string harnessModel)
        {
            var parts = StringSpanConverter.SplitWithoutAllocationReturnArray(harnessModel.AsSpan(), '-');
            if (parts.Length != 3)
            {
                _messaglama.messanger($"Невалиден формат на харнес модел: {harnessModel}");
                throw new FormatException("Невалиден формат на харнес модел");
            }

            return (
                parts[0].ToString(),
                parts[1].ToString(),
                ValidateSuffix(parts[2].ToString())
            );
        }
    }
} 