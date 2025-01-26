namespace Citizen
{
    public interface ICitizenState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}