using FireImpact.Enums;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;

namespace FireImpact
{

    /// <summary>
    /// This class has the main method that runs at project startup.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// This method runs at project startup.
        /// Creates a weapon. Binds EffectAres.
        /// Subscribes ApplyBlindnessAndDeafness method to Weapon Fire Event.
        /// Creates some soldiers nearby weapon. Then fires the weapon.
        /// </summary>
        private static void Main()
        {
            //Create a weapon
            var weapon = new Weapon(muzzleCaliber: 40, muzzleDir: new Vector3D(1, 0, 0), muzzlePos: new Vector3D(0, 0, 2));

            //Create Effect Areas as shown at the documentation image
            var flash = new EffectArea(ref weapon, type: AreaProps.Type.Flash, priority: AreaProps.Priority.VeryHigh, rotation: 0, lengthFactor: AreaProps.Range.Short, impactFactorAtConeTip: 1, impactFactorAtConeBase: 0.8);
            var high = new EffectArea(ref weapon, type: AreaProps.Type.HighEffect, priority: AreaProps.Priority.High, rotation: 0, lengthFactor: AreaProps.Range.Long, impactFactorAtConeTip: 0.8, impactFactorAtConeBase: 0.4);
            var low1 = new EffectArea(ref weapon, type: AreaProps.Type.LowEffect, priority: AreaProps.Priority.Low, rotation: 90, lengthFactor: AreaProps.Range.Medium, impactFactorAtConeTip: 0.6, impactFactorAtConeBase: 0.2);
            var low2 = new EffectArea(ref weapon, type: AreaProps.Type.LowEffect, priority: AreaProps.Priority.Low, rotation: -90, lengthFactor: AreaProps.Range.Medium, impactFactorAtConeTip: 0.6, impactFactorAtConeBase: 0.2);

            //Attach Effect Areas to weapon
            weapon.EffectAreas = new List<EffectArea>() { flash, high, low1, low2 };

            //Create some randomly placed soldiers for test purposes.
            GameEnvironment.InitializeRandomlyPlacedSoldiers(count: 50, maxDistanceToMuzzle: 12);

            //Subscribe ApplyBlindnessAndDeafness method to Weapon Fire Event.
            weapon.WeaponFired += EffectsOnNearbyUnits.ApplyBlindnessAndDeafness;

            //Fire!...
            weapon.Fire();


        }

    }

}
