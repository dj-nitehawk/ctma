using Ctma;

namespace Members.Signup;

sealed class DuplicateInfoChecker : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> ctx, CancellationToken c)
    {
        var tEmail = DB.Find<Dom.Member>()
                       .Match(m => m.Email == ctx.Request.Email.LowerCase())
                       .ExecuteAnyAsync(cancellation: c);

        var tNic = DB.Find<Dom.Member>()
                     .Match(m => m.Nic == ctx.Request.Nic.UpperCase())
                     .ExecuteAnyAsync(cancellation: c);

        var tSlmc = DB.Find<Dom.Member>()
                      .Match(m => m.Slmc == ctx.Request.Slmc.Trim())
                      .ExecuteAnyAsync(cancellation: c);

        var tMobile = DB.Find<Dom.Member>()
                        .Match(m => m.MobileNumber == ctx.Request.Contact.MobileNumber.Trim())
                        .ExecuteAnyAsync(cancellation: c);

        var (eml, nic, slmc, mob) = await Tasks.Run(tEmail, tNic, tSlmc, tMobile);

        if (eml)
            ctx.ValidationFailures.Add(new(nameof(Request.Email), "Email address is in use!"));
        if (nic)
            ctx.ValidationFailures.Add(new(nameof(Request.Nic), "NIC is in use!"));
        if (slmc)
            ctx.ValidationFailures.Add(new(nameof(Request.Slmc), "SLMC number is in use!"));
        if (mob)
            ctx.ValidationFailures.Add(new($"{nameof(Request.Contact)}.{nameof(Request.Contact.MobileNumber)}", "Mobile number is in use!"));

        if (ctx.ValidationFailures.Count > 0)
            await ctx.HttpContext.Response.SendErrorsAsync(ctx.ValidationFailures, cancellation: c);
    }
}