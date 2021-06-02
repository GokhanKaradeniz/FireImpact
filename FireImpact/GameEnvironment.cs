using MathNet.Spatial.Euclidean;
using System;
using System.Resources;
using FireImpact.Resources;


namespace FireImpact
{
    /// <summary>
    /// This is a helper class to simulate data will be fetched from Game Environment.
    /// </summary>
    public static class GameEnvironment
    {
        #region Properties
        
        
        /// <summary>
        /// Max Blindness Duration
        /// </summary>
        public const double MaxBlindnessDuration = 2.0d;
        /// <summary>
        /// Max Deafness Duration
        /// </summary>
        public const double MaxDeafnessDuration = 60.0d;
        /// <summary>
        /// all soldiers are considered as standing, and they are 1.8 meters.
        /// </summary>
        public const double HeightOfSoldier = 1.8; 
        /// <summary>
        /// Returns all infantry units.
        /// </summary>
        public static Soldier[] AllUnits;

        #endregion

        #region Methods

        /// <summary>
        /// SetBlindness function as stated in the Co
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded.</param>
        /// <param name="duration">Duration of Blindness</param>
        /// <remarks>Writes info about soldier and duration to console</remarks>
        public static void SetBlindness(Soldier soldier, double duration)
        {
            if (duration > 0)
                Console.WriteLine(StringResource.ResourceManager.GetString("InfoBlindnessApplied"), soldier.Id, duration);
        }

        /// <summary>
        /// SetBlindness function as stated in the Co
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded.</param>
        /// <param name="duration">Duration of Deafness</param>
        /// <remarks>Writes info about soldier and duration to console</remarks>
        public static void SetDeafness(Soldier soldier, double duration)
        {
            if (duration > 0)
                Console.WriteLine(StringResource.ResourceManager.GetString("InfoDeafnessApplied"), soldier.Id, duration);
        }


        /// <summary>
        /// This method created randomly placed soldiers for quick test purposes.
        /// </summary>
        /// <param name="count">Number of soldiers that will be initialized.</param>
        /// <param name="maxDistanceToMuzzle">Random X and Y values of soldier position will be in range of [-maxDistanceToMuzzle, maxDistanceToMuzzle]</param>
        public static void InitializeRandomlyPlacedSoldiers(int count, int maxDistanceToMuzzle)
        {

            AllUnits = new Soldier[count];
            Random r = new Random();

            for (int i = 0; i < count; i++)
            {
                var x = r.Next(-maxDistanceToMuzzle, maxDistanceToMuzzle);
                var y = r.Next(-maxDistanceToMuzzle, maxDistanceToMuzzle);

                AllUnits[i] = new Soldier(i, new Vector3D(x, y, HeightOfSoldier)); 
            }

        }

    

        #endregion

    }



}
