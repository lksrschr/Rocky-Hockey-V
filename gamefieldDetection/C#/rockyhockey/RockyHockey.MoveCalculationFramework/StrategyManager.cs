﻿using RockyHockey.Common;
using RockyHockey.MovementFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockyHockey.MoveCalculationFramework
{
    public class StrategyManager
    {
        private IMovementController movementController;
        private PathPrediction prediction;
        private TimedCoordinate defense;

        private SimpleStrategie[] strategies;

        int distanceParts;

        bool calculated = false;
        bool onlyDefended = false;

        /// <summary>
        /// creates an instance of the StrategyManager
        /// </summary>
        /// <param name="movementController">Instance of the movement controller</param>
        /// <param name="prediction">Instance of the trajectory prediction</param>
        public StrategyManager(IMovementController movementController, PathPrediction prediction)
        {
            this.movementController = movementController;
            this.prediction = prediction;

            prediction.OnInit += onPathPredictionInit;

            distanceParts = (int)(Config.Instance.BatRadius + Config.Instance.Tolerance);
        }

        /// <summary>
        /// moves the bat to a defensive position if the left time is not enough to work out a counterattack
        /// </summary>
        private void moveToDefensePosition()
        {
            defense = prediction.getTimeForPosition(distanceParts).TimedEnd;

            movementController.Move(defense);
        }

        /// <summary>
        /// calculates based on the trajectory and velocity of the puck different strategies
        /// </summary>
        public void calculate()
        {
            if (!calculated)
            {
                try
                {
                    moveToDefensePosition();

                    onlyDefended = createStrategies(2) == strategies.Length;

                    calculated = true;
                }
                catch (Exception e)
                {
                    ;
                }
            }
        }

        private void onPathPredictionInit()
        {
            if (calculated)
            {
                calculated = false;

                if (onlyDefended)
                {
                    onlyDefended = false;

                    createStrategies(4);
                }
            }
        }

        /// <summary>
        /// calculates different strategies for counterattacks
        /// </summary>
        /// <param name="distanceFactor">a factor for rng strategies</param>
        /// <returns>different strategies to hit the puck on its trajectory</returns>
        private int createStrategies(int distanceFactor)
        {
            int numberOfStrategies = 4;

            strategies = new SimpleStrategie[numberOfStrategies];
            Task[] strategieTasks = new Task[numberOfStrategies];

            for (int a = numberOfStrategies + distanceFactor - 1, b = 0; b < 4; a--, b++)
            {
                SimpleStrategie strategie = new SimpleStrategie();
                strategies[b] = strategie;

                strategie.init(movementController, prediction, a * distanceParts);

                strategieTasks[b] = strategie.startCalculation();
            }

            Task.WaitAll(strategieTasks);

            int strategieIterator = 0;
            for (; strategieIterator < strategies.Length; strategieIterator++)
                if (strategies[strategieIterator].enoughTimeLeft())
                {
                    strategies[strategieIterator].execute();

                    break;
                }

            return strategieIterator;
        }
    }
}
