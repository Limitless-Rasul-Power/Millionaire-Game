using System;
using System.Text;
using System.Media;

namespace Millionaire_Game
{
    class Program
    {
        const byte MaxQuestionCount = 10;
        const byte MaxChoiceCount = 3;

        static void RandomizeChoices(int[] numbers)
        {
            var random = new Random();
            for (int i = 0; i < MaxChoiceCount; i++)
            {
                numbers[i] = random.Next(0, MaxChoiceCount);

                for (int j = 0; j < i + 1 && i > 0; j++)
                {
                    if (numbers[j] == numbers[i] && i != j)
                    {
                        numbers[i] = random.Next(0, MaxChoiceCount);
                        j = -1;
                    }
                }
            }
        }
        static string[] GetAnswers()
        {
            string[] answers = {"Eminem", "Viktor Hugo", "1914", "Paradise, Nevada", "Komodo dragon", "Constaninople",
            "Gauss's Law", "Jumping over an office chair", "Scarface", "Golf"};
            return answers;
        }
        static void ConfirmOption(ref char option, char[] symbols)
        {
            while (option != symbols[0] && option != symbols[1] && option != symbols[2])
            {
                Console.Write("Pick one of this choices (A, B, C): ");
                option = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
            }
        }
        static bool IsTrueAnswer(in string choice, in string answer)
        {
            return choice.EndsWith(answer);
        }
        static string ChoosedOption(in char option, StringBuilder[] text)
        {
            for (int i = 0; i < MaxChoiceCount; i++)
                if (text[i][0] == option)
                    return text[i].ToString();

            return null;
        }
        static void PlaySound(in string music)
        {
            SoundPlayer sound = new SoundPlayer(music);
            sound.PlaySync();
        }
        static void WhoWantsToBeMillionaire(in string[] questions, in string[][] choices)
        {
            string[] answers = GetAnswers();
            int[] prizes = GetPrizes();

            byte currentQuestionNumber = 0;
            byte choiceCount = 0;
            int point = 0;
            const string GameName = "WHO WANTS TO BE MILLIONAIRE ?";
            while (currentQuestionNumber != MaxQuestionCount)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine();
                Console.Write(new string(' ', (Console.WindowWidth - GameName.Length) / 2));
                Console.WriteLine(GameName);
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;

                char[] symbols = { 'A', 'B', 'C' };
                int[] numbers = { -1, -1, -1 };

                StringBuilder[] text = new StringBuilder[MaxChoiceCount];
                for (int i = 0; i < text.Length; i++)
                    text[i] = new StringBuilder();

                RandomizeChoices(numbers);
                Console.WriteLine($"{currentQuestionNumber + 1}.{questions[currentQuestionNumber]}");

                for (int j = choiceCount; j < choiceCount + MaxChoiceCount; j++)
                {
                    int currentChoice = j - choiceCount; int index = numbers[currentChoice];
                    text[currentChoice].Append(symbols[currentChoice] + ")" + choices[currentQuestionNumber][index]);
                    Console.Write($"{text[currentChoice]}    ");
                }

                Console.WriteLine();
                Console.Write("Pick Choice: ");
                char option = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                ConfirmOption(ref option, symbols);
                string choosedOption = ChoosedOption(option, text);

                if (IsTrueAnswer(choosedOption, answers[currentQuestionNumber]))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("It is true answer, Congratulations!");
                    PlaySound("Applause.wav");
                    point += 10;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"It is Wrong answer! True answer is => {answers[currentQuestionNumber]} !");
                    PlaySound("Buzzer.wav");
                    if (point != 0)
                        point -= 10;
                }

                Console.ForegroundColor = ConsoleColor.White;

                if (currentQuestionNumber != 9)
                {
                    StringBuilder nextQuestionEntry = new StringBuilder("We are going to next question, " + "and its value is " + prizes[currentQuestionNumber].ToString("C") + " $");
                    Console.Write(new string(' ', (Console.WindowWidth - nextQuestionEntry.Length) / 2));
                    Console.WriteLine(nextQuestionEntry);
                }

                System.Threading.Thread.Sleep(777);
                Console.Clear();

                ++currentQuestionNumber;
                choiceCount += 3;
            }
            Console.Clear();
            Console.WriteLine($"Your reached Point is {point} of 100!");

            int money = (point != 0) ? prizes[(point / 10) - 1] : 0;
            Console.WriteLine($"And Your Prize is {money.ToString("C")}");
        }
        static string[] GetQuestions()
        {
            string[] questions = { "Which Rapper has \"Lose Yourself\" music ?", "Who is the author of \"What is human?\" ?",
            "What year did World War I begin ?", "Where is the Luxor Hotel & Casino located ?", "Which of these species is not extinct ?",
            "What name was historically used for the Turkish city currently known as Istanbul ?",
            "What physics principle relates the net electric flux out of a closed surface to the charge enclosed by that surface ?",
            "In a 1994 CBS interview, Microsoft co-founder Bill Gates performed what unusual trick on camera ?",
            "Which movie contains the quote, \"Say hello to my little friend!\"?",
            "What was the first sport to have been played on the moon?"};

            return questions;
        }
        static string[][] GetChoices()
        {
            string[][] choices = new string[][]
           {
                new string[MaxChoiceCount] {"Eminem",  "2Pac", "Snoop Dog"},
                new string[MaxChoiceCount] {"Don Kixot",  "Fyodor Dostoevsky", "Viktor Hugo"},
                new string[MaxChoiceCount] {"1923",  "1914", "1941"},
                new string[MaxChoiceCount] { "Paradise, Nevada", "Las Vegas, Nevada", "Jackpot, Nevada"},
                new string[MaxChoiceCount] { "Saudi gazelle", "Komodo dragon", "Japanese sea lion"},
                new string[MaxChoiceCount] { "Hudavendigar", "Soghut", "Constaninople"},
                new string[MaxChoiceCount] { "Faraday's Law", "Ampere's Law", "Gauss's Law"},
                new string[MaxChoiceCount] { "Standing on his head", "Jumping over an office chair", "Jumping backwards over a desk"},
                new string[MaxChoiceCount] { "Scarface", "Reservoir Dogs", "Goodfellas"},
                new string[MaxChoiceCount] { "Golf", "Tennis", "Soccer" }
           };
            return choices;
        }

        static int[] GetPrizes()
        {
            int[] prizes = { 1_000, 2_000, 4_000, 16_000, 32_000, 64_000, 125_000, 250_000, 500_000, 1_000_000 };
            return prizes;
        }
        static void ConsoleSettings()
        {
            Console.Title = "Who Wants to be MILLIONAIRE ?";
            Console.CursorVisible = false;
        }
        static void Play()
        {
            ConsoleSettings();
            string[] questions = GetQuestions();
            string[][] choices = GetChoices();
            WhoWantsToBeMillionaire(questions, choices);
        }
        static void Main(string[] args)
        {            
            Play();
        }
    }
}
