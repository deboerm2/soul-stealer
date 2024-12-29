public class PlayerHealth : Health
{
    /// <summary>
    /// The health of the possesed body. Does not count the main body
    /// </summary>
    public float possesedHealth { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(currentHealth);
    }

    public override void Die()
    {

    }
}
