using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace amongus_game_flow
{
    public class MeetingControl
    {
        private static MeetingControl _instance;
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
                int conf_discuss = 1;//TODO: timeconf
                long t = DateTimeOffset.Now.ToUnixTimeSeconds();
                return this.startDiscussTime + conf_discuss - t;
            }
        }
        private long LeftVote
        {
            get
            {
                int conf_vote = 1;//TODO: timeconf
                long t = DateTimeOffset.Now.ToUnixTimeSeconds();
                return this.startVoteTime + conf_vote - t;
            }
        }
        private void UpdateTime()
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
        public void StartDiscuss()
        {
            Console.WriteLine("startDiscuss");
            startDiscussTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            startVoteTime = 0;
            // TODO: timeconf
            Task.Delay(2000).ContinueWith((a) =>
            {
                this.StartVote();
                VoteControl.Instance.Init();
            });
        }
        public void StartVote()
        {
            this.startDiscussTime = 0;
            this.startVoteTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            // TODO: timeconf
            Task.Delay(2000).ContinueWith((a) =>
            {
                VoteControl.Instance.AutoSkip();
                Console.WriteLine("voteFinish");
                this.UpdateVoteUI();
                VoteControl.Instance.CheckEjects();
            });
        }
        public void UpdateVoteUI()
        {

        }

    }
    public class VoteControl
    {
        Dictionary<string, List<int>> data;
        public static VoteControl _instance;
        public static VoteControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VoteControl();
                }
                return _instance;
            }
        }
        public void Init()
        {
            this.data = new Dictionary<string, List<int>>();
            for (int i = 0; i < 2; i++)
            {
                if (true)
                {
                    data[i.ToString()] = new List<int> { };
                }
            }
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
                //Global.Main.playerController.updatePlayerDisplay();
            }
        }
    }
}