using System.Threading.Tasks;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Utils.Otp
{
    public interface IOtpSender
    {
        Task SendOtpMobileMessage(OtpRequestModel model,string otp);
        Task SendOtpEmailMessage(OtpRequestModel model, string otp);
    }
}