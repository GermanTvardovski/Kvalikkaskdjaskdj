namespace Matie.Services;

public interface IImagePickerService
{
    Task<byte[]?> PickImageBytesAsync(CancellationToken cancellationToken = default);
}
