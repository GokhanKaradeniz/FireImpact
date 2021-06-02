using System;
using System.Collections.Generic;
using FireImpact.Resources;
using MathNet.Spatial.Euclidean;

namespace FireImpact
{
    /// <summary>
    ///     Class <c>Weapon</c> models a simple weapon with a muzzle
    ///     as a placeholder for Game Environment's weapon systems.
    /// </summary>
    public class Weapon
    {
        #region Properties
        /// <summary>
        /// List of <c>EffectArea</c>s attached to the weapon.
        /// </summary>
        public List<EffectArea> EffectAreas;
        /// <summary>
        ///  Radius of muzzle, in millimeters.
        /// </summary>
        public double MuzzleCaliber;
        /// <summary>
        /// he [X,Y,Z] vector the muzzle is pointing.
        /// </summary>
        public Vector3D MuzzleDir;
        /// <summary>
        /// The [X,Y,Z] position of the muzzle, Z is height.
        /// </summary>
        public Vector3D MuzzlePos;
        /// <summary>
        /// This parameter will be used for deciding if a weapon is a "large weapon".
        /// Calibers greater then this value is considered as "large".
        /// </summary>
        private const int MinCaliberForLargeWeapon = 15;


        /// <summary>
        /// Delegate for weapon fire.
        /// </summary>
        /// <param name="allUnits">Units nearby weapon.</param>
        /// <param name="weaponFired">Weapon that fired.</param>
        public delegate void OnWeaponFiredHandler(Soldier[] allUnits, Weapon weaponFired);

        /// <summary>
        /// Weapon Fired event.
        /// </summary>
        public event OnWeaponFiredHandler WeaponFired;





        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a <c>Weapon</c> at <paramref name="muzzlePos" />
        ///     with a muzzle looks toward <paramref name="muzzleDir" />
        ///     with a caliber of <paramref name="muzzleCaliber" />
        /// </summary>
        /// <param name="muzzleCaliber">Radius of muzzle, in millimeters.</param>
        /// <param name="muzzleDir">The [X,Y,Z] vector the muzzle is pointing.</param>
        /// <param name="muzzlePos">The [X,Y,Z] position of the muzzle, Z is height.</param>
        public Weapon(double muzzleCaliber, Vector3D muzzleDir, Vector3D muzzlePos)
        {
            MuzzleCaliber = muzzleCaliber;
            MuzzleDir = muzzleDir;
            MuzzlePos = muzzlePos;
        }

        #endregion

        #region Methods

        /// <summary>
        /// A placeholder for Game Environment's weapon fire event.
        /// </summary>
        public void Fire()
        {


            // If muzzleCaliber is greater dan MinCaliberForLargeWeapon (15) weapon will be considered as large weapon.
            if (MuzzleCaliber > MinCaliberForLargeWeapon)
            {
                //Trigger WeaponFired event and call subscribers.
                WeaponFired(GameEnvironment.AllUnits, this);

                //Write info about firing direction and muzzle position.
                Console.WriteLine(StringResource.ResourceManager.GetString("InfoWeaponFired"), MuzzleDir, MuzzlePos);
            }

        }


        #endregion
    }
}