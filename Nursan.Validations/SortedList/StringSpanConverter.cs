﻿namespace Nursan.Validations.SortedList
{
    public static class StringSpanConverter
    {

        public static ReadOnlySpan<char> GetCharsMyDecoder(ReadOnlySpan<char> input, char chrackter)
        {
            ReadOnlySpan<char> output;
            var getparchalama = SplitWithoutAllocation(input, chrackter);
            output = getparchalama;
            return output;
        }
        public static int GetCharsIsDigitPadingLeft(ReadOnlySpan<char> span, int padingLeft)
        {
            int startIndex = span.Length - padingLeft;

            ReadOnlySpan<char> lastPart = span.Slice(startIndex);
            return int.Parse(lastPart);
        }
        public static int GetCharsIsDigit(ReadOnlySpan<char> span)
        {
            int startIndex = span.Length - 8;

            ReadOnlySpan<char> lastPart = span.Slice(startIndex);
            return int.Parse(lastPart);
        }
        public static ReadOnlySpan<char> GetCharsIsNonDigit(ReadOnlySpan<char> span)
        {
            int startIndex = 0;

            for (int i = 0; i < span.Length; i++)
            {
                if (char.IsDigit(span[i]))
                {
                    // Извличане на подспан без допълнително заделяне на памет
                    ReadOnlySpan<char> part = span.Slice(startIndex, i - startIndex);


                    // Задаване на новия начален индекс
                    startIndex = i;
                }
            }

            // Обработка на последния подспан
            ReadOnlySpan<char> lastPart = span.Slice(startIndex);
            return lastPart;
        }
        public static ReadOnlySpan<char> SplitWithoutAllocation(ReadOnlySpan<char> span, char delimiter)
        {
            //List<ReadOnlySpan<char>> parts = new List<ReadOnlySpan<char>>();

            int startIndex = 0;

            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == delimiter)
                {
                    // Извличане на подспан без допълнително заделяне на памет
                    ReadOnlySpan<char> part = span.Slice(startIndex, i - startIndex);

                    // Задаване на новия начален индекс
                    startIndex = i + 1;
                }
            }

            // Обработка на последния подспан
            ReadOnlySpan<char> lastPart = span.Slice(startIndex);
            return lastPart;
            //Console.WriteLine(lastPart.ToString());
        }
        public static string[] SplitWithoutAllocationReturnArray(ReadOnlySpan<char> span, char delimiter)
        {
            // Списък за съхранение на частите
            List<string> result = new List<string>();

            // Разделяме span по разделителя
            while (true)
            {
                int index = span.IndexOf(delimiter); // Намираме индекса на разделителя

                if (index == -1)
                {
                    // Няма повече разделители, обработваме последната част
                    result.Add(new string(span)); // Добавяме последната част като низ
                    break;
                }

                // Получаваме частта от началото до разделителя
                var part = span.Slice(0, index);
                result.Add(new string(part)); // Добавяме сегмента като низ

                // Преместваме span напред, след разделителя
                span = span.Slice(index + 1);
            }

            // Връщаме резултата като масив от низове
            return result.ToArray();
        }
        public static ReadOnlySpan<char> SafeSubSpan(ReadOnlySpan<char> span, int start, int length)
        {
            // Проверка за празен span
            if (span.IsEmpty)
                return ReadOnlySpan<char>.Empty;

            // Нормализиране на start параметъра
            if (start < 0)
                start = 0;

            // Проверка дали началната позиция е в обхвата
            if (start >= span.Length)
                return ReadOnlySpan<char>.Empty;

            // Проверка за отрицателна дължина
            if (length < 0)
                return ReadOnlySpan<char>.Empty;

            // Корекция на дължината, ако надвишава размера на span
            if (start + length > span.Length)
                length = span.Length - start;

            // Проверка дали коригираната дължина е валидна
            if (length == 0)
                return ReadOnlySpan<char>.Empty;

            try
            {
                return span.Slice(start, length);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ако все пак възникне изключение, връщаме празен span
                return ReadOnlySpan<char>.Empty;
            }
        }
        public static ReadOnlySpan<char> ExtractText(ReadOnlySpan<char> input)
        {
            if (input.IsEmpty)
                return input;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(input[i]))
                    return input.Slice(0, i + 1);
            }
            return input;
        }

        public static ReadOnlySpan<char> ExtractDigits(ReadOnlySpan<char> input)
        {
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(input[i]))
                    return input.Slice(i + 1);
            }
            return ReadOnlySpan<char>.Empty;
        }

    }
}
