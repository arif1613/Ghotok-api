namespace GhotokApi.Models
{
    public enum ErrorCodes
    {
        RequiredFieldsMissing,
        UserAlreadyRegistered,
        UserAlreadyLoggedin,
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