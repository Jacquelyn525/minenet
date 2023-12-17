using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization; //Added for access to [Authorize]
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Drawing;//Added for access to Image class

using MvcWeb.Models;


namespace GadgetStore.UI.MVC.Controllers {

  //[Authorize(Roles = "Admin")]
  public class HistoryController : Controller {

    private readonly MineNetDBContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;


    public HistoryController(MineNetDBContext context, IWebHostEnvironment webHostEnvironment) {
      _context = context;
      _webHostEnvironment = webHostEnvironment;
    }

    // GET: Products
    [AllowAnonymous]
    public async Task<IActionResult> Index() {
      //By default, let's show only those products that are not discontinued
      var entries = _context.TagHistories.Where(t => t.TagId != null).Select(b => b);

      return View(await entries.ToListAsync());
    }

    // GET: Products/Details/5
    //[AllowAnonymous]
    //public async Task<IActionResult> Details(int? id, string? prevAction) {
    //  if (id == null || _context.Products == null) {
    //    return NotFound();
    //  }

    //  var product = await _context.Products
    //      .Include(p => p.Category)
    //      .Include(p => p.Supplier)
    //      .FirstOrDefaultAsync(m => m.ProductId == id);
    //  if (product == null) {
    //    return NotFound();
    //  }

    //  if (prevAction == "Index") {
    //    ViewBag.PrevAction = "Index";
    //  } else {
    //    ViewBag.PrevAction = "TiledProducts";
    //  }

    //  return View(product);
    //}

    // POST: Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,UnitsInStock,UnitsOnOrder,IsDiscontinued,CategoryId,SupplierId,ProductImage,Image")] Product product) {

    //  ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
    //  ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
    //  return View(product);
    //}

  }
}

/*


  [Authorize(Roles = "Admin")]
  public class ProductsController : Controller {

    private readonly GadgetStoreContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;


    public ProductsController(GadgetStoreContext context, IWebHostEnvironment webHostEnvironment) {
      _context = context;
      _webHostEnvironment = webHostEnvironment;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index() {
      //By default, let's show only those products that are not discontinued
      var products = _context.Products.Where(p => !p.IsDiscontinued)
          .Include(p => p.Category)
          .Include(p => p.Supplier)
          .Include(p => p.OrderProducts);

      return View(await products.ToListAsync());
    }

    [AllowAnonymous]
    public async Task<IActionResult> TiledProducts() {
      var products = _context.Products.Where(p => !p.IsDiscontinued)
          .Include(p => p.Category)
          .Include(p => p.Supplier)
          .Include(p => p.OrderProducts);

      return View(await products.ToListAsync());
    }


    // GET: Products/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id, string? prevAction) {
      if (id == null || _context.Products == null) {
        return NotFound();
      }

      var product = await _context.Products
          .Include(p => p.Category)
          .Include(p => p.Supplier)
          .FirstOrDefaultAsync(m => m.ProductId == id);
      if (product == null) {
        return NotFound();
      }

      if (prevAction == "Index") {
        ViewBag.PrevAction = "Index";
      } else {
        ViewBag.PrevAction = "TiledProducts";
      }

      return View(product);
    }

    // GET: Products/Create
    public IActionResult Create() {
      ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
      ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
      return View();
    }

    // POST: Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,UnitsInStock,UnitsOnOrder,IsDiscontinued,CategoryId,SupplierId,ProductImage,Image")] Product product) {
      if (ModelState.IsValid) {
        #region File Upload - Create
        //check to see if an image was uploaded
        if (product.Image != null) {
          //check to see if the file type is one we would like to use
          //- Retrieve the extension of the uploaded file
          string ext = Path.GetExtension(product.Image.FileName);

          //-Create a list of valid extentions
          string[] validExts = { ".jpg", ".jpeg", ".gif", ".png" };

          //-Verify that the uploaded file has an extension matching one of the extensions
          //  in the list above AND verify the file size is not too big for our .NET app
          if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303) {
            //Generate a unique file name
            product.ProductImage = Guid.NewGuid() + ext;

            //Save the file to the web server (here, we will save to the wwwroot/images.)
            //In order to access the webroot folder we MUST add a field to the controller
            //for the _webHostEnvironment (see the top of this file for an example).

            //REtrive the path to the wwwroot
            string webRootPath = _webHostEnvironment.WebRootPath;

            //Create a variable for the full image path ( this is where we will save the image)
            string fullImagePath = webRootPath + "/images/";

            //Create a MemoryStream to read the image into the server memory
            using (var memoryStream = new MemoryStream()) {
              //Transfer the file from the request to server memory
              await product.Image.CopyToAsync(memoryStream);

              using (var img = Image.FromStream(memoryStream)) {
                //Now we can send the image to the ImageUtility for resizing
                //and thumbnail creation. Here are the items we need:
                //1) A string with the path where we would like to save the image.
                //2) A string with the name of the image file.
                //3) The image itself.
                //4) An int with the maximum size of our image (in pixels)
                //5) An int with the maximum size of our thumbnail image (in pixels)

                int maxImageSize = 500; //pixels
                int maxThumbSize = 100;

                //Use the ImageUtility to resize and save the images
                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
              }
            }
          }
        } else {
          //If no image was uploaded, assign a default filename.
          //We can then download a default image and give it the same
          //file name and copy it to the wwwroot/images folder.
          product.ProductImage = "noimage.png";
        }

        #endregion


        _context.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
      ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
      return View(product);
    }


    private bool ProductExists(int id) {
      return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
    }
  }
}

*/
