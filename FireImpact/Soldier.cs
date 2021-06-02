using MathNet.Spatial.Euclidean;
using System;

namespace FireImpact
{
    /// <summary>
    /// <c>Soldier</c> class as o placeholder for Game Environment's soldier units. Each soldier is represented by a single point.
    /// </summary>
    public class Soldier
    {
        public int Id { get; set; }
        public Vector3D Position { get; set; }
        /// <summary>
        /// Initializes a soldier at <param name="position"> with name <param name="id"></param> </param>
        /// </summary>
        /// <param name="id">Id of the <c>Soldier</c>.</param>
        /// <param name="position"> The [X,Y,Z] position of the <c>Soldier</c>. Z is height.</param>
        public Soldier(int id, Vector3D position)
        {
            Id = id;
            Position = position;
        }
    }
}
