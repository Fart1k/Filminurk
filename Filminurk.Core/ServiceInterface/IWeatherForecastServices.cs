using Filminurk.Core.Dto.AccuWeatherDTOs;

namespace Filminurk.Core.ServiceInterface
{
    public interface IWeatherForecastServices
    {
        Task<AccuLocationWeatherResultDTO> AccuWeatherResult(AccuLocationWeatherResultDTO dto);
    }
}
