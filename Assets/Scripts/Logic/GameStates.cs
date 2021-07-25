using UnityEngine;

namespace Logic
{
   public enum GameTurn
   {
       Player = 1,
       Opponent = 2
   }
   
   
   public enum OpponentType
   {
       Unknown = 0,
       LocalHuman = 1,
       // RemoteHuman = 2,
       Computer = 3
   }
   
   
   public enum PlayerMark
   {
       Unknown = 0,
       Noughts = 1, 
       Crosses = 2
   }
   
   
   public enum DifficultyLevel
   {
       Unknown = 0, 
       Easy = 1,
       Medium = 2,
       Hard = 3
   }
}
