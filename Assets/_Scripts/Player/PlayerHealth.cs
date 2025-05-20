public class PlayerHealth : Health
{
    /// <summary>
    /// The health of the possesed body. Does not count the main body
    /// </summary>
    public float possesedHealth { get; private set; } 

    public override void Die()
    {
        Death();
    }
}
