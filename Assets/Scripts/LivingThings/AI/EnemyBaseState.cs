
public abstract class EnemyBaseState
{
    /// <summary>
    /// Executes logic when the state is entered.
    /// </summary>
    /// <param name="ctx">Context</param>
    public abstract void EnterState(EnemyThing ctx);

    /// <summary>
    /// Executes the state logic once per frame.
    /// </summary>
    /// 
    /// <remarks>
    /// It's preferable to use this in a MonoBehaviour Update() method.
    /// </remarks>
    /// 
    /// <param name="ctx"></param>
    public abstract void UpdateState(EnemyThing ctx);
}