using CricSummit.Application.DTO;

namespace CricSummit.Application.Interfaces
{
    public interface ISuperOverCommentaryService
    {
        SuperOverResultDto PlaySuperOver(List<SuperOverRequestDto> input);
    }
}
