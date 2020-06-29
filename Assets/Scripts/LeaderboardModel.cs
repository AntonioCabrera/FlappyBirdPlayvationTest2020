using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardModel
{

    //Data model for the leaderboard service provided by Dreamlo ( http://dreamlo.com/ )
    [Serializable]
    public class Entry
    {
        [SerializeField]
        public string name { get; set; }
        [SerializeField]
        public string score { get; set; }
        [SerializeField]
        public string seconds { get; set; }
        [SerializeField]
        public string text { get; set; }
        [SerializeField]
        public string date { get; set; }


    }

    [Serializable]
    public class Leaderboard
    {
        [SerializeField]
        public List<Entry> entry { get; set; }

    }

    [Serializable]
    public class Dreamlo
    {
        [SerializeField]
        public Leaderboard leaderboard { get; set; }

    }

    [Serializable]
    public class Root
    {
        [SerializeField]
        public Dreamlo dreamlo { get; set; }

    }

}
