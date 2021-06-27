using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using RockyHockey.Common;
using RockyHockey.MovementFramework;

namespace RockyHockeyGUI.VirtualTable
{
    /// <summary>
    /// This window hosts and interacts with a virtual table simulation (<see cref="VirtualTableWorker"/>).
    /// The virtual table is modeling the real table, bat and puck at a scale of 2 mm : 1 pixel (at 100% display scale).
    /// Since the table size is defined like this, all coordinates going in and out have to be scaled to the play field size in the <see cref="Config"/>.
    ///
    /// In addition to scaling, the virtual table, path prediction and motor controls all have their origin - point (0,0) - in different places.
    /// The new strategies see the table rotated by 180° compared to the old strategies.
    /// The virtual table has its origin in the top-left corner. This is the left corner when standing on the robot side.
    /// Path tracing (the main window) uses the bottom-left corner. This is the right corner of the robot side.
    /// Motor controls have it in the bottom-left corner. 
    /// </summary>
    public partial class VirtualTableView : Form
    {
        private const int FieldWidth = 800; //       1600 mm
        private const int FieldHeight = 450; //       900 mm
        private const float BatDiameter = 47.5f; //    95 mm
        private const float BatRadius = 23.75f; //   47.5 mm
        private const int goalWidth = 24;
        private const int goalheight = 114;
        // Not quite sure about the puck size - forgot to measure... Standardized puck sizes are 63 and 50 mm.
        // Putting 63 mm for now but I have a feeling it might be wrong.
        // Todo: Use the correct puck size here.
        private const float PuckDiameter = 31.5f; //   63 mm
        private const float PuckRadius = 15.75f; //  31.5 mm
        //private const float PuckDiameter = 25.5f; //   50 mm
        //private const float PuckRadius = 12.25f; //  25.5 mm

        private int mouseHeldX, mouseHeldY;

        private bool isMouseHeld;
        private bool hasVelocityLine;
        public bool moveBat;
        Vector2 currentPos;

        internal VirtualTableWorker Table { get; }

        /// <summary>
        /// Creates a new virtual table window.
        /// Automatically sets up the table simulation.
        /// </summary>
        public VirtualTableView()
        {
            InitializeComponent();

            // Enable double buffering on the panel to eliminate flickering. Has to be done via reflection because DoubleBuffered is protected in Panel.
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panel,
                new object[] {true});

            Table = new VirtualTableWorker(FieldWidth, FieldHeight, PuckRadius, BatRadius);
            Table.Start();

            TrackBarScroll(trackBar, EventArgs.Empty);
            ActiveControl = goButton;

            MovementController.Instance.OnMove += AxisOnMove;
        }
        
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            MovementController.Instance.OnMove -= AxisOnMove;
            Table.Stop();
        }

        private void PanelPaintGoal(object sender, PaintEventArgs e)
        {
            // Paint the goals onto the playfield
            Pen blackPen = new Pen(Color.Red, 5);
            Rectangle leftrect = new Rectangle(-24,169,goalWidth,goalheight);
            Rectangle rightrect = new Rectangle(824,169,goalWidth,goalheight);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawRectangle(blackPen, leftrect);
            e.Graphics.DrawRectangle(blackPen, rightrect);
        }

        private void PanelPaint(object sender, PaintEventArgs e)
        {

            // Paint puck and bat onto the playfield
            var puckPos = Vector2.Zero;
            var batPos = Vector2.Zero;
            var robPos = Vector2.Zero;

            Table.AccessState(state =>
            {
                if(state.GoalHappened==true)
                {
                    UpdateScore();
                }
                
                puckPos = state.Position;
                batPos = state.BatPosition;
                robPos = state.RobotPosition;
            });

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(Color.PaleVioletRed), puckPos.X - PuckRadius, puckPos.Y - PuckRadius,
                PuckDiameter, PuckDiameter);
            e.Graphics.FillEllipse(new SolidBrush(Color.LimeGreen), batPos.X - BatRadius, batPos.Y - BatRadius,
                BatDiameter, BatDiameter);
            e.Graphics.FillEllipse(new SolidBrush(Color.DarkBlue), robPos.X - BatRadius, robPos.Y - BatRadius,
                BatDiameter, BatDiameter);
            // Paint puck and bat onto the playfield
            //Draw the velocity vector of the puck
            if (hasVelocityLine)
            {
                e.Graphics.DrawLine(new Pen(Color.Red, 3), puckPos.X, puckPos.Y, mouseHeldX, mouseHeldY);
            }
        }

        private void PanelMouseDown(object sender, MouseEventArgs e)
        {
            var isStationary = false;

            Table.AccessState(state =>
            {
                isStationary = state.IsPuckStationary;

                if ((isStationary)&&(moveBatMode.Enabled==false)) // Move puck if MoveBatMode isn't active
                {
                    state.Position = new Vector2(e.X, e.Y);
                }
                else if ((moveBatMode.Enabled==true)&&(moveBat==true)) // Move bat if MoveBat Button is clicked
                {
                    moveBat=false;
                }
                
            });

            if (isStationary)
            {
                mouseHeldX = e.X;
                mouseHeldY = e.Y;

                isMouseHeld = true;
                hasVelocityLine = true;
                panel.Invalidate();
                UpdateTextboxes();
            }
        }

        private void PanelMouseMove(object sender, MouseEventArgs e)
        {
            // Move bat when MoveBatMode is active
            MoveBatOnMouseMove(sender, e);
            // Get current mouse location
            currentPos.X = e.Location.X*5f;
            currentPos.Y = e.Location.Y*5f;
            if (isMouseHeld)
            {
                MoveBatOnMouseMove(sender, e);
                mouseHeldX = e.X;
                mouseHeldY = e.Y;
                
                panel.Invalidate();
                UpdateTextboxes();
            }
        }

        private void PanelMouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseHeld)
            {
                isMouseHeld = false;

                goButton.Enabled = true;

                panel.Invalidate();
                UpdateTextboxes();
            }
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            if (hasVelocityLine)
            {
                Table.AccessState(state =>
                {
                    var position = state.Position;
                    state.Velocity =
                        new Vector2(mouseHeldX - position.X, mouseHeldY - position.Y)/25; // 0.25 * ticks/s
                });

                hasVelocityLine = false;
                moveBatMode.Enabled = true;
                goButton.Enabled = false;
                stopButton.Enabled = true;
            }
        }

        private void MoveBatClick(object sender, EventArgs e)
        { 

            if(moveBat==false)
            {
                Table.AccessState(state => state.IsBatStationary = false);
                moveBat = true;
            }
           
            else if (moveBat==true)
            {
                moveBat = false;
                Table.AccessState(state => state.IsBatStationary = true
                
                );
            }
            
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            Table.AccessState(state => state.IsPuckStationary = true);
            stopButton.Enabled = false;
        }

        private void TrackBarScroll(object sender, EventArgs e)
        { // initial value 0.001f
            Table.AccessState(state => state.Friction = trackBar.Value * 0.00075f); // second value 0.00075f
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Table.Evaluate(state => state.IsPuckStationary))
            {
                stopButton.Enabled = false;
            }

            panel.Invalidate();
        }

        private void UpdateTextboxes()
        {
            var position = Table.Evaluate(state => state.Position);
            x0TextBox.Text = position.X.ToString(CultureInfo.InvariantCulture);
            y0TextBox.Text = position.Y.ToString(CultureInfo.InvariantCulture);
            x1TextBox.Text = mouseHeldX.ToString();
            y1TextBox.Text = mouseHeldY.ToString();
        }

        private void UpdateBatTextboxes()
        {
            var batPos = Table.Evaluate(state => state.BatPosition);
            xBatTextBox.Text = batPos.X.ToString(CultureInfo.InvariantCulture);
            yBatTextBox.Text = batPos.Y.ToString(CultureInfo.InvariantCulture);

        }

        private void UpdateScore()
        {
            var player = Vector2.Zero;
            var bot = Vector2.Zero;
            Table.AccessState(state =>
            {
                score.Text = System.Convert.ToString(state.pointsplayer);
                score.Refresh();
                scoreB.Text  = System.Convert.ToString(state.pointsbot);
                scoreB.Refresh();                  
            });

        }

        private void AxisOnMove(double x, double y)
        {
            Invoke(new Action(() =>
            {
                xBatTextBox.Text = x.ToString(CultureInfo.InvariantCulture);
                yBatTextBox.Text = y.ToString(CultureInfo.InvariantCulture);
            }));
        }
        // TODO implement a function to move to bat on click while the puck is moving
        // TODO NEXT: Puck is getting stuck inside the bat try to fix next time
        private void MoveBatOnMouseMove(object sender, MouseEventArgs e)
        {
            UpdateScore();
            Table.AccessState(state =>
            {
                var batPosition = state.BatPosition;
                var position = state.Position;
                if( (moveBatMode.Enabled==true)&&(moveBat==true) )
                {
                    // set the velocity of the bat upon movement of the mouse
                    state.VelocityBat = new Vector2(currentPos.X - e.Location.X*4.99f, currentPos.Y - e.Location.Y*4.99f )/10f;

                    if ( ( (Math.Abs(position.X - batPosition.X) >= 0f)&&(Math.Abs(position.X - batPosition.X) <= 39.5f) ) && 
                    ( (Math.Abs(position.Y - batPosition.Y) >= 0f)&&(Math.Abs(position.Y - batPosition.Y) <= 39.5f) ) ) 
                    {
                        // EXPERIMENTAL
                        if((!state.IsBatStationary)&&(state.VelocityBat.Length()>0.2f))
                        {
                            state.Velocity.Y +=  (state.VelocityBat.Y);
                            state.Velocity.X +=  (state.VelocityBat.X);
                        }
                        else if(state.VelocityBat.Length()<=0.1f)
                        {
                            state.IsBatStationary=true;
                            state.Velocity.Y *=  -1;
                            state.Velocity.X *=  -1;
                        }
                        // EXPERIMENTAL
                    }
                }
            }
            );
            // Move bat if MoveBat Button is clicked
            Table.AccessState(state =>
            {
                if ((moveBatMode.Enabled==true)&&(moveBat==true)) 
                {
                    state.BatPosition.X = e.Location.X;
                    state.BatPosition.Y = e.Location.Y;
                    UpdateBatTextboxes();
                }
            });
        }
    }
}
