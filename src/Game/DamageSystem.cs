using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace amongus_game_flow
{
    public class DamageSystem
    {
        Task t;
        CancellationTokenSource tokenSource2;
        public DamageSystem(int id)
        {
            this.cd = 10; // TODO:get cd by id;
            this.startTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;
            t = Task.Run(async () =>
            {
                for (; ; )
                {
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                        break;
                    }
                    await Task.Delay(1000);
                    Update();
                }
            }, tokenSource2.Token);
        }
        int cd;
        long startTime;
        public void Destroy()
        {
            tokenSource2.Cancel();
            t.Dispose();
        }

        void Update()
        {
            long l = this.startTime + this.cd - DateTimeOffset.Now.ToUnixTimeSeconds();
            if (l < 0)
            {
                Console.WriteLine("Damage to imp win");
                Global.game.ShowGameOver(false);
                Destroy();
            }
            else
            {
                Console.WriteLine("Damage:" + l);
            }
        }
    }
}
