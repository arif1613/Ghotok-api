namespace GhotokApi.Models
{
    public enum ErrorCodes
    {
        RequiredFieldsMissing,
        UserAlreadyRegistered,
        UserAlreadyLoggedin,
        UserAlreadyLoggedinInMOreThanThreeDevices,
        InvalidOtp,
        RecordNotFound,
        CouldNotCreateData,
        CouldNotUpdateData,
        CouldNotDeleteData,
        UserLoggedOut,
        InvalidInput,
        GenericError,
        UserIsNotRegistered
    }

    public enum AcceptedCodes
    {
        DataAdded,
        DataUpdated,
        DataRemoved
    }
}