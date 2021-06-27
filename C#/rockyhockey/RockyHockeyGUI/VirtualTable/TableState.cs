using System.Numerics;

namespace RockyHockeyGUI.VirtualTable
{
    internal class TableState
    {
        /// <summary>
        /// Position of the puck on the play field.
        /// </summary>
        internal Vector2 Position;
        internal Vector2 BatPosition;
        internal Vector2 RobotPosition;
        /// <summary>
        /// Velocity of the puck.
        /// <c>Vector2.Zero</c> means the puck is stationary.
        /// </summary>
        internal Vector2 Velocity;
        internal Vector2 VelocityBat;
        internal Vector2 VelocityRobot;


        /// <summary>
        /// Reduces puck velocity to simulate friction.
        /// Measured in relative speed lost per simulation tick.
        /// Not super realistic but should work well enough.
        /// Should be within the range of 0.00 to 0.01.
        /// </summary>
        internal float Friction;

        internal int pointsplayer = 0;
        internal int pointsbot = 0;
        internal bool goal = false;

        internal TableState(Vector2 position, Vector2 batPosition,Vector2 robPosition, int pointpl, int pointbot)
        {
            Position = position;
            BatPosition = batPosition;
            RobotPosition = robPosition;
            pointsplayer = pointpl;
            pointsbot = pointbot;
        }

        internal bool GoalHappened
        {
            get => goal == true;
            set
            {
                if (value)
                {
                    goal = true;
                }
            }
        }

        internal bool IsPuckStationary
        {
            get => Velocity == Vector2.Zero;
            set
            {
                if (value)
                {
                    Velocity = Vector2.Zero;
                }
            }
        }

        internal bool IsBatStationary
        {
            get => VelocityBat == Vector2.Zero;
            set
            {
                if (value)
                {
                    VelocityBat = Vector2.Zero;
                }
            }
        }

        internal bool IsRobotStationary
        {
            get => VelocityRobot == Vector2.Zero;
            set
            {
                if (value)
                {
                    VelocityRobot = Vector2.Zero;
                }
            }
        }
    }
}
