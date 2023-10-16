namespace EShop.IdentityServer.SeedDatabase
{
    public interface ISeedDatabaseInitializer
    {
        void InitializeSeedRoles();
        void InitializeSeedUsers();
    }
}
