using System;
using System.Collections.Generic;
using System.Text;

namespace e_inf_20okt_fl
{
    class Episode
    {
        public Date date;
        public string series;
        public string number;
        public int length;
        public bool watchedState;
    }
    class Date
    {
        public int year;
        public int month;
        public int day;
        public Date()
        {
        }
        public Date(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }
    }
    class Series
    {
        public string name;
        public int watchedMinutes;
        public int episodeAmount;
        public Series()
        {
        }
        public Series(string Name, int WatchedMinutes, int EpisodeAmount)
        {
            this.name = Name;
            this.watchedMinutes = WatchedMinutes;
            this.episodeAmount = EpisodeAmount;
        }
        public Series(string Name)
        {
            this.name = Name;
        }
    }
}
