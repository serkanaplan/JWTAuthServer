using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Models;
using JWTAuthServer.Core.Services;

namespace JWTAuthServer.API.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController(IServiceGeneric<Product, ProductDto> productService) : CustomBaseController
{
    private readonly IServiceGeneric<Product, ProductDto> _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetProducts() => ActionResultInstance(await _productService.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> SaveProduct(ProductDto productDto) => ActionResultInstance(await _productService.AddAsync(productDto));

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductDto productDto) => ActionResultInstance(await _productService.Update(productDto, productDto.Id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id) => ActionResultInstance(await _productService.Remove(id));
}