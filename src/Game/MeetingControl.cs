using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Timers;

namespace amongus_game_flow
{
    public class MeetingControl
    {
        private static MeetingControl _instance;
        private VoteControl _vote;
        public static MeetingControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MeetingControl();
                }
                return _instance;
            }
        }
        long startDiscussTime = 0;
        long startVoteTime = 0;
        private long LeftDiscuss
        {
            get
            {
                long t = DateTimeOffset.Now.ToUnixTimeSeconds();
                return this.startDiscussTime + GameConfig.DiscussionTime - t;
            }
        }
        private long LeftVote
        {
            get
            {
                long t = DateTimeOffset.Now.ToUnixTimeSeconds();
                return this.startVoteTime + GameConfig.VotingTime - t;
            }
        }
        private void UpdateTime(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (LeftDiscuss > 0)
            {
                Console.WriteLine("discuss time:" + LeftDiscuss + "s");
            }
            else
            {
                if (LeftVote > 0)
                {
                    Console.WriteLine("vote time:" + LeftVote + "s");
                }
                else
                {
                    //Console.WriteLine("");
                }
            }
        }
        private static Timer aTimer;
        public void StartDiscuss()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 1000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += UpdateTime;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            Console.WriteLine("StartDiscuss");
            startDiscussTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            startVoteTime = 0;
            Task.Delay(GameConfig.DiscussionTime * 1000).ContinueWith((a) =>
              {
                  this.StartVote();
              });
        }
        public void StartVote()
        {
            Console.WriteLine("StartVote");
            this._vote = new VoteControl();
            this.startDiscussTime = 0;
            this.startVoteTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            Task.Delay(GameConfig.VotingTime * 1000).ContinueWith((a) =>
            {
                this._vote.AutoSkip();
                Console.WriteLine("voteFinish");
                this.UpdateVoteUI();
                this._vote.CheckEjects();
            });
        }
        public void UpdateVoteUI()
        {

        }

        public void Vote(int idx, int targetIdx)
        {
            this._vote.Vote(idx, targetIdx);
        }

    }
    public class VoteControl
    {
        readonly Dictionary<string, List<int>> data;
        public VoteControl()
        {
            this.data = new Dictionary<string, List<int>>() { { "skip", new List<int>() } };
            Global.room.players.ForEach(p =>
            {
                if (!p.dead)
                {
                    data[p.id] = new List<int> { };
                }
            });
        }
        public void AutoSkip()
        {
            foreach (KeyValuePair<string, List<int>> entry in this.data)
            {
                if (entry.Key == "skip")
                {
                    continue;
                }
                bool voted = false;
                int idx = Int16.Parse(entry.Key);
                foreach (KeyValuePair<string, List<int>> entry2 in this.data)
                {
                    if (entry2.Value.Contains(idx))
                    {
                        voted = true;
                        break;
                    }
                }
                if (!voted)
                {
                    this.data["skip"].Add(idx);
                }
            }
        }
        public void Skip(int idx)
        {
            if (this.data["skip"].Contains(idx))
            {
                return;
            }
            this.data["skip"].Add(idx);
        }
        public void Vote(int idx, int targetIdx)
        {
            if (this.data[targetIdx.ToString()].Contains(idx))
            {
                return;
            }
            this.data[targetIdx.ToString()].Add(idx);
            Console.WriteLine($"{idx} vote {targetIdx}\n" + JsonConvert.SerializeObject(this.data, Formatting.Indented));
        }
        public bool IsVoted(int idx)
        {
            foreach (KeyValuePair<string, List<int>> entry in this.data)
            {
                if (entry.Value.Contains(idx))
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckEjects()
        {
            Console.WriteLine("checkEjects\n" + JsonConvert.SerializeObject(this.data, Formatting.Indented));
            int total = 0, max = 0, maxIdx = -1;
            foreach (string i in this.data.Keys)
            {
                total += this.data[i].Count;
                if (i != "skip" && this.data[i].Count > max)
                {
                    max = this.data[i].Count;
                    maxIdx = +Int16.Parse(i);
                }
            }
            // 弃票
            if (this.data["skip"].Count >= total / 2)
            {
                Console.WriteLine("No one was ejected. [Skipped]");
            }
            else
            {
                // 平票
                int maxCount = 0;
                foreach (string i in this.data.Keys)
                {
                    if (this.data[i].Count == max)
                    {
                        maxCount++;
                    }
                }
                if (maxCount >= 2)
                {
                    Console.WriteLine("No one was ejected. [Tie]");
                }
                else if (maxIdx > -1)
                {
                    Console.WriteLine("eject idx:" + maxIdx);
                    //PlayerModel.data[maxIdx].dead = 1; //TODO:
                }
            }
            var rs = Global.game.CheckWin();
            if (rs == GameResultType.Continue)
            {
                // ghost
                // Global.Main.playerController.updatePlayerDisplay();
            }
        }
    }
}