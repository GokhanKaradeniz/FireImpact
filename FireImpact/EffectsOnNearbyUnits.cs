using FireImpact.Resources;
using System;

namespace FireImpact
{
    /// <summary>
    /// This class contains methods for applying large weapon firing effects on nearby units.
    /// </summary>
    public static class EffectsOnNearbyUnits
    {
        /// <summary>
        /// This method iterates <paramref name="allUnits"/> and applies deafness and blindness if necessary.
        /// Blindness and deafness applied 
        /// </summary>
        /// <param name="allUnits">Array of all soldiers to iterate through.</param>
        /// <param name="weaponFired">Weapon that will cause the Blindness and Deafness</param>
        public static void ApplyBlindnessAndDeafness(Soldier[] allUnits, Weapon weaponFired)
        {

            //If a parameter is null, do nothing.
            if (allUnits == null || weaponFired == null)
                return;

            //Sort Effect Areas from highest priority to lowest. 
            //To be sure about Iteration will occur in priority order. 
            weaponFired.EffectAreas.Sort((x, y) => x.Priority.CompareTo(y.Priority));


            //For every single soldier we will check if the soldier position is inside of any Effect Area.
            foreach (var soldier in allUnits)
            {
                //Iterate through Effect Areas of the weapon.
                //Iteration will take place form highest priority to lowest, as Effect Areas are ordered so.
                foreach (var zone in weaponFired.EffectAreas)
                {
                    // If soldier is not in this Effect Area, continue with next Effect Area
                    if (!zone.IsPointInsideEffectArea(soldier.Position)) continue;

                    //If soldier is actually in the zone, blindness and deafness will be applied.
                    //The effects will scale based on the distance from the muzzle.
                    //GetDuration method calculates how much time effects will last. 
                    GameEnvironment.SetBlindness(soldier, duration: GetDuration(soldier, zone, GameEnvironment.MaxBlindnessDuration));
                    GameEnvironment.SetDeafness(soldier, duration: GetDuration(soldier, zone, GameEnvironment.MaxDeafnessDuration));

                    Console.WriteLine(StringResource.ResourceManager.GetString("InfoSoldierPosition"), soldier.Id, soldier.Position, zone.Type);


                    //Effect Areas can overlap each other
                    //If a soldier is inside of multiple Effect Areas, only the EffectArea wit the highest priority will be taken into account.
                    //Effect Area Iteration is from highest priority to lowest.
                    //So after a soldier is affected by an Effect Area, we will "break" this iteration to prevent lower priority Effect Areas making effects.
                    //Soldier Iteration will continue with next soldier.
                    break;
                }

            }

            //To prevent console screen from closing.
            Console.ReadLine();

        }

        /// <summary>
        /// Calculates durations for blindness/deafness.
        /// 
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded/deafened</param>
        /// <param name="effectArea">Effect Area that soldier is in.</param>
        /// <param name="maxDuration">Maximum Duration for blindness or deafness effect.</param>
        /// <returns></returns>
        private static double GetDuration(Soldier soldier, EffectArea effectArea, double maxDuration)
        {
            //Calculate the distance between soldier and the muzzle.
            var distance = Math.Abs((soldier.Position - effectArea.Weapon.MuzzlePos).Length);

            //Calculate distance proportional to total length. Nearest point (Muzzle position) will be %0 and furthest point (base of the cone) will be %100)
            var proportionalDistance = distance / effectArea.GetLength();

            //proportionalDistance will be use to calculate the linear decrease between ImpactFactorAtConeTip and ImpactFactorAtConeBase.
            var impactFactorAtDistanceOfSoldier = (effectArea.ImpactFactorAtConeTip - effectArea.ImpactFactorAtConeBase) * (1 - proportionalDistance) + effectArea.ImpactFactorAtConeBase;

            //If somehow an improper impactFactor is entered for the zone, that will prevent exceeding maximum duration allowed.
            impactFactorAtDistanceOfSoldier = impactFactorAtDistanceOfSoldier > 1 ? 1 : impactFactorAtDistanceOfSoldier;

            //Duration is calculated as a ratio of Max Duration Allowed in the documentation. Rounded for simplicity.
            return Math.Round(impactFactorAtDistanceOfSoldier * maxDuration, 2);


        }
    }
}
