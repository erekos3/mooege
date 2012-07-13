/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Mooege.Net.GS.Message.Definitions.Quest;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.Logging;

namespace Mooege.Core.GS.Games
{
    public interface QuestProgressHandler
    {
        void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value);
        void NotifyBonus(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value);
    }

    public class Quest : QuestProgressHandler
    {

        private static readonly Logger Logger = LogManager.CreateLogger(); // add for debugging purposes

        /// <summary>
        /// Keeps track of a single quest step
        /// </summary>
        public class QuestStep : QuestProgressHandler
        {

            private static readonly Logger Logger = LogManager.CreateLogger(); // add for debugging purposes

            /// <summary>
            /// Keeps track of a single quest step objective
            /// </summary>
            public class QuestObjective : QuestProgressHandler
            {

                private static readonly Logger Logger = LogManager.CreateLogger(); // add for debugging purposes

                public int Counter { get; private set; }
                public bool Done { get { return (objective.CounterTarget == 0 && Counter > 0) || Counter == objective.CounterTarget; } }
                public int ID { get; private set; }

                // these are only needed to show information in console
                public Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType ObjectiveType { get { return objective.ObjectiveType; } }
                public int ObjectiveValue { get { return objective.SNOName1.Id; } }

                private Mooege.Common.MPQ.FileFormats.QuestStepObjective objective;
                public QuestStep questStep;

                public QuestObjective(Mooege.Common.MPQ.FileFormats.QuestStepObjective objective, QuestStep questStep, int id)
                {
                    Logger.Debug(" (QuestObjective ctor) creating an objective with ID {0}, QuestStepObjective {1} and QuestStep ID {2}", id, objective.Group1Name, questStep.QuestStepID);
                    ID = id;
                    this.objective = objective;
                    this.questStep = questStep;
                }
                
                /// <summary>
                /// Notifies the objective (if it is flagged as abonus objective), that an event occured. The objective checks if that event matches the event it waits for
                /// </summary>
                public void NotifyBonus(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
                {
                    Logger.Debug(" (NotifyBonus) objective details SNOName 1 : {0}, ID {1} \n SNOName 2  : {2},ID {3} ", objective.SNOName1.Name, objective.SNOName1.Id, objective.SNOName2.Name, objective.SNOName2.Id);
                    Logger.Debug(" (NotifyBonus) in QuestObjective for type {0} and value {1} and objective.ObjectiveType is {2}", type, value, objective.ObjectiveType);
                    //if (type != objective.ObjectiveType) return;
                    switch (type)
                    {
                        
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.BonusStep:                          
                            {
                                Counter++;
                                questStep.UpdateBonusCounter(this);
                            }
                            break;
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterWorld:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterScene:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.InteractWithActor:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.KillMonster:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.CompleteQuest:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.HadConversation:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterLevelArea:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterTrigger:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EventReceived:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.GameFlagSet:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.KillGroup:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.PlayerFlagSet:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.PossessItem:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.TimedEventExpired:
                            throw new NotImplementedException();
                    }
                }

                /// <summary>
                /// Notifies the objective, that an event occured. The objective checks if that event matches the event it waits for
                /// </summary>
                public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
                {
                    if (value == -1) // this is a simple way to FORCE
                    {
                        Logger.Debug(" (Notify) objective SNOName1  Name : {0}, Id {1}, Valid {2} ", objective.SNOName1.Name, objective.SNOName1.Id, objective.SNOName1.IsValid);
                        Logger.Debug(" (Notify) objective SNOName2  Name : {0}, Id {1}, Valid {2} ", objective.SNOName2.Name, objective.SNOName2.Id, objective.SNOName2.IsValid);
                        Logger.Debug(" (Notify) objective Group1Name : {0} ", objective.Group1Name);
                        Logger.Debug(" (Notify) objective I0 : {0} ", objective.I0);
                        Logger.Debug(" (Notify) objective I2 : {0} ", objective.I2);
                        Logger.Debug(" (Notify) objective I4 : {0} ", objective.I4);
                        Logger.Debug(" (Notify) objective I5 : {0} ", objective.I5);
                        Logger.Debug(" (Notify) objectiveType : {0} ", objective.ObjectiveType);
                        Logger.Debug(" (Notify) objective GBID1 : {0} ", objective.GBID1);
                        Logger.Debug(" (Notify) objective GBID2 : {0} ", objective.GBID2);
                    // objective in quest step {4} and objective type {5} and quest objective ID {6}  :
                    }
                    if (type != objective.ObjectiveType) return;
                    switch (type)
                    {
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterWorld:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterScene:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.InteractWithActor:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.KillMonster:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.CompleteQuest:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.HadConversation:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterLevelArea:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EventReceived:   
                            if (value == objective.SNOName1.Id)
                            {
                                Counter++;
                                questStep.UpdateCounter(this);
                            }
                            break;
                                               
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterTrigger:                        
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.GameFlagSet:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.KillGroup:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.PlayerFlagSet:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.PossessItem:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.TimedEventExpired:
                            throw new NotImplementedException();
                    }
                }
            }

            // this is only public for GameCommand / Debug
            public struct ObjectiveSet
            {
                public List<QuestObjective> Objectives;
                public int FollowUpStepID;
            }

            public List<ObjectiveSet> ObjectivesSets = new List<ObjectiveSet>(); // this is only public for GameCommand / Debug
            public List<List<QuestObjective>> bonusObjectives = new List<List<QuestObjective>>(); // this is now public for GameCOmmand /Debug
            private Mooege.Common.MPQ.FileFormats.IQuestStep _questStep = null;
            private Quest _quest = null;
            public int QuestStepID { get { return _questStep.ID; } }

            // working on it erekose 
            private void UpdateBonusCounter(QuestObjective objective)
            {
                Logger.Debug(" calling updateBonus Counter snoQuest {0}, step ID {1}, TaskIndex {2}, counter {3}, Checked {4}", _quest.SNOHandle.Id, _questStep.ID, objective.ID+1, objective.Counter, objective.Done ? 1 : 0);
                foreach (var player in _quest.game.Players.Values)
                    player.InGameClient.SendMessage(new QuestCounterMessage()
                    {
                        snoQuest = _quest.SNOHandle.Id,
                        snoLevelArea = -1,
                        StepID = _questStep.ID,
                        TaskIndex = objective.ID+1, // TODO really ugly what about counting the legit obj + the bonus obj ?
                        Counter = objective.Counter,
                        Checked = objective.Done ? 1 : 0,
                    });
             
            }

            private void UpdateCounter(QuestObjective objective)
            {
                Logger.Debug(" (UpdateCounter) calling updateCounter snoQuest {0}, step ID {1}, TaskIndex {2}, counter {3}, Checked {4}", _quest.SNOHandle.Id, _questStep.ID, objective.ID, objective.Counter, objective.Done ? 1 : 0);
                if (_questStep is Mooege.Common.MPQ.FileFormats.QuestUnassignedStep == false)
                {
                    foreach (var player in _quest.game.Players.Values)
                        player.InGameClient.SendMessage(new QuestCounterMessage()
                        {
                            snoQuest = _quest.SNOHandle.Id,
                            snoLevelArea = -1,
                            StepID = _questStep.ID,
                            TaskIndex = objective.ID,
                            Counter = objective.Counter,
                            Checked = objective.Done ? 1 : 0,
                        });
                }

                var completedObjectiveList = from objectiveSet in ObjectivesSets
                                             where (from o in objectiveSet.Objectives select o.Done).Aggregate((r, o) => r && o)
                                             select objectiveSet.FollowUpStepID;
                if (completedObjectiveList.Count() > 0)
                    _quest.StepCompleted(completedObjectiveList.First());
            }

            /// <summary>
            /// Debug method, completes a given objective set
            /// </summary>
            /// <param name="index"></param>
            public void CompleteObjectiveSet(int index)
            {
                ////Logger.Debug("CompleteObjectiveSet {0} ", QuestStepID  );
                ////Logger.Debug(" quest step contains {0} objetive ", _questStep.StepObjectiveSets.Count);

                ////Logger.Debug(" %%%% in _questStep %%%%%");
                                
                //foreach (var stepobjective in _questStep.StepObjectiveSets) 
                //{
                //   // Logger.Debug(" quest step objectives are {0} ", stepobjective.FollowUpStepID);
                //}

                ////Logger.Debug(" %%%% in bonusObjectives %%%%%");

                ////Logger.Debug(" quest step contains {0}  lists of bonus quest objective ", bonusObjectives.Count);
                
                //foreach (var list_quest_objective in bonusObjectives)
                //{
                //    //Logger.Debug(" current list of quest bonus objectives contains {0} quest objective  ", list_quest_objective.Capacity);

                //    foreach (var quest_objective in list_quest_objective)
                //    {
                //        //Logger.Debug(" current quest bonus objective is {0} and type is {1} and value is {2}", quest_objective.ID, quest_objective.ObjectiveType, quest_objective.ObjectiveValue);
                //    }
                //}


                _quest.StepCompleted(_questStep.StepObjectiveSets[index].FollowUpStepID);
            }

            public QuestStep(Mooege.Common.MPQ.FileFormats.IQuestStep assetQuestStep, Quest quest)
            {
                _questStep = assetQuestStep;
                _quest = quest;
                int c = 0;

                foreach (var objectiveSet in assetQuestStep.StepObjectiveSets)
                {
                    ObjectivesSets.Add(new ObjectiveSet()
                    {
                        FollowUpStepID = objectiveSet.FollowUpStepID,
                        Objectives = new List<QuestObjective>(from objective in objectiveSet.StepObjectives select new QuestObjective(objective, this, c++))
                    });
                    
                    Logger.Debug(" adding an objectiveSet for quest {0} in step {1} ", quest.SNOHandle, quest.CurrentStep);
                }
                c = 0;

                if (assetQuestStep is Mooege.Common.MPQ.FileFormats.QuestStep)
                {
                    var step = assetQuestStep as Mooege.Common.MPQ.FileFormats.QuestStep;

                    if (step.StepBonusObjectiveSets != null)
                        foreach (var objectiveSet in step.StepBonusObjectiveSets)
                        {
                            Logger.Debug(" (questStep ctor) adding a bonus objective for quest {0} in step {1} ", quest.SNOHandle, quest.CurrentStep);
                            bonusObjectives.Add(new List<QuestObjective>(from objective in objectiveSet.StepBonusObjectives select new QuestObjective(objective, this, c++)));                            
                        }

                }

                Logger.Debug(" (questStep ctor) Displaying objectives sets for quest {0} in step {1} (if any) ", quest.SNOHandle, quest.CurrentStep);

                foreach (var objective_set in ObjectivesSets)
                    foreach (var objective in objective_set.Objectives)
                    {
                        Logger.Debug("(questStep ctor) % objective has ID {0}, type {1}, value {2}, counter {4}, sub quest step {3} ", objective.ID, objective.ObjectiveType, objective.ObjectiveValue, objective.questStep.QuestStepID, objective.Counter);
                        Logger.Debug("(questStep ctor) % objective in string is {0}", objective.ToString());
                    }


                Logger.Debug(" (questStep ctor) Displaying bonus objectives for quest {0} in step {1} (if any) ", quest.SNOHandle, quest.CurrentStep);
                
                foreach (var bonus_objective_set in bonusObjectives)
                    foreach (var bonus_objective in bonus_objective_set)
                    {
                        Logger.Debug("(questStep ctor) % bonus objective has ID {0}, type {1}, value {2}, counter {4}, sub quest step {3} ", bonus_objective.ID, bonus_objective.ObjectiveType, bonus_objective.ObjectiveValue, bonus_objective.questStep.QuestStepID, bonus_objective.Counter);
                        Logger.Debug("(questStep ctor) % bonus objective in string is {0}", bonus_objective.ToString());
                    }
                Logger.Debug("(questStep ctor)  raw quest step {0} ", _questStep);

            }

            public void NotifyBonus(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
            {
                foreach (var bonus_objective_set in bonusObjectives)
                    foreach (var bonus_objective in bonus_objective_set)
                    {
                        //Logger.Debug(" NotifyBonus through QuestStep impl for type {0} and value {1} ", type, value);
                        bonus_objective.NotifyBonus(type, value);
                    }
            }

            public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
            {
                foreach (var objectiveSet in ObjectivesSets)
                    foreach (var objective in objectiveSet.Objectives)
                    {
                        //Logger.Debug(" Notify through QuestStep impl for type {0} and value {1} ", type, value);
                        objective.Notify(type, value);
                    }
            }
        }

        public delegate void QuestProgressDelegate(Quest quest);
        public event QuestProgressDelegate OnQuestProgress;
        private Mooege.Common.MPQ.FileFormats.Quest asset = null;
        public SNOHandle SNOHandle { get; set; }
        private Game game { get; set; }
        public QuestStep CurrentStep { get; set; }
        private List<int> completedSteps = new List<int>();           // this list has to be saved if quest progress should be saved. It is required to keep track of questranges

        public Quest(Game game, int SNOQuest)
        {
            this.game = game;
            SNOHandle = new SNOHandle(SNOGroup.Quest, SNOQuest);
            asset = SNOHandle.Target as Mooege.Common.MPQ.FileFormats.Quest;
            CurrentStep = new QuestStep(asset.QuestUnassignedStep, this);
        }

        // 
        public bool HasStepCompleted(int stepID)
        {
            return completedSteps.Contains(stepID); // || CurrentStep.ObjectivesSets.Select(x => x.FollowUpStepID).Contains(stepID);
        }

        public void Advance()
        {
            //Logger.Debug(" Advancing Current step  {0}", CurrentStep.QuestStepID);
            foreach (var objsetelm in CurrentStep.ObjectivesSets)
            {
                //Logger.Debug(" Current step  Objective sets type {0}", objsetelm.GetType());
            }
            CurrentStep.CompleteObjectiveSet(0);
        }

        public void StepCompleted(int FollowUpStepID)
        {
            foreach (var player in game.Players.Values)
                player.InGameClient.SendMessage(new QuestUpdateMessage()
                {
                    snoQuest = SNOHandle.Id,
                    snoLevelArea = -1,
                    StepID = FollowUpStepID,
                    Field3 = true,
                    Failed = false
                });
            completedSteps.Add(CurrentStep.QuestStepID);
            CurrentStep = (from step in asset.QuestSteps where step.ID == FollowUpStepID select new QuestStep(step, this)).FirstOrDefault(); //TODO Maintain all possible quests EREKOSE ??
            if (OnQuestProgress != null)
                OnQuestProgress(this);
        }

        public void NotifyBonus(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
        {
            if (CurrentStep != null)
            {
                //Logger.Debug(" NotifyBonus through Quest impl for type {0} and value {1} ", type, value);
                CurrentStep.NotifyBonus(type, value);
            }
        }

        public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
        {

            if (CurrentStep != null)
            {
                // Logger.Debug(" Notify through Quest impl for type {0} and value {1} ", type, value);
                CurrentStep.Notify(type, value);
            }
        }
    }

}