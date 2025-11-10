namespace TaxiSimulation.Api
{
    public class ParallelLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _limit;
        private static int _current = 0;

        public ParallelLimitMiddleware(RequestDelegate next, IConfiguration cfg)
        {
            _next = next;
            _limit = cfg.GetValue<int>("Settings:ParallelLimit");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            int cur = Interlocked.Increment(ref _current);
            if (cur > _limit)
            {
                Interlocked.Decrement(ref _current);
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Сервис перегружен. Попробуйте позже.");
                return;
            }

            try
            {
                await _next(context);
            }
            finally
            {
                Interlocked.Decrement(ref _current);
            }
        }
    }

}
