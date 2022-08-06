namespace DAL;

public class AncIngredientRepository
{
    private readonly AppDbContext _db;
    public static AncIngredientRepository Instance = new AncIngredientRepository(AppDbContext.Instance);

    private AncIngredientRepository(AppDbContext db)
    {
        _db = db;
    }
}