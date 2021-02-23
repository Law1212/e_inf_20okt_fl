using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//https://www.informatikatanarok.hu/media/uploads/Informatika_erettsegi/Emelt/e_inf_20okt_fl.pdf

namespace e_inf_20okt_fl
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1.Feladat
            string rawTextFile = readAndTrimFile("lista.txt");
            string[] cutTextFile = cutIntoPieces(rawTextFile);

            Episode[] episodes = createEpisodesArray(
               getDates(cutTextFile),
               getSeries(cutTextFile),
               getNumbers(cutTextFile),
               getLengths(cutTextFile),
               getWatchedStates(cutTextFile),
               cutTextFile
            );

            // 2. Feladat

            Console.WriteLine("// 2. Feladat");
            Console.WriteLine(getEpisodeAmountWithValidDate(episodes));

            // 3. Feladat

            Console.WriteLine("\n// 3. Feladat");
            Console.WriteLine(getWatchedPercent(episodes).ToString("0.##"));

            // 4. Feladat

            Console.WriteLine("\n// 4. Feladat");
            int watchTimeInMinutes = getWatchedTimeInMinutes(episodes);
            int[] watchTimeInFormat = getWatchTimeDividedIntoFormat(watchTimeInMinutes);
            Console.WriteLine("A sorozatnezessel " + watchTimeInFormat[0] + " napot " + watchTimeInFormat[1] + " orat es " + watchTimeInFormat[2] + " percet toltott el.");

            // 5.Feladat

            Console.WriteLine("\n// 5. Feladat");
            Console.WriteLine("Adjon meg egy datumot! Datum= ");
            Date inputDate = createDate(Console.ReadLine());
            Episode[] episodesBeforeInputDate = getEpisodesUntilDate(inputDate, episodes);
            for (int i = 0; i < episodesBeforeInputDate.Length; i++)
            {
                Console.WriteLine(episodesBeforeInputDate[i].series);
                Console.WriteLine(episodesBeforeInputDate[i].number);
                Console.WriteLine(episodesBeforeInputDate[i].date.year + " " + episodesBeforeInputDate[i].date.month + " " + episodesBeforeInputDate[i].date.day);
                Console.WriteLine();
            }

            // 7.Feladat

            Console.WriteLine("\n// 7. Feladat");
            Console.WriteLine("Adjon meg a het egyik napjat(peldaul cs) ! Nap= ");
            string inputDay = Console.ReadLine();
            string[] seriesOnInputDay = collectGivenDayTitlesFromEpisodes(inputDay, episodes);
            if(seriesOnInputDay.Length > 0)
            {
                for (int i = 0; i < seriesOnInputDay.Length; i++)
                {
                    Console.WriteLine(seriesOnInputDay[i]);
                }
            }
            else
                Console.WriteLine("Az adott napon nem kerul adasba sorozat!");

            // 8.Feladat

            Console.WriteLine("\n// 8. Feladat"); // i use cw because too lazy to create a txt

            string[] seriesTitles = collectAllSeriesTitles(episodes);
            Series[] series = createSeriesWithWatchedData(seriesTitles, episodes);

            for (int i = 0; i < series.Length; i++)
            {
                Console.WriteLine(series[i].name);
                Console.WriteLine(series[i].watchedMinutes);
                Console.WriteLine(series[i].episodeAmount);
            }
        }

        static string readAndTrimFile(string fileName)
        {
            StreamReader streamReader = new StreamReader(fileName, System.Text.Encoding.Default);
            return streamReader.ReadToEnd().Trim();
        }
        static string[] cutIntoPieces(string inputText)
        {
            return Regex.Split(inputText, "\\n", RegexOptions.None);
        }
        static Episode[] createEpisodesArray(Date[] dates, string[] series, string[] numbers, int[] lengths, bool[] states, string[] cutTextFile)
        {
            List<Episode> episodes = new List<Episode>();
            dates = getDates(cutTextFile);
            series = getSeries(cutTextFile);
            numbers = getNumbers(cutTextFile);
            lengths = getLengths(cutTextFile);
            states = getWatchedStates(cutTextFile);

            for (int i = 0; i < cutTextFile.Length / 5; i++)
            {
                episodes.Add(new Episode());
                episodes[i].date = dates[i];
                episodes[i].series = series[i];
                episodes[i].number = numbers[i];
                episodes[i].length = lengths[i];
                episodes[i].watchedState = states[i];
            }
            return episodes.ToArray();
        }
        static Date[] getDates(string[] cutInputText)
        {
            List<Date> dates = new List<Date>();
            for (int i = 0; i < cutInputText.Length; i += 5)
            {
                if (cutInputText[i].Length > 3) // Because there are episodes with no dates
                {
                    string[] dateDivided = Regex.Split(cutInputText[i], "\\.");
                    dates.Add(new Date(Convert.ToInt32(dateDivided[0]), Convert.ToInt32(dateDivided[1]), Convert.ToInt32(dateDivided[2])));
                }
                else
                    dates.Add(new Date(0, 0, 0));
            }
            return dates.ToArray();
        }
        static string[] getSeries(string[] inputEpisodes)
        {
            List<string> series = new List<string>();
            for (int i = 1; i < inputEpisodes.Length; i += 5)
            {
                series.Add(inputEpisodes[i]);
            }
            return series.ToArray();
        }
        static string[] getNumbers(string[] inputEpisodes)
        {
            List<string> numbers = new List<string>();
            for (int i = 2; i < inputEpisodes.Length; i += 5)
            {
                numbers.Add(inputEpisodes[i]);
            }
            return numbers.ToArray();
        }
        static int[] getLengths(string[] inputEpisodes)
        {
            List<int> lengths = new List<int>();
            for (int i = 3; i < inputEpisodes.Length; i += 5)
            {
                lengths.Add(Convert.ToInt32(inputEpisodes[i]));
            }
            return lengths.ToArray();
        }
        static bool[] getWatchedStates(string[] inputEpisodes)
        {
            List<bool> states = new List<bool>();
            for (int i = 4; i < inputEpisodes.Length; i += 5)
            {
                states.Add(Convert.ToBoolean(Convert.ToInt32(inputEpisodes[i])));
            }
            return states.ToArray();
        }
        static int getEpisodeAmountWithValidDate(Episode[] inputEpisodes)
        {
            int counter = 0;
            for (int i = 0; i < inputEpisodes.Length; i++)
            {
                if (inputEpisodes[i].date.year > 0)
                    counter++;
            }
            return counter;
        }
        static double getWatchedPercent(Episode[] inputEpisodes)
        {
            double trueCounter = 0;
            for (int i = 0; i < inputEpisodes.Length; i++)
            {
                if (inputEpisodes[i].watchedState == true)
                {
                    trueCounter++;
                }
            }
            return trueCounter / inputEpisodes.Length * 100;
        }
        static int getWatchedTimeInMinutes(Episode[] inputEpisodes)
        {
            int watchTimeInMinutes = 0;
            for (int i = 0; i < inputEpisodes.Length; i++)
            {    
                if (inputEpisodes[i].watchedState == true)
                    watchTimeInMinutes += inputEpisodes[i].length;
            }
            return watchTimeInMinutes;
        }
        static int[] getWatchTimeDividedIntoFormat(int watchedTimeInMinutes)
        {
            int[] watchTimeInFormat = new int[3];
            int days = 0;
            int hours = 0;
            int minutes = 0;
            if (watchedTimeInMinutes > 0)
            {
                int minutesInADay = 1440;
                int minutesInAnHour = 60;

                if (watchedTimeInMinutes / minutesInADay % 1 > 0.5) // so that our conversion to int wont be off
                    days = watchedTimeInMinutes / minutesInADay - 1;
                else
                    days = watchedTimeInMinutes / minutesInADay;

                if (watchedTimeInMinutes - minutesInADay * days / minutesInAnHour % 1 > 0.5)
                    hours = (watchedTimeInMinutes - minutesInADay * days - 1) / minutesInAnHour;
                else
                    hours = (watchedTimeInMinutes - minutesInADay * days) / minutesInAnHour;

                minutes = watchedTimeInMinutes - (days * minutesInADay) - (hours * minutesInAnHour);
            }
            watchTimeInFormat[0] = days;
            watchTimeInFormat[1] = hours;
            watchTimeInFormat[2] = minutes;
            return watchTimeInFormat;
        }
        static Date createDate(string inputText)
        {
            string[] dateInPieces = Regex.Split(inputText, "\\.");
            return new Date(Convert.ToInt32(dateInPieces[0]), Convert.ToInt32(dateInPieces[1]), Convert.ToInt32(dateInPieces[2]));
        }
        static Episode[] getEpisodesUntilDate(Date date, Episode[] episodes)
        {
            List<Episode> episodesInPeriod = new List<Episode>();
            for (int i = episodes.Length - 1; i > 0; i--)
            {
                if (episodes[i].watchedState == false && episodes[i].date.year != 0)
                {
                    if (episodes[i].date.year == date.year)
                    {
                        if (episodes[i].date.month <= date.month)
                        {
                            if (episodes[i].date.day <= date.day)
                            {
                                episodesInPeriod.Add(episodes[i]);
                            }
                        }
                    }
                    else if (episodes[i].date.year < date.year)
                        episodesInPeriod.Add(episodes[i]);
                }
            }
            return episodesInPeriod.ToArray();
        }
        static string getDayOfTheWeek(Date date) // 6. Feladat, its very confusing, so i advice you to read the docs
        {
            string[] days = { "v", "h", "k", "sze", "cs", "p", "szo"};
            int[] months = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4};
            if (date.year > 0)
            {
                if (date.month < 3)
                    date.year--;
                return days[(date.year + (date.year / 4) - (date.year / 100) + (date.year / 400) + months[date.month - 1] + date.day) % 7];
            }
            else
                return "";
        }
        static bool isItTheGivenDay(string givenDay, Episode episode)
        {
            return (getDayOfTheWeek(episode.date) == givenDay ? true : false);
        }
        static string[] collectGivenDayTitlesFromEpisodes(string givenDay, Episode[] inputEpisodes)
        {
            List<string> correctDays = new List<string>();

            for (int i = 0; i < inputEpisodes.Length; i++)
            {
                if (isItTheGivenDay(givenDay, inputEpisodes[i]) == true)
                {
                    if (correctDays.Contains(inputEpisodes[i].series) == false)
                    {
                        correctDays.Add(inputEpisodes[i].series);
                    }
                }
            }
            return correctDays.ToArray();
        }
        static string[] collectAllSeriesTitles(Episode[] episodes)
        {
            List<string> seriesNames = new List<string>();
            for (int i = 0; i < episodes.Length; i++)
            {
                if (seriesNames.Contains(episodes[i].series) == false)
                    seriesNames.Add(episodes[i].series);
            }
            return seriesNames.ToArray();
        }
        static Series[] createSeriesWithWatchedData(string[] seriesTitles, Episode[] episodes)
        {
            List<Series> dates = new List<Series>();
            for (int i = 0; i < seriesTitles.Length; i++) // because the items need to be initialized for addition
            {
                dates.Add(new Series(seriesTitles[i]));
            }
            for (int i = 0; i < seriesTitles.Length; i++)
            {
                for (int j = 0; j < episodes.Length; j++)
                {
                    if (episodes[j].series == seriesTitles[i])
                    {
                        dates[i].watchedMinutes += episodes[j].length;
                        dates[i].episodeAmount++;
                    }
                }
            }
            return dates.ToArray();
        }
    }
}
