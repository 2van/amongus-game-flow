using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace amongus_game_flow
{
    public enum GameResultType
    {
        Continue,// 未结束
        FinishTask, //完成所有任务
        ImpostorOut,//imp全部淘汰
        CrewmateOut,//船员人数不够
        EmergencyTimeOut,//紧急会议未完成
    }
    public class GameState
    {
        private static GameState _instance;
        public static GameState Instance
        {
            get
            {
                if (GameState._instance == null)
                {
                    GameState._instance = new GameState();
                }
                return GameState._instance;
            }
        }

        public GameResultType CheckWin()
        {
            GameResultType rs = this.CheckTaskWin();
            if (rs != GameResultType.Continue)
            {
                rs = this.CheckSurviveWin();
            }
            if (rs == GameResultType.FinishTask || rs == GameResultType.ImpostorOut)
            {
                ShowGameOver(true);
            }
            // || rs === GameResultType.emergencyTimeOut //TODO-FINAL: [emergency] emergencyTimeOut
            if (rs == GameResultType.CrewmateOut)
            {
                ShowGameOver(false);
            }
            return rs;
        }

        private GameResultType CheckTaskWin()
        {
            List<int> counts = Global.task.GetAllTaskProgress();
            if (counts[0] == counts[1])
            {
                Console.WriteLine("crewmate win：finishTask");
                return GameResultType.FinishTask;
            }
            return GameResultType.Continue;
        }
        private GameResultType CheckSurviveWin()
        {
            //let ps = PlayerModel.data;
            //let goodCount = ps.filter(v => v.type == 1 && !v.dead).length,
            //    badCount = ps.filter(v => v.type == 2 && !v.dead).length;
            //if (badCount == 0)
            //{
            //    debuglog('crewmate win：impostorOut');
            //    return GameResultType.ImpostorOut;
            //}
            //if (badCount >= goodCount)
            //{
            //    debuglog('impostor win：crewmateOut');
            //    return GameResultType.CrewmateOut;
            //}
            return GameResultType.Continue;
        }
        public void ShowGameOver(bool crewmateVictory)
        {
            Console.WriteLine("game over");
            if (crewmateVictory)
            {
                Console.WriteLine("crewmate victory");
            }
            else
            {

                Console.WriteLine("impostor victory");
            }
        }
    }
}
