using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{
    public interface RobotAI<out Tai>
    {
        Tai GetCommand();
    }

    public class ShooterAI : RobotAI<ShooterCommand>
    {
        int counter = 1;

        public ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : RobotAI<BuilderCommand>
    {
        int counter = 1;
        public BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface Device<Tdevice>
    {
        string ExecuteCommand(Tdevice command);
    }

    public class Mover : Device<IMoveCommand>
    {
        public string ExecuteCommand(IMoveCommand allCommand)
        {
            var command = allCommand;
            if (command == null)
                throw new ArgumentException();
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class Robot
    {
        RobotAI<IMoveCommand> ai;
        Device<IMoveCommand> device;

        public Robot(RobotAI<IMoveCommand> ai, Device<IMoveCommand> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
             for (int i=0;i<steps;i++)
             {
                 var command = ai.GetCommand();
                 if (command == null)
                     break;
                 yield return device.ExecuteCommand(command);
             }
        }

        public static Robot Create(RobotAI<IMoveCommand> ai, Device<IMoveCommand> executor)
        {
            return new Robot(ai, executor);
        }
    }
}
