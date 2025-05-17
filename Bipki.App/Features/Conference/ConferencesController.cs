using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace Bipki.App.Features.Conference;

[Route("conferences")]
public class ConferencesController : ControllerBase
{
    private readonly IConferenceRepository conferenceRepository;
    private readonly UserManager<User> userManager;

    public ConferencesController(IConferenceRepository conferenceRepository, UserManager<User> userManager)
    {
        this.conferenceRepository = conferenceRepository;
        this.userManager = userManager;
    }
    
    [Route("{conferenceId:guid}")]
    public IActionResult GetConference([FromRoute] Guid conferenceId)
    {
        throw new NotImplementedException();
    }
    
    [Route("{conferenceId:guid}/qrcode")]
    [Authorize]
    public IActionResult GetConferencentranceQr([FromRoute] Guid conferenceId)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId))
            return Unauthorized();

        // TODO you see the problem, I believe
        var qrCodeData = QRCodeGenerator.GenerateQrCode(new PayloadGenerator.Url($"http://localhost/api/admin/checkInGuest/{userId}"));
        
        return Ok(new PngByteQRCode(qrCodeData).GetGraphic(10));
    }
}