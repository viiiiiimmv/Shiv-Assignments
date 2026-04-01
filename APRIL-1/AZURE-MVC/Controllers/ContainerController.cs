using AZURE_MVC.Models;
using AZURE_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AZURE_MVC.Controllers;

public class ContainerController : Controller
{
    private readonly IContainerService _containerService;
    public ContainerController(IContainerService containerService)
    {
        _containerService = containerService;
    }
    public async Task<IActionResult> Index()
    {
        var allContainer = await _containerService.GetAllContainer();
        return View(allContainer);
    }

    public async Task<IActionResult> Create()
    {
        return View(new ContainerModel());
    }
    [HttpPost]
    public async Task<IActionResult> Create(ContainerModel container)
    {
        await _containerService.CreateContainer(container.Name);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(string containerName)
    {
        await _containerService.DeleteContainer(containerName);
        return RedirectToAction(nameof(Index));
    }
}