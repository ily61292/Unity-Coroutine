using System.Collections;


namespace Hi
{
    public interface IWaitable 
    {
        bool IsTickOver(float deltaTime);
        void Reset();
    }
}
