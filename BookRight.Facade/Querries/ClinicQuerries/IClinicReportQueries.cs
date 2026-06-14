using BookRight.Facade.Dtos.QuerryDto.ClinicReportDtos;

namespace BookRight.Facade.Querries.ClinicQuerries;

public interface IClinicReportQueries
{
    Task<IReadOnlyList<ClinicReportDto>> GetClinicReportsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);
}