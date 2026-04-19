namespace Matie.Services;

public interface IUiDialogService
{
    Task<bool> ConfirmCloseAsync();
    Task<bool> ShowServiceEditorAsync(int? serviceId);
}
