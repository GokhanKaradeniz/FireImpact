using FireImpact.Enums;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System;

namespace FireImpact
{
    /// <summary>
    /// Class <c>EffectArea</c> models a cone shaped volume 
    /// in which soldiers will have temporary blindness and deafness effects when a large weapon fired.
    /// </summary>
    /// <remarks>Uses Degrees for Angles</remarks>
    public class EffectArea
    {

        #region Properties

        /// <summary>
        /// Rotation RELATIVE to muzzle direction, in Degrees
        /// </summary>
        public double Rotation { get; }
        /// <summary>
        /// The ref of the weapon this Effect Area is attached to.
        /// </summary>
        public Weapon Weapon { get; }
        /// <summary>
        /// This parameter is used to calculate MAX. blindness and deafness durations of this <c>EffectArea</c>.
        /// Durations will decrease linearly as the distance from muzzle increases and will reach to MIN. at the cone base.
        /// </summary>
        public double ImpactFactorAtConeTip { get; }
        /// <summary>
        /// This parameter is used to calculate MIN. blindness and deafness durations of this <c>EffectArea</c>.
        /// Durations will increase linearly as the distance from muzzle decreases and will reach to MAX. at the cone tip.
        /// </summary>
        public double ImpactFactorAtConeBase { get; }
        /// <summary>
        /// Type of the Effect Area
        /// </summary>
        public AreaProps.Type Type { get; }
        /// <summary>
        /// When an Effect Area is overlapping another, the one with the higher priority will be taken into account.
        /// </summary>
        public AreaProps.Priority Priority { get; }
        /// <summary>
        /// This parameter will be multiplied by the muzzle caliber and used to find the cone Length. (aka. r, Radius at the documentation image)
        /// </summary>
        private AreaProps.Range LengthFactor { get; }
        /// <summary>
        /// Arc of Cone. Default set to 50.
        /// </summary>
        private double ConeAngle { get; }

        #endregion

        #region Constructors

        /// <summary>This constructor initializes a new  <c>EffectArea</c> in which the soldiers will be temporarily blinded and/or deafened
        /// when the owner weapon of this  <c>EffectArea</c> is fired.
        /// </summary>
        /// <param name="weapon">The ref of the weapon this Effect Area is attached to.</param>
        /// <param name="type">Type of the Effect Area</param>
        /// <param name="priority">When an Effect Area is overlapping another, the one with the higher priority will be taken into account</param>
        /// <param name="rotation">Rotation RELATIVE to muzzle direction, in Degrees</param>
        /// <param name="coneAngle">Arc of Cone. Default set to 50.</param>
        /// <param name="lengthFactor">This parameter will be multiplied by the muzzle caliber and used to find the cone Length. (aka. r, Radius at the documentation image)</param>
        /// <param name="impactFactorAtConeTip">This parameter is used to calculate MAX. blindness and deafness durations of this <c>EffectArea</c>.
        /// Durations will decrease linearly as the distance from muzzle increases and will reach to MIN. at the cone base.</param>
        /// <param name="impactFactorAtConeBase">This parameter is used to calculate MIN. blindness and deafness durations of this <c>EffectArea</c>.
        /// Durations will increase linearly as the distance from muzzle decreases and will reach to MAX. at the cone tip.</param>
        public EffectArea(ref Weapon weapon, AreaProps.Type type, AreaProps.Priority priority, double rotation, AreaProps.Range lengthFactor, double impactFactorAtConeTip, double impactFactorAtConeBase, double coneAngle = 50)
        {

            Type = type;
            Priority = priority;
            Rotation = rotation;
            LengthFactor = lengthFactor;
            ImpactFactorAtConeTip = impactFactorAtConeTip;
            ImpactFactorAtConeBase = impactFactorAtConeBase;
            ConeAngle = coneAngle;
            Weapon = weapon;

        }

        #endregion

        #region Methods
        /// <summary>
        /// Multiplies the LengthFactor of this <c>EffectArea</c> and the muzzleCaliber of the <c>Weapon</c> it's attached to.
        /// </summary>
        /// <returns>Length of <c>EffectArea in meters. (AKA "r, radius" at documentation image.)</c></returns>
        public double GetLength()
        {
            //Convert muzzleCaliber from milimeters to meters, then multiply by LengthFactor 
            return Weapon.MuzzleCaliber / 1000 * (int)LengthFactor;
        }

        /// <summary>
        /// The [X,Y,Z] vector the axix of Effect Area Cone is pointing.
        /// </summary>
        /// <returns>Returns current The [X,Y,Z] vector the axix of Effect Area Cone is pointing.</returns>
        public Vector3D GetDirection()
        {
            return Weapon.MuzzleDir.Rotate(UnitVector3D.ZAxis, Angle.FromDegrees(this.Rotation));
        }

        /// <summary>
        /// Checks if <paramref name="point"/> is inside of this Effect Area.
        /// </summary>
        /// <param name="point">The [X,Y,Z] point that will be checked.</param>
        /// <returns>Returns True if the <paramref name="point"/> is inside of owner Effect Area. Otherwise False.</returns>
        public bool IsPointInsideEffectArea(Vector3D point)
        {

            //Effect Area is a cone shaped volume.
            //Distance between point and cone tip (MuzzlePos)
            var distance = point - Weapon.MuzzlePos;
            //Distance between cone axis and point
            var coneDistanceAlongConeAxis = distance.DotProduct(GetDirection());
            
            //Height is altitude of cone.
            var height = GetLength();

            //Radius at Cone Base
            var baseRadius = height * Math.Tan((Math.PI / 180) * ConeAngle);

            //cone radius at that point along the axis
            var coneRadius = (coneDistanceAlongConeAxis / height) * baseRadius;

            //If coneDistance is negative or greater than cone altitude it's not in.
            if (coneDistanceAlongConeAxis <= 0 || coneDistanceAlongConeAxis >= height)
            {
                return false;
            }

            //Point's orthogonal distance from the axis to compare against the cone radius
            var orthogonalDistance = (distance - coneDistanceAlongConeAxis * GetDirection()).Length;

            //If orthogonalDistance is lower than coneRadius it is inside the cone.
            var isPointInsideCone = (orthogonalDistance < coneRadius);

            return isPointInsideCone;
        }

        #endregion
    }

}
