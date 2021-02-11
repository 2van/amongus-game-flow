using System;
using System.Collections.Generic;

namespace amongus_game_flow
{
    public class SkillControl
    {
        public SkillControl()
        {
            this.skills.Add("Use", new Skill(10, 0, "Use"));
            this.skills.Add("Use1", new Skill(10, 0, "Use1"));
            this.skills.Add("Use2", new Skill(10, 0, "Use2"));
            this.skills.Add("Use3", new Skill(10, 0, "Use3"));
        }
        public Dictionary<string, Skill> skills = new Dictionary<string, Skill>();
        public void Use(string skill, string data)
        {
            Skill _skill;
            if (!skills.TryGetValue(skill, out _skill))
            {
                return; // or whatever you want to do
            }
            _skill.lastUseTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            switch (skill)
            {
                case "ShowTask":
                    this.ShowTask(data);
                    break;
                case "Meeting":
                    this.Meeting();
                    break;
                case "Kill":
                    this.Kill();
                    break;
                case "Pohuai":
                    this.Pohuai(data);
                    break;
                default:
                    break;
            }
        }
        void ShowTask(string taskName)
        {
            Console.WriteLine("open mini game:" + taskName);
        }
        void Meeting()
        {
            Global.meeting.StartDiscuss();
        }
        void Kill()
        {
            // TODO: at kill range
            int idx = Global.room.players.FindIndex(v => !v.isImpostor && !v.dead);
            if (idx >= 0)
            {
                Console.WriteLine("kill player " + idx);
                Global.room.players[idx].dead = true;
                // sync game
            }
        }
        void Pohuai(string type)
        {
            Global.task.setJinji(Int16.Parse(type));
        }
    }
    public class Skill
    {
        private int cd = 10;
        private int delay = 0;
        private string skillName = "";
        public long lastUseTime = 0;
        public Skill(int cd, int delay, string skill)
        {
            this.cd = cd;
            this.delay = delay;
            this.skillName = skill;
            if (this.delay > 0)
            {
                this.lastUseTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                    + this.cd - this.delay;
            }
        }

        void UpdateSkillDisplay()
        {

        }

        void Update()
        {
            if (this.skillName == "use")
            {
                return;
            }
            long l = this.lastUseTime + this.cd - DateTimeOffset.Now.ToUnixTimeSeconds();
            if (l < 0)
            {

            }
            else
            {
                Console.WriteLine(this.skillName + ":" + l);
            }
        }
    }
}
