using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerySimpleStateMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            var stateMachine = new StateMachine(StateMachine.State.Draft);
            stateMachine.ProcessEvent(StateMachine.Event.Create);
            stateMachine.ProcessEvent(StateMachine.Event.Submit);
            stateMachine.ProcessEvent(StateMachine.Event.Reject);
            stateMachine.ProcessEvent(StateMachine.Event.Modify);
            stateMachine.ProcessEvent(StateMachine.Event.Submit);
            stateMachine.ProcessEvent(StateMachine.Event.Approve);
            stateMachine.ProcessEvent(StateMachine.Event.Archive);
            Console.ReadKey();
        }
    }
}
