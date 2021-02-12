using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace amongus_game_flow
{
    public class Room
    {
        public Room()
        {
            int impIdx = new Random().Next(GameConfig.PlayerCount);
            for (int i = 0; i < GameConfig.PlayerCount; i++)
            {
                PlayerControl p = new PlayerControl
                {
                    isImpostor = i == impIdx,
                    idx = i,
                    id = i.ToString()
                };
                players.Add(p);
                Global.task.GenerateTask(i);
            }
            impIdxs.Add(impIdx);
            selfIdx = impIdxs[0]; // TEST Impostor
            Console.WriteLine(Self.isImpostor ? "impostor" : "crewmate");
            //TODO: 2 impostors
        }
        public List<int> impIdxs= new List<int>();
        public List<PlayerControl> players = new List<PlayerControl> { };
        public int selfIdx = 0;
        public PlayerControl Self => players[selfIdx];
        public void SetPlayer(PlayerControl p, int i)
        {
            string id = p.id;
            Console.WriteLine(id, i);
            //let n = cc.instantiate(this.playerPrefab);
            //this.node.addChild(n);
            //n.setPosition
            //setLabel
        }
        /* simulator: set self index. */
        public void SelectPlayer(int i)
        {
            selfIdx = i;
            Global.task.ShowTaskInfo();
            //onCurTaskChanged;
            //updateSkillList
            //updatePlayerDisplay;
            //updateCamera updateMoveBind updateFogRender
        }
    }
}
