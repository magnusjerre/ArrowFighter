using System.Collections.Generic;

namespace Jerre.GameSettings
{
    public class GameRoundState
    {
        public int CurrentRoundNumber = 1;
        public List<object> roundScores = new List<object>();
        public List<object> roundInfos = new List<object>();

        public GameRoundState()
        {
            
        }

        // Should only return round scores for already completed rounds. Don't expect this to contain live updates
        public List<T> GetRoundScores<T>() {
            var output = new List<T>();
            foreach (var roundScore in roundScores)
            {
                output.Add((T)roundScore);
            }
            return output;
        }

        // Should only return round infos for already completed rounds. Don't expect this to contain live updates
        public List<T> GetRoundInfos<T>()
        {
            var output = new List<T>();
            foreach (var roundInfo in roundInfos)
            {
                output.Add((T)roundInfo);
            }
            return output;
        }
    }
}
