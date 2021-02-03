using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace amongus_game_flow
{
    // TODO: ROOM
    public class Room
    {
        public Room()
        {
            int playerCount = 4;
            int impIdx = new Random().Next(playerCount);
            for (int i = 0; i < playerCount; i++)
            {
                PlayerControl p = new PlayerControl
                {
                    isImpostor = i == impIdx,
                    idx = i,
                    id = i.ToString()
                };
                players.Add(p);
            }
        }
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
