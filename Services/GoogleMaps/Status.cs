namespace JewishBot.Services.GoogleMaps
{
    public enum Status
    {
        Ok,
        ZeroResults,
        OverQueryLimit,
        RequestDenied,
        InvalidRequest,
        UnknownError
    }
}