namespace Members.Signup;

sealed class DuplicateInfoChecker : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> ctx, CancellationToken c)
        => Task.CompletedTask;
}