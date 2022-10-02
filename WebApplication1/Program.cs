using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/product", (Product product) =>
{
    ProductRepository.Add(product);
});
app.MapGet("/product/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetBy(code);
    return product;
});

app.MapPut("/product", (Product product) =>
{
   var produto = ProductRepository.GetBy(product.Code);
    produto.Name = product.Name;
});
app.MapDelete("/product/{code}", ([FromRoute] string code) =>
{
    ProductRepository.Delete(code);
});


app.Run();

public static class ProductRepository
{
    public static List<Product> Products { get; set; }
    //método de adição
    public static void Add(Product product)
    {
        if (Products == null)
            Products = new List<Product>();
        Products.Add(product);
    }
    //método de buscar produto
    public static Product GetBy(string code)
    {
        return Products.FirstOrDefault(p => p.Code == code);
    }
    public static void Delete(string code)
    {
        Products.Remove(ProductRepository.GetBy(code));
    }

}

public class Category
{
    public int Id { get; set; }
    public string Description { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public List<Category> Category { get; set; }
}
//classe de configuração da conexão com o banco sqlserver
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2019;Encrypt=NO;Trusted_Connection=NO");
}