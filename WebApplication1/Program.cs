using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
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

public class Product
{
    public string Code { get; set; }
    public string Name { get; set; }
}