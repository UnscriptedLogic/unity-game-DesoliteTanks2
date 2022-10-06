using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelManagement
{
    public class MainMenuFactory : LevelStateFactory
    {
        private MMS_Spawning spawning;
        private MMS_Waiting waiting;

        public MainMenuFactory(LevelStateContext context) : base(context)
        {
            spawning = new MMS_Spawning(context, this);
            waiting = new MMS_Waiting(context, this);
        }

        public MMS_Spawning SpawningState() => spawning;
        public MMS_Waiting WaitingState() => waiting;
    }
}
