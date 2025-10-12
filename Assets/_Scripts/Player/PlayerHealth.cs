public class PlayerHealth : Health
{

    /// <summary>
    /// The health of the possesed body. Does not count the main body
    /// </summary>
    public float possesedHealth { get; private set; } 
    public float possesedHealthMax { get; private set; }
    public bool noPossessedHealth = true;

    private Health possessedHealthScript;

    public override void Update()
    {
        possessedHealthScript = PlayerController.Instance.currentBody.GetComponentInParent<Health>();

        if(possessedHealthScript == this || possessedHealthScript == null)
        {
            noPossessedHealth = true;
        }
        else
        {
            noPossessedHealth = false;
            possesedHealth = possessedHealthScript.currentHealth;
            possesedHealthMax = possessedHealthScript.maxHealth;
        }

        base.Update();
    }

    public void HealPlayer(float amount)
    {
        currentHealth += amount;
    }

    public override void Die()
    {
        Death();
    }
}
