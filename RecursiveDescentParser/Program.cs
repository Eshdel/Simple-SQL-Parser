using System;

class RecursiveDescentParser
{
    private string input;
    private int position;

    public bool Parse(string query)
    {
        input = query;
        position = 0;

        try
        {
            ParseS();
            if (position == input.Length) // Если достигли конца входной строки
            {
                Console.WriteLine("Анализ успешно завершен.");
                return true;
            }
            else
            {
                Console.WriteLine("Ошибка анализа: неправильный символ после позиции " + position);
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка анализа: " + ex.Message);
            return false;
        }
    }

    private void ParseS()
    {
        ParseKeyword("select");
        SkipSpace();
        ParseX();
        SkipSpace();
        ParseKeyword("from");
        SkipSpace();
        ParseX();
    }

    private void ParseX()
    {
        ParseA();
        ParseY();
    }

    private void SkipSpace() 
    {
        if (position < input.Length && input[position] == ' ') 
            position++;         
    }

    private void ParseY()
    {
        if (position < input.Length && input[position] == ',')
        {
            position++;// Пропускаем символ ','

            ParseA();
            SkipSpace();
            ParseY();
        }
    }

    private void ParseA()
    {
        ParseLetter();

        if (position < input.Length && input[position] != ' ' && input[position] != ',')
           ParseA();
    }

    private void ParseLetter()
    {
        if (position < input.Length && IsLetter(input[position]))
        {
            position++; // Пропускаем символ буквы
        }
        else
        {
            throw new Exception("Ожидался символ буквы ");
        }
    }

    private void ParseKeyword(string keyword)
    {
        int keywordLength = keyword.Length;
        if (position + keywordLength <= input.Length && input.Substring(position, keywordLength) == keyword)
        {
            position += keywordLength; // Пропускаем ключевое слово
        }
        else
        {
            throw new Exception("Ожидалось ключевое слово: " + keyword);
        }
    }

    private bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
}

class Program
{
    static void Main()
    {
        RecursiveDescentParser parser = new RecursiveDescentParser();

        // Примеры корректных запросов
        string query1 = "select column from table";
        string query2 = "select column,column from table,table";
        string query3 = "select column,column,column from table,table,table";

        // Примеры некорректных запросов
        string query4 = "select column1 from";
        string query5 = "select column1 from table1,";
        string query6 = "select column1 column2 from table1";

       Console.WriteLine("Анализ запроса: " + query1);
        parser.Parse(query1);

        Console.WriteLine("Анализ запроса: " + query2);
        parser.Parse(query2);

        Console.WriteLine("Анализ запроса: " + query3);
        parser.Parse(query3);

        Console.WriteLine("Анализ запроса: " + query4);
        parser.Parse(query4);

        Console.WriteLine("Анализ запроса: " + query5);
        parser.Parse(query5);

        Console.WriteLine("Анализ запроса: " + query6);
        parser.Parse(query6);

        Console.ReadLine();
    }
}
