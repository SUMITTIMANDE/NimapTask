using NimapTask.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System;
using System.Linq;

public class ProductController : Controller
{
    private StoreDbContext dc = new StoreDbContext();

    // Display Products with Pagination
    public ViewResult DisplayProducts(int pageNumber = 1, int pageSize = 10)
    {
        // Calculate skip and take for pagination
        int skip = (pageNumber - 1) * pageSize;
        int take = pageSize;

        // Fetch the products and include the Category information
        var products = dc.Products
                         .Include(p => p.Category)
                         .OrderBy(p => p.ProductId) // Ensure products are ordered by ProductId (or any other property)
                         .Skip(skip)  // Skip records from previous pages
                         .Take(take)  // Take the current page's products
                         .ToList();

        // Get total product count to calculate total pages
        int totalProducts = dc.Products.Count();
        int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        // Pass necessary data to the view
        ViewBag.PageNumber = pageNumber;
        ViewBag.TotalPages = totalPages;
        ViewBag.PageSize = pageSize;

        return View(products);
    }

    // Other action methods (Add, Edit, Delete) remain the same
    public ViewResult DisplayProduct(int id)
    {
        Product product = dc.Products.Include(p => p.Category).Where(p => p.ProductId == id).Single();
        return View(product);
    }

    public ViewResult AddProduct()
    {
        ViewBag.CategoryId = new SelectList(dc.Categories, "CategoryId", "CategoryName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public RedirectToRouteResult AddProduct(Product product)
    {
        dc.Products.Add(product);
        dc.SaveChanges();
        return RedirectToAction("DisplayProducts");
    }

    public ViewResult EditProduct(int id)
    {
        Product product = dc.Products.Find(id);
        ViewBag.CategoryId = new SelectList(dc.Categories, "CategoryId", "CategoryName", product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public RedirectToRouteResult UpdateProduct(Product product)
    {
        dc.Entry(product).State = EntityState.Modified;
        dc.SaveChanges();
        return RedirectToAction("DisplayProducts");
    }

    public RedirectToRouteResult DeleteProduct(int id)
    {
        Product product = dc.Products.Find(id);
        dc.Products.Remove(product);
        dc.SaveChanges();
        return RedirectToAction("DisplayProducts");
    }
}
