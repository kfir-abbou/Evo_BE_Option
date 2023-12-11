using CatheterPosition.Models;
using MessageBus.Infra.Interface;

namespace MessageBus.Infra.Reply.PositionService
{
    public class CatheterPositionReply : ICatheterPositionReply
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Yaw { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }
        public string Id { get; set; }
        public byte[] Content { get; set; }

        public CatheterPositionReply()
        {

        }

        public CatheterPositionReply(double x, double y, double z, double yaw, double pitch, double roll)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }

        public CatheterPositionReply(IPosition position)
        {
            X = position.X;
            Y = position.Y;
            Z = position.Z;
            Yaw = position.Yaw;
            Pitch = position.Pitch;
            Roll = position.Roll;
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z} - - - {Yaw}, {Pitch}, {Roll}";
        }
    }

    public interface ICatheterPositionReply : IReply
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double Yaw { get; }
        public double Pitch { get; }
        public double Roll { get; }
    }
}
