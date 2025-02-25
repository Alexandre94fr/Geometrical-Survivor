public interface S_IProjectile
{
    public void LaunchProjectile(
        S_DefaultProjectile.ProjectileOwnerEnum p_projectileOwner,
        float p_projectileLifetime,
        float p_projectileRange,
        int p_projectileDamage
    );
}