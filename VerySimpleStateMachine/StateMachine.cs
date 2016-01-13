using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VerySimpleStateMachine
{
    public class StateMachine
    {
        public enum State
        {
            Draft = 0,
            Pending,
            Approved,
            Rejected,
            Archived
        }

        public enum Event
        {
            Create = 0,
            Modify,
            Submit,
            Approve,
            Reject,
            Archive
        }

        public delegate void StateMachineAction(Event stateEvent);

        /// <summary>
        /// Current State
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// Array of actions, 1 for each State and StateEvent
        /// </summary>
        private StateMachineAction[,] actions;

        public StateMachine(State currentState) : this()
        {
            this.CurrentState = currentState;
        }

        public StateMachine()
        {
            this.actions = new StateMachineAction[5, 6]
            {
                //  Create,                     Modify,             Submit,                 Approve,                    Reject,                     Archive
                {   this.CreateQualification,   this.DoNothing,     this.ReadyForApproval,  this.DoNothing,             this.DoNothing,             this.ArchiveQualification   }, // Draft
                {   this.DoNothing,             this.RevertToDraft, this.DoNothing,         this.ApproveQualification,  this.RejectQualification,   this.ArchiveQualification   }, // Pending
                {   this.DoNothing,             this.RevertToDraft, this.DoNothing,         this.DoNothing,             this.DoNothing,             this.ArchiveQualification   }, // Approved
                {   this.DoNothing,             this.RevertToDraft, this.DoNothing,         this.DoNothing,             this.DoNothing,             this.ArchiveQualification   }, // Rejected
                {   this.DoNothing,             this.RevertToDraft, this.DoNothing,         this.DoNothing,             this.DoNothing,             this.DoNothing              }  // Archived
            };
        }


        public State ProcessEvent(Event stateEvent)
        {
            var method = this.actions[(int)this.CurrentState, (int)stateEvent].Method;
            var parameters = new object[1] { stateEvent };
            method.Invoke(this, parameters);
            return this.CurrentState;
        }

        #region Actions
        private void DoNothing(Event stateEvent)
        {
            LogAction(stateEvent, this.CurrentState, this.CurrentState);
        }

        private void CreateQualification(Event stateEvent)
        {
            LogAction(stateEvent, this.CurrentState, this.CurrentState);
        }

        private void RevertToDraft(Event stateEvent)
        {
            var preState = this.CurrentState;
            this.CurrentState = State.Draft;
            LogAction(stateEvent, preState, this.CurrentState);
        }

        private void ReadyForApproval(Event stateEvent)
        {
            var preState = this.CurrentState;
            this.CurrentState = State.Pending;
            LogAction(stateEvent, preState, this.CurrentState);
        }

        private void ApproveQualification(Event stateEvent)
        {
            var preState = this.CurrentState;
            this.CurrentState = State.Approved;
            LogAction(stateEvent, preState, this.CurrentState);
        }

        private void RejectQualification(Event stateEvent)
        {
            var preState = this.CurrentState;
            this.CurrentState = State.Rejected;
            LogAction(stateEvent, preState, this.CurrentState);
        }

        private void ArchiveQualification(Event stateEvent)
        {
            var preState = this.CurrentState;
            this.CurrentState = State.Archived;
            LogAction(stateEvent, preState, this.CurrentState);
        }
        #endregion

        private void LogAction(Event stateEvent, State preState, State postState)
        {
            Console.WriteLine("Event: {0}", stateEvent.ToString());
            Console.WriteLine(" - Pre  State: {0}" + preState.ToString());
            Console.WriteLine(" - Post State: {0}" + postState.ToString());
        }
    }
}
