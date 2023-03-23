namespace Glaidiator.Model.Actions
{
    public interface IAction
    {
        float Duration { get; }
        bool CanMove { get; }
        bool CanAction { get; }
    }
}