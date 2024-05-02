namespace ProductService.Model.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public void UpdatePrice(int NewPrice)
    {
        if (NewPrice==0)
        {
            throw new Exception("");
        }
        this.Price = NewPrice;
    }
}
