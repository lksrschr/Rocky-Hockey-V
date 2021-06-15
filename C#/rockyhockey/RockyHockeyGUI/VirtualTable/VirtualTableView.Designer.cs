namespace RockyHockeyGUI.VirtualTable
{
    partial class VirtualTableView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.x0TextBox = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.y0TextBox = new System.Windows.Forms.TextBox();
            this.x1TextBox = new System.Windows.Forms.TextBox();
            this.y1TextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.stopButton = new System.Windows.Forms.Button();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.xBatTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.score = new System.Windows.Forms.Label();
            this.scoreB = new System.Windows.Forms.Label();
            this.labelScore = new System.Windows.Forms.Label();
            this.labelScorePlayer = new System.Windows.Forms.Label();
            this.labelScoreBot = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.yBatTextBox = new System.Windows.Forms.TextBox();
            this.moveBatMode = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // x0TextBox
            // 
            this.x0TextBox.Location = new System.Drawing.Point(894, 12);
            this.x0TextBox.Name = "x0TextBox";
            this.x0TextBox.Size = new System.Drawing.Size(100, 20);
            this.x0TextBox.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Location = new System.Drawing.Point(37, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(827, 452);
            this.panel.TabIndex = 1;
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelPaintGoal);
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelPaint);
            this.panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelMouseDown);
            this.panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelMouseMove);
            this.panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelMouseUp);
            // 
            // y0TextBox
            // 
            this.y0TextBox.Location = new System.Drawing.Point(1026, 12);
            this.y0TextBox.Name = "y0TextBox";
            this.y0TextBox.Size = new System.Drawing.Size(100, 20);
            this.y0TextBox.TabIndex = 0;
            // 
            // x1TextBox
            // 
            this.x1TextBox.Location = new System.Drawing.Point(894, 38);
            this.x1TextBox.Name = "x1TextBox";
            this.x1TextBox.Size = new System.Drawing.Size(100, 20);
            this.x1TextBox.TabIndex = 0;
            // 
            // y1TextBox
            // 
            this.y1TextBox.Location = new System.Drawing.Point(1026, 38);
            this.y1TextBox.Name = "y1TextBox";
            this.y1TextBox.Size = new System.Drawing.Size(100, 20);
            this.y1TextBox.TabIndex = 0;
            // 
            // goButton
            // 
            this.goButton.Enabled = false;
            this.goButton.Location = new System.Drawing.Point(1054, 412);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 3;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.GoButtonClick);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 5; // original value 20
            this.timer.Tick += new System.EventHandler(this.TimerTick);

            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(1054, 441);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(875, 354);
            this.trackBar.Maximum = 20;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(254, 45);
            this.trackBar.TabIndex = 4;
            this.trackBar.Value = 2;
            this.trackBar.Scroll += new System.EventHandler(this.TrackBarScroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(878, 385);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1102, 385);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "0.02";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(872, 338);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Friction";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(868, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "X0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(868, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "X1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1000, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Y0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1000, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Y1";
            // 
            // xBatTextBox
            // 
            this.xBatTextBox.Location = new System.Drawing.Point(908, 87);
            this.xBatTextBox.Name = "xBatTextBox";
            this.xBatTextBox.Size = new System.Drawing.Size(86, 20);
            this.xBatTextBox.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1002, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "YBat";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(872, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "XBat";
            // 
            // yBatTextBox
            // 
            this.yBatTextBox.Location = new System.Drawing.Point(1038, 87);
            this.yBatTextBox.Name = "yBatTextBox";
            this.yBatTextBox.Size = new System.Drawing.Size(88, 20);
            this.yBatTextBox.TabIndex = 0;
            //
            // activate move bat
            //
            this.moveBatMode.Enabled = false;
            this.moveBatMode.Location = new System.Drawing.Point(925, 441);
            this.moveBatMode.Name = "moveBatButton";
            this.moveBatMode.Size = new System.Drawing.Size(125, 23);
            this.moveBatMode.TabIndex = 3;
            this.moveBatMode.Text = "MoveBat";
            this.moveBatMode.UseVisualStyleBackColor = true;
            this.moveBatMode.Click += new System.EventHandler(this.MoveBatClick);
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(950, 150);
            this.labelScore.Name = "Score";
            this.labelScore.Size = new System.Drawing.Size(30, 13);
            this.labelScore.TabIndex = 7;
            this.labelScore.Text = "Score";
            this.labelScore.Font = new System.Drawing.Font("Arial", 20,System.Drawing.FontStyle.Italic);
            // 
            // labelScorePlayer
            // 
            this.labelScorePlayer.AutoSize = true;
            this.labelScorePlayer.Location = new System.Drawing.Point(880, 180);
            this.labelScorePlayer.Name = "scorePlayer";
            this.labelScorePlayer.Size = new System.Drawing.Size(30, 13);
            this.labelScorePlayer.TabIndex = 7;
            this.labelScorePlayer.Text = "Player";
            this.labelScorePlayer.Font = new System.Drawing.Font("Arial", 12,System.Drawing.FontStyle.Italic);
            // 
            // labelScoreBot
            // 
            this.labelScoreBot.AutoSize = true;
            this.labelScoreBot.Location = new System.Drawing.Point(1012, 180);
            this.labelScoreBot.Name = "scoreBot";
            this.labelScoreBot.Size = new System.Drawing.Size(30, 13);
            this.labelScoreBot.TabIndex = 7;
            this.labelScoreBot.Text = "Mr. Robot";
            this.labelScoreBot.Font = new System.Drawing.Font("Arial", 12,System.Drawing.FontStyle.Italic);
            // 
            // Score player
            // 
            this.score.AutoSize = true;
            this.score.Location = new System.Drawing.Point(872, 201);
            this.score.Name = "Score";
            this.score.Size = new System.Drawing.Size(70, 10);
            this.score.TabIndex = 7;
            this.score.Text = "0";
            this.score.Font = new System.Drawing.Font("Arial", 60,System.Drawing.FontStyle.Bold);
            // 
            // Score bot
            // 
            this.scoreB.AutoSize = true;
            this.scoreB.Location = new System.Drawing.Point(1002, 201);
            this.scoreB.Name = "ScoreB";
            this.scoreB.Size = new System.Drawing.Size(70, 10);
            this.scoreB.TabIndex = 7;
            //this.scoreB.Text = "0";
            this.scoreB.Text += new System.Windows.Forms.PaintEventHandler(this.PanelChangeScore);
            this.scoreB.Font = new System.Drawing.Font("Arial", 60,System.Drawing.FontStyle.Bold);
            // 
            // VirtualTableView
            // 
            this.AcceptButton = this.goButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1138, 476);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.yBatTextBox);
            this.Controls.Add(this.xBatTextBox);
            this.Controls.Add(this.y0TextBox);
            this.Controls.Add(this.y1TextBox);
            this.Controls.Add(this.x1TextBox);
            this.Controls.Add(this.x0TextBox);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.labelScorePlayer);
            this.Controls.Add(this.labelScoreBot);
            this.Controls.Add(this.score);
            this.Controls.Add(this.scoreB);
            this.Controls.Add(this.moveBatMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VirtualTableView";
            this.Text = "Virtual Table";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox x0TextBox;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TextBox y0TextBox;
        private System.Windows.Forms.TextBox x1TextBox;
        private System.Windows.Forms.TextBox y1TextBox;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox xBatTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox yBatTextBox;
        private System.Windows.Forms.Button moveBatMode;
        private System.Windows.Forms.Label score;
        private System.Windows.Forms.Label scoreB;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelScorePlayer;
        private System.Windows.Forms.Label labelScoreBot;

    }
}